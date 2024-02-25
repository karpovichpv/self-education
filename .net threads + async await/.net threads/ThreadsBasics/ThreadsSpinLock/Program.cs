public class SpinLock
{
    // flag [0 - free; 1 - block is busy]
    private int _block;

    // Checking interval
    private int _wait;

    public SpinLock(int wait)
    {
        _wait = wait;
    }

    // Set lock
    public void Enter()
    {
        int result = Interlocked.CompareExchange(ref _block, 1, 0);

        while (result == 1)
        {
            Thread.Sleep(_wait);
            result = Interlocked.CompareExchange(ref _block, 1, 0);
        }
    }

    // Reset lock
    public void Exit()
    {
        Interlocked.Exchange(ref _block, 0);
    }
}

public class SpinLockManager : IDisposable
{
    private SpinLock _block;

    public SpinLockManager(SpinLock block)
    {
        _block = block;
        block.Enter();
    }

    public void Dispose()
    {
        _block.Exit();
    }
}

class Program
{
    private static Random _random = new();
    private static SpinLock _block = new(10);

    private static FileStream _stream = File.Open(
        "log.txt",
        FileMode.Create,
        FileAccess.Write,
        FileShare.None);
    private static StreamWriter _writer = new StreamWriter(_stream);

    private static void Function()
    {
        using (new SpinLockManager(_block))
        {
            _writer.WriteLine($"Thread {Thread.CurrentThread.GetHashCode()} is starting");
            _writer.Flush();
        }

        int time = _random.Next(10, 200);
        Thread.Sleep(time);

        using (new SpinLockManager(_block))
        {
            _writer.WriteLine($"Thread [{Thread.CurrentThread.GetHashCode()}] is finishing");
            _writer.Flush();
        }
    }

    public static void Main()
    {
        Thread[] threads = new Thread[50];

        for (uint i = 0; i < 50; ++i)
        {
            threads[i] = new Thread(Function);
            threads[i].Start();
        }
    }
}

