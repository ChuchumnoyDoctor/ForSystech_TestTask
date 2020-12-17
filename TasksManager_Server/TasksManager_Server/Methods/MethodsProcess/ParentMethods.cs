using CommonDll.Helps;
using CommonDll.Structs;
using CommonDll.Structs.F_ProgressOfUpdate;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using TasksManager_Server.MethodsProcess;
using Task = System.Threading.Tasks.Task;
using Timer = System.Timers.Timer;

namespace TasksManager_Server.Methods.MethodsProcess
{
    public abstract class ParentMethods
    {
        public ParentMethods()
        {
            ParentProcessMain = new ParentProcessMain();
        }

        #region Menu/Main 
        /// <summary>
        /// У каждого наследованного класса свой эталонный объект. Set config Standart.
        /// </summary>      
        public static ProgressOfUpdateAtStructAttribute GetStandart()
        {
            return new ProgressOfUpdateAtStructAttribute()
            {
                NonSerializedConfig = new NonSerializedConfig()
                {
                    Method_ConsoleColor = default,
                    UpdateMethod = new Action(() => ValueForClient.ReadyStructure.UpdateTime_Module_ProgressOfUpdate = DateTime.Now),
                },
            };
        }

        internal static ParentProcessMain ParentProcessMain { get; set; }

        /// <summary>
        /// Лок-объект на случай перекрестного обращения к структуре на изменение. У каждой отнаследованной структуры/участка свой лок-объект
        /// </summary>
        internal object GetLockObject()
        {
            object LockObject = ValueForClient.GetLockObject(StructName);

            return LockObject is null ? new object() : LockObject;
        }
        public List<dynamic> Config { get; set; }
        internal TimeSpan Interval { get; set; }
        public string StructName { get; set; }
        public string AreaName { get; set; }
        public void SetConfigAndStart(ThreadPriority ThreadPriority, string AssemblyQualifiedName, string AreaName, string StructName, ProgressOfUpdateAtStructAttribute Parent, TimeSpan Interval, TimeSpan HowOftenCheck, bool MenuStarted, ConsoleColor CurrentColor)
        {
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(SetConfigAndStart) + ": " + AreaName);
            Progress.NonSerializedConfig.Method_ConsoleColor = CurrentColor;
            Progress.NonSerializedConfig.IsInTask = false;
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                this.StructName = StructName;
                this.AreaName = AreaName;
                this.Interval = Interval;
                Config = new List<dynamic>() { ThreadPriority, Progress };

                /*if (!string.IsNullOrEmpty(this.Path))
                    ParentProcessMain.Watcher_Watch(AreaName, "Menu", AssemblyQualifiedName, Path, new Tuple<bool, bool, bool, bool>(true, true, true, false), Config);*/

                if (HowOftenCheck > default(TimeSpan))
                    TimerStart(Convert.ToInt32(HowOftenCheck.TotalMilliseconds), Progress);

                if (MenuStarted)
                    Menu(ThreadPriority, Progress, "By Стартующая конфигурация");
                else
                    Progress_Menu = Progress.NonSerializedConfig.GetOnEntry; // Костыль
            });
            Progress.Start();
        }
        public string ClassName { get { return this.GetType().Name; } }
        public string Temp_PathName
        {
            get
            {
                return LocalSettings.ApplicationBasePath + "Temp\\" + ClassName + "\\";
            }
        }
        private object Menu_LockObject = new object();
        Thread Thread { get; set; }
        protected ProgressOfUpdateAtStructAttribute Progress_Menu { get; set; }
        /// <summary>
        /// This is a start of import.
        /// </summary>
        /// <param name="Parent">Control-aspect</param>
        /// <param name="FromStartedName">Started by what</param>        
        public void Menu(ThreadPriority ThreadPriority, ProgressOfUpdateAtStructAttribute Parent, string FromStartedName)
        {
            Progress_Menu = Parent.NonSerializedConfig.GetOnEntry;
            Progress_Menu.SetName(string.Format("Конфигуративное меню. Модуль: '{0}'", AreaName));
            Progress_Menu.NonSerializedConfig.IsInTask = false;
            Progress_Menu.NonSerializedConfig.Method = new Action(() =>
            {
                if (Helper.IsLocked(Menu_LockObject))
                    if (Thread is null ? false : Thread.IsAlive)
                        try
                        {
                            Thread.Abort();
                            Thread.Join();
                            Thread = null;
                        }
                        catch
                        {

                        }

                Thread = new Thread(() =>
                {
                    if (Thread.CurrentThread.Name is null)
                        Thread.CurrentThread.Name = Progress_Menu.Name;

                    lock (Menu_LockObject)
                    {
                        Progress_Menu.Comment = FromStartedName;
                        ParentProcessMain.Watcher_Turn(false, Parent.Name);

                        MainParentClass To_Value = Menu_Virtual(Progress_Menu);

                        if (!string.IsNullOrEmpty(StructName))
                        {
                            ApplyChanges(StructName, To_Value, out MainParentClass Returned);
                            To_Value = Returned;

                            lock (GetLockObject())
                                ValueForClient.SetToStruct(StructName, To_Value, DateTime.Now);

                            var ReadyStructure = ValueForClient.ReadyStructure; // test
                        }

                        ParentProcessMain.Watcher_Turn(true, Parent.Name);
                    }
                });
                Thread.Priority = ThreadPriority;
                Thread.IsBackground = true;

                try
                {
                    Thread.Start();
                    Thread.Join();
                }
                catch (ThreadStateException ex)
                {
                    Progress_Menu.ExceptionMessage = ex.Message.ToString();
                }
                catch (ThreadAbortException ex)
                {
                    Progress_Menu.ExceptionMessage = ex.Message.ToString();
                }
                catch (NullReferenceException ex)
                {
                    Progress_Menu.ExceptionMessage = ex.Message.ToString();
                }
            });
            Progress_Menu.Start();
        }

        /// <summary>
        /// This is direct import from some database
        /// </summary>
        /// <param name="Parent">Control-aspect</param>
        /// <returns></returns>
        public abstract MainParentClass Menu_Virtual(ProgressOfUpdateAtStructAttribute Parent);
        #endregion      

        #region Export from User to Azure  
        public static string SetToServer(StructureValueForClient StructureValueForClient, ProgressOfUpdateAtStructAttribute Parent)
        {
            string Exception = "";
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName("Экспорт данных клиента в БД");
            Progress.NonSerializedConfig.IsInTask = false;
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                if (!(StructureValueForClient is null))
                    if (Changed(StructureValueForClient, out Exception, Progress))
                    {
                        Task Task = Task.Run(() =>
                        {
                            if (Export(StructureValueForClient, Progress))
                            {

                            }
                            else if (Export(StructureValueForClient, Progress))
                            {

                            }
                            else
                            {

                            }
                        });
                    }
                    else
                    {

                    }
            });
            Progress.Start();

            Exception = string.IsNullOrEmpty(Progress.ExceptionMessage) ? Exception : Progress.ExceptionMessage;

            return Exception;
        }
        public static bool Export(StructureValueForClient StructureValueForClient, ProgressOfUpdateAtStructAttribute Parent)
        {
            bool Exported = false;
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName("Непосредственный экспорт");
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                Dictionary<string, MainParentClass> props = Helper.GetProperties<MainParentClass, StructureValueForClient>(StructureValueForClient);

                if (props is null ? false : props.Count > 0)
                    foreach (var From in props)
                    {
                        string Name = From.Key;
                        MainParentClass From_Value = (MainParentClass)From.Value.Clone();
                        var ReadyStructure_Type = ValueForClient.ReadyStructure.GetType();

                        if (!(ReadyStructure_Type is null))
                        {
                            var To_Property = ReadyStructure_Type.GetProperty(Name);

                            if (!(To_Property is null))
                            {
                                MainParentClass To_Value = (MainParentClass)To_Property.GetValue(ValueForClient.ReadyStructure);

                                if (MainParentClass.ToFindTypeRecord(From_Value, out TimeSpan TimeS, out MainParentClass FindedObject))
                                    if (MainParentClass.ToExport(From_Value, false, Progress))
                                        Exported = true;
                                    else
                                    {

                                    }
                            }
                            else
                                throw new NullReferenceException(string.Format("{0} is not found", Name));
                        }
                        else
                            throw new NullReferenceException(string.Format("{0} is not found", nameof(ValueForClient.ReadyStructure)));
                    }
            });
            Progress.Start();

            return Exported;
        }
        private static bool Changed(StructureValueForClient StructureValueForClient, out string Exception, ProgressOfUpdateAtStructAttribute Parent)
        {
            bool IsChanged = false;
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName("Внесение изменений");
            Progress.NonSerializedConfig.IsInTask = false;
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                Dictionary<string, MainParentClass> props = Helper.GetProperties<MainParentClass, StructureValueForClient>(StructureValueForClient);
                bool ToFindTypeRecord = false;

                foreach (var From in props)
                {
                    string Name = From.Key;
                    MainParentClass From_Value = (MainParentClass)From.Value.Clone();

                    lock (ValueForClient.GetLockObject(Name))
                    {
                        var ReadyStructure_Type = ValueForClient.ReadyStructure.GetType();

                        if (!(ReadyStructure_Type is null))
                        {
                            var To_Property = ReadyStructure_Type.GetProperty(Name);

                            if (!(To_Property is null))
                            {
                                MainParentClass To_Value = (MainParentClass)To_Property.GetValue(ValueForClient.ReadyStructure);

                                if (Changed(Name, From_Value, To_Value, out ToFindTypeRecord, Progress))
                                {
                                    IsChanged = true;
                                    CollectChanges(Name, (MainParentClass)From.Value.Clone());
                                }

                                ValueForClient.SetToStruct(Name, To_Value, DateTime.Now); // Update in anyway
                            }
                        }
                    }
                }

                if (!IsChanged & ToFindTypeRecord)
                    Progress.ExceptionMessage = "Не удалось изменить";
            });
            Progress.Start();

            Exception = Progress.ExceptionMessage;
            return IsChanged;
        }
        private static bool Changed(string Name, MainParentClass From_Value, MainParentClass To_Value, out bool ToFindTypeRecord, ProgressOfUpdateAtStructAttribute Parent)
        {
            bool IsChanged = false;
            bool ToFindTypeRecord_Local = false;
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName("Внесение изменений");
            Progress.NonSerializedConfig.IsInTask = false;
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                if (!Helper.IsLocked(ValueForClient.GetLockObject(Name)))
                    lock (ValueForClient.GetLockObject(Name))
                    {
                        if (MainParentClass.ToFindTypeRecord(From_Value, out TimeSpan TimeS, out MainParentClass FindedObject))
                        {
                            ToFindTypeRecord_Local = true;

                            if (MainParentClass.ToChanged(From_Value, To_Value, out TimeSpan TimeSpend, false, out string Exception))
                            {
                                IsChanged = true;

                                if (!string.IsNullOrEmpty(Exception))
                                {

                                }
                            }
                            else if (MainParentClass.ToChanged(From_Value, To_Value, out TimeSpan Time, false, out Exception))
                            {
                                IsChanged = true;

                                if (!string.IsNullOrEmpty(Exception))
                                {

                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Exception))
                                {

                                }
                            }

                            if (!IsChanged)
                            {

                            }
                        }
                    }
            });
            Progress.Start();
            ToFindTypeRecord = ToFindTypeRecord_Local;

            return IsChanged;
        }
        #endregion

        #region CollectChanges
        static Dictionary<string, List<MainParentClass>> Changes { get; set; } = new Dictionary<string, List<MainParentClass>>();
        public static void CollectChanges(string Name, MainParentClass From_Value)  // Собираем изменения в коллекцию, пока готовится новый экземпляр
        {
            lock (ValueForClient.GetLockObject(Name))
                if (MainParentClass.ToFindTypeRecord(From_Value, out TimeSpan TimeS, out MainParentClass FindedObject))
                    if (Changes.ContainsKey(Name))
                        Changes[Name].Add(From_Value);
                    else
                        Changes.Add(Name, new List<MainParentClass>() { From_Value });
                else
                {

                }
        }
        public static bool ApplyChanges(string Name, MainParentClass To_Value, out MainParentClass Returned)  // Применяем собранные изменения в итоговую структуру
        {
            bool IsChanged = false;

            lock (ValueForClient.GetLockObject(Name))
                foreach (var Change in Changes)
                    if (Change.Key == Name)
                        if (!(Change.Value is null))
                        {
                            foreach (var From_Value in Change.Value)
                                if (!(From_Value is null))
                                    if (Changed(Name, From_Value, To_Value, out bool ToFindTypeRecord, new ProgressOfUpdateAtStructAttribute()))
                                    {
                                        IsChanged = true;
                                    }
                                    else
                                    {

                                    }

                            Change.Value.Clear();
                        }

            Returned = To_Value;

            return IsChanged;
        }
        #endregion

        #region Timer
        public Timer Timer { get; set; }

        private void TimerStart(int milliseconds, ProgressOfUpdateAtStructAttribute Parent)
        {
            Timer_Parent = Parent.NonSerializedConfig.GetOnEntry;
            Timer_Parent.SetName("Запуск таймера. Переодичность: " + TimeSpan.FromMilliseconds(milliseconds) + ". Интервал: " + Interval);
            Timer_Parent.NonSerializedConfig.Method = new Action(() =>
            {
                if (milliseconds > 0)
                {
                    Timer = new Timer(milliseconds);
                    Timer.Elapsed += Timer_Elapsed;
                    Timer.Start();
                }
                else
                {
                    Timer_Parent.ExceptionMessage = "milliseconds = 0";
                }
            });
            Timer_Parent.Start();
        }
        ProgressOfUpdateAtStructAttribute Timer_Parent { get; set; }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ProgressOfUpdateAtStructAttribute Progress = Timer_Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(StructName) + ": " + StructName + ". " + nameof(Timer_Elapsed) + " at: " + DateTime.Now);
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                if (!Helper.IsLocked(Timer))
                    lock (Timer)
                        if (Timer_Elapsed_ToStartAgain())
                        {
                            Progress.Comment = true.ToString();
                            Menu(Config[0], Config[1], "By Timer");
                        }
                        else
                            Progress.Comment = false.ToString();
            });
            Progress.Start();
        }
        public virtual bool Timer_Elapsed_ToStartAgain()
        {
            if (DateTime.Now - Progress_Menu.LastChange_DateTime > Interval)
                return true;
            else
                return false;
        }
        #endregion
    }
}
