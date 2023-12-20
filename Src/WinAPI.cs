using System.Runtime.InteropServices;

namespace RT.Keyboard;

/// <summary>WinAPI function wrappers</summary>
static class WinAPI
{
    // Low-Level Keyboard Constants
    public const int HC_ACTION = 0;
    public const int LLKHF_EXTENDED = 0x1;
    public const int LLKHF_INJECTED = 0x10;
    public const int LLKHF_ALTDOWN = 0x20;
    public const int LLKHF_UP = 0x80;
    public const int WH_KEYBOARD_LL = 13;

    public const int WM_KEYDOWN = 0x100;
    public const int WM_KEYUP = 0x101;
    public const int WM_SYSKEYDOWN = 0x104;
    public const int WM_SYSKEYUP = 0x105;

    // For keybd_event
    public const int KEYEVENTF_KEYUP = 0x2;
    public const int KEYEVENTF_UNICODE = 0x4;

    // For mouse_event
    public const int MOUSEEVENTF_LEFTDOWN = 0x02;
    public const int MOUSEEVENTF_LEFTUP = 0x04;
    public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
    public const int MOUSEEVENTF_RIGHTUP = 0x10;
    public const int MOUSEEVENTF_ABSOLUTE = 0x8000;

    // For the 'type' field in the INPUT struct
    public const int INPUT_MOUSE = 0;
    public const int INPUT_KEYBOARD = 1;
    public const int INPUT_HARDWARE = 2;

    public struct KeyboardHookStruct
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    public struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    public struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)]
        public MOUSEINPUT Mouse;
        [FieldOffset(0)]
        public KEYBDINPUT Keyboard;
        [FieldOffset(0)]
        public HARDWAREINPUT Hardware;
    }

    public struct INPUT
    {
        public int Type;
        public MOUSEKEYBDHARDWAREINPUT SpecificInput;
    }

    /// <summary>Defines the callback type for a keyboard hook procedure.</summary>
    public delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

    [DllImport("user32.dll")]
    public static extern short GetKeyState(int vKey);

    /// <summary>
    ///     Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null</summary>
    /// <param name="idHook">
    ///     The id of the event you want to hook</param>
    /// <param name="callback">
    ///     The callback.</param>
    /// <param name="hInstance">
    ///     The handle you want to attach the event to, can be null</param>
    /// <param name="threadId">
    ///     The thread you want to attach the event to, can be null</param>
    /// <returns>
    ///     a handle to the desired hook</returns>
    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowsHookEx(int idHook, WinAPI.KeyboardHookProc callback, IntPtr hInstance, uint threadId);

    /// <summary>
    ///     Unhooks the windows hook.</summary>
    /// <param name="hInstance">
    ///     The hook handle that was returned from SetWindowsHookEx</param>
    /// <returns>
    ///     True if successful, false otherwise</returns>
    [DllImport("user32.dll")]
    public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

    /// <summary>
    ///     Calls the next hook.</summary>
    /// <param name="idHook">
    ///     The hook id</param>
    /// <param name="nCode">
    ///     The hook code</param>
    /// <param name="wParam">
    ///     The wparam.</param>
    /// <param name="lParam">
    ///     The lparam.</param>
    /// <returns/>
    [DllImport("user32.dll")]
    public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

    /// <summary>
    ///     Loads the library.</summary>
    /// <param name="lpFileName">
    ///     Name of the library</param>
    /// <returns>
    ///     A handle to the library</returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
}
