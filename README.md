DenonCMD
========

DenonCMD is a C# app created to control Denon Network AV Receivers via commandline. I created it to control my Denon via a Rainmeter Skin

How to use
----------

After building the following commands are available

    DenonCMD <hostname> <command> [command...]

hostname is the IP or hostname of your Denon AVR
command can be one of the following:

 * powerOn
   powers on the device
 * powerOff
   powers off the device
 * power
   detects current power state and switches it on or off accordingly
 * muteOn
   enabled mute
 * muteOff
   disabled mute
 * mute
   detects current mute state and switches it on or off accordingly
 * volUp
   increases the volume
 * volDown
   decreases the volume
 * vol-x
   set volume to -x *dB*
 * inputTuner
   sets "Tuner" as the current input
 * tunerPresetUp
   selects the next tuner preset
 * tunerPresetDown
   selects the previous tuner preset
 * netPlayPause
   pauses and plays the current track in network input
 * netPrevTrack
   plays next track on network input
 * netNextTrack
   play previous track on network input

You can send multiple commands in one by adding them to the command line. So if you want to increase the volume by 2 in one command and your receiver
normaly increases volume by 0.5 with one command, just issue

    DenonCMD <hostname> volUp volUp volUp volUp

Additional Information
----------------------

The app generates errors if you don't use it right, but it won't show it to you in a console window. I disabled the console window so I don't 
get a new window everytime I use my rainmeter skin. The debug output is readable with apps listening to the Win32 debug output (like
[DebugView](http://technet.microsoft.com/en-us/sysinternals/bb896647.aspx) from Sysinternals).

Forks and pull requests are welcome, if you need help adding a command, just tell me what you need.
