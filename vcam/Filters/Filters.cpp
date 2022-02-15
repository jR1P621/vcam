#pragma warning(disable:4244)
#pragma warning(disable:4711)

#include <opencv2\opencv.hpp>
#include <cor.h>
#include <streams.h>
#include <stdio.h>
#include <olectl.h>
#include <dvdmedia.h>
#include "filters.h"
#include <fstream>

//////////////////////////////////////////////////////////////////////////
//  CVCam is the source filter which masquerades as a capture device
//////////////////////////////////////////////////////////////////////////
CUnknown * WINAPI CVCam::CreateInstance(LPUNKNOWN lpunk, HRESULT *phr)
{
    ASSERT(phr);
    CUnknown *punk = new CVCam(lpunk, phr);
    return punk;
}

CVCam::CVCam(LPUNKNOWN lpunk, HRESULT *phr) : 
    CSource(NAME("Virtual Cam"), lpunk, CLSID_VirtualCam)
{
    ASSERT(phr);
    CAutoLock cAutoLock(&m_cStateLock);
    // Create the one and only output pin
    m_paStreams = (CSourceStream **) new CVCamStream*[1];
    m_paStreams[0] = new CVCamStream(phr, this, L"Virtual Cam");
}

HRESULT CVCam::QueryInterface(REFIID riid, void **ppv)
{
    //Forward request for IAMStreamConfig & IKsPropertySet to the pin
    if(riid == _uuidof(IAMStreamConfig) || riid == _uuidof(IKsPropertySet))
        return m_paStreams[0]->QueryInterface(riid, ppv);
    else
        return CSource::QueryInterface(riid, ppv);
}

//////////////////////////////////////////////////////////////////////////
// CVCamStream is the one and only output pin of CVCam which handles 
// all the stuff.
//////////////////////////////////////////////////////////////////////////
CVCamStream::CVCamStream(HRESULT *phr, CVCam *pParent, LPCWSTR pPinName) :
    CSourceStream(NAME("Virtual Cam"),phr, pParent, pPinName), m_pParent(pParent)
{
    // Set the default media type as 320x240x24@15
    GetMediaType(4, &m_mt);

    // Get env variable for config path
    char* buf = nullptr;
    size_t sz = 0;
    if (_dupenv_s(&buf, &sz, "APPDATA") == 0 && buf != nullptr)
    {
        configPath = string(buf) + "\\camApp\\listener.dat";
        free(buf);
    }
}

CVCamStream::~CVCamStream()
{
} 

HRESULT CVCamStream::QueryInterface(REFIID riid, void **ppv)
{   
    // Standard OLE stuff
    if(riid == _uuidof(IAMStreamConfig))
        *ppv = (IAMStreamConfig*)this;
    else if(riid == _uuidof(IKsPropertySet))
        *ppv = (IKsPropertySet*)this;
    else
        return CSourceStream::QueryInterface(riid, ppv);

    AddRef();
    return S_OK;
}


//////////////////////////////////////////////////////////////////////////
//  This is the routine where we create the data being output by the Virtual
//  Camera device.
//////////////////////////////////////////////////////////////////////////

HRESULT CVCamStream::FillBuffer(IMediaSample *pms)
{
    REFERENCE_TIME rtNow;
    
    REFERENCE_TIME avgFrameTime = ((VIDEOINFOHEADER*)m_mt.pbFormat)->AvgTimePerFrame;

    rtNow = m_rtLastTime;
    m_rtLastTime += avgFrameTime;
    pms->SetTime(&rtNow, &m_rtLastTime);
    pms->SetSyncPoint(TRUE);

    BYTE *pData;
    long lDataLen;
    pms->GetPointer(&pData);
    lDataLen = pms->GetSize();

    if (streamEnabled && videoCap.isOpened()) {
        long lStride;
        DWORD dwWidth, dwHeight;
        VIDEOINFOHEADER* pVih = (VIDEOINFOHEADER*)m_mt.pbFormat;
        GetVideoInfoParameters(pVih, &dwWidth, &dwHeight, &lStride, false);

        Mat frame(dwWidth, dwHeight, CV_8U, lStride);
        videoCap.read(frame);
        resize(frame, frame, Size(dwWidth, dwHeight));
        flip(frame, frame, 0);

        uchar* p = frame.data;
        for (int i = 0; i < lDataLen; i++)
            pData[i] = p[i];
    }
    else
    {

        for (int i = 0; i < lDataLen; ++i)
            pData[i] = rand();
    }

    return NOERROR;
} // FillBuffer


//
// Notify
// Ignore quality management messages sent from the downstream filter
STDMETHODIMP CVCamStream::Notify(IBaseFilter * pSender, Quality q)
{
    return E_NOTIMPL;
} // Notify

//////////////////////////////////////////////////////////////////////////
// This is called when the output format has been negotiated
//////////////////////////////////////////////////////////////////////////
HRESULT CVCamStream::SetMediaType(const CMediaType *pmt)
{
    DECLARE_PTR(VIDEOINFOHEADER, pvi, pmt->Format());
    HRESULT hr = CSourceStream::SetMediaType(pmt);
    return hr;
}

// See Directshow help topic for IAMStreamConfig for details on this method
HRESULT CVCamStream::GetMediaType(int iPosition, CMediaType *pmt)
{
    if(iPosition < 0) return E_INVALIDARG;
    if(iPosition > 8) return VFW_S_NO_MORE_ITEMS;

    if(iPosition == 0) 
    {
        *pmt = m_mt;
        return S_OK;
    }

    DECLARE_PTR(VIDEOINFOHEADER, pvi, pmt->AllocFormatBuffer(sizeof(VIDEOINFOHEADER)));
    ZeroMemory(pvi, sizeof(VIDEOINFOHEADER));

    pvi->bmiHeader.biCompression = BI_RGB;
    pvi->bmiHeader.biBitCount    = 24;
    pvi->bmiHeader.biSize       = sizeof(BITMAPINFOHEADER);
    pvi->bmiHeader.biWidth      = 160 * iPosition;
    pvi->bmiHeader.biHeight     = 90 * iPosition;
    pvi->bmiHeader.biPlanes     = 1;
    pvi->bmiHeader.biSizeImage  = GetBitmapSize(&pvi->bmiHeader);
    pvi->bmiHeader.biClrImportant = 0;

    pvi->AvgTimePerFrame = 1000000;

    SetRectEmpty(&(pvi->rcSource)); // we want the whole image area rendered.
    SetRectEmpty(&(pvi->rcTarget)); // no particular destination rectangle

    pmt->SetType(&MEDIATYPE_Video);
    pmt->SetFormatType(&FORMAT_VideoInfo);
    pmt->SetTemporalCompression(FALSE);

    // Work out the GUID for the subtype from the header info.
    const GUID SubTypeGUID = GetBitmapSubtype(&pvi->bmiHeader);
    pmt->SetSubtype(&SubTypeGUID);
    pmt->SetSampleSize(pvi->bmiHeader.biSizeImage);
    
    return NOERROR;

} // GetMediaType

// This method is called to see if a given output format is supported
HRESULT CVCamStream::CheckMediaType(const CMediaType *pMediaType)
{
    VIDEOINFOHEADER *pvi = (VIDEOINFOHEADER *)(pMediaType->Format());
    if(*pMediaType != m_mt) 
        return E_INVALIDARG;
    return S_OK;
} // CheckMediaType

// This method is called after the pins are connected to allocate buffers to stream data
HRESULT CVCamStream::DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pProperties)
{
    CAutoLock cAutoLock(m_pFilter->pStateLock());
    HRESULT hr = NOERROR;

    VIDEOINFOHEADER *pvi = (VIDEOINFOHEADER *) m_mt.Format();
    pProperties->cBuffers = 1;
    pProperties->cbBuffer = pvi->bmiHeader.biSizeImage;

    ALLOCATOR_PROPERTIES Actual;
    hr = pAlloc->SetProperties(pProperties,&Actual);

    if(FAILED(hr)) return hr;
    if(Actual.cbBuffer < pProperties->cbBuffer) return E_FAIL;

    return NOERROR;
} // DecideBufferSize

// Called when graph is run
HRESULT CVCamStream::OnThreadCreate()
{
    m_rtLastTime = 0;
    return NOERROR;
} // OnThreadCreate

HRESULT CVCamStream::OnThreadStartPlay()
{
    videoCap.set(CAP_PROP_FPS, 15);
    if (!videoCap.isOpened()) {
        videoCap.open(getStream());
    }

    if (videoCap.isOpened())
    {
        videoCap.set(CAP_PROP_BUFFERSIZE, 0);
        videoCap.set(CAP_PROP_BUFFERSIZE, 15);
        return NOERROR;
    }
    return E_FAIL;
}

HRESULT CVCamStream::OnThreadDestroy()
{
    if (videoCap.isOpened())
    {
        videoCap.release();
    }

    return S_OK;
}

//////////////////////////////////////////////////////////////////////////
//  IAMStreamConfig
//////////////////////////////////////////////////////////////////////////

HRESULT STDMETHODCALLTYPE CVCamStream::SetFormat(AM_MEDIA_TYPE *pmt)
{
    DECLARE_PTR(VIDEOINFOHEADER, pvi, m_mt.pbFormat);
    m_mt = *pmt;
    IPin* pin; 
    ConnectedTo(&pin);
    if(pin)
    {
        IFilterGraph *pGraph = m_pParent->GetGraph();
        pGraph->Reconnect(this);
    }
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CVCamStream::GetFormat(AM_MEDIA_TYPE **ppmt)
{
    *ppmt = CreateMediaType(&m_mt);
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CVCamStream::GetNumberOfCapabilities(int *piCount, int *piSize)
{
    *piCount = 8;
    *piSize = sizeof(VIDEO_STREAM_CONFIG_CAPS);
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CVCamStream::GetStreamCaps(int iIndex, AM_MEDIA_TYPE **pmt, BYTE *pSCC)
{
    *pmt = CreateMediaType(&m_mt);
    DECLARE_PTR(VIDEOINFOHEADER, pvi, (*pmt)->pbFormat);

    if (iIndex == 0) iIndex = 4;

    pvi->bmiHeader.biCompression = BI_RGB;
    pvi->bmiHeader.biBitCount    = 24;
    pvi->bmiHeader.biSize       = sizeof(BITMAPINFOHEADER);
    pvi->bmiHeader.biWidth      = 160 * iIndex;
    pvi->bmiHeader.biHeight     = 90 * iIndex;
    pvi->bmiHeader.biPlanes     = 1;
    pvi->bmiHeader.biSizeImage  = GetBitmapSize(&pvi->bmiHeader);
    pvi->bmiHeader.biClrImportant = 0;

    SetRectEmpty(&(pvi->rcSource)); // we want the whole image area rendered.
    SetRectEmpty(&(pvi->rcTarget)); // no particular destination rectangle

    (*pmt)->majortype = MEDIATYPE_Video;
    (*pmt)->subtype = MEDIASUBTYPE_RGB24;
    (*pmt)->formattype = FORMAT_VideoInfo;
    (*pmt)->bTemporalCompression = FALSE;
    (*pmt)->bFixedSizeSamples= FALSE;
    (*pmt)->lSampleSize = pvi->bmiHeader.biSizeImage;
    (*pmt)->cbFormat = sizeof(VIDEOINFOHEADER);
    
    DECLARE_PTR(VIDEO_STREAM_CONFIG_CAPS, pvscc, pSCC);
    
    pvscc->guid = FORMAT_VideoInfo;
    pvscc->VideoStandard = AnalogVideo_None;
    pvscc->InputSize.cx = 1280;
    pvscc->InputSize.cy = 720;
    pvscc->MinCroppingSize.cx = 160;
    pvscc->MinCroppingSize.cy = 90;
    pvscc->MaxCroppingSize.cx = 1280;
    pvscc->MaxCroppingSize.cy = 720;
    pvscc->CropGranularityX = 160;
    pvscc->CropGranularityY = 90;
    pvscc->CropAlignX = 0;
    pvscc->CropAlignY = 0;

    pvscc->MinOutputSize.cx = 160;
    pvscc->MinOutputSize.cy = 90;
    pvscc->MaxOutputSize.cx = 1280;
    pvscc->MaxOutputSize.cy = 720;
    pvscc->OutputGranularityX = 0;
    pvscc->OutputGranularityY = 0;
    pvscc->StretchTapsX = 0;
    pvscc->StretchTapsY = 0;
    pvscc->ShrinkTapsX = 0;
    pvscc->ShrinkTapsY = 0;
    pvscc->MinFrameInterval = 625000;   //50 fps
    pvscc->MaxFrameInterval = 50000000; // 0.2 fps
    pvscc->MinBitsPerSecond = (160 * 90 * 3 * 8) / 5;
    pvscc->MaxBitsPerSecond = 1280 * 720 * 3 * 8 * 16;

    return S_OK;
}

//////////////////////////////////////////////////////////////////////////
// IKsPropertySet
//////////////////////////////////////////////////////////////////////////


HRESULT CVCamStream::Set(REFGUID guidPropSet, DWORD dwID, void *pInstanceData, 
                        DWORD cbInstanceData, void *pPropData, DWORD cbPropData)
{// Set: Cannot set any properties.
    return E_NOTIMPL;
}

// Get: Return the pin category (our only property). 
HRESULT CVCamStream::Get(
    REFGUID guidPropSet,   // Which property set.
    DWORD dwPropID,        // Which property in that set.
    void *pInstanceData,   // Instance data (ignore).
    DWORD cbInstanceData,  // Size of the instance data (ignore).
    void *pPropData,       // Buffer to receive the property data.
    DWORD cbPropData,      // Size of the buffer.
    DWORD *pcbReturned     // Return the size of the property.
)
{
    if (guidPropSet != AMPROPSETID_Pin)             return E_PROP_SET_UNSUPPORTED;
    if (dwPropID != AMPROPERTY_PIN_CATEGORY)        return E_PROP_ID_UNSUPPORTED;
    if (pPropData == NULL && pcbReturned == NULL)   return E_POINTER;
    
    if (pcbReturned) *pcbReturned = sizeof(GUID);
    if (pPropData == NULL)          return S_OK; // Caller just wants to know the size. 
    if (cbPropData < sizeof(GUID))  return E_UNEXPECTED;// The buffer is too small.
        
    *(GUID *)pPropData = PIN_CATEGORY_CAPTURE;
    return S_OK;
}

// QuerySupported: Query whether the pin supports the specified property.
HRESULT CVCamStream::QuerySupported(REFGUID guidPropSet, DWORD dwPropID, DWORD *pTypeSupport)
{
    if (guidPropSet != AMPROPSETID_Pin) return E_PROP_SET_UNSUPPORTED;
    if (dwPropID != AMPROPERTY_PIN_CATEGORY) return E_PROP_ID_UNSUPPORTED;
    // We support getting this property, but not setting it.
    if (pTypeSupport) *pTypeSupport = KSPROPERTY_SUPPORT_GET; 
    return S_OK;
}

string CVCamStream::getStream() {
    string enabled, path;
    ifstream fileIn(configPath);
    getline(fileIn, enabled);
    getline(fileIn, path);
    fileIn.close();
    if (stoi(enabled) == 0) { streamEnabled = false; }
    else { streamEnabled = true; }
    return path;
}

void CVCamStream::GetVideoInfoParameters(
    const VIDEOINFOHEADER* pvih, // Pointer to the format header. 
    DWORD* pdwWidth,         // Returns the width in pixels. 
    DWORD* pdwHeight,        // Returns the height in pixels. 
    LONG* plStrideInBytes,  // Add this to a row to get the new row down 
    bool bYuv                // Is this a YUV format? (true = YUV, false = RGB) 
) {

    LONG lStride;


    //  For 'normal' formats, biWidth is in pixels.  
    //  Expand to bytes and round up to a multiple of 4. 
    if (pvih->bmiHeader.biBitCount != 0 &&
        0 == (7 & pvih->bmiHeader.biBitCount))
    {
        lStride = (pvih->bmiHeader.biWidth * (pvih->bmiHeader.biBitCount / 8) + 3) & ~3;
    }
    else   // Otherwise, biWidth is in bytes. 
    {
        lStride = pvih->bmiHeader.biWidth;
    }

    *pdwWidth = (DWORD)pvih->bmiHeader.biWidth;
    *pdwHeight = (DWORD)(abs(pvih->bmiHeader.biHeight));

    if (pvih->bmiHeader.biHeight < 0 || bYuv)   // Top-down bitmap.  
    {
        *plStrideInBytes = lStride; // Stride goes "down" 
    }
    else        // Bottom-up bitmap 
    {
        *plStrideInBytes = -lStride;    // Stride goes "up" 
    }
}
