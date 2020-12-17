using CommonDll.Client_Server;
using CommonDll.Helps;
using CommonDll.Structs.F_HR;
using CommonDll.Structs.F_LogFile.F_LogRecord;
using CommonDll.Structs.F_ProgressOfUpdate;
using CommonDll.Structs.F_Users;
using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace CommonDll.Structs.F_StructDataOnServer
{
    [Serializable]
    public static class ValueForClient
    {
        public static StructureValueForClient ReadyStructure { get; set; } // Для немедленной отдачи готового параметра    

        #region Serialize / Deserialize Config
        public static void DeserializeConfig(TimeSpan TimeOut)
        {
            TimeSpan timeSpan_StartConfig = DateTime.Now.TimeOfDay;
            DataToBinary.IsDeSerializeObject = true;
            StructureValueForClient.IsServer = true;
            ProgressOfUpdateAtStructAttribute.WriteInConsole_Default = true;
            ProgressOfUpdateAtStructAttribute.IsSaveProgress = true;
            ReadyStructure = new StructureValueForClient();
            DataToBinary.TimeOut = TimeOut;

            {
                ReadyStructure.HR = DataToBinary.DeSerializeObject_ToFile<HR>(nameof(ReadyStructure.HR), out TimeSpan SpendOnIt_Statistic);

                if (ReadyStructure.HR is null ? false : ReadyStructure.HR.Workers_Hierarchy.Count > 0)
                    ReadyStructure.UpdateTime_Module_HR = DateTime.Now;
                else
                    ReadyStructure.UpdateTime_Module_HR = default;
            } // HR

            {
                ReadyStructure.Users = DataToBinary.DeSerializeObject_ToFile<Users>(nameof(ReadyStructure.Users), out TimeSpan SpendOnIt_Statistic);

                if (ReadyStructure.Users is null ? false : ReadyStructure.Users.User_s.Count > 0)
                    ReadyStructure.UpdateTime_Module_Users = DateTime.Now;
                else
                    ReadyStructure.UpdateTime_Module_Users = default;
            } // Users

            DataToBinary.IsDeSerializeObject = false;
            ConsoleWriteLine.WriteInConsole("ValueForClient", "StartConfig", "Done", "Time on it: " + (DateTime.Now.TimeOfDay - timeSpan_StartConfig), ConsoleColor.Magenta);
        }
        #endregion 
        public static void SetToStruct(string Name, MainParentClass Main, DateTime TimeUpdate)
        {
            Main = LogRecord.LogFile_Clear(Main);
            var ReadyStructure_Type = ReadyStructure.GetType();

            if (!(ReadyStructure_Type is null))
            {
                #region Value
                var Main_Property = ReadyStructure_Type.GetProperty(string.Format("{0}", Name));

                if (!(Main_Property is null))
                    Main_Property.SetValue(ReadyStructure, Main);
                #endregion

                #region UpdateTime
                var UpdateTime_Main_Property = ReadyStructure_Type.GetProperty(string.Format("UpdateTime_Module_{0}", Name));

                if (!(UpdateTime_Main_Property is null))
                    UpdateTime_Main_Property.SetValue(ReadyStructure, TimeUpdate);
                #endregion
            }
        }
        public static object GetLockObject(string Name)
        {
            object Returned = default;
            var ReadyStructure_Type = ReadyStructure.GetType();

            if (!(ReadyStructure_Type is null))
            {
                var Main_Property_LockObject = ReadyStructure_Type.GetProperty(string.Format("{0}_LockObject", Name));

                if (!(Main_Property_LockObject is null))
                    Returned = Main_Property_LockObject.GetValue(ReadyStructure);
            }

            if (Returned is null)
                Returned = new object();

            return Returned;
        }

        #region Timer      
        private static Timer Timer { get; set; }
        public static void TimerStart(TimeSpan TimeSpan)
        {
            if (Thread.CurrentThread.Name is null)
                Thread.CurrentThread.Name = nameof(TimerStart) + "_ToClearTemp";

            if (TimeSpan > default(TimeSpan))
            {
                Timer = new Timer(TimeSpan.TotalMilliseconds);
                Timer.Elapsed += Timer_Elapsed;
                Timer.Start();
            }
            else
            {

            }
        }
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex) // "FatalExecutionEngineError" 
            {
            }
        }
        #endregion
    }
}
