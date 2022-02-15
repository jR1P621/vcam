# Thegoldteam

**GOAL:  Create program with ability to use phone as a webcam**

Program will work using a mobile app and a desktop app.  The mobile app interfaces with the device camera and microphone to receive feeds, then packages and streams a/v feed to desktop app via UDP.  Desktop app unpacks feed and streams feed to virtual hardware that can be used by third-party software.

REQUIREMENTS:
- [x] Virtual audio device must be useable by any third-party software - Skype, Zoom, etc.
- [x] Virtual video device must be useable by any third-party software - Skype, Zoom, etc.
- [x] Virtual audio device must function on 32 & 64 bit systems
- [x] Virtual video device must function on 32 & 64 bit systems
- [x] Program must be plug-and-play.  No additional end user configuration.
- [ ] A/V lag must be minimal
- [x] Stream format must be universal; Recognizable regardless of OS
- [x] Desktop application must function on 32 & 64 bit systems
- [ ] Mobile app must persistant notify when service is running
- [x] Desktop application must minimize to systray
- [x] Desktop application must notify when service is running; systray icon

OPTIONAL:
- [ ] Virtual audio device must receive input directly from application
- [ ] Mobile app adjusts theme based on phone theme
- [ ] Mobile app shows camera feed overlay while service is running
- [ ] Program may allow users to change default port numbers
