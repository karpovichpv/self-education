﻿using System;
using System.Threading;

namespace AsyncDelegateEndInvoke
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"The primary thread: Id {Thread.CurrentThread.ManagedThreadId}");

            Action myDelegate = new Action(Method);
            IAsyncResult asyncResult = myDelegate.BeginInvoke(null, null);

            // Asyncronous invoke of the method (with using ThreadPool)
            myDelegate.EndInvoke(asyncResult); // Invoke() - syncronous call

            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(100);
                Console.Write("1 ");
            }

            Console.ReadKey();
        }

        static void Method()
        {
            Console.WriteLine($"Secondary thread: Id {Thread.CurrentThread.ManagedThreadId}");

            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(100);
                Console.Write("2 ");
            }
        }
    }
}
