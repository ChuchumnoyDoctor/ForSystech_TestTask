using CommonDll.Helps;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace CommonDll.Structs.F_ProgressOfUpdate
{
    /// <summary>
    /// Обёртка, направленная на уменьшение кода, самостоятельную обработку: затраченного времени, пулов, потоков, лок объектов и сбор в единную структуру в виде статистики.
    /// Передаётся на клиент посредством сериализации и отображение в соот. представлении.
    /// </summary>  
    [Serializable]
    public class ProgressOfUpdateAtStructAttribute : MainParentClass
        , ICloneable, IParentable<ProgressOfUpdateAtStructAttribute>
    {
        #region override basement changes
        internal protected override MainParentClass[] Base_Childs
        {
            get
            {
                return default;
            }
            set
            {

            }
        }
        private protected override MainParentClass Base_UpdateObject_Original { get { return default; } set { } }
        public override string Base_Name { get { return Name_RussianEquivalent + Name + (NonSerializedConfig is null ? null : NonSerializedConfig.Method_ClassName + NonSerializedConfig.Method_MethodName) + ClassName; } }

        #region Server Export Part
        [field: NonSerialized()]
        public override KeyValuePair<string, DbParameter[]> Select { get; set; }
        public override string Select_WHERE { get { return ""; } }

        [field: NonSerialized()]
        public override KeyValuePair<string, DbParameter[]> Select_FullMatch { get; set; }

        [field: NonSerialized()]
        internal protected override KeyValuePair<string, DbParameter[]> Insert { get; set; }

        [field: NonSerialized()]
        internal protected override string Update { get; set; }

        [field: NonSerialized()]
        internal protected override string Delete { get; set; }
        #endregion
        #endregion  
        private void GetProgress(string ClassName, string Method, ProgressOfUpdateAtStructAttribute Parent)
        {
            if (!(Parent is null))
            {
                ProgressOfUpdateAtStructAttribute Progress = new ProgressOfUpdateAtStructAttribute()
                {
                    Name = string.IsNullOrEmpty(ClassName) & string.IsNullOrEmpty(Method) ? null : ClassName + ";" + Method,
                    Submodules = new List<ProgressOfUpdateAtStructAttribute>(),
                    NonSerializedConfig = new NonSerializedConfig()
                    {
                        IsStarted = false,
                        Method_ClassName = ClassName,
                        Method_MethodName = Method,
                        UpdateMethod = Parent.NonSerializedConfig.UpdateMethod,
                        Method_ConsoleColor = Parent.NonSerializedConfig.Method_ConsoleColor,
                        WriteInConsole = this.NonSerializedConfig.WriteInConsole
                    }
                };
                Progress.Parent = Parent;
                Parent.NonSerializedConfig.GetOnEntry = Progress; // Set parent on entry
            }
        }
        public void OnEntry()
        {
            GetProgress(null, null, this);
        }

        #region IParentable<T>
        [field: NonSerialized()]
        public ProgressOfUpdateAtStructAttribute Parent { get; set; }
        #endregion

        #region Propreties, Serializable for client
        public string Name { get; internal set; }
        public string Name_RussianEquivalent { get; internal set; }
        /// <summary>
        /// Планируемое количество исполняемых суб-объектов
        /// </summary>
        public int Count { get; set; }

        private string exceptionMessage { get; set; }
        /// <summary>
        /// Сообщение о провальном исполнении. Определяет статус текущего объекта.
        /// </summary>
        public virtual string ExceptionMessage
        {
            get
            {
                try
                {
                    List<ProgressOfUpdateAtStructAttribute> Submodules = this.Submodules.Select(x => x).ToList();

                    if (string.IsNullOrEmpty(exceptionMessage))
                        exceptionMessage = Submodules.Select(x => x.ExceptionMessage).FirstOrDefault(x => !string.IsNullOrEmpty(x));

                    return exceptionMessage;
                }
                catch (IndexOutOfRangeException ex)
                {
                    return ExceptionMessage;
                }
            }
            set
            {
                exceptionMessage = value;

                if (!string.IsNullOrEmpty(exceptionMessage))
                    if (NonSerializedConfig.WriteInConsole)
                    {
                        ProgressOfUpdateAtStructAttribute Parent = this.Parent;
                        ConsoleWriteLine.WriteInConsole(string.IsNullOrEmpty(NonSerializedConfig.Method_ClassName) ? Parent is null ? null : string.IsNullOrEmpty(Parent.Name_RussianEquivalent) ? Parent.Name : Parent.Name_RussianEquivalent : NonSerializedConfig.Method_ClassName, string.IsNullOrEmpty(NonSerializedConfig.Method_MethodName) ? string.IsNullOrEmpty(Name_RussianEquivalent) ? this.Name : this.Name_RussianEquivalent : NonSerializedConfig.Method_MethodName, "Failed", "Exception: " + ExceptionMessage, NonSerializedConfig.Method_ConsoleColor);
                    }
            }
        }

        protected List<ProgressOfUpdateAtStructAttribute> submodules_Private;
        /// <summary>
        /// Суб-модули/дочерние.
        /// </summary>
        public List<ProgressOfUpdateAtStructAttribute> Submodules
        {
            get
            {
                return Submodules_Get();
            }
            internal set
            {
                try
                {
                    this.submodules_Private = value is null ? new List<ProgressOfUpdateAtStructAttribute>() : value;
                    this.submodules_Private = this.submodules_Private.Where(x => x is null ? false : x != this & x.Name != this.Name & x.Name_RussianEquivalent != this.Name_RussianEquivalent).ToList();

                    try
                    {
                        this.submodules_Private.Sort((a, b) => (a is null ? default : a.NonSerializedConfig is null ? default : a.NonSerializedConfig.StartDateTime).CompareTo(b is null ? default : b.NonSerializedConfig is null ? default : b.NonSerializedConfig.StartDateTime));
                    }
                    catch (NullReferenceException ex)
                    {

                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    Submodules = value;
                }
                catch (InvalidOperationException ex)
                {
                    Submodules = value;
                }
            }
        }
        public List<ProgressOfUpdateAtStructAttribute> Submodules_Get([Optional] int RecurseTry) // Принудительный Get. Иногда скипает тело свойства get
        {
            try
            {
                if (this.submodules_Private is null)
                    this.submodules_Private = new List<ProgressOfUpdateAtStructAttribute>();

                int ICountFirst = submodules_Private.Count();
                var submodules_Private_Temp = this.submodules_Private.Where(x => x is null ? false : x != this & x.Name != this.Name & x.Name_RussianEquivalent != this.Name_RussianEquivalent).ToList();
                int ICountSecond = submodules_Private.Count();

                if (ICountFirst != ICountSecond)
                {

                }

                this.submodules_Private = submodules_Private_Temp;

                return this.submodules_Private;
            }
            catch (IndexOutOfRangeException ex)
            {
                RecurseTry++;

                if (RecurseTry < 3)
                    return Submodules_Get(RecurseTry);
            }
            catch (InvalidOperationException ex)
            {
                RecurseTry++;

                if (RecurseTry < 3)
                    return Submodules_Get(RecurseTry);
            }

            return new List<ProgressOfUpdateAtStructAttribute>();
        }

        /// <summary>
        /// Количество выполненных объектов/модулей.
        /// </summary>
        public int Submodules_Done([Optional] int RecurseTry)
        {
            try
            {
                int i = 0;
                List<ProgressOfUpdateAtStructAttribute> Submodules = this.Submodules_Get();

                if (Submodules.Count > 0)
                    foreach (ProgressOfUpdateAtStructAttribute r in Submodules)
                        if (r.Count == r.Submodules_Done())
                            i++;

                return i;
            }
            catch (IndexOutOfRangeException ex)
            {
                if (RecurseTry < 3)
                    return Submodules_Done(RecurseTry);
            }
            catch (InvalidOperationException ex)
            {
                RecurseTry++;

                if (RecurseTry < 3)
                    return Submodules_Done(RecurseTry);
            }

            return default;
        }

        private DateTime lastChange_DateTime;
        /// <summary>
        /// Последнее изменение.
        /// </summary>
        public virtual DateTime LastChange_DateTime
        {
            get
            {
                try
                {
                    List<ProgressOfUpdateAtStructAttribute> Submodules = this.Submodules_Get().Select(x => x).ToList();
                    List<DateTime> r = Submodules.Count > 0 ? Submodules.Select(x => x.LastChange_DateTime).ToList() : new List<DateTime>();
                    DateTime Local = r.Count > 0 ? r.Max() : lastChange_DateTime;

                    return Local > lastChange_DateTime ? Local : lastChange_DateTime;
                }
                catch (IndexOutOfRangeException ex)
                {
                    return LastChange_DateTime;
                }
                catch (InvalidOperationException ex)
                {
                    return LastChange_DateTime;
                }
            }
            set
            {
                lastChange_DateTime = value;
            }
        }
        private TimeSpan timeSpend;
        /// <summary>
        /// Затраченное время.
        /// </summary>
        public virtual TimeSpan TimeSpend
        {
            get
            {
                try
                {
                    List<ProgressOfUpdateAtStructAttribute> Submodules = this.Submodules_Get().Select(x => x).ToList();
                    TimeSpan Time = default(TimeSpan);

                    if (timeSpend == default(TimeSpan))
                        foreach (ProgressOfUpdateAtStructAttribute r in Submodules)
                            Time += r.TimeSpend;
                    else
                        Time = timeSpend;

                    return Time;
                }
                catch (IndexOutOfRangeException ex)
                {
                    return TimeSpend;
                }
                catch (InvalidOperationException ex)
                {
                    return TimeSpend;
                }
            }
            internal set
            {
                timeSpend = value;
            }
        }

        public const string Status_Failed = "Не удалось";
        public const string Status_InProcess = "В процессе";
        public const string Status_Done = "Выполнено";
        /// <summary>
        /// Статус объекта.
        /// </summary>
        public virtual string Status
        {
            get
            {
                try
                {
                    List<ProgressOfUpdateAtStructAttribute> Submodules = this.Submodules.ToList();

                    if (!string.IsNullOrEmpty(ExceptionMessage))
                        return Status_Failed;
                    else if (Count == Submodules_Done() & (TimeSpend > default(TimeSpan) || ((Submodules_Done() == 0) & (DateTime.Now - LastChange_DateTime > TimeSpan.FromMinutes(1)))))
                        if (Submodules.Count == 0 ? true : !Submodules.Select(x => x.Status).Contains(Status_InProcess))
                            return Status_Done;

                    return Status_InProcess;
                }
                catch (IndexOutOfRangeException ex)
                {
                    return Status;
                }/*
                catch (Exception ex)
                {
                    return Status;
                }*/
            }
            private protected set
            {

            }
        }
        /// <summary>
        /// Сопровождающий комментарий с важным замечанием. Не путать с сообщением о ошибке.
        /// </summary>
        public new string Comment { get; set; }
        /// <summary>
        /// Прогресс выполнения задачи/модуля
        /// </summary>
        public virtual int Progress
        {
            get
            {
                try
                {
                    List<ProgressOfUpdateAtStructAttribute> Submodules = this.Submodules_Get().ToList();
                    float d = 0;

                    if (Submodules_Done() == 0 & Count == 0)
                        d = Submodules.Count > 0 ? 0 : 100;
                    else
                        d = Submodules_Done() == 0 ? 0 : ((float)Submodules_Done() / (float)Count);

                    int Val = (int)(d * 100);
                    Val = Val > 100 ? 100 : Val;

                    if (Val < 0)
                        Val = 0;

                    return Val;
                }
                catch (IndexOutOfRangeException ex)
                {
                    return Progress;
                }
                catch (InvalidOperationException ex)
                {
                    return Progress;
                }
            }
            private protected set
            {

            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Приведение всех дочерних объектов и текущего к выполненному
        /// </summary>
        /// <param name="RecurseTry">Рекурсивная попытка. По дефолту ставить значение в 0</param>
        /// <param name="Parent"></param>
        private void SetDone([Optional] int RecurseTry, ProgressOfUpdateAtStructAttribute Parent)
        {
            try
            {
                foreach (var r in this.Submodules)
                    r.SetDone(0, this);

                if (Status == Status_InProcess)
                {
                    LastChange_DateTime = DateTime.Now;
                    try
                    {
                        TimeSpend = NonSerializedConfig is null ? default : DateTime.Now - NonSerializedConfig.StartDateTime;
                    }
                    catch (NullReferenceException ex)
                    {

                    }
                }

                if (this.TimeSpend <= default(TimeSpan))
                    TimeSpend = TimeSpan.FromMilliseconds(1);

                if (this.Count != Submodules_Done())
                    this.Count = Submodules_Done();
            }
            catch (InvalidOperationException ex)
            {
                RecurseTry++;

                if (RecurseTry < 3)
                    SetDone(RecurseTry, Parent);
            }
            catch (IndexOutOfRangeException ex)
            {
                RecurseTry++;

                if (RecurseTry < 3)
                    SetDone(RecurseTry, Parent);
            }
        }
        /// <summary>
        /// Внешний API для запуска Action. Регулирует try-catch по фиксированию ошибки.
        /// </summary>
        public void Start()
        {
            if (!(NonSerializedConfig is null))
            {
                NonSerializedConfig.IsStarted = true;

                if (Thread.CurrentThread.Name is null)
                    Thread.CurrentThread.Name = string.IsNullOrEmpty(NonSerializedConfig.Method_MethodName) ? string.IsNullOrEmpty(Name_RussianEquivalent) ? this.Name : this.Name_RussianEquivalent : NonSerializedConfig.Method_MethodName;

                ConfigStarted(this.Parent);
            }
        }
        /// <summary>
        /// Непосредственный обработчик Action'а.
        /// </summary>
        /// <param name="Parent"></param>
        private void ConfigStarted(ProgressOfUpdateAtStructAttribute Parent)
        {
            NonSerializedConfig.StartDateTime = DateTime.Now;
            LastChange_DateTime = NonSerializedConfig.StartDateTime;

            if (!(Parent is null))
                if (Parent != this)
                    FindAndReplace(Parent, this, this); // Remove another 
                else
                {

                }

            if (NonSerializedConfig.WriteInConsole) // Message in console about start of configuration
                ConsoleWriteLine.WriteInConsole(string.IsNullOrEmpty(NonSerializedConfig.Method_ClassName) ? Parent is null ? null : string.IsNullOrEmpty(Parent.Name_RussianEquivalent) ? Parent.Name : Parent.Name_RussianEquivalent : NonSerializedConfig.Method_ClassName, string.IsNullOrEmpty(NonSerializedConfig.Method_MethodName) ? string.IsNullOrEmpty(Name_RussianEquivalent) ? this.Name : this.Name_RussianEquivalent : NonSerializedConfig.Method_MethodName, "Start", "at: " + NonSerializedConfig.StartDateTime, NonSerializedConfig.Method_ConsoleColor);

            if (!(NonSerializedConfig.Method is null))
                NonSerializedConfig.Method.Invoke(); // Main process by Action          

            if (!(NonSerializedConfig.UpdateMethod is null)) // Update ProgressOfUpdateAtStructAttribute's main struct
                NonSerializedConfig.UpdateMethod.Invoke();

            LastChange_DateTime = DateTime.Now; // Update last change
            TimeSpend = DateTime.Now - NonSerializedConfig.StartDateTime;

            if (NonSerializedConfig.WriteInConsole) // Message in console about end of configuration
                ConsoleWriteLine.WriteInConsole(string.IsNullOrEmpty(NonSerializedConfig.Method_ClassName) ? Parent is null ? null : string.IsNullOrEmpty(Parent.Name_RussianEquivalent) ? Parent.Name : Parent.Name_RussianEquivalent : NonSerializedConfig.Method_ClassName, string.IsNullOrEmpty(NonSerializedConfig.Method_MethodName) ? string.IsNullOrEmpty(Name_RussianEquivalent) ? this.Name : this.Name_RussianEquivalent : NonSerializedConfig.Method_MethodName, "Done", "Time on it: " + TimeSpend + "; " + Comment, NonSerializedConfig.Method_ConsoleColor);

            {
                if (Thread.CurrentThread.Name is null)
                    Thread.CurrentThread.Name = string.IsNullOrEmpty(NonSerializedConfig.Method_MethodName) ? string.IsNullOrEmpty(Name_RussianEquivalent) ? this.Name : this.Name_RussianEquivalent : NonSerializedConfig.Method_MethodName;

                SetDone(0, Parent);
                LastChange_DateTime = DateTime.Now;
                NonSerializedConfig.Method = null;
                //NonSerializedConfig.UpdateMethod = null;
                //NonSerializedConfig = null; // обнулить, чтобы не хранить инфо. Мусорщик потом соберет и свободит от него
            }
        }
        public void SetName(string Name)
        {
            this.Name = Name;
            this.Name_RussianEquivalent = Name;
        }
        public void SetAmount(int Amount)
        {
            this.Count = Amount;
        }
        public override object ClonePart()
        {
            return new ProgressOfUpdateAtStructAttribute()
            {
                Name_RussianEquivalent = this.Name_RussianEquivalent is null ? null : (string)this.Name_RussianEquivalent.Clone(),
                Name = this.Name is null ? null : (string)this.Name.Clone(),
                Submodules = this.Submodules.Select(x => (ProgressOfUpdateAtStructAttribute)x.Clone()).ToList(),
                lastChange_DateTime = this.lastChange_DateTime,
                Count = this.Count,
                ExceptionMessage = this.ExceptionMessage,
                timeSpend = this.timeSpend,
                Comment = Comment
            };
        }
        public object Clone_Full()
        {
            return new ProgressOfUpdateAtStructAttribute()
            {
                Name_RussianEquivalent = this.Name_RussianEquivalent is null ? null : (string)this.Name_RussianEquivalent.Clone(),
                Name = this.Name is null ? null : (string)this.Name.Clone(),
                Submodules = this.Submodules.Select(x => (ProgressOfUpdateAtStructAttribute)x.Clone()).ToList(),
                lastChange_DateTime = this.lastChange_DateTime,
                Count = this.Count,
                ExceptionMessage = this.ExceptionMessage,
                timeSpend = this.timeSpend,
                Comment = Comment,
                NonSerializedConfig = NonSerializedConfig is null ? null : (NonSerializedConfig)NonSerializedConfig.Clone(),
            };
        }
        object findAndReplace_LockObject;
        internal object FindAndReplace_LockObject
        {
            get
            {
                if (findAndReplace_LockObject is null)
                    findAndReplace_LockObject = new object();

                return findAndReplace_LockObject;
            }
            set
            {
                findAndReplace_LockObject = value;
            }
        }
        private static void FindAndReplace(ProgressOfUpdateAtStructAttribute Parent, ProgressOfUpdateAtStructAttribute ChangeOnThis, ProgressOfUpdateAtStructAttribute ToRemove)
        {
            if (IsSaveProgress)
            {
                if (!(Parent is null))
                    lock (Parent.FindAndReplace_LockObject)
                    {
                        try
                        {
                            FindAndReplace_Clear(Parent, ChangeOnThis, ToRemove);
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            FindAndReplace(Parent, ChangeOnThis, ToRemove);
                        }
                        catch (InvalidOperationException ex)
                        {
                            FindAndReplace(Parent, ChangeOnThis, ToRemove);
                        }

                        if (!(ChangeOnThis is null))
                            try
                            {
                                List<ProgressOfUpdateAtStructAttribute> Submodules = Parent.Submodules_Get();
                                Submodules.Add(ChangeOnThis);
                                Parent.Submodules = Submodules;
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                FindAndReplace(Parent, ChangeOnThis, ToRemove);
                            }
                            catch (InvalidOperationException ex)
                            {
                                FindAndReplace(Parent, ChangeOnThis, ToRemove);
                            }
                    }
            }
            else
            {
                if (!(ChangeOnThis is null))
                    ChangeOnThis.Parent = null;

                if (!(ToRemove is null))
                    ToRemove.Parent = null;
            }
        }
        private static void FindAndReplace_Clear(ProgressOfUpdateAtStructAttribute Parent, ProgressOfUpdateAtStructAttribute ClearOnThis, ProgressOfUpdateAtStructAttribute ToRemove)
        {
            List<ProgressOfUpdateAtStructAttribute> Submodules = Parent.Submodules_Get();
            List<ProgressOfUpdateAtStructAttribute> Finded = Submodules.Where(x =>
                    (ClearOnThis is null ? false : (x.Name == ClearOnThis.Name & x.Name_RussianEquivalent == ClearOnThis.Name_RussianEquivalent ||
                    x.Name == ToRemove.Name & x.Name_RussianEquivalent == ToRemove.Name_RussianEquivalent)
                    ||
                    ((DateTime.Now - x.LastChange_DateTime > TimeSpan.FromMinutes(30)) & (x.Status == Status_Done)) // Select where spend time more then 30 minutes and already done
                    ||
                    (DateTime.Now - x.LastChange_DateTime > TimeSpan.FromMinutes(50)) // Select where spend time more then 50 minutes
                    )).ToList();

            foreach (var Find in Finded) // delete selected
                try
                {
                    Submodules.Remove(Find);
                }
                catch
                {

                }

            if (Parent.NonSerializedConfig is null ? false : Parent.NonSerializedConfig.WriteInConsole)
                if (Finded.Count > 3)
                    ConsoleWriteLine.WriteInConsole(string.IsNullOrEmpty(Parent.NonSerializedConfig.Method_ClassName) ? Parent is null ? null : string.IsNullOrEmpty(Parent.Name_RussianEquivalent) ? Parent.Name : Parent.Name_RussianEquivalent : Parent.NonSerializedConfig.Method_ClassName, string.IsNullOrEmpty(Parent.NonSerializedConfig.Method_MethodName) ? string.IsNullOrEmpty(Parent.Name_RussianEquivalent) ? Parent.Name : Parent.Name_RussianEquivalent : Parent.NonSerializedConfig.Method_MethodName, "Clear progress's", "Removed: " + Finded.Count, Parent.NonSerializedConfig.Method_ConsoleColor);
        }
        #endregion

        #region NonSerialized 
        [NonSerialized()]
        private NonSerializedConfig nonSerializedConfig;
        public NonSerializedConfig NonSerializedConfig
        {
            get
            {
                if (nonSerializedConfig is null)
                    nonSerializedConfig = new NonSerializedConfig();

                nonSerializedConfig.Parent = this;

                return nonSerializedConfig;
            }
            set
            {
                nonSerializedConfig = value;
            }
        }
        #endregion
        [field: NonSerialized()]
        public static bool WriteInConsole_Default { get; set; }
        [field: NonSerialized()]
        public static bool IsSaveProgress { get; set; }
    }
    //[Serializable]
    public class NonSerializedConfig : ICloneable, IDisposable
    {
        public NonSerializedConfig()
        {
            WriteInConsole = ProgressOfUpdateAtStructAttribute.WriteInConsole_Default;
            IsInTask = true;
            StartDateTime = DateTime.Now;
        }

        #region IParentable<T>
        [field: NonSerialized()]
        public ProgressOfUpdateAtStructAttribute Parent { get; set; }
        #endregion

        [field: NonSerialized()]
        public DateTime StartDateTime { get; set; }
        [field: NonSerialized()]
        public bool IsInTask { get; set; }

        [field: NonSerialized()]
        public bool WriteInConsole { get; set; }

        [field: NonSerialized()]
        public bool IsStarted { get; set; }

        [NonSerialized()]
        private ProgressOfUpdateAtStructAttribute getOnEntry;
        public ProgressOfUpdateAtStructAttribute GetOnEntry
        {
            get
            {
                if (!(this.Parent is null))
                    this.Parent.OnEntry();

                return getOnEntry;
            }
            internal set
            {
                getOnEntry = value;
            }
        }

        [field: NonSerialized()]
        public string Method_ClassName { get; set; }

        [field: NonSerialized()]
        public string Method_MethodName { get; set; }

        [NonSerialized()]
        private ConsoleColor method_ConsoleColor;
        public ConsoleColor Method_ConsoleColor { get { return method_ConsoleColor; } set { method_ConsoleColor = value == ConsoleColor.Black ? ConsoleColor.Gray : value; } }

        /// <summary>
        /// Основной исполненяемый метод/делегат
        /// </summary>
        [field: NonSerialized()]
        public Action Method { get; set; }

        /// <summary>
        /// Исполненяемый метод/делегат для обновления времени
        /// </summary>
        [field: NonSerialized()]
        public Action UpdateMethod { get; set; } // Обновление статистики
        public object Clone()
        {
            return new NonSerializedConfig()
            {
                UpdateMethod = this.UpdateMethod,
                Method = this.Method,
                Method_ConsoleColor = this.Method_ConsoleColor,
                Method_MethodName = this.Method_MethodName,
                Method_ClassName = this.Method_ClassName,
                GetOnEntry = this.GetOnEntry is null ? null : (ProgressOfUpdateAtStructAttribute)this.GetOnEntry.Clone(),
                IsStarted = false,
                WriteInConsole = this.WriteInConsole
            };
        }
        public void Dispose()
        {

        }
    }
}
