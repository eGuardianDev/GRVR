using Avalonia;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Software
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            SetupConsole();
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);

        }

        private static void SetupConsole()
        {
            AllocConsole();
            var handle = GetConsoleWindow();
            [DllImport("kernel32.dll")]
            static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll")]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            const int SW_HIDE = 0;
           // const int SW_SHOW = 5;
            // Hide

            // Show
            Console.WriteLine("Starting Console");
            //Thread.Sleep(1000);
            ShowWindow(handle, 5);

            // Show
           // ShowWindow(handle, SW_SHOW);


            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool AllocConsole();

        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
