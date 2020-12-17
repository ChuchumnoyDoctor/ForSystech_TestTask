using CommonDll.Client_Server;
using CommonDll.Structs;
using CommonDll.Structs.F_HR;
using CommonDll.Structs.F_ProgressOfUpdate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager_Server.Methods.MethodsProcess
{
    public class HR_Methods : ParentMethods
    {
        #region override
        #region Menu/Main  
        public override MainParentClass Menu_Virtual(ProgressOfUpdateAtStructAttribute Parent)
        {
            HR HR = new HR();
            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(HR_Methods));
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                Tuple<SQLiteConnection, string> tuple = ConnectToDataBase.TryToConnectToSQLite(LocalSettings.NewBasePath);

                using (SQLiteConnection SQLiteConnection = tuple.Item1)
                {
                    HR.Workers_Hierarchy = GetWorkers(SQLiteConnection);
                    HR.Groups = GetGroups(SQLiteConnection);
                }
            });
            Progress.Start();

            return HR;
        }
        #endregion
        #endregion
       
        private static DataTable GetTable(SQLiteConnection SQLiteConnection)
        {
            string query = "SELECT [Name],[EnrollmentDate],[BaseRate],[FkId_Group],[Id] " +
                            "FROM [HR_Worker] ";

            bool IsSelect = ConnectToDataBase.MainImport(SQLiteConnection,
                query,
                out DataTable dataTable,
                new ProgressOfUpdateAtStructAttribute(),
                out TimeSpan TimeOnQuerySelect);

            return dataTable;
        }
        private static DataTable GetTable(int FkId_Chief, SQLiteConnection SQLiteConnection)
        {
            string query = "SELECT [Name],[EnrollmentDate],[BaseRate],[FkId_Group],[Id] " +
                            "FROM [HR_Worker] " +
                            "WHERE [FkId_Chief] = @FkId_Chief";

            bool IsSelect = ConnectToDataBase.MainImport(SQLiteConnection,
                query,
                out DataTable dataTable,
                new ProgressOfUpdateAtStructAttribute(),
                out TimeSpan TimeOnQuerySelect,
                new DbParameter[]
                {
                    ConnectToDataBase.GetParammeter("@" + nameof(FkId_Chief), FkId_Chief, SQLiteConnection),
                }
                );

            return dataTable;
        }
        public static List<HR_Worker> GetWorkers(int FkId_Chief, SQLiteConnection SQLiteConnection)
        {
            return GetWorkers(GetTable(FkId_Chief, SQLiteConnection), SQLiteConnection);
        }
        public static List<HR_Worker> GetWorkers(SQLiteConnection SQLiteConnection)
        {
            return GetWorkers(GetTable(SQLiteConnection), SQLiteConnection);
        }
        private static List<HR_Worker> GetWorkers(DataTable dataTable, SQLiteConnection SQLiteConnection)
        {
            List<HR_Worker> Workers_Hierarchy = new List<HR_Worker>();

            if (dataTable.Rows.Count > 0)
                foreach (DataRow row in dataTable.Rows)
                {
                    string Name = (row[0] + "").Trim(); ;
                    DateTime EnrollmentDate = DateTime.TryParse((row[1] + "").Trim(), out DateTime DED) ? DED : default;
                    float BaseRate = float.TryParse((row[2] + "").Trim(), out float FBR) ? FBR : default;
                    int FkId_Group = int.TryParse((row[3] + "").Trim(), out int IFIG) ? IFIG : default;
                    int Id = int.TryParse((row[4] + "").Trim(), out int II) ? II : default;
                    Workers_Hierarchy.Add(new HR_Worker()
                    {
                        Name = Name,
                        EnrollmentDate = EnrollmentDate,
                        BaseRate = BaseRate,
                        Group = GetGroup(FkId_Group, SQLiteConnection),
                        SubWorkers = GetWorkers(Id, SQLiteConnection)
                    });
                }
            else
            {

            }

            return Workers_Hierarchy;
        }
        public static HR_Worker GetWorker(int Id, SQLiteConnection SQLiteConnection)
        {
            HR_Worker HR_Worker = null;
            string query = "SELECT [Name],[EnrollmentDate],[BaseRate],[FkId_Group],[Id] " +
                            "FROM [HR_Worker] " +
                            "WHERE [Id] = @Id";

            bool IsSelect = ConnectToDataBase.MainImport(SQLiteConnection,
                query,
                out DataTable dataTable,
                new ProgressOfUpdateAtStructAttribute(),
                out TimeSpan TimeOnQuerySelect,
                new DbParameter[]
                {
                    ConnectToDataBase.GetParammeter("@" + nameof(Id), Id, SQLiteConnection),
                }
                );

            if (dataTable.Rows.Count == 1)
            {
                var row = dataTable.Rows[0];
                string Name = (row[0] + "").Trim(); ;
                DateTime EnrollmentDate = DateTime.TryParse((row[1] + "").Trim(), out DateTime DED) ? DED : default;
                float BaseRate = float.TryParse((row[2] + "").Trim(), out float FBR) ? FBR : default;
                int FkId_Group = int.TryParse((row[3] + "").Trim(), out int IFIG) ? IFIG : default;
                HR_Worker = new HR_Worker()
                {
                    Name = Name,
                    EnrollmentDate = EnrollmentDate,
                    BaseRate = BaseRate,
                    Group = HR_Methods.GetGroup(FkId_Group, SQLiteConnection),
                    SubWorkers = HR_Methods.GetWorkers(Id, SQLiteConnection)
                };
            }
            else
            {

            }

            return HR_Worker;
        }
        public static List<HR_Group> GetGroups(SQLiteConnection SQLiteConnection)
        {
            List<HR_Group> HR_Groups = new List<HR_Group>();
            string query = "SELECT [Percent],[MaxPercent],[DeepLevelSubWorkers],[PercentSubWorkers],[Name] " +
                           "FROM [HR_Group] ";

            bool IsSelect = ConnectToDataBase.MainImport(SQLiteConnection,
                query,
                out DataTable dataTable,
                new ProgressOfUpdateAtStructAttribute(),
                out TimeSpan TimeOnQuerySelect);

            if (dataTable.Rows.Count > 0)
                foreach (DataRow row in dataTable.Rows)
                {
                    int Percent = int.TryParse((row[0] + "").Trim(), out int IP) ? IP : default;
                    int MaxPercent = int.TryParse((row[1] + "").Trim(), out int IMP) ? IMP : default;
                    int DeepLevelSubWorkers = int.TryParse((row[2] + "").Trim(), out int IDLSW) ? IDLSW : default;
                    float PercentSubWorkers = float.TryParse((row[3] + "").Trim(), out float IPSW) ? IPSW : default;
                    string Name = (row[4] + "").Trim();

                    HR_Groups.Add(new HR_Group()
                    {
                        Name = Name,
                        Percent = Percent,
                        MaxPercent = MaxPercent,
                        DeepLevelSubWorkers = DeepLevelSubWorkers,
                        PercentSubWorkers = PercentSubWorkers
                    });
                }
            else
            {

            }

            return HR_Groups;
        }
        public static HR_Group GetGroup(int Id, SQLiteConnection SQLiteConnection)
        {
            HR_Group HR_Group = null;
            string query = "SELECT [Percent],[MaxPercent],[DeepLevelSubWorkers],[PercentSubWorkers],[Name] " +
                           "FROM [HR_Group] " +
                           "WHERE [Id] = @Id";

            bool IsSelect = ConnectToDataBase.MainImport(SQLiteConnection,
                query,
                out DataTable dataTable,
                new ProgressOfUpdateAtStructAttribute(),
                out TimeSpan TimeOnQuerySelect,
                new DbParameter[]
                {
                    ConnectToDataBase.GetParammeter("@" + nameof(Id), Id, SQLiteConnection),
                }
                );

            if (dataTable.Rows.Count == 1)
            {
                int Percent = int.TryParse((dataTable.Rows[0][0] + "").Trim(), out int IP) ? IP : default;
                int MaxPercent = int.TryParse((dataTable.Rows[0][1] + "").Trim(), out int IMP) ? IMP : default;
                int DeepLevelSubWorkers = int.TryParse((dataTable.Rows[0][2] + "").Trim(), out int IDLSW) ? IDLSW : default;
                float PercentSubWorkers = float.TryParse((dataTable.Rows[0][3] + "").Trim(), out float IPSW) ? IPSW : default;
                string Name = (dataTable.Rows[0][4] + "").Trim();

                HR_Group = new HR_Group()
                {
                    Name = Name,
                    Percent = Percent,
                    MaxPercent = MaxPercent,
                    DeepLevelSubWorkers= DeepLevelSubWorkers,
                    PercentSubWorkers = PercentSubWorkers
                };
            }
            else
            {

            }

            return HR_Group;
        }
    }
}
