using CommonDll.Client_Server.Client;
using CommonDll.Helps;
using CommonDll.Structs.F_ProgressOfUpdate;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using TasksManager.OtherForm;
using Task = System.Threading.Tasks.Task;
using Timer = System.Timers.Timer;

namespace TasksManager.LocalConfig
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (Thread.CurrentThread.Name is null)
                Thread.CurrentThread.Name = nameof(Main);

            Process.GetCurrentProcess().PriorityBoostEnabled = true;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ValueForClient.TimerStart(TimeSpan.FromMinutes(2)); // Таймер для GC.Collect();    
            ProgressOfUpdateAtStructAttribute.WriteInConsole_Default = false;

            ConsoleWriteLine.WriteInConsole_Default = false;
            Application.Run(new OtherForm.Authorization());
        }
    }
}
