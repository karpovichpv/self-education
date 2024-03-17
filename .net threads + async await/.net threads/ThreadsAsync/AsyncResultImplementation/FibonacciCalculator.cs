using System;
using System.Collections.Generic;
using System.Threading;

namespace AsyncResultImplementation
{
    class FibonacciCalculator // Task (LongTask)
    {
        private int _count;
        private List<int> _fibonacciSequence = new List<int>();

        // Синхронная версия.
        public List<int> Invoke(int count)
        {
            this._count = count;
            Fibonacci(null);
            return _fibonacciSequence;
        }

        // Асинхронная версия.
        public IAsyncResult BeginInvoke(int count, AsyncCallback callback, object @object)
        {
            this._count = count;
            Message message = new Message(new WaitCallback(Fibonacci), callback, @object);
            AsyncResult asyncResult = new AsyncResult();
            asyncResult.SyncProcessMessage(message);

            return asyncResult;
        }

        public List<int> EndInvoke(IAsyncResult result)
        {
            if (!(result as AsyncResult).isInvokeAsyncCallback)
                result.AsyncWaitHandle.WaitOne();

            return _fibonacciSequence;
        }

        void Fibonacci(object arg)
        {
            Func<int, int> fib = null;
            fib = (x) => x > 1 ? fib(x - 1) + fib(x - 2) : x;

            for (int i = 0; i < _count; ++i)
                _fibonacciSequence.Add(fib.Invoke(i));
        }
    }
}