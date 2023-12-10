using System;
using System.Runtime.InteropServices;

namespace COMDynamicDispatch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            dynamic shell = Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0D43FE01-F093-11CF-8940-00A0C9054228")));

            foreach (var drive in shell.Drives)
            {
                Console.WriteLine(drive.Path);
                if (drive.IsReady)
                {
                    Console.WriteLine($" Free Space: {(long)drive.FreeSpace >> 20}MB");
                }
                else
                {
                    Console.WriteLine("Not Ready");
                }
            }
            Console.ReadLine();
        }
    }
}
