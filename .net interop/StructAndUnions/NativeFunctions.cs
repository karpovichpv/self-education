using System;
using System.Runtime.InteropServices;

namespace StructAndUnions
{
    public enum VirtialKeys : ushort
    {
        VK_0 = 0x30,
        VK_PLUS = 0x6b,
        VK_ENTER = 0xd
    }

    public enum InputTypes
    {
        INPUT_MOUSE = 0,
        INPUT_KEYBOARD = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public VirtialKeys wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct INPUT
    {
        [FieldOffset(0)]
        public InputTypes type;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        //[FieldOffset(4)]
        //HARDWAREINPUT hi;
    }


    static class NativeFunctions
    {
        [DllImport("user32")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32")]
        public static extern uint SendInput(uint count,
[MarshalAs(UnmanagedType.LPArray)] INPUT[] inputs,
int size);
    }

}
