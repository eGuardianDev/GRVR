using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Software.Classes
{
    abstract class Logger
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private static string data = "";
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        public static bool DebugMode = true;
        public static void ShowHide()
        {
            var handle = GetConsoleWindow();
          
            ShowWindow(handle, SW_SHOW);

            Info("IF YOU CLOSE THE CONSOLE, THE WHOLE APPLICATION WILL CLOSE!");

        }
        public static void Log(string information)
        {
            if (!DebugMode) return;
            data = $"[Log] {information}";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($" ({DateTime.Now:HH:mm:ss}) ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("[Log] ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {information}");
        }
        public static void Info(string information)
        {
            data = $"[Info] {information}";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($" ({DateTime.Now:HH:mm:ss}) ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write ("[Info] ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {information}");
        }
        public static void Warn(string information)
        {
            data = $"[Warning] {information}";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($" ({DateTime.Now:HH:mm:ss}) ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[Warning] ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {information}");
        }
        public static void Error(string information)
        {
            data = $"[Error] {information}";
        }
    }
}
