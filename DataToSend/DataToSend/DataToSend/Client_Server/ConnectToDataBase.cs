using CommonDll.Helps;
using CommonDll.Structs.F_ProgressOfUpdate;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace CommonDll.Client_Server
{
    /// <summary>
    /// Подключение к БД
    /// </summary>
    public static class ConnectToDataBase
    {
        #region Подключение к БД
        /// <summary>
        /// Проверка подключения
        /// </summary>
        /// <param name="ExceptionMessage"></param>
        /// <returns></returns>
        public static bool CheckConnect(DbConnection DbConnection, out string ExceptionMessage)
        {
            return CheckConnect(DbConnection, out ExceptionMessage, 0);
        }
        public static bool CheckConnect(DbConnection DbConnection, out string ExceptionMessage, int RecurseTry)
        {
            bool IsOpened = false;
            ExceptionMessage = null;

            if (DbConnection is null ? false : DbConnection.State == ConnectionState.Closed)
                try
                {
                    DbConnection.Open();
                }
                catch (Exception ex)
                {
                    ExceptionMessage = ex.Message.ToString();
                }

            if (DbConnection is null ? false : DbConnection.State == ConnectionState.Open)
                IsOpened = true;
            else if (DbConnection is null ? false : DbConnection.State == ConnectionState.Connecting & RecurseTry < 3)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                RecurseTry++;
                IsOpened = CheckConnect(DbConnection, out ExceptionMessage, RecurseTry);
            }
            else
                ExceptionMessage = "Not connected to database";

            return IsOpened;
        }
        public static DbCommand GetCommand(string cmdText, DbConnection DbConnection, int CommandTimeout)
        {
            if (DbConnection is SqlConnection)
                return new SqlCommand(cmdText, (SqlConnection)DbConnection) { CommandTimeout = CommandTimeout };
            else if (DbConnection is OleDbConnection)
                return new OleDbCommand(cmdText, (OleDbConnection)DbConnection) { CommandTimeout = CommandTimeout };
            else if (DbConnection is SQLiteConnection)
                return new SQLiteCommand(cmdText, (SQLiteConnection)DbConnection) { CommandTimeout = CommandTimeout };
            else
                return default(DbCommand);
        }
        public static DbDataAdapter GetAdapter(DbCommand cmd, DbConnection DbConnection)
        {
            if (DbConnection is SqlConnection)
                return new SqlDataAdapter((SqlCommand)cmd);
            else if (DbConnection is OleDbConnection)
                return new OleDbDataAdapter((OleDbCommand)cmd);
            else if (DbConnection is SQLiteConnection)
                return new SQLiteDataAdapter((SQLiteCommand)cmd);
            else
                return default(DbDataAdapter);
        }
        public static DbParameter GetParammeter<T>(string ParammeterName, T Value, DbConnection DbConnection)
        {
            if (DbConnection is SqlConnection)
                return new SqlParameter(ParammeterName, Value);
            else if (DbConnection is OleDbConnection)
                return new OleDbParameter(ParammeterName, Value);
            else if (DbConnection is SQLiteConnection)
                return new SQLiteParameter(ParammeterName, Value);
            else
                return default(DbParameter);
        }
        public static OleDbConnection ConnectToAccess(string m_db_path)
        {
            OleDbConnection m_db_conn = new OleDbConnection();
            AppDomain.CurrentDomain.ProcessExit += delegate (object sender, EventArgs e) // Close connect to BD;
            {
                try
                {
                    m_db_conn.Close();
                }
                catch { }
            };
            m_db_conn = TryToConnectToDB(m_db_path).Item1; // Open connect to BD;       

            return m_db_conn;
        }
        [HandleProcessCorruptedStateExceptions()]
        [SecurityCritical]
        public static Tuple<OleDbConnection, string> TryToConnectToDB(string DBPath, [Optional] int Times)
        {
            OleDbConnection m_db_conn = new OleDbConnection();

            m_db_conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DBPath + ';';

            if (string.IsNullOrEmpty(DBPath))
                return new Tuple<OleDbConnection, string>(m_db_conn, "Ошибка на пути к БД: " + string.IsNullOrEmpty(DBPath).ToString()); //  Код исполнения при не успешном исполнении.

            if (!File.Exists(DBPath))
                return new Tuple<OleDbConnection, string>(m_db_conn, "Ошибка на пути к БД: " + File.Exists(DBPath).ToString()); //  Код исполнения при не успешном исполнении.

            try
            {
                m_db_conn.Open();
                string Exception = "";

                return new Tuple<OleDbConnection, string>(m_db_conn, Exception); //  Код исполнения при успешном исполнении.
            }
            catch (NullReferenceException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
            catch (InvalidOperationException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<OleDbConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToDB(DBPath, Times);
                }
            }
            catch (AccessViolationException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<OleDbConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToDB(DBPath, Times);
                }
            }
            catch (StackOverflowException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<OleDbConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToDB(DBPath, Times);
                }
            }
            catch (SEHException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<OleDbConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToDB(DBPath, Times);
                }
            }
            catch (OutOfMemoryException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<OleDbConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToDB(DBPath, Times);
                }
            }
            catch (Exception ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<OleDbConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToDB(DBPath, Times);
                }
            }
        }

        #region Azure
        private static SqlConnection azureConnection;
        public static SqlConnection AzureConnection
        {
            get
            {
                if (azureConnection is null ? true : azureConnection.State != ConnectionState.Open)
                {
                    azureConnection = TryToConnectToAzure().Item1;
                    azureConnection.Open();
                }

                return azureConnection;
            }
        }
        internal static Tuple<SqlConnection, string> TryToConnectToAzure([Optional] bool Retry)
        {
            SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();
            cb.DataSource = "";
            cb.UserID = "";
            cb.Password = "";
            cb.InitialCatalog = "";
            cb.MultipleActiveResultSets = true;
            cb.ConnectTimeout = (int)TimeSpan.FromMinutes(1).TotalSeconds;

            try
            {
                SqlConnection connection = new SqlConnection(cb.ConnectionString);
                connection.Open();

                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();

                return new Tuple<SqlConnection, string>(connection, "");
            }
            catch (Exception ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", "Retry connect to SQL Azure.", ConsoleColor.Red);

                if (Retry)
                {
                    return TryToConnectToAzure(true);
                }
                else
                {
                    return new Tuple<SqlConnection, string>(null, "Failed coonect");
                }
            }
        }
        #endregion

        #region SQLite
        public static SQLiteConnection sqlite_conn { get; set; }
        public static Tuple<SQLiteConnection, string> TryToConnectToSQLite(string DBPath, [Optional] int Times)
        {
            SQLiteConnection m_db_conn = new SQLiteConnection();
            m_db_conn.ConnectionString = string.Format(@"Data Source={0}; Version=3; PRAGMA journal_mode = WAL; PRAGMA synchronous = NORMAL;", DBPath);

            if (string.IsNullOrEmpty(DBPath))
                return new Tuple<SQLiteConnection, string>(m_db_conn, "Ошибка на пути к БД: " + string.IsNullOrEmpty(DBPath).ToString()); //  Код исполнения при не успешном исполнении.

            if (!File.Exists(DBPath))
                return new Tuple<SQLiteConnection, string>(m_db_conn, "Ошибка на пути к БД: " + File.Exists(DBPath).ToString()); //  Код исполнения при не успешном исполнении.

            try
            {
                m_db_conn.Open();
                string Exception = "";

                return new Tuple<SQLiteConnection, string>(m_db_conn, Exception); //  Код исполнения при успешном исполнении.
            }
            catch (NullReferenceException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
            catch (InvalidOperationException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<SQLiteConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToSQLite(DBPath, Times);
                }
            }
            catch (AccessViolationException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<SQLiteConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToSQLite(DBPath, Times);
                }
            }
            catch (StackOverflowException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<SQLiteConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToSQLite(DBPath, Times);
                }
            }
            catch (SEHException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<SQLiteConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToSQLite(DBPath, Times);
                }
            }
            catch (OutOfMemoryException ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<SQLiteConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToSQLite(DBPath, Times);
                }
            }
            catch (Exception ex)
            {
                string Exception = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", Exception, ConsoleColor.Red);
                Times++;

                if (Times == 5)
                {
                    return new Tuple<SQLiteConnection, string>(null, Exception);
                }
                else
                {
                    return TryToConnectToSQLite(DBPath, Times);
                }
            }
        }
        #endregion
        #endregion

        #region Command to database
        private static object LockObjectOnCommand { get; set; } = new object();
        public static bool MainExport(DbConnection DbConnection, string CommandText, ProgressOfUpdateAtStructAttribute Parent, out TimeSpan TimeOnQuery, [Optional] DbParameter[] Parameters, [Optional] int RecurseTry)
        {
            bool IsExported = false;
            string LocalName = Parameters is null ? null : Parameters.Count() > 0 ? Parameters[0].Value + "" : null;
            TimeSpan Start = DateTime.Now.TimeOfDay;

            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(MainExport) + "_" + LocalName);
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                if (ConnectToDataBase.CheckConnect(DbConnection, out string Except))
                    using (DbCommand cmd = ConnectToDataBase.GetCommand(CommandText, DbConnection, (int)TimeSpan.FromSeconds(20).TotalSeconds))
                    {
                        if (Parameters is null ? false : Parameters.Count() > 0)
                        {
                            foreach (var r in Parameters)
                                r.Value = GetOrDBNull(r.Value);

                            cmd.Parameters.AddRange(Parameters.Select(x => GetParammeter(x.ParameterName, x.Value, DbConnection)).ToArray());
                        }

                        lock (LockObjectOnCommand)
                            if (MainExport(cmd, Progress, RecurseTry))
                                IsExported = true;
                    }
                else
                    Progress.ExceptionMessage = "Not connected to database";
            });
            Progress.Start();
            TimeOnQuery = DateTime.Now.TimeOfDay - Start;


            if (IsExported)
                ConsoleWriteLine.WriteInConsole(null, null, "Done", string.Format("Exported: {0}. Time: {1}", LocalName, TimeOnQuery), ConsoleColor.White);
            else
                ConsoleWriteLine.WriteInConsole(null, null, "Done", string.Format("Export failed: {0}. Time: {1}", LocalName, TimeOnQuery), ConsoleColor.White);

            if (TimeOnQuery > TimeSpan.FromSeconds(5))
            {

            }

            if (TimeOnQuery > TimeSpan.FromSeconds(10))
            {

            }

            if (TimeOnQuery > TimeSpan.FromSeconds(15))
            {

            }

            return IsExported;
        }
        public static bool MainExport(DbCommand cmd, ProgressOfUpdateAtStructAttribute Progress, [Optional] int RecurseTry)
        {
            bool IsExported = false;

            try
            {
                cmd.ExecuteNonQuery();
                IsExported = true;
            }
            catch (DbException ex)
            {
                cmd.Cancel();

                if (!ex.Message.ToString().Contains("locked"))
                    RecurseTry++;

                if (RecurseTry < 5)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    IsExported = MainExport(cmd, Progress, RecurseTry);
                }
                else
                    Progress.ExceptionMessage = ex.Message.ToString();
            }
            catch (InvalidOperationException ex)
            {
                cmd.Cancel();
                Progress.ExceptionMessage = ex.Message.ToString();
            }
            catch (NullReferenceException ex)
            {
                cmd.Cancel();
                Progress.ExceptionMessage = ex.Message.ToString();
            }

            return IsExported;
        }
        public static bool MainImport(DbConnection DbConnection, string CommandText, out DataTable DataReader_Fill, ProgressOfUpdateAtStructAttribute Parent, out TimeSpan TimeOnQuery, [Optional] DbParameter[] Parameters, [Optional] int RecurseTry)
        {
            bool IsImported = false;
            DataTable DataReader_Fill_Anonym = new DataTable();
            string LocalName = Parameters is null ? null : Parameters.Count() > 0 ? Parameters[0].Value + "" : null;
            TimeSpan Start = DateTime.Now.TimeOfDay;

            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(MainImport) + "_" + LocalName);
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                if (ConnectToDataBase.CheckConnect(DbConnection, out string Except))
                    using (DbCommand cmd = ConnectToDataBase.GetCommand(CommandText, DbConnection, (int)TimeSpan.FromSeconds(20).TotalSeconds))
                    {
                        if (Parameters is null ? false : Parameters.Count() > 0)
                        {
                            foreach (var r in Parameters)
                                r.Value = GetOrDBNull(r.Value);

                            cmd.Parameters.AddRange(Parameters.Select(x => GetParammeter(x.ParameterName, x.Value, DbConnection)).ToArray());
                        }

                        DbDataAdapter sqlDataReader = ConnectToDataBase.GetAdapter(cmd, DbConnection);

                        try
                        {
                            sqlDataReader.Fill(DataReader_Fill_Anonym);
                            IsImported = true;
                        }
                        catch (DbException ex)
                        {
                            cmd.Cancel();

                            if (!ex.Message.ToString().Contains("locked"))
                                RecurseTry++;

                            if (RecurseTry < 5)
                                MainImport(DbConnection, CommandText, out DataReader_Fill_Anonym, Progress, out TimeSpan TimeOnQuery, Parameters, RecurseTry);
                            else
                                Progress.ExceptionMessage = ex.Message.ToString();
                        }
                        catch (InvalidOperationException ex)
                        {
                            cmd.Cancel();
                            Progress.ExceptionMessage = ex.Message.ToString();
                        }
                        catch (NullReferenceException ex)
                        {
                            cmd.Cancel();
                            Progress.ExceptionMessage = ex.Message.ToString();
                        }
                    }
                else
                    Progress.ExceptionMessage = "Not connected to database";
            });
            Progress.Start();
            DataReader_Fill = DataReader_Fill_Anonym;
            TimeOnQuery = DateTime.Now.TimeOfDay - Start;

            if (IsImported)
                ConsoleWriteLine.WriteInConsole(null, null, "Done", string.Format("Imported: {0}. Rows: {1}. Time: {2}", LocalName, DataReader_Fill_Anonym.Rows.Count, TimeOnQuery), ConsoleColor.White);
            else
                ConsoleWriteLine.WriteInConsole(null, null, "Done", string.Format("Import failed: {0}. Rows: {1}. Time: {2}", LocalName, DataReader_Fill_Anonym.Rows.Count, TimeOnQuery), ConsoleColor.White);

            if (TimeOnQuery > TimeSpan.FromSeconds(5))
            {

            }

            if (TimeOnQuery > TimeSpan.FromSeconds(10))
            {

            }

            if (TimeOnQuery > TimeSpan.FromSeconds(15))
            {

            }

            return IsImported;
        }
        public static bool MainImport(DbConnection DbConnection, string CommandText, out object ExecuteScalar, ProgressOfUpdateAtStructAttribute Parent, out TimeSpan TimeOnQuery, [Optional] DbParameter[] Parameters, [Optional] int RecurseTry)
        {
            bool IsImported = false;
            object ExecuteScalar_Anonym = new object();
            string LocalName = Parameters is null ? null : Parameters.Count() > 0 ? Parameters[0].Value + "" : null;
            TimeSpan Start = DateTime.Now.TimeOfDay;

            ProgressOfUpdateAtStructAttribute Progress = Parent.NonSerializedConfig.GetOnEntry;
            Progress.SetName(nameof(MainImport) + "_" + LocalName);
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                if (ConnectToDataBase.CheckConnect(DbConnection, out string Except))
                    using (DbCommand cmd = ConnectToDataBase.GetCommand(CommandText, DbConnection, (int)TimeSpan.FromSeconds(20).TotalSeconds))
                    {
                        if (Parameters is null ? false : Parameters.Count() > 0)
                        {
                            foreach (var r in Parameters)
                                r.Value = GetOrDBNull(r.Value);

                            cmd.Parameters.AddRange(Parameters.Select(x => GetParammeter(x.ParameterName, x.Value, DbConnection)).ToArray());
                        }

                        DbDataAdapter sqlDataReader = ConnectToDataBase.GetAdapter(cmd, DbConnection);

                        try
                        {
                            ExecuteScalar_Anonym = cmd.ExecuteScalar();
                            IsImported = true;
                        }
                        catch (DbException ex)
                        {
                            cmd.Cancel();

                            if (!ex.Message.ToString().Contains("locked"))
                                RecurseTry++;

                            if (RecurseTry < 5)
                                MainImport(DbConnection, CommandText, out ExecuteScalar_Anonym, Progress, out TimeSpan TimeOnQuery, Parameters, RecurseTry);
                            else
                                Progress.ExceptionMessage = ex.Message.ToString();
                        }
                        catch (InvalidOperationException ex)
                        {
                            cmd.Cancel();
                            Progress.ExceptionMessage = ex.Message.ToString();
                        }
                        catch (NullReferenceException ex)
                        {
                            cmd.Cancel();
                            Progress.ExceptionMessage = ex.Message.ToString();
                        }
                    }
                else
                    Progress.ExceptionMessage = "Not connected to database";
            });
            Progress.Start();
            ExecuteScalar = ExecuteScalar_Anonym;
            TimeOnQuery = DateTime.Now.TimeOfDay - Start;

            if (IsImported)
                ConsoleWriteLine.WriteInConsole(null, null, "Done", string.Format("Imported: {0}. Object: {1}. Time: {2}", LocalName, ExecuteScalar_Anonym + "", TimeOnQuery), ConsoleColor.White);
            else
                ConsoleWriteLine.WriteInConsole(null, null, "Done", string.Format("Import failed: {0}. Object: {1}. Time: {2}", LocalName, ExecuteScalar_Anonym + "", TimeOnQuery), ConsoleColor.White);

            if (TimeOnQuery > TimeSpan.FromSeconds(5))
            {

            }

            if (TimeOnQuery > TimeSpan.FromSeconds(10))
            {

            }

            if (TimeOnQuery > TimeSpan.FromSeconds(15))
            {

            }

            return IsImported;
        }
        #endregion

        #region GetOrDBNull    
        private static dynamic GetOrDBNull(dynamic dynamicObject) // For Export on server
        {
            if (dynamicObject is null)
                return DBNull.Value;
            else if (dynamicObject is bool ? ((object)dynamicObject).Equals(Helper.ReturnDefault(dynamicObject)) : false)
                return dynamicObject;
            else if (dynamicObject is int ? ((object)dynamicObject).Equals(Helper.ReturnDefault(dynamicObject)) : false)
                return dynamicObject;
            else if (((object)dynamicObject).Equals(Helper.ReturnDefault(dynamicObject)))
                return DBNull.Value;
            else
                return dynamicObject;
        }
        #endregion
    }
}
