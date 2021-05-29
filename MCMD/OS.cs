using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MCModDownloader
{
    public static class OS
    {
        public static void OpenURL(String url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start("explorer", url);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start("xdg-open", url);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Process.Start("open", url);
        }
    }
}