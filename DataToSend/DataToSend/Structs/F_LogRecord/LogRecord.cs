using CommonDll.Structs.F_LogFile.F_LogRecord.F_TypeRecord;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CommonDll.Structs.F_LogFile.F_LogRecord
{
    /// <summary>
    /// Лог-запись. На основании её принимаются решения о внесении данных
    /// </summary>
    [Serializable]
    public class LogRecord : MainParentClass, IUpdateable_Helper<LogRecord>
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
        private protected override MainParentClass Base_UpdateObject_Original { get { return UpdateObject_Original; } set { UpdateObject_Original = (LogRecord)value; } }
        public override string Base_Name { get { return Id + ""; } }

        #region Server Export Part
        [field: NonSerialized()]
        KeyValuePair<string, DbParameter[]> select;
        public override KeyValuePair<string, DbParameter[]> Select
        {
            get
            {
                return select;
            }
            set
            {
                select = value;
            }
        }
        public override string Select_WHERE { get { return "WHERE [MaskFor_KeyWord] = @MaskFor_KeyWord and [KeyWord] = @KeyWord"; } }

        [field: NonSerialized()]
        public override KeyValuePair<string, DbParameter[]> Select_FullMatch { get; set; } // Переопределить

        [field: NonSerialized()]
        KeyValuePair<string, DbParameter[]> insert;
        internal protected override KeyValuePair<string, DbParameter[]> Insert
        {
            get
            {
                return insert;
            }
            set
            {
                insert = value;
            }
        }

        [field: NonSerialized()]
        string update_Query;
        internal protected override string Update
        {
            get
            {
                return update_Query;
            }
            set
            {
                update_Query = value;
            }
        }

        [field: NonSerialized()]
        string delete;
        internal protected override string Delete
        {
            get
            {
                return delete;
            }
            set
            {
                delete = value;
            }
        }
        #endregion
        #endregion
        public int Id { get; set; }
        public DateTime DateRecord { get; set; }
        public TypeRecord TypeRecord { get; set; }
        public new string Comment { get; set; }
        public override object ClonePart()
        {
            int Id = this.Id;
            DateTime DateRecord = this.DateRecord;
            TypeRecord TypeRecord = this.TypeRecord is null ? null : (TypeRecord)this.TypeRecord.Clone();
            string Comment = this.Comment is null ? null : (string)this.Comment.Clone();

            return new LogRecord
            {
                Id = Id,
                DateRecord = DateRecord,
                TypeRecord = TypeRecord,
                Comment = Comment,
                UpdateObject_Original = this.UpdateObject_Original is null ? null : (LogRecord)this.UpdateObject_Original.Clone(),
                Log = null,
            };
        }
        private LogRecord updateObject;
        public LogRecord UpdateObject_Original
        {
            get
            {
                if (!(updateObject is null))
                    if (updateObject.TypeRecord is null || updateObject.DateRecord == default(DateTime))
                        updateObject = null;

                return updateObject;
            }
            set
            {
                updateObject = value;

                if (!(updateObject is null))
                    if (updateObject.TypeRecord is null || updateObject.DateRecord == default(DateTime))
                        updateObject = null;

                if (!(updateObject is null))
                    updateObject.Parent = this.Parent;
            }
        }
        public static void LogFile_Modify(TypeRecord TypeRecord, string Comment, MainParentClass MainParentClass)
        {
            if (!(MainParentClass is null))
            {
                LogRecord logRecord = new LogRecord()
                {
                    Id = -1,
                    DateRecord = DateTime.Now,
                    TypeRecord = TypeRecord,
                    Comment = Comment
                };

                MainParentClass.Log = logRecord;

                if (MainParentClass.Base_Childs is null ? false : MainParentClass.Base_Childs.Count() > 0)
                    MainParentClass.Base_Childs = LogFile_Modify(TypeRecord, Comment, MainParentClass.Base_Childs);
            }
        }
        public static MainParentClass[] LogFile_Modify(TypeRecord TypeRecord, string Comment, MainParentClass[] MainParentClass)
        {
            foreach (var main in MainParentClass)
                LogFile_Modify(TypeRecord, Comment, main);

            return MainParentClass;
        }
        public static MainParentClass LogFile_Clear(MainParentClass MainParentClass)
        {
            if (!(MainParentClass is null))
            {
                MainParentClass.Log = null;

                if (MainParentClass.Base_Childs is null ? false : MainParentClass.Base_Childs.Count() > 0)
                    MainParentClass.Base_Childs = LogFile_Clear(MainParentClass.Base_Childs);
            }

            return MainParentClass;
        }
        public static MainParentClass[] LogFile_Clear(MainParentClass[] MainParentClass)
        {
            foreach (var main in MainParentClass)
                LogFile_Clear(main);

            return MainParentClass;
        }
    }
}
