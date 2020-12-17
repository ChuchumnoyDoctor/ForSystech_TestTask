using CommonDll.Client_Server;
using CommonDll.Structs.F_LogFile.F_LogRecord;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CommonDll.Structs.F_HR
{
    [Serializable]
    public class HR_Group : MainParentClass, IUpdateable_Helper<HR_Group>
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
        private protected override MainParentClass Base_UpdateObject_Original { get { return UpdateObject_Original; } set { UpdateObject_Original = (HR_Group)value; } }
        public override string Base_Name { get { return Name; } }

        #region Server Export Part
        [field: NonSerialized()]
        KeyValuePair<string, DbParameter[]> select;
        public override KeyValuePair<string, DbParameter[]> Select
        {
            get
            {
                if (select.Key is null || select.Value is null)
                {
                    return new KeyValuePair<string, DbParameter[]>
                          ("SELECT Count(*) FROM [HR_Group] " +
                          Select_WHERE,
                          new DbParameter[]
                          {
                              ConnectToDataBase.GetParammeter("@Name", Name, DbConnection),
                          });
                }

                return select;
            }
            set
            {
                select = value;
            }
        }
        public override string Select_WHERE { get { return "WHERE [Name] = @Name"; } }

        [field: NonSerialized()]
        KeyValuePair<string, DbParameter[]> select_FullMatch;
        public override KeyValuePair<string, DbParameter[]> Select_FullMatch
        {
            get
            {
                if (select_FullMatch.Key is null || select_FullMatch.Value is null)
                {
                    string s1 = string.Join(" AND ", Insert.Value.Select(x => string.Format("[{0}] = @{0}", x.ParameterName.Replace("@", ""))));

                    return new KeyValuePair<string, DbParameter[]>
                               (string.Format("SELECT Count(*) FROM [HR_Group] WHERE {0}", s1),
                               insert.Value);
                }

                return select_FullMatch;
            }
            set
            {
                select_FullMatch = value;
            }
        }

        [field: NonSerialized()]
        KeyValuePair<string, DbParameter[]> insert;
        internal protected override KeyValuePair<string, DbParameter[]> Insert
        {
            get
            {
                if (insert.Key is null || insert.Value is null)
                {
                    var Value = new DbParameter[]
                    {
                        ConnectToDataBase.GetParammeter("@" + nameof(Name), Name, DbConnection),
                        ConnectToDataBase.GetParammeter("@" + nameof(Percent), Percent, DbConnection),
                        ConnectToDataBase.GetParammeter("@" + nameof(MaxPercent), MaxPercent, DbConnection),
                        ConnectToDataBase.GetParammeter("@" + nameof(DeepLevelSubWorkers), DeepLevelSubWorkers, DbConnection),
                        ConnectToDataBase.GetParammeter("@" + nameof(PercentSubWorkers), PercentSubWorkers, DbConnection),
                    };

                    string s1 = string.Join(", ", Value.Select(x => string.Format("[{0}]", x.ParameterName.Replace("@", ""))));
                    string s2 = string.Join(", ", Value.Select(x => string.Format("@{0}", x.ParameterName.Replace("@", ""))));

                    return new KeyValuePair<string, DbParameter[]>
                          (string.Format("INSERT INTO [HR_Group] ({0}) VALUES ({1})", s1, s2),
                          Value);
                }

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
                if (update_Query is null)
                {
                    string s1 = string.Join(", ", Insert.Value.Where(x =>
                    !x.ParameterName.Contains("@FkId_Group")).Select(x => string.Format("[{0}] = @{0}", x.ParameterName.Replace("@", ""))));

                    return string.Format("UPDATE [HR_Group] SET {1}, " +
                        "[FkId_Group] = (SELECT [Id] FROM [HR_Group] WHERE [Name] == @FkId_Group) " +
                        "WHERE [Name] = @{0}Name", WHERE_Reference, s1);
                }

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
                if (delete is null)
                    return "DELETE FROM [HR_Group] " +
                        Select_WHERE;

                return delete;
            }
            set
            {
                delete = value;
            }
        }
        #endregion
        #endregion

        public string Name { get; set; }
        public int Percent { get; set; } // for year
        public int MaxPercent { get; set; }
        public int DeepLevelSubWorkers { get; set; } // -1 all, 0 - none, 1 - first level, 2 - second and etc
        public float PercentSubWorkers { get; set; }
        public HR_Group UpdateObject_Original { get; set; }
        public override object ClonePart()
        {
            return new HR_Group
            {
                Name = this.Name,
                Percent = this.Percent,
                MaxPercent = this.MaxPercent,
                DeepLevelSubWorkers = this.DeepLevelSubWorkers,
                PercentSubWorkers = this.PercentSubWorkers,

                Log = this.Log is null ? null : (LogRecord)this.Log.Clone(),
                UpdateObject_Original = this.UpdateObject_Original is null ? null : (HR_Group)this.UpdateObject_Original.Clone(),
            };
        }
    }
}
