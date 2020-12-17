using CommonDll.Client_Server;
using CommonDll.Structs.F_HR;
using CommonDll.Structs.F_LogFile.F_LogRecord;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDll.Structs.F_Users
{
    [Serializable]
    public class User : MainParentClass, IUpdateable_Helper<User>
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
        private protected override MainParentClass Base_UpdateObject_Original { get { return UpdateObject_Original; } set { UpdateObject_Original = (User)value; } }
        public override string Base_Name { get { return ClassName; } }

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
                          ("SELECT Count(*) FROM [User] " +
                          Select_WHERE,
                          new DbParameter[]
                          {
                        ConnectToDataBase.GetParammeter("@" + nameof(Login), Login, DbConnection),
                          });
                }

                return select;
            }
            set
            {
                select = value;
            }
        }
        public override string Select_WHERE { get { return "WHERE [Login] = @Login"; } }

        [field: NonSerialized()]
        KeyValuePair<string, DbParameter[]> select_FullMatch;
        public override KeyValuePair<string, DbParameter[]> Select_FullMatch
        {
            get
            {
                if (select_FullMatch.Key is null || select_FullMatch.Value is null)
                {
                    string s1 = string.Join(" AND ", Insert.Value.Where(x =>
                   !x.ParameterName.Contains("@FkId_Group")).Select(x => string.Format("[{0}] = @{0}", x.ParameterName.Replace("@", ""))));

                    return new KeyValuePair<string, DbParameter[]>
                               (string.Format("SELECT Count(*) FROM [User] WHERE {0} AND [FkId_Group] = (SELECT [Id] FROM [HR_Group] WHERE [Name] == @FkId_Group)", s1),
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
                        ConnectToDataBase.GetParammeter("@" + nameof(Login), Login, DbConnection),
                        ConnectToDataBase.GetParammeter("@" + nameof(Password), Password, DbConnection),
                    };

                    string s1 = string.Join(", ", Value.Select(x => string.Format("[{0}]", x.ParameterName.Replace("@", ""))));
                    string s2 = string.Join(", ", Value.Select(x => string.Format("@{0}", x.ParameterName.Replace("@", ""))));

                    Value = Value.Concat(new DbParameter[]
                       {
                            ConnectToDataBase.GetParammeter("@FkId_Worker", HR_Worker is null ? null : HR_Worker.Name, DbConnection),
                       }).ToArray();

                    return new KeyValuePair<string, DbParameter[]>
                          (string.Format("INSERT INTO [User] ({0},[FkId_Worker]) VALUES ({1},(SELECT [Id] FROM [HR_Group] WHERE [Name] == @FkId_Worker))", s1, s2),
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

                    return string.Format("UPDATE [User] SET {1}, " +
                        "[FkId_Worker] = (SELECT [Id] FROM [HR_Worker] WHERE [Name] == @FkId_Worker) " +
                        "WHERE [Login] = @{0}Login", WHERE_Reference, s1);
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
                    return "DELETE FROM [HR_Worker] " +
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

        public string Login { get; set; }
        public string Password { get; set; }
        public HR_Worker HR_Worker { get; set; }
        public User UpdateObject_Original { get; set; }
        public override object ClonePart()
        {
            return new User
            {
                Login = this.Login,
                Password = this.Password,
                HR_Worker = this.HR_Worker,

                Log = this.Log is null ? null : (LogRecord)this.Log.Clone(),
                UpdateObject_Original = this.UpdateObject_Original is null ? null : (User)this.UpdateObject_Original.Clone(),
            };
        }
    }
}
