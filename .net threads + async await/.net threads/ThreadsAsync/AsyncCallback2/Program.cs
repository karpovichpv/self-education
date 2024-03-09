public class Program
{
    public static void Main()
    {
        Console.WriteLine($"The primary thread: ID {Thread.CurrentThread.ManagedThreadId}");
        Action action = new Action(Method);
        AsyncCallback callback = new AsyncCallback(CallBack);

        action.BeginInvoke(callback, "Hello world!");
        Console.WriteLine("The primary thread continues working");

        Console.ReadLine();
    }

    private static void Method()
    {
        Console.WriteLine($"\nThe secondary thread: Id {Thread.CurrentThread.ManagedThreadId}");

        for (int i = 0; i < 80; i++)
        {
            Thread.Sleep(20);
            Console.Write(".");
        }
    }

    private static void CallBack(IAsyncResult asyncResult)
    {
        Console.WriteLine($"Callback method: Thread Id {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine("Information is linked to the asynchronous operation - " + asyncResult.AsyncState);
    }
}