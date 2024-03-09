using System;
using System.IO;

namespace AsyncBeginRead2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stream stream = new FileStream("file.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            byte[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

            stream.BeginWrite(array, 0, array.Length, new AsyncCallback(CleanUp), stream);

            Console.ReadKey();
        }

        private static void CleanUp(IAsyncResult ar)
        {
            Console.WriteLine("File has been written");
            Stream stream = ar.AsyncState as Stream;

            if (stream != null)
                stream.Close();
        }
    }
}
