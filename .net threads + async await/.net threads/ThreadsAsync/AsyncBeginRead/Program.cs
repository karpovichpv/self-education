using System;
using System.IO;

namespace AsyncBeginRead
{
    internal class Program
    { // 1: 11: 44
        static void Main(string[] args)
        {
            Stream stream = new FileStream("file.txt", FileMode.Open, FileAccess.Read);

            byte[] array = new byte[stream.Length];

            IAsyncResult asyncResult = stream.BeginRead(array, 0, array.Length, null, null);

            Console.WriteLine("Reading of the file...");

            stream.EndRead(asyncResult);

            foreach (byte item in array)
                Console.WriteLine(item + " ");

            stream.Close();
        }
    }
}
