using System;
using System.Threading;

namespace AsyncBeginWrite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The primary thread has started its work");

            Action work = new Action(Procedure);
            work.BeginInvoke(new AsyncCallback(CallBack), work);

            Thread.Sleep(1000);
            Console.WriteLine("\nThe primary thread has finished its work");

            Console.ReadLine();
        }

        private static void CallBack(IAsyncResult ar)
        {
            Action work = ar.AsyncState as Action;
            if (work != null)
                work.EndInvoke(ar);
        }

        private static void Procedure()
        {
            //Thread.CurrentThread.IsBackground = false;
            Console.WriteLine("The secondary thread has started its work");

            for (int i = 0; i < 240; i++)
            {
                Thread.Sleep(20);
                Console.Write(".");
            }

            Console.WriteLine("\nThe secondary thread has finished its work");
        }
    }
}
