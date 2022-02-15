using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LibVLCSharp;
using LibVLCSharp.Shared;
using LibVLCSharp.Shared.Structures;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Net.NetworkInformation;

namespace DesktopApp
{
    public partial class AppUI : Form
    {

        //
        
        string configPath = Environment.GetEnvironmentVariable("APPDATA") + "\\camApp\\listener.dat";
        IPAddress ipAddress;
        bool running = false;
        string path;
        int deviceNum = 0;
        LibVLC _libvlc;
        AudioOutputDevice[] audioDevices;
        //AudioOutputDescription[] audioDevices;
        string streamType = "rtsp";
        string streamSuffix = "/h264_pcm.sdp";

        public AppUI()
        {

            InitializeComponent();
            LibVLCSharp.Shared.Core.Initialize();
            _libvlc = new LibVLC();
            ipAddress = getIPAddress();
            labelIP.Text = ipAddress.ToString();
            setStreamPath();
        }

        private void playAudio()
        {
            var url = path;
            if (videoView1.MediaPlayer != null){ videoView1.MediaPlayer.Dispose();}
            videoView1.MediaPlayer = new MediaPlayer(new Media(_libvlc, url, FromType.FromLocation)) { EnableHardwareDecoding = false };
            foreach (AudioOutputDevice d in videoView1.MediaPlayer.AudioOutputDeviceEnum)
            {
                if (d.Description.Equals(audioDevices[deviceNum].Description))
                {
                    videoView1.MediaPlayer.SetOutputDevice(d.DeviceIdentifier);
                }
            }
            videoView1.MediaPlayer.NetworkCaching = 500;
            videoView1.MediaPlayer.Play();
            notifyIcon1.Icon = Properties.Resources.icon16Y;
            Thread iconListener = new Thread(iconListenThread);
            iconListener.Start();
        }

        private void stopAudio()
        {
            videoView1.MediaPlayer.Stop();
            videoView1.MediaPlayer.Dispose();
        }

        private int setStreamPath()
        {
            try
            {
                streamType = textBoxType.Text;
                streamSuffix = textBoxSuffix.Text;
                path = streamType + "://" + textBox1.Text + ":" + textBoxPort.Text + "/" + streamSuffix;
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("APPDATA") + "\\camApp");
                if (running)
                    File.WriteAllText(configPath,"1\n");
                else
                    File.WriteAllText(configPath,"0\n");
                File.AppendAllText(configPath, path);
            }
            catch
            {
                return 1;
            }
            return 0;
        }

        private IPAddress getIPAddress()
        {
            foreach(var i in NetworkInterface.GetAllNetworkInterfaces()){
                if (i.OperationalStatus == OperationalStatus.Up && i.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    foreach(UnicastIPAddressInformation j in i.GetIPProperties().UnicastAddresses)
                    {
                        if(j.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return j.Address;
                        }
                    }
                }
            }

            return null;
        }

        private class Taskbar {
            public char position;
            public int size; 

            public Taskbar()
            {
                if (Screen.PrimaryScreen.WorkingArea.Width == Screen.PrimaryScreen.Bounds.Width)
                {
                    if (Screen.PrimaryScreen.WorkingArea.Top > 0)
                    {
                        position = 'T';
                        size = Screen.PrimaryScreen.WorkingArea.Top;
                    }
                    else
                    {
                        position = 'B';
                        size = Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Bottom;
                    }
                }
                else
                {
                    if (Screen.PrimaryScreen.WorkingArea.Left > 0)
                    {
                        position = 'L';
                        size = Screen.PrimaryScreen.WorkingArea.Left;
                    }
                    else
                    {
                        position = 'R';
                        size = Screen.PrimaryScreen.Bounds.Width - Screen.PrimaryScreen.WorkingArea.Width;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Visible = false;
            notifyIcon1.Visible = true;
            populateAudioDevices();
        }

        private void populateAudioDevices()
        {
            audioDevices = _libvlc.AudioOutputDevices("mmdevice");
            //audioDevices = _libvlc.AudioOutputs;

            for (int i = 0; i < audioDevices.Length; i++)
                {
                    comboBoxSpeakers.Items.Add(audioDevices[i].Description);
                    if (audioDevices[i].Description.Contains("CABLE Input"))
                    {
                        deviceNum = i;
                    }
                }
            if (deviceNum >= 0) { comboBoxSpeakers.SelectedIndex = deviceNum; }
        }

        private void setLocation()
        {
            if (WindowState == FormWindowState.Normal)
            {
                Taskbar currentTaskbar = new Taskbar();
                int[] pos = new int[2];
                switch (currentTaskbar.position)
                {
                    case 'B':
                        pos[0] = Cursor.Position.X - (Width / 2);
                        pos[1] = Screen.PrimaryScreen.WorkingArea.Bottom - Height;
                        break;
                    case 'T':
                        pos[0] = Cursor.Position.X - (Width / 2);
                        pos[1] = Screen.PrimaryScreen.WorkingArea.Top;
                        break;
                    case 'L':
                        pos[0] = Screen.PrimaryScreen.WorkingArea.Left;
                        pos[1] = Cursor.Position.Y - (Height / 2);
                        break;
                    case 'R':
                        pos[0] = Screen.PrimaryScreen.WorkingArea.Right - Width;
                        pos[1] = Cursor.Position.Y - (Height / 2);
                        break;
                    default:
                        break;
                }
                pos[0] = Math.Min(Screen.PrimaryScreen.WorkingArea.Right - Width, pos[0]);
                pos[0] = Math.Max(Screen.PrimaryScreen.WorkingArea.Left, pos[0]);
                pos[1] = Math.Min(Screen.PrimaryScreen.WorkingArea.Bottom - Height, pos[1]);
                pos[1] = Math.Max(Screen.PrimaryScreen.WorkingArea.Top, pos[1]);

                Location = new Point(pos[0], pos[1]);
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            Visible = false;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized || Visible == false)
            {
               
                Visible = true;
                WindowState = FormWindowState.Normal;
                setLocation();
            }
            else
            {
                WindowState = FormWindowState.Minimized;
                Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (running) { running = !running; }
            setStreamPath();
            Dispose();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            running = !running;
            setStreamPath();
            if (running)
            {
                btnListen.Text = "Stop";
                textBox1.Enabled = false;
                textBoxPort.Enabled = false;
                textBoxSuffix.Enabled = false;
                textBoxType.Enabled = false;
                comboBoxSpeakers.Enabled = false;
                deviceNum = comboBoxSpeakers.SelectedIndex;
                playAudio();
            }
            else
            {
                btnListen.Text = "Start";
                textBox1.Enabled = true;
                textBoxPort.Enabled = true;
                textBoxSuffix.Enabled = true;
                textBoxType.Enabled = true;
                comboBoxSpeakers.Enabled = true;
                notifyIcon1.Icon = Properties.Resources.icon16R;
                stopAudio();
            }
        }
        private void iconListenThread()
        {
            while (running)
            {
                if (videoView1.MediaPlayer.IsPlaying && notifyIcon1.Icon != Properties.Resources.icon16G)
                {
                    notifyIcon1.Icon = Properties.Resources.icon16G;
                }
                if (!videoView1.MediaPlayer.IsPlaying && notifyIcon1.Icon == Properties.Resources.icon16G)
                {
                    notifyIcon1.Icon = Properties.Resources.icon16R;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
