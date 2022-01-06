using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TearEngine.Engine.Tools
{
    internal class DebugConsole
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public static void Init()
        {
            AllocConsole();
            Console.OpenStandardOutput();
            Console.Title = "Debug";
            Console.WriteLine("Tear Engine debug console\nVersion 0.1");
        }

        public static void Log(object Message)
        {
            Console.WriteLine($"[Info : {DateTime.Now.ToString()}] {Message}");
        }
    }
}
