using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace TestBackgroundWindow
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private static void SendWpfWindowBack(Window window)
        {
            var hWnd = new WindowInteropHelper(window).Handle;
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            SendWpfWindowBack(this);
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            SendWpfWindowBack(this);
        }
    }
}