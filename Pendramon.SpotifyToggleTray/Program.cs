using System.Diagnostics;
using System.Runtime.InteropServices;

const int swHide = 0;
const int swRestore = 9;

if (TryGetSpotifyWindowHandle(out var spotifyWindowHandle))
{
    ToggleSpotifyWindow(spotifyWindowHandle);
}
else
{
    // Hacky way to bring spotify up from background without messing with its inner state.
    var spotifyPath = GetSpotifyPath();
    StartSpotify(spotifyPath);
}

return;

bool TryGetSpotifyWindowHandle(out nint windowHandle)
{
    var spotifyProcess = Process.GetProcessesByName("Spotify")
        .FirstOrDefault(p => p.MainWindowHandle != nint.Zero);

    windowHandle = spotifyProcess?.MainWindowHandle ?? nint.Zero;
    return windowHandle != nint.Zero;
}

string GetSpotifyPath()
{
    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Spotify", "Spotify.exe");
}

void StartSpotify(string path)
{
    var spotify = new Process
    {
        StartInfo = new ProcessStartInfo(path)
    };
    spotify.Start();
}

// Toggles the Spotify window visibility: hides or restores based on its current state
void ToggleSpotifyWindow(nint windowHandle)
{
    if (GetForegroundWindow() == windowHandle)
    {
        ShowWindow(windowHandle, swHide);
    } else {
        ShowWindow(windowHandle, swRestore);
        SetForegroundWindow(windowHandle);
    }
}

[DllImport("user32.dll")]
static extern nint GetForegroundWindow();

[DllImport("user32.dll")]
static extern bool SetForegroundWindow(nint hWnd);

[DllImport("user32.dll")]
static extern bool ShowWindow(nint hWnd, int nCmdShow);

