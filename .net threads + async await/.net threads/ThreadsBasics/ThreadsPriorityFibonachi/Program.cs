class PriorityTest
{
    public bool Stop = false;

    public void Method()
    {
        Console.WriteLine(
            $"Thread {Thread.CurrentThread.ManagedThreadId}" +
            $" with priority {Thread.CurrentThread.Priority}" +
            $" began work");

        Func<double, double> fib = null;
        fib = (x) => x > 1
        ? fib(x - 1) + fib(x - 2)
        : x;

        for (int i = 0; i < 43; i++)
            fib(i);

        Console.WriteLine(
            $"Thread {Thread.CurrentThread.ManagedThreadId} with priority " +
            $"{Thread.CurrentThread.Priority} finished.");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Press any key...");
        Console.ReadKey();

        Console.WriteLine(
            $"Default priority of " +
            $"the primary thread {Thread.CurrentThread.Priority}");

        PriorityTest priorityTest = new();

        int threadNumber = 3;
        Thread[] threads = new Thread[threadNumber];
        for (int i = 0; i < threadNumber; i++)
            threads[i] = new Thread(priorityTest.Method);

        threads[0].Priority = ThreadPriority.Lowest;

        for (int i = 1; i < threadNumber; i++)
            threads[i].Priority = ThreadPriority.Highest;

        threads[0].Start();

        Thread.Sleep(2000);

        for (int i = 1; i < threadNumber; i++)
            threads[i].Start();

        Thread.Sleep(10000);

        Console.WriteLine("The primary thread woke up and run");

        priorityTest.Stop = true;

        Console.ReadKey();
    }
}
