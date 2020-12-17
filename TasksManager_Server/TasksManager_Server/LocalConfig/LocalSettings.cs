
using CommonDll.Client_Server;
using CommonDll.Structs.F_ProgressOfUpdate;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using TasksManager_Server.Methods.MethodsProcess;

namespace TasksManager_Server
{
    public static class LocalSettings
    {
        #region Parameters / Properties
        public static ListenSockets ListenSockets = new ListenSockets();
        public static HR_Methods HR_Methods = new HR_Methods();
        public static Users_Methods Users_Methods = new Users_Methods();
        #endregion

        public static readonly string ApplicationBasePath = DataToBinary.ApplicationBasePath;
        public static string NewBasePath {get;set;}
        public static void UpdateConfig(ProgressOfUpdateAtStructAttribute Parent)
        {
            string CurrDirect = Directory.GetParent(LocalSettings.ApplicationBasePath).Parent.Parent.Parent.Parent.FullName;
            NewBasePath = Path.Combine(CurrDirect, "ForSYSTECH_DB.db");
            Tuple<SQLiteConnection, string> tuple = ConnectToDataBase.TryToConnectToSQLite(NewBasePath);
            ConnectToDataBase.sqlite_conn = tuple.Item1;
        }
        public static void OnLoad(ProgressOfUpdateAtStructAttribute Parent)
        {
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName("Запуск основных участком/модулей");
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                #region ServerMethods
                Thread thread_HR_Methods = new Thread(() =>
                {
                    bool StartConfiguration_AtNow = true;
                    TimeSpan HowOftenCheck =
                    //default; 
                    TimeSpan.FromMinutes(1);
                    TimeSpan Interval = new TimeSpan(1, 0, 0);
                    HR_Methods.SetConfigAndStart(ThreadPriority.BelowNormal, LocalSettings.HR_Methods.GetType().AssemblyQualifiedName, nameof(StructureValueForClient.HR), nameof(StructureValueForClient.HR), new ProgressOfUpdateAtStructAttribute(), Interval, HowOftenCheck, StartConfiguration_AtNow, ConsoleColor.Yellow);
                });
                thread_HR_Methods.Priority = ThreadPriority.BelowNormal;
                thread_HR_Methods.IsBackground = true;
                thread_HR_Methods.Start();
                //thread_HR_Methods.Join();

                Thread thread_Users_Methods = new Thread(() =>
                {
                    bool StartConfiguration_AtNow = true;
                    TimeSpan HowOftenCheck =
                    //default; 
                    TimeSpan.FromMinutes(1);
                    TimeSpan Interval = new TimeSpan(1, 0, 0);
                    Users_Methods.SetConfigAndStart(ThreadPriority.BelowNormal, LocalSettings.Users_Methods.GetType().AssemblyQualifiedName, nameof(StructureValueForClient.Users), nameof(StructureValueForClient.Users), new ProgressOfUpdateAtStructAttribute(), Interval, HowOftenCheck, StartConfiguration_AtNow, ConsoleColor.Yellow);
                });
                thread_Users_Methods.Priority = ThreadPriority.BelowNormal;
                thread_Users_Methods.IsBackground = true;
                thread_Users_Methods.Start();
                //thread_Users_Methods.Join();
                #endregion

                ListenerTcpIp(Progress); // Listen sockets Thread
            });
            Progress.Start();
        }
        private static void ListenerTcpIp(ProgressOfUpdateAtStructAttribute Parent)
        {
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName("Сетевой протокол");
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                TimeSpan Interval = new TimeSpan(0, 2, 0);
                ListenSockets.SetConfigAndStart(ThreadPriority.Highest, ListenSockets.GetType().AssemblyQualifiedName, "Прослушка сокета", null, Progress, Interval,
                    Interval
                    , true, ConsoleColor.White);
            });
            Progress.Start();
        }
    }
}
