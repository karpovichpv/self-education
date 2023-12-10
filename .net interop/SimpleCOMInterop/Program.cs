using MathServerLib;
using System.Runtime.InteropServices;

namespace SimpleCOMInterop
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                ICalculator calc = new Calculator();
                int result = calc.Add(3, 4);
                Console.WriteLine($"result = {result}");
            }
            catch (COMException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}