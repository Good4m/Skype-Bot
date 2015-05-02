using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace SkypeBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int RegisterWindowMessage(string lpString);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);
        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        const int WM_SETTEXT  = 0x000C;
        const int WM_GETTEXT  = 0x000D;
        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP   = 0x0101;
        const uint WM_CHAR    = 0x0102;
        const int VK_RETURN   = 0x0D;

        const string windowName = "SKYPE USERNAME HERE";

        public MainWindow()
        {
            InitializeComponent();

            // Get main window
            IntPtr hWnd = FindChat();
            if (hWnd == IntPtr.Zero) return;

            // Go one level deeper
            IntPtr control1hwnd = IntPtr.Zero;
            control1hwnd = FindWindowEx(hWnd, control1hwnd, "TConversationForm", null);
            if (control1hwnd == IntPtr.Zero)
            {
                MessageBox.Show("Error finding control.");
                return;
            }

            // One more level deeper
            IntPtr control2hwnd = IntPtr.Zero;
            control2hwnd = FindWindowEx(control1hwnd, control2hwnd, "TChatEntryControl", null);
            if (control2hwnd == IntPtr.Zero)
            {
                MessageBox.Show("Error finding control.");
                return;
            }

            // Get chat history window
            IntPtr chathistoryhwnd = IntPtr.Zero;
            chathistoryhwnd = FindWindowEx(control1hwnd, chathistoryhwnd, "TChatContentControl", null);
            if (chathistoryhwnd == IntPtr.Zero)
            {
                MessageBox.Show("Error finding textbox.");
                return;
            }

            // Get text entry window
            IntPtr textboxhwnd = IntPtr.Zero;
            textboxhwnd = FindWindowEx(control2hwnd, textboxhwnd, "TChatRichEdit", null);
            if (textboxhwnd == IntPtr.Zero)
            {
                MessageBox.Show("Error finding textbox.");
                return;
            }

            // Try getting chat history (doesn't work)
            StringBuilder sb = new StringBuilder(655355);
            SendMessage(chathistoryhwnd, WM_GETTEXT, sb.Capacity, sb);
            Console.WriteLine(sb);

            // Send message to text entry window
            for (int i = 0; i < 10; i++)
            {
                // Send msg
                SendMessage(textboxhwnd, WM_SETTEXT, IntPtr.Zero, "THIS IS A TEST MSG");
                // Press enter
                PostMessage(textboxhwnd, WM_KEYDOWN, (IntPtr)VK_RETURN, IntPtr.Zero);
                PostMessage(textboxhwnd, WM_CHAR, (IntPtr)VK_RETURN, IntPtr.Zero);
                SendMessage(textboxhwnd, WM_CHAR, (IntPtr)VK_RETURN, IntPtr.Zero);
                PostMessage(textboxhwnd, WM_KEYUP, (IntPtr)VK_RETURN, IntPtr.Zero);
            }
           
        }

        private IntPtr FindChat()
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(windowName))
                {
                    hWnd = pList.MainWindowHandle;
                    break;
                }
            }

            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Error finding window.");
                return IntPtr.Zero;
            }
            return hWnd;
        }


    }
}
