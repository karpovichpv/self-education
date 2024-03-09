using System;
using System.Collections.Generic;

namespace AsyncResultImplementation
{
    internal class Program
    {
        private static FibonacciCalculator _calculator = new FibonacciCalculator();
        private static List<int> _fibonacciSequence;

        static void Main(string[] args)
        {
            #region Synchronous call

            //_fibonacciSequence = _calculator.Invoke(40);
            //foreach (int item in _fibonacciSequence)
            //{
            //    Console.Write($"{item} ");
            //}

            #endregion

            #region Asyncronous call
            //IAsyncResult asyncResult = _calculator.BeginInvoke(40, null, null);
            //Console.WriteLine("My work!!!");
            //_fibonacciSequence = _calculator.EndInvoke(asyncResult);

            //foreach (int item in _fibonacciSequence)
            //{
            //    Console.Write($"{item} ");
            //}

            #endregion

            #region Asyncronous call with using the Callback method

            IAsyncResult asyncResult = _calculator.BeginInvoke(40, Callback, _calculator);

            #endregion

            Console.ReadKey();
        }

        private static void Callback(IAsyncResult asyncResult)
        {

            FibonacciCalculator calculator = (FibonacciCalculator)asyncResult.AsyncState;

            List<int> fibonacciSequence = calculator.EndInvoke(asyncResult);

            foreach (int item in fibonacciSequence)
            {
                Console.Write($"{item} ");
            }
        }
    }
}
