
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BasicInterop
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter process ID: ");
            int pid = int.Parse(Console.ReadLine());
            var process = Process.GetProcessById(pid);
            bool is32bit = Is32BitProcess(process);
            Console.WriteLine($"Process {process.ProcessName} ({pid}) is 32 bit: {is32bit}");
        }

        [DllImport("kernel32")]
        static extern bool IsWow64Process(IntPtr hProcess, out bool wow64);

        [DllImport("kernel32")]
        static extern void GetNativeSystemInfo(out SystemInfo si);

        [StructLayout(LayoutKind.Sequential)]
        struct SystemInfo
        {
            public ushort wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        }

        static bool Is32BitProcess(Process process)
        {
            SystemInfo si;
            GetNativeSystemInfo(out si);
            bool isWow64Process;
            IsWow64Process(process.Handle, out isWow64Process);
            return si.wProcessorArchitecture == 0 || si.wProcessorArchitecture == 9 && isWow64Process;
        }
    }
}