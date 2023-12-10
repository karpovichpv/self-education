
using System.Runtime.InteropServices;

class Program
{
    [DllImport("kernel32", EntryPoint = "Sleep")]
    static extern void DoNothing(uint msec);

    [DllImport("kernel32", ExactSpelling = true)]
    static extern IntPtr CreateJobObjectW(IntPtr securityAttributes, string name);

    [DllImport("kernel32")]
    static extern bool CloseHandle(IntPtr handle);

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("waiting for a while ...");
            DoNothing(2000);

            var newJob = CreateJobObjectW(IntPtr.Zero, "myjob");
            Console.WriteLine("job handle: {0}", newJob);

            CloseHandle(newJob);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}