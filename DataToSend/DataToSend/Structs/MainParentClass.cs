using CommonDll.Client_Server;
using CommonDll.Structs.F_LogFile.F_LogRecord;
using CommonDll.Structs.F_LogFile.F_LogRecord.F_TypeRecord;
using CommonDll.Structs.F_ProgressOfUpdate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;

namespace CommonDll.Structs
{
    [Serializable]
    public abstract class MainParentClass : ICloneable, IParentable<MainParentClass>
    {
        internal virtual Action<MainParentClass> AlternativeExportAction { get; } // not to database
        private void InvokeAction()
        {
            if (!(AlternativeExportAction is null) & (this.GetTypeRecord is null ? false : this.GetTypeRecord.Exproted))
                AlternativeExportAction.Invoke(this);
        }
        #region Methods     
        /// <summary> 
        /// Смотреть в интерфейсе <seealso cref="IChangeable_Exportable{T}.ToChanged"/>
        /// </summary>
        public static bool ToChanged(MainParentClass From, MainParentClass To, out TimeSpan TimeSpend, bool CollectChanges, out string Exception)
        {
            lock (To)
            {
                TimeSpan Start = DateTime.Now.TimeOfDay;
                bool Changed = false;
                Exception = null;

                if (!(From is null))
                    if (!(To is null))
                    {
                        if (From.GetType() == To.GetType() & (From.Base_Name == To.Base_Name))
                        {
                            List<MainParentClass> To_Main = To.Base_Childs is null ? new List<MainParentClass>() : To.Base_Childs.Where(x => !(x is null)).ToList();
                            List<MainParentClass> From_Main = From.Base_Childs is null ? new List<MainParentClass>() : From.Base_Childs.Where(x => !(x is null)).ToList();
                            List<MainParentClass> To_CopyOfMain = To_Main.Select(x => x).ToList();

                            if (From_Main is null ? false : From_Main.Count > 0)
                                foreach (var From_Ch in From_Main)
                                    if (MainParentClass.ToFindTypeRecord(From_Ch, out TimeSpan TimeSpendd, out MainParentClass Finde))
                                    {
                                        var TypeRecord = From_Ch.GetTypeRecord;
                                        bool Updated = false;

                                        foreach (var To_Ch in To_CopyOfMain)
                                            if (From_Ch.GetType() == To_Ch.GetType())
                                                if ((From_Ch.Base_Name == To_Ch.Base_Name) || (From_Ch.Base_UpdateObject_Original_WithUpdateParent is null ? false : From_Ch.Base_UpdateObject_Original_WithUpdateParent.Base_Name == To_Ch.Base_Name))
                                                {
                                                    if (MainParentClass.ToChanged(From_Ch, To_Ch, out TimeSpan Time, CollectChanges, out Exception)) // Изменение деток
                                                    {
                                                        Changed = true;
                                                        Exception = null;

                                                        if (CollectChanges) // Для коллекционирования изменений, всегда оставляем только ветку с измениями                                                           
                                                            To_Main = new List<MainParentClass>()
                                                            {
                                                                (MainParentClass)To_Ch.Clone(), // Добавили изменение
                                                            }.Concat(To_Main.Where(x => MainParentClass.ToFindTypeRecord(x, out TimeSpan TimeSpendd, out MainParentClass Finde)).ToArray()).ToList(); // а также все предыдущие изменения
                                                    }
                                                    else
                                                        Exception = "Not changed childs";

                                                    if (TypeRecord is null ? false : !string.IsNullOrEmpty(TypeRecord.Text))
                                                        if (TypeRecord.Text == TypeRecord.Update)
                                                        {
                                                            MainParentClass UpdateObject_Original = From_Ch.Base_UpdateObject_Original_WithUpdateParent;
                                                            UpdateObject_Original = UpdateObject_Original is null ? From_Ch : UpdateObject_Original;
                                                            int IndexRemoved = -1;

                                                            if (!(UpdateObject_Original is null))
                                                            {
                                                                var FindedIndex_Original = To_Main.FindIndex(x => x.Base_Name == UpdateObject_Original.Base_Name);
                                                                var FindRecord_Original = FindedIndex_Original > -1 ? To_Main.ElementAt(FindedIndex_Original) : null;

                                                                if (!(FindRecord_Original is null))
                                                                {
                                                                    To_Main.Remove(FindRecord_Original);
                                                                    IndexRemoved = FindedIndex_Original;
                                                                }
                                                            }

                                                            var FindedIndex = To_Main.FindIndex(x => x.Base_Name == From_Ch.Base_Name);
                                                            var Finded = FindedIndex > -1 ? To_Main.ElementAt(FindedIndex) : null;

                                                            if (!(Finded is null))
                                                            {
                                                                To_Main.Remove(Finded);
                                                                IndexRemoved = FindedIndex;
                                                            }

                                                            From_Ch.Base_Childs = To_Ch.Base_Childs; // Перенос деток
                                                            From_Ch.Base_UpdateObject_Original_WithUpdateParent = default;
                                                            Changed = true;
                                                            Updated = true;
                                                            Exception = null;
                                                            var NewToMain = new List<MainParentClass>();

                                                            if (IndexRemoved > -1)
                                                                for (int i = 0; i < IndexRemoved; i++)
                                                                    NewToMain.Add(To_Main[i]);
                                                            else
                                                                NewToMain = To_Main;

                                                            if (CollectChanges) // Для коллекционирования изменений, всегда оставляем только ветку с измениями
                                                                NewToMain = new List<MainParentClass>()
                                                                {
                                                                    (MainParentClass)From_Ch.Clone(), // Добавили изменение
                                                                }.Concat(NewToMain.Where(x => MainParentClass.ToFindTypeRecord(x, out TimeSpan TimeSpendd, out MainParentClass Finde)).ToArray()).ToList(); // а также все предыдущие изменения
                                                            else
                                                                NewToMain.Add(LogRecord.LogFile_Clear((MainParentClass)From_Ch.Clone())); // очистка лога        

                                                            if (IndexRemoved > -1)
                                                            {
                                                                for (int i = IndexRemoved; i < To_Main.Count; i++)
                                                                    NewToMain.Add(To_Main[i]);

                                                                To_Main = NewToMain;
                                                            }
                                                        }
                                                        else if (TypeRecord.Text == TypeRecord.Delete)
                                                        {
                                                            Changed = true;
                                                            Updated = true;
                                                            Exception = null;

                                                            if (CollectChanges)
                                                            {
                                                                From_Ch.Base_Childs = To_Ch.Base_Childs; // Перенос деток
                                                                From_Ch.Base_UpdateObject_Original_WithUpdateParent = default;

                                                                To_Main = new List<MainParentClass>()
                                                                {
                                                                    (MainParentClass)From_Ch.Clone(),
                                                                }.Concat(To_Main.Where(x => MainParentClass.ToFindTypeRecord(x, out TimeSpan TimeSpendd, out MainParentClass Finde)).ToArray()).ToList();
                                                            }
                                                            else
                                                            {
                                                                var Finded = To_Main.FirstOrDefault(x => x.Base_Name == From_Ch.Base_Name);

                                                                if (!(Finded is null))
                                                                    To_Main.Remove(Finded);
                                                                else
                                                                    Exception = "To delete not finded";
                                                            }
                                                        }
                                                        else
                                                            Exception = "Not Updated and not Deleted";
                                                    else
                                                        Exception = "TypeRecord is null";
                                                }

                                        if (!Updated)
                                            if (TypeRecord is null ? false : !string.IsNullOrEmpty(TypeRecord.Text))
                                                if (TypeRecord.Text == TypeRecord.Insert || TypeRecord.Text == TypeRecord.Update)
                                                {
                                                    Changed = true;
                                                    Exception = null;

                                                    if (CollectChanges) // Для коллекционирования изменений, всегда оставляем только ветку с измениями
                                                        To_Main = new List<MainParentClass>()
                                                        {
                                                            (MainParentClass)From_Ch.Clone(),
                                                        }.Concat(To_Main.Where(x => MainParentClass.ToFindTypeRecord(x, out TimeSpan TimeSpendd, out MainParentClass Finde)).ToArray()).ToList();
                                                    else if (To_Main.FirstOrDefault(x => x.Base_Name == From_Ch.Base_Name) is null)
                                                    {
                                                        From_Ch.Base_UpdateObject_Original_WithUpdateParent = default;
                                                        To_Main.Add(LogRecord.LogFile_Clear((MainParentClass)From_Ch.Clone())); // очистка лога
                                                    }
                                                    else
                                                        Exception = "Not Inserted, because included";
                                                }
                                                else
                                                    Exception = "Not 'TypeRecord.Text == TypeRecord.Insert || TypeRecord.Text == TypeRecord.Update'";
                                            else
                                                Exception = "TypeRecord is null";
                                    }
                                    else
                                        Exception = "Not ToFindTypeRecord";

                            if (Changed)
                            {
                                List<MainParentClass> To_Main_Temp = new List<MainParentClass>();

                                foreach (var r in To_Main) // only unique
                                    if (!CollectChanges || MainParentClass.ToFindTypeRecord(r, out TimeSpan TimeSpendd, out MainParentClass Finde))
                                        if (To_Main_Temp.FirstOrDefault(z => z.Base_Name == r.Base_Name) is null)
                                            To_Main_Temp.Add(r);
                                        else
                                        {

                                        }
                                    else
                                    {

                                    }

                                To_Main = To_Main_Temp;
                                To.Base_Childs = To_Main.ToArray();
                                Exception = null;
                            }
                            else
                                Exception = "Not Changed";
                        }
                    }
                    else
                    {
                        To = From;
                        Changed = true;
                        Exception = null;
                    }

                TimeSpend = DateTime.Now.TimeOfDay - Start;

                return Changed;
            }
        }
        public static bool ToFindTypeRecord(MainParentClass From, out TimeSpan TimeSpend, out MainParentClass FindedObject)
        {
            lock (From)
            {
                FindedObject = default;
                bool Finded = false;
                TimeSpan Start = DateTime.Now.TimeOfDay;

                if (!(From is null))
                    if (From.GetTypeRecord is null)
                    {
                        List<MainParentClass> From_Temp = From.Base_Childs is null ? new List<MainParentClass>() : From.Base_Childs.Where(x => !(x is null)).ToList();

                        foreach (var From_Ch in From_Temp)
                            if (ToFindTypeRecord(From_Ch, out TimeSpan Time, out MainParentClass FindedObj))
                            {
                                Finded = true;
                                FindedObject = FindedObj;

                                break;
                            }
                            else
                            {

                            }
                    }
                    else
                    {
                        Finded = true;
                        FindedObject = From;
                    }

                TimeSpend = DateTime.Now.TimeOfDay - Start;

                return Finded;
            }
        }
        public static bool ToFindObject_AtMainStruct(MainParentClass From, MainParentClass ToFind, out TimeSpan TimeSpend, out MainParentClass FindedObject)
        {
            FindedObject = default;
            bool Finded = false;
            TimeSpan Start = DateTime.Now.TimeOfDay;

            if (!(From is null) & !(ToFind is null))
                if (From.GetType() != ToFind.GetType() ? true : From.Base_Name != ToFind.Base_Name)
                {
                    List<MainParentClass> From_Temp = From.Base_Childs is null ? new List<MainParentClass>() : From.Base_Childs.Where(x => !(x is null)).ToList();

                    foreach (var From_Ch in From_Temp)
                        if (ToFindObject_AtMainStruct(From_Ch, ToFind, out TimeSpan Time, out MainParentClass FindedObj))
                        {
                            Finded = true;
                            FindedObject = FindedObj;

                            break;
                        }
                        else
                        {

                        }
                }
                else
                {
                    Finded = true;
                    FindedObject = From;
                }

            TimeSpend = DateTime.Now.TimeOfDay - Start;

            return Finded;
        }
        public static bool CheckOnSelect(MainParentClass Selected, out string ExceptionMessageOnSelect, ProgressOfUpdateAtStructAttribute Progress, [Optional] int RecurseTry)
        {
            int IsExist = -1;
            ExceptionMessageOnSelect = null;

            #region Check on Select
            if (!(Selected.Select.Key is null) & !(Selected.Select.Value is null))
            {
                ConnectToDataBase.MainImport(Selected.DbConnection, Selected.Select.Key, out object ExecuteScalar, Progress, out TimeSpan TimeOnQuery, Selected.Select.Value);

                string Result = ExecuteScalar + "";

                if (string.IsNullOrEmpty(Result) ? false : !Int32.TryParse(Result + "", out IsExist))
                {
                    RecurseTry++;

                    if (RecurseTry < 5)
                        return CheckOnSelect(Selected, out ExceptionMessageOnSelect, Progress, RecurseTry);
                    else
                    {
                        ExceptionMessageOnSelect = "Не удалось 'Int32.TryParse(ExecuteScalar() + '', out IsExist)'";
                        Progress.ExceptionMessage = ExceptionMessageOnSelect;
                    }
                }
            }
            #endregion

            return IsExist > 0 ? true : false;
        }
        public static bool CheckOnFullSelect(MainParentClass Selected, out string ExceptionMessageOnSelect, ProgressOfUpdateAtStructAttribute Progress, [Optional] int RecurseTry)
        {
            int IsExist = -1;
            ExceptionMessageOnSelect = null;

            #region Check on Select
            if (!(Selected.Select_FullMatch.Key is null) & !(Selected.Select_FullMatch.Value is null))
            {
                ConnectToDataBase.MainImport(Selected.DbConnection, Selected.Select_FullMatch.Key, out object ExecuteScalar, Progress, out TimeSpan TimeOnQuery, Selected.Select_FullMatch.Value);

                string Result = ExecuteScalar + "";

                if (string.IsNullOrEmpty(Result) ? false : !Int32.TryParse(Result + "", out IsExist))
                {
                    RecurseTry++;

                    if (RecurseTry < 5)
                        return CheckOnSelect(Selected, out ExceptionMessageOnSelect, Progress, RecurseTry);
                    else
                    {
                        ExceptionMessageOnSelect = "Не удалось 'Int32.TryParse(ExecuteScalar() + '', out IsExist)'";
                        Progress.ExceptionMessage = ExceptionMessageOnSelect;
                    }
                }
            }
            #endregion

            return IsExist > 0 ? true : false;
        }
        public static bool ToExport(MainParentClass MainParentClass, bool ExportByServer, ProgressOfUpdateAtStructAttribute Parent)
        {
            bool IsExported = false;
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(ToExport) + string.Format(". Exported type: {0}. Name: {1}", MainParentClass is null ? null : MainParentClass.GetType().Name, MainParentClass is null ? null : MainParentClass.Base_Name));
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                if (!(MainParentClass is null))
                {
                    TypeRecord TypeRecord = MainParentClass.GetTypeRecord;

                    if ((TypeRecord is null ? false : !string.IsNullOrEmpty(TypeRecord.Text) & TypeRecord.Exproted) || ExportByServer)
                    {
                        if (!(MainParentClass.Select.Key is null) & !(MainParentClass.Select.Value is null) &
                        !(MainParentClass.Insert.Key is null) & !(MainParentClass.Insert.Value is null) &
                        (!(MainParentClass.Delete is null)))
                        {
                            int IsExist = -1;
                            string ExceptionMessageOnSelect = "";

                            MainParentClass Selected = MainParentClass.Base_UpdateObject_Original_WithUpdateParent is null ? MainParentClass : MainParentClass.Base_UpdateObject_Original_WithUpdateParent;
                            #region Check on Select
                            IsExist = MainParentClass.CheckOnSelect(Selected, out ExceptionMessageOnSelect, Progress) ? 1 : 0;
                            #endregion

                            if (string.IsNullOrEmpty(ExceptionMessageOnSelect))
                            {
                                if (((TypeRecord is null ? false : TypeRecord.Text == TypeRecord.Update) || ExportByServer) & IsExist > 0) // UPDATE
                                {
                                    bool IsFullExist = false;
                                    #region Check on full select
                                    IsFullExist = MainParentClass.CheckOnFullSelect(Selected, out ExceptionMessageOnSelect, Progress);
                                    #endregion

                                    if (!IsFullExist)
                                    {
                                        DbParameter[] Setted_Parameters = MainParentClass.Insert.Value;
                                        DbParameter[] WHERE_Parameters = Selected.Select.Value;

                                        if (WHERE_Parameters is null ? false : WHERE_Parameters.Count() > 0)
                                            foreach (var r in WHERE_Parameters)
                                                r.ParameterName = r.ParameterName.Replace("@", "@" + WHERE_Reference);

                                        Setted_Parameters = (Setted_Parameters is null ? new List<DbParameter>().ToArray() : Setted_Parameters).Concat(WHERE_Parameters is null ? new List<DbParameter>().ToArray() : WHERE_Parameters).ToArray();

                                        if (MainParentClass.BeforeUpdate())
                                            if (ToExport(MainParentClass.DbConnection, TypeRecord, MainParentClass.Update, Setted_Parameters, MainParentClass.Base_Childs, ExportByServer, Progress))
                                            {
                                                IsExported = true;
                                                MainParentClass.AfterUpdate();
                                            }
                                            else
                                            {

                                            }
                                    }
                                    else
                                    {

                                    }
                                }
                                else if (((TypeRecord is null ? false : TypeRecord.Text == TypeRecord.Insert || TypeRecord.Text == TypeRecord.Update) || ExportByServer) & IsExist == 0) // INSERT
                                {
                                    if (MainParentClass.BeforeInsert())
                                        if (ToExport(MainParentClass.DbConnection, TypeRecord, MainParentClass.Insert.Key, MainParentClass.Insert.Value, MainParentClass.Base_Childs, ExportByServer, Progress))
                                        {
                                            IsExported = true;
                                            MainParentClass.AfterInsert();
                                        }
                                        else
                                        {

                                        }
                                }
                                else if ((TypeRecord is null ? false : TypeRecord.Text == TypeRecord.Delete) & IsExist > 0) // DELETE
                                {
                                    if (MainParentClass.BeforeDelete())
                                        if (ToExport(MainParentClass.DbConnection, TypeRecord, MainParentClass.Delete, MainParentClass.Select.Value, MainParentClass.Base_Childs, ExportByServer, Progress))
                                        {
                                            IsExported = true;
                                            MainParentClass.AfterDelete();
                                        }
                                        else
                                        {

                                        }
                                }
                            }
                        }
                        else
                            MainParentClass.InvokeAction(); // вызов альтернативного экспорта
                    }

                    if (ToExport(MainParentClass.Base_Childs, ExportByServer, Progress))
                        IsExported = true;
                }
            });
            Progress.Start();

            return IsExported;
        }
        private static bool ToExport(DbConnection DbConnection, TypeRecord TypeRecord, string CommandText, DbParameter[] Parameters, MainParentClass[] Base_Childs, bool ExportByServer, ProgressOfUpdateAtStructAttribute Parent)
        {
            bool IsExported = false;
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            string Text = TypeRecord is null ? null : TypeRecord.Text;

            Progress.SetName(nameof(ToExport) + ". TypeRecord: " + Text);
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                if (CommandText != "")
                    if (Parameters is null ? false : Parameters.Count() > 0)
                    {
                        if (TypeRecord is null ? false : TypeRecord.Text == TypeRecord.Delete) // Экспорт, дочерних в случае, есль Delete. Порядок железный: сначала дети, потом родительский
                            if (!ToExport(Base_Childs, ExportByServer, Progress))
                            {

                            }
                            else
                            {

                            }

                        if (ConnectToDataBase.MainExport(DbConnection, CommandText, Progress, out TimeSpan TimeOnQuery, Parameters))
                            IsExported = true;
                    }
            });
            Progress.Start();

            return IsExported;
        }
        public static bool ToExport(MainParentClass[] Childs, bool ExportByServer, ProgressOfUpdateAtStructAttribute Parent)
        {
            bool IsExported = false;
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(ToExport) + ". Export for childs");
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                if (Childs is null ? false : Childs.Count() > 0)
                    foreach (var Child in Childs)
                        if (ToExport(Child, ExportByServer, Progress))
                            IsExported = true;
            });
            Progress.Start();

            return IsExported;
        }
        #endregion

        #region non static: abstract interface & public properties     
        #region abstract properties
        private protected abstract MainParentClass Base_UpdateObject_Original { get; set; }
        internal protected MainParentClass Base_UpdateObject_Original_WithUpdateParent
        {
            get
            {
                if (!(Base_UpdateObject_Original is null))
                    Base_UpdateObject_Original.Parent = this.Parent;

                return Base_UpdateObject_Original;
            }
            set
            {
                Base_UpdateObject_Original = value;

                if (!(Base_UpdateObject_Original is null))
                    Base_UpdateObject_Original.Parent = this.Parent;
            }
        }
        internal protected abstract MainParentClass[] Base_Childs { get; set; }
        public abstract string Base_Name { get; }

        #region Server part. Not use for a client. Строки и объекты экспорта. 
        /// <summary>
        /// Select по ключевым полям на нахождение строки
        /// </summary>
        public abstract KeyValuePair<string, DbParameter[]> Select { get; set; }
        public abstract string Select_WHERE { get; }

        /// <summary>
        /// Select на полное совпадение строки
        /// </summary>
        public abstract KeyValuePair<string, DbParameter[]> Select_FullMatch { get; set; }

        /// <summary>
        /// Название sql-параметра должно совпадать с параметром/свойством в классе
        /// </summary>
        internal protected abstract KeyValuePair<string, DbParameter[]> Insert { get; set; }
        internal protected virtual bool BeforeInsert() { return true; }
        internal protected virtual void AfterInsert() { }

        internal protected virtual string Update { get; set; }
        internal protected virtual bool BeforeUpdate() { return true; }
        internal protected virtual void AfterUpdate() { }

        /// <summary>
        /// Сценарий Update'а: Delete(Delete.Key, Delete.Value) => Insert(Insert.Key, Update), где Key - строка запроса, Value - SQL-параметры.
        /// </summary>
        internal const string WHERE_Reference = "Original_";

        internal protected abstract string Delete { get; set; }
        internal protected virtual bool BeforeDelete() { return true; }
        internal protected virtual void AfterDelete() { }
        internal virtual DbConnection DbConnection
        {
            get
            {
                return DefaultConnection;
            }
        }
        public static DbConnection DefaultConnection
        {
            get
            {
                return
                //ConnectToDataBase.azure_conn
                ConnectToDataBase.sqlite_conn
                ;
            }
        }
        #endregion
        #endregion

        #region properties
        /// <summary>
        /// Последний лог 2-ух минутной давности в лог-файле. Достаточно для передачи на сервер и принятия решения о внесении изменений.
        /// </summary>
        public TypeRecord GetTypeRecord
        {
            get
            {
                TypeRecord typeRecord = default;

                if (!(Log is null))
                    typeRecord = Log.TypeRecord;

                return typeRecord;
            }
        }

        private LogRecord log;
        public LogRecord Log
        {
            get
            {
                try
                {
                    if (!(log is null))
                        if (log.TypeRecord is null || log.DateRecord == default(DateTime))
                            log = null;
                }
                catch (NullReferenceException ex)
                {

                }

                try
                {
                    if (!(log is null))
                        log.Parent = this;
                }
                catch (NullReferenceException ex)
                {

                }

                return log;
            }
            set
            {
                if (log != value)
                    log = value;

                try
                {
                    if (!(log is null))
                        if (log.TypeRecord is null || log.DateRecord == default(DateTime))
                            log = null;
                }
                catch (NullReferenceException ex)
                {

                }
            }
        }

        /// <summary>
        /// Текущий класс объекта. 
        /// </summary>
        public string ClassName
        {
            get
            {
                return this.GetType().Name;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Передача интерфейса клонирования. Клонирование - передача значимых данных - обязательный элемент при работе со сценарием Update'а. 
        /// </summary>
        public object Clone()
        {
            try
            {
                return ClonePart();
            }
            catch (InvalidOperationException ex)
            {
                return ClonePart();
            }
            catch (InvalidCastException ex)
            {
                return ClonePart();
            }
        }

        /// <summary>
        /// Передача интерфейса клонирования. Клонирование - передача значимых данных - обязательный элемент при работе со сценарием Update'а. 
        /// </summary>
        public abstract object ClonePart();

        #region IParentable<T>
        [field: NonSerialized()]
        public MainParentClass Parent { get; set; }
        /// <summary>
        /// Получить родителя до указанного типа/колена
        /// </summary>
        /// <typeparam name="T">Тип искомого колена родителя: родитель родителя</typeparam>
        /// <returns></returns>
        public T GetParent<T>() where T : MainParentClass
        {
            var Parent = this.Parent;

            if (Parent is null)
                return null;
            else if (Parent is T)
                return (T)Parent;
            else
                return Parent.GetParent<T>();
        }
        #endregion
    }

    /// <summary>
    /// Часть сценария Update'а. Призван унифицировать именнование и вызов.
    /// </summary>
    internal interface IUpdateable_Helper<T>
    {
        /// <summary>
        /// Предназначено для хранения оригинальной копии по сценарию Update'а.
        /// </summary>
        T UpdateObject_Original { get; set; }
    }

    /// <summary>
    /// Интерфейс вызова родителя из дочернего(Ассоциация).
    /// </summary>
    public interface IParentable<T> where T : class
    {
        /// <summary>
        /// Родитель <seealso cref="T"/> дочернего элемента 
        /// </summary>
        /// <returns>Возвращает родителя</returns>
        T Parent { get; set; }
    }
}
