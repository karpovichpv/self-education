class PriorityTest
{
    private bool _loopSwitch;

    public PriorityTest()
    {
        _loopSwitch = true;
    }

    public bool LoopSwitch
    {
        set => _loopSwitch = value;
    }

    public void ThreadMethod()
    {
        long threadCount = 0;

        while (_loopSwitch)
            threadCount++;

        Console.WriteLine(
            $"{Thread.CurrentThread.Name} " +
            $"with {Thread.CurrentThread.Priority} " +
            $"priority has a count = {threadCount:N0} ");
    }
}

public class Program
{
    static void Main()
    {
        PriorityTest priorityTest = new PriorityTest();
        ThreadStart startDelegate = priorityTest.ThreadMethod;

        Thread threadOne = new Thread(startDelegate);
        threadOne.Priority = ThreadPriority.Lowest;
        threadOne.Name = "ThreadOne";

        Thread threadTwo = new Thread(startDelegate);
        threadTwo.Priority = ThreadPriority.Highest;
        threadTwo.Name = "ThreadSecond";

        threadOne.Start();
        threadTwo.Start();

        Thread.Sleep(1000);
        priorityTest.LoopSwitch = false;

        Console.ReadKey();
    }
}