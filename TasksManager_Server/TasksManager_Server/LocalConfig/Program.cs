using CommonDll.Helps;
using CommonDll.Structs.F_ProgressOfUpdate;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using TasksManager_Server.Methods.MethodsProcess;

namespace TasksManager_Server
{
    class Program
    {
        public static ProgressOfUpdateAtStructAttribute Progress { get; set; }
        static void Main(string[] args)
        {
            //Helper.CheckDirectories();
            var CurrentDomain = AppDomain.CurrentDomain.BaseDirectory;
            var GetDirectoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine("AppDomain.CurrentDomain.BaseDirectory: {0}", CurrentDomain);
            Console.WriteLine("Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location): {0}", GetDirectoryName);
            ManualResetEvent manualResetEvent = new ManualResetEvent(true);
            manualResetEvent.WaitOne();

            ConsoleWriteLine.WriteInConsole(null, null, null, "Project started", ConsoleColor.White);
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            Process.GetCurrentProcess().PriorityBoostEnabled = true;

            Helper_WINWORD.Clear();
            Helper_EXCEL.Clear();
            ClearNormal();

            ProgressOfUpdateAtStructAttribute Standart = (ProgressOfUpdateAtStructAttribute)ParentMethods.GetStandart().Clone_Full();
            ValueForClient.DeserializeConfig(new TimeSpan(0, 5, 0)); // Deserialization data'es

            if (ValueForClient.ReadyStructure.ProgressOfUpdate is null ? true : ValueForClient.ReadyStructure.ProgressOfUpdate.List_ProgressOfUpdateAtStructAttribute.Count != 1)
            {
                Progress = Standart;
                ValueForClient.ReadyStructure.ProgressOfUpdate = new ProgressOfUpdate() { List_ProgressOfUpdateAtStructAttribute = new List<ProgressOfUpdateAtStructAttribute>() { Progress } };
            }
            else
            {
                Progress = ValueForClient.ReadyStructure.ProgressOfUpdate.List_ProgressOfUpdateAtStructAttribute.FirstOrDefault();
                Progress.NonSerializedConfig = Standart.NonSerializedConfig;
            }

            Progress.SetName("Запуск проекта");
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                ValueForClient.TimerStart(TimeSpan.FromMinutes(2)); // Таймер для GC.Collect();
                LocalSettings.UpdateConfig(Progress);
                LocalSettings.OnLoad(Progress);
            });
            Progress.Start();

            new ManualResetEvent(false).WaitOne(); // block to close project
        }
        static void ClearNormal()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "";
            string FolderWord = path + "\\Microsoft\\Word";
            string FolderShabloni = path + "\\Microsoft\\Шаблоны";

            if (Directory.Exists(FolderWord))
            {
                Helper.RemoveBackupFiles(FolderWord, "*.doc");
                Helper.RemoveBackupFiles(FolderWord, "*.docx");
                Helper.RemoveBackupFiles(FolderWord, "*.dotm");
            }

            if (Directory.Exists(FolderShabloni))
            {
                Helper.RemoveBackupFiles(FolderShabloni, "*.doc");
                Helper.RemoveBackupFiles(FolderShabloni, "*.docx");
                Helper.RemoveBackupFiles(FolderShabloni, "*.dotm");
            }
        }
        ~Program()
        {
            Helper_WINWORD.Clear();
            Helper_EXCEL.Clear();
            ClearNormal();
        }
    }
}
