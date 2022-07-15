using System.Runtime.InteropServices;

namespace TribalWarsBot
{
    class Program
    {
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
        private static bool keepRunning = true;
        // A delegate type to be used as the handler routine 
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);
        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
        // Graceful exit resource cleanup
        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            //File.AppendAllText(@"Log.txt", "closed");
            keepRunning = false;
            // Close the open web browser
            Browser.CloseDriverInstance();
            return true;
        }
        // Entry point
        public static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            // Application gracefull exit handler
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            // Application main body entry point
            Console.WriteLine("Tribal Wars bot started");
            ScavengeModule scavengeModule = new();
            do
            {
                Task.Delay(5000).Wait();
                scavengeModule.Refresh();
            } while (keepRunning);
            // Close the open web browser
            Browser.CloseDriverInstance();
            Console.WriteLine("Tribal Wars bot exited");
        }
    }
}
