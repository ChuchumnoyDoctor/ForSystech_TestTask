using CommonDll.Client_Server;
using CommonDll.Structs;
using CommonDll.Structs.F_HR;
using CommonDll.Structs.F_ProgressOfUpdate;
using CommonDll.Structs.F_Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager_Server.Methods.MethodsProcess
{
    public class Users_Methods : ParentMethods
    {
        #region override
        #region Menu/Main  
        public override MainParentClass Menu_Virtual(ProgressOfUpdateAtStructAttribute Parent)
        {
            Users Users = new Users();
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(HR_Methods));
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                Tuple<SQLiteConnection, string> tuple = ConnectToDataBase.TryToConnectToSQLite(LocalSettings.NewBasePath);

                using (SQLiteConnection SQLiteConnection = tuple.Item1)
                {
                    Users.User_s = GetUsers(SQLiteConnection);
                }
            });
            Progress.Start();

            return Users;
        }
        #endregion
        #endregion
        public List<User> GetUsers(SQLiteConnection SQLiteConnection)
        {
            List<User> Users = new List<User>();
            string query = "SELECT [Login],[Password],[FkId_Worker] " +
                           "FROM [User] ";

            bool IsSelect = ConnectToDataBase.MainImport(SQLiteConnection,
                query,
                out DataTable dataTable,
                new ProgressOfUpdateAtStructAttribute(),
                out TimeSpan TimeOnQuerySelect);

            if (dataTable.Rows.Count > 0)
                foreach (DataRow row in dataTable.Rows)
                {
                    string Login = (row[0] + "").Trim();
                    string Password = (row[1] + "").Trim();
                    int FkId_Worker = int.TryParse((row[2] + "").Trim(), out int IP) ? IP : default;

                    Users.Add(new User()
                    {
                        Login = Login,
                        Password = Password,
                        HR_Worker = HR_Methods.GetWorker(FkId_Worker, SQLiteConnection)
                    });
                }
            else
            {

            }

            return Users;
        }
    }
}
