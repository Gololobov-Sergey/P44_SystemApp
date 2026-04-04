using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp1
{

    enum MessageBoxType
    {
        MB_OK = 0x00000000,
        MB_OKCANCEL = 0x00000001,
        MB_ABORTRETRYIGNORE = 0x00000002,
        MB_YESNOCANCEL = 0x00000003,
        MB_YESNO = 0x00000004,
        MB_RETRYCANCEL = 0x00000005,
        MB_CANCELTRYCONTINUE = 0x00000006
    }

    enum MessageBoxIcon
    {
        MB_ICONHAND = 0x00000010,
        MB_ICONQUESTION = 0x00000020,
        MB_ICONEXCLAMATION = 0x00000030,
        MB_ICONASTERISK = 0x00000040
    }

    enum ShowWindowCommands : int
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_SHOWDEFAULT = 10,
        SW_FORCEMINIMIZE = 11
    }


    enum WindowsMessages : uint
    {
        WM_NULL = 0x0000,
        WM_CREATE = 0x0001,
        WM_DESTROY = 0x0002,
        WM_MOVE = 0x0003,
        WM_SIZE = 0x0005,
        WM_ACTIVATE = 0x0006,
        WM_SETFOCUS = 0x0007,
        WM_KILLFOCUS = 0x0008,
        WM_ENABLE = 0x000A,
        WM_SETREDRAW = 0x000B,
        WM_SETTEXT = 0x000C,
        WM_GETTEXT = 0x000D,
        WM_GETTEXTLENGTH = 0x000E,
        WM_PAINT = 0x000F,
        WM_CLOSE = 0x0010,
        // ... (other messages can be added as needed)
    }

    public class Ext
    {
        [DllImport("User32.dll", EntryPoint = "MessageBoxA")]
        public static extern int MessageBox(IntPtr hWnd, string str, string caption, int type);


        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindowByTitle(IntPtr parentHandle, string windowTitle);


        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        [DllImport("user32.dll")]
        public static extern SendOrPostCallback SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    }
}
