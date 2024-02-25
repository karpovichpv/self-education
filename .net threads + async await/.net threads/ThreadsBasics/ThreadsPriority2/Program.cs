
class PriorityTest
{
    public bool Stop = false;

    public void Method()
    {
        Console.WriteLine(
            $"Thread {Thread.CurrentThread.ManagedThreadId}" +
            $" with priority {Thread.CurrentThread.Priority}" +
            $"began work");

        long count = 0;

        while (!Stop)
            count++;

        Console.WriteLine(
            $"Thread {Thread.CurrentThread.ManagedThreadId} with priority " +
            $"{Thread.CurrentThread.Priority} finished. Count = {count:0}");
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

        int threadNumber = 4;
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