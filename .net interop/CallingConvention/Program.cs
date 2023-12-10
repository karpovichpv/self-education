using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

class Program
{
    [DllImport("msvcrt", CallingConvention = CallingConvention.Cdecl)]
    extern static int sprintf(StringBuilder buffer, string format, int id);

    static void Main(string[] args)
    {
        var buffer = new StringBuilder(128);

        sprintf(buffer, "Process %d is using sprintf!", Process.GetCurrentProcess().Id);

        Console.WriteLine(buffer.ToString());
        Console.ReadLine();
    }
}