using System;
using System.Runtime.InteropServices;

namespace COMCsMathLibrary
{
    [Guid("17415B5A-D9D2-4E97-8FBD-9A61C6D00284")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICalculator
    {
        int Multiply(int a, int b);
    }

    [Guid("AE2F0526-105C-4690-85BE-470FD447C93D")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Calculator : ICalculator
    {
        public int Multiply(int a, int b)
            => a * b;
    }
}
