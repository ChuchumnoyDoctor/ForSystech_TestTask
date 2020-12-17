using CommonDll.Client_Server.Client;
using CommonDll.Helps;
using CommonDll.Structs.F_LogFile.F_LogRecord;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TasksManager.LocalConfig;

namespace TasksManager.TabControl_Main
{
    public class ImportProtocol
    {
        #region Обновление/Импорт
        public static void Set(List<string> ListNameOfValue)
        {
            if (ListNameOfValue is null ? false : ListNameOfValue.Count > 0)
            {
                ImportProtocol.ListNameOfValue = ListNameOfValue;
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
        }
        static System.Windows.Forms.Timer Timer { get; set; } = new System.Windows.Forms.Timer() { Interval = 1000 };
        private static void Timer_Tick(object sender, EventArgs e) // Timer
        {
            Task Task = Task.Run(() =>
            {
                if (Thread.CurrentThread.Name is null)
                    Thread.CurrentThread.Name = string.Format("{0}: {1}", nameof(ImportProtocol), nameof(Timer_Tick));

                if (!Helper.IsLocked(Timer))
                    lock (Timer)
                        if (!(OnUpdate is null))
                        {
                            OnUpdate.Invoke(Process(out StructureValueForClient StructureValueForClient, out TimeSpan TimeLoad, out ConnectionBackInformation ConnectionBackInformationLoad), TimeLoad, StructureValueForClient, ConnectionBackInformationLoad);
                            StructureValueForClient = null;
                        }
            });
        }
        public delegate void onUpdate(bool Update, TimeSpan TimeLoad, StructureValueForClient StructureValueForClient, ConnectionBackInformation ConnectionBackInformationLoad);
        public static event onUpdate OnUpdate;
        public static List<string> ListNameOfValue { get; set; }
        public static bool Process(out StructureValueForClient StructureValueForClient, out TimeSpan TimeOnLoad, out ConnectionBackInformation ConnectionBackInformationLoad) // Process
        {
            StructureValueForClient = null;
            TimeOnLoad = default;
            ConnectionBackInformationLoad = null;

            if (ListNameOfValue is null ? false : ListNameOfValue.Count > 0)
            {
                TimeSpan StartLoad = DateTime.Now.TimeOfDay;
                Tuple<StructureValueForClient, string, string> tuple = MethodsCall.GetParamatersForClientReady(ListNameOfValue, out ConnectionBackInformationLoad);
                TimeOnLoad = DateTime.Now.TimeOfDay - StartLoad;
                Console.WriteLine("GetParamatersForClientReady, time on it: " + TimeOnLoad);

                if (ConnectionBackInformationLoad.Recieve_GetResponse)
                {
                    var Time = TimeOnLoad - ConnectionBackInformationLoad.Recieve_TimeSpend - ConnectionBackInformationLoad.Send_TimeSpend;

                    if (TimeOnLoad > TimeSpan.FromSeconds(20))
                    {

                    }

                    if (Time > TimeSpan.FromSeconds(2))
                    {

                    }
                }
                else
                {

                }

                if (!(tuple.Item1 is null))
                {
                    TimeSpan TimeServerWhenGetProperties = DateTime.Now.TimeOfDay - tuple.Item1.TimeServerWhenGetProperties;
                    TimeSpan TimeOnSentFromServer = TimeOnLoad - TimeServerWhenGetProperties;
                }

                if (tuple.Item3 == "<Not connected>")
                    Console.WriteLine("Result: <Not connected>");
                else if (!(tuple.Item1 is null))
                {
                    StructureValueForClient = tuple.Item1;

                    if (!(StructureValueForClient is null))
                        return true;
                    else
                        Console.WriteLine("Ошибка. Не полученны данные.");
                }
                else
                    Console.WriteLine("Ошибка. Не полученны данные.");
            }
            else
            {

            }

            return false;
        }
        #endregion

        #region MainStructs
        private static CommonDll.Structs.F_HR.HR hR;
        public static CommonDll.Structs.F_HR.HR HR { get { return hR; } set { hR = value is null ? null : (CommonDll.Structs.F_HR.HR)LogRecord.LogFile_Clear(value); } }
        #endregion
    }
}
