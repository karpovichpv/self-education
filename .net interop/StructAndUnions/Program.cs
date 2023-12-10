using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace StructAndUnions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var calc = Process.GetProcessesByName("notepad")[0];
            var hCalc = calc.MainWindowHandle;

            NativeFunctions.SetForegroundWindow(hCalc);

            INPUT[] inputs = new INPUT[1];
            inputs[0].type = InputTypes.INPUT_KEYBOARD;
            inputs[0].ki.wVk = VirtialKeys.VK_0 + 3;
            NativeFunctions.SendInput(1, inputs, Marshal.SizeOf(inputs[0]));

            Thread.Sleep(2000);

            inputs[0].ki.wVk = VirtialKeys.VK_0 + 4;
            NativeFunctions.SendInput(1, inputs, Marshal.SizeOf(inputs[0]));

            Thread.Sleep(2000);

            inputs[0].ki.wVk = VirtialKeys.VK_ENTER;
            NativeFunctions.SendInput(1, inputs, Marshal.SizeOf(inputs[0]));

            Console.WriteLine("All done!");
        }
    }
}
