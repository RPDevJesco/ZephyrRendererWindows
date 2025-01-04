using System.Runtime.InteropServices;

namespace ZephyrRenderer.Platform
{
    internal static class Win32
    {
        public const int CS_HREDRAW = 0x0002;
        public const int CS_VREDRAW = 0x0001;
        public const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
        public const int SW_SHOW = 5;
        public const int WM_CLOSE = 0x0010;
        public const int WM_DESTROY = 0x0002;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const uint BI_RGB = 0;
        public const uint DIB_RGB_COLORS = 0;
        public const int SRCCOPY = 0x00CC0020;
        public const int IDC_ARROW = 32512;
        public const int PM_REMOVE = 0x0001;
        public const int CW_USEDEFAULT = unchecked((int)0x80000000);
        
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(int color);

        [DllImport("user32.dll")]
        public static extern int FillRect(IntPtr hdc, [In] ref RECT lprc, IntPtr hbr);

        [DllImport("gdi32.dll")]
        public static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);

        [DllImport("gdi32.dll")]
        public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern ushort RegisterClassEx(ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
            uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle,
            int x, int y, int nWidth, int nHeight,
            IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern bool DispatchMessage(ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern int SetDIBitsToDevice(
            IntPtr hdc, int xDest, int yDest, uint dwWidth, uint dwHeight,
            int xSrc, int ySrc, uint uStartScan, uint cScanLines,
            IntPtr lpvBits, ref BITMAPINFO lpbmi, uint fuColorUse);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(
            IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
            IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
    }
}