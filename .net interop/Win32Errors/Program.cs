using System.Runtime.InteropServices;

class Program
{
    [DllImport("kernel32", EntryPoint = "OpenThread", SetLastError = true)]
    static extern IntPtr OpenThreadInternal(uint access, bool ingeritHandle, int id);

    static IntPtr OpenThread(uint access, int id)
    {
        var handle = OpenThreadInternal(access, false, id);
        if (handle != IntPtr.Zero)
            return handle;

        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        return IntPtr.Zero;
    }

    const int SYNCHRONIZE = 0x00100000;

    static void Main(string[] args)
    {
        Console.WriteLine("Enter thread ID: ");
        int id = int.Parse(Console.ReadLine());
        var hTread = OpenThread(SYNCHRONIZE, id);
        try
        {
            Console.WriteLine($"Opened handle successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
        }
    }
}