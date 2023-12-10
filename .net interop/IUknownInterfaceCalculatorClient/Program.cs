using CalculatorSvrLib;

class Program
{
    public static void Main(string[] args)
    {
        ISimpleCalculator calc = new Calculator();
        Console.WriteLine(calc.Add(3, 4));

        var trig = calc as ITrigCalculator;
        if (trig != null)
        {
            Console.WriteLine(trig.Sin(30));
            trig.Degrees = true;
            Console.WriteLine(trig.Sin(30));
        }
    }
}