
using CalculatorSvrLib;

public class Program
{
    public static void Main(string[] args)
    {
        var calc = new Calculator();
        Console.WriteLine(calc.Add(3, 4));

        Console.WriteLine("Before setting to null");
        Console.ReadLine();
        calc = null;

        Console.WriteLine("Before calling GC.Collect");
        Console.ReadLine();
        GC.Collect();

        Console.WriteLine("Done");
        Console.ReadLine();
    }
}