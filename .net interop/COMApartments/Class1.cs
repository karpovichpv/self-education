using CalculatorSvrLib;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace COMApartments
{
    [Guid("00000020-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMultiQI
    {
    }

    class Program
    {
        static ISimpleCalculator MainCalculator;

        [STAThread]
        public static void Main(string[] args)
        {
            MainCalculator = new Calculator();
            Console.WriteLine($"Main Thread: {Thread.CurrentThread.GetApartmentState()}");
            Console.WriteLine($"Main calc: Proxy?  {IsProxy(MainCalculator)}");

            var th = new Thread(ThreadMethod);
            th.SetApartmentState(ApartmentState.MTA);
            th.Start();
            th.Join();

            th = new Thread(ThreadMethod);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            th.Join();
        }

        private static void ThreadMethod()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} ({Thread.CurrentThread.GetApartmentState()})");
            ISimpleCalculator calculator = new Calculator();
            Console.WriteLine($"Main calc proxy? {IsProxy(MainCalculator)}, local calc proxy? {IsProxy(calculator)}");
        }

        static bool IsProxy(object obj) => obj is IMultiQI;
    }
}
