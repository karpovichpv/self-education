using System;
using System.Runtime.InteropServices;

namespace COMPlayVideo
{
    internal class Program
    {
        [Guid("56a868b1-0ad4-11ce-b03a-0020af0ba770")]
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsDual)]
        interface IPlayVideo
        {
            void Run();
            void Pause();
            void Stop();
            int GetState(int timeout);
            void RenderFile([MarshalAs(UnmanagedType.BStr)] string filename);
            object AddSourceFilter(string filter);
            object FilterCollection { get; }
            object RegFilterCollection { get; }
            void StopWhenReady();
        }

        [Guid("E436EBB3-524F-11CE-9F53-0020AF0BA770")]
        [ComImport]
        class VideoPlayer
        {
        }

        static void Main(string[] args)
        {
            IPlayVideo play = (IPlayVideo)new VideoPlayer();
            play.RenderFile(@"c:\other\1.wmv");
            play.Run();

            Console.ReadLine();
        }
    }
}
