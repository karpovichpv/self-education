
public class Program
{
    public static void Procedure()
    {
        Console.ForegroundColor = ConsoleColor.Green;

        while (true)
        {
            try
            {
                //  while (true)
                {
                    Thread.Sleep(10);
                    Console.WriteLine("");
                }
            }
            catch (ThreadAbortException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                // Attempt "to swallow" an exception and continue execute in this loop
                // Return into the loop and continue to write counter

                for (int i = 0; i < 160; i++)
                {
                    Thread.Sleep(20);
                    Console.Write(".");
                }

                Console.ForegroundColor = ConsoleColor.Green;

                // If do not execute Thread.ResetAbort() - the exception won't repeat
                Thread.ResetAbort();
            }

        }
        Console.WriteLine("======= DOES NOT EXECUTE ==========");

    }

    public static void Main()
    {
        Thread thread = new(new ThreadStart(Procedure));

        thread.Start();

        Thread.Sleep(2000);

        thread.Abort();

        thread.Join();

        Console.ForegroundColor = ConsoleColor.White;

        while (true)
        {
            Thread.Sleep(20);
            Console.WriteLine("-");
        }
    }
}