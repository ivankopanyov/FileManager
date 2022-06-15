using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FileManager.Utils
{
    /// <summary>
    /// Класс для взаимодействия с операционной системой.
    /// </summary>
    public static class API
    {
        public const uint MAX_PATH = 255;
        public const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetShortPathName(
           [MarshalAs(UnmanagedType.LPTStr)]
           string lpszLongPath,
           [MarshalAs(UnmanagedType.LPTStr)]
           StringBuilder lpszShortPath,
           uint cchBuffer);
    }
}
