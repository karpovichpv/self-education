using System.Runtime.InteropServices;
using System.Text;

class Program
{
    delegate bool EnumWindowProc(IntPtr hwnd, int lParam);

    [DllImport("user32")]
    static extern bool EnumWindows(EnumWindowProc proc, int lParam);

    [DllImport("user32", CharSet = CharSet.Unicode)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int id);

    [DllImport("user32")]
    static extern bool IsWindowVisible(IntPtr hWnd);

    static void Main(string[] args)
    {
        var sb = new StringBuilder(256);
        EnumWindows((hwnd, param) =>
        {
            if (IsWindowVisible(hwnd))
            {
                GetWindowText(hwnd, sb, 256);
                if (sb.Length > 0)
                {
                    Console.WriteLine("{0,8:X}: {1}", hwnd.ToInt32(), sb.ToString());
                }
            }
            return true;
        }, 0);
    }
}