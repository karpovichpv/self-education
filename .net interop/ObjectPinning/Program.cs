
using System.Runtime.InteropServices;

class Program
{
    [DllImport("SampleNativeLib")]
    static extern int SetData([MarshalAs(UnmanagedType.LPArray)] int[] data);

    [DllImport("SampleNativeLib")]
    static extern int DoCalc();

    static void Main(string[] args)
    {
        var data = Enumerable.Range(0, 10).Select(i => i + 1).ToArray();
        GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        Console.WriteLine(SetData(data));
        Console.WriteLine(DoCalc());
        Console.ReadLine();
        GC.Collect();
        Console.WriteLine(DoCalc());
        handle.Free();
    }
}
