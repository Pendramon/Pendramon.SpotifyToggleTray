using System.Diagnostics;
using System.Runtime.InteropServices;

const int swHide = 0;
const int swRestore = 9;

var spotifyProcess = Process.GetProcessesByName("Spotify").FirstOrDefault();
var spotifyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Spotify\Spotify.exe");

/*
 * Launch spotify if it's not started and also brings window up if it's hidden.
 * Hacky way to bring spotify out of the tray without messing up its inner state.
*/
if (spotifyProcess is null || spotifyProcess.MainWindowHandle == IntPtr.Zero)
{
    var spotify = new Process()
    {
        StartInfo = new ProcessStartInfo(spotifyPath),
    };
    spotify.Start();
    return;
}

// Makes the spotify window focused or hidden if it already is focused.
if (GetForegroundWindow() == spotifyProcess.MainWindowHandle)
{
    ShowWindow(spotifyProcess.MainWindowHandle, swHide);
} else {
    ShowWindow(spotifyProcess.MainWindowHandle, swRestore);
    SetForegroundWindow(spotifyProcess.MainWindowHandle);
}

return;

[DllImport("user32.dll")]
static extern IntPtr GetForegroundWindow();

[DllImport("user32.dll")]
static extern bool SetForegroundWindow(IntPtr hWnd);

[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);