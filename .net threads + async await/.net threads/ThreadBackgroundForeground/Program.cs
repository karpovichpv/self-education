
public class Program
{
    public static void Main()
    {
        var thread = new Thread(Procedure);
        //thread.IsBackground = true;
        thread.Start();

        Thread.Sleep(500);

        Console.WriteLine("\n Finishing of the primary thread");
    }

    private static void Procedure()
    {
        for (int i = 0; i < 500; i++)
        {
            Thread.Sleep(10);
            Console.Write(".");
        }

        Console.WriteLine("\n Finishing of the secondary thread");
    }
}