using CommonDll.Client_Server;
using CommonDll.Structs;
using CommonDll.Structs.F_ProgressOfUpdate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager_Server.Methods.MethodsProcess
{
    public static class AzureToSQLite
    {
        static public void Start()
        {
            //Professionss();
            //TypesOfWork();
            //TypeOfRecord();
            //Statistic_WorksArea();
            //Statistic_WorkPlaces();
            //root_tasks();
            //subtasks();
            //details();
            //statistic();
            //Detail_Record();
            //Users();
            //Statistic_Sessions();
            //Statistic_Relations_SessionsPrograms();
        }
        private static void details()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT [i_count],(SELECT [s_subtask_name] FROM [subtasks] WHERE[Id] = [details].[i_parent_id]),[s_detail_name],[s_size],[t_process_time],(SELECT [s_task_name] FROM[root_tasks] WHERE[Id] = (SELECT [i_parent_id] FROM [subtasks] WHERE[Id] = [details].[i_parent_id])),(SELECT [subtasks].[i_parent_id] FROM [subtasks] WHERE[Id] = [details].[i_parent_id]),[i_parent_id] " +
                      "FROM[details]",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            int ICount = 0;
            
            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    ICount++;
                    int @i_count = int.TryParse(row[0] + "", out int Ii_count) ? Ii_count : default(int);
                    string @s_subtask_name = row[1] + "";
                    string @s_detail_name = row[2] + "";
                    string @s_size = row[3] + "";
                    string @t_process_time = row[4] + "";
                    string s_task_name = row[5] + "";                    

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[details] ([i_count],[i_parent_id],[s_detail_name],[s_size],[t_process_time]) " +
                                                  "VALUES (@i_count,(SELECT [Id] FROM [subtasks] WHERE[s_subtask_name] = @s_subtask_name AND [i_parent_id] = (SELECT [Id] FROM[root_tasks] WHERE[s_task_name] = @s_task_name)),@s_detail_name,@s_size,@t_process_time)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@i_count", @i_count, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_subtask_name", @s_subtask_name, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_detail_name", @s_detail_name, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_size", @s_size, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_process_time", @t_process_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_task_name", s_task_name, MainParentClass.DefaultConnection),
                                                  });

                    Console.WriteLine("{0}/{1}", ICount, @table_Programs.Rows.Count);
                }
        }
        private static void subtasks()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT [cut_length],[d_start_date],[holes],[i_count]," +
                      "(SELECT [s_task_name] FROM[root_tasks] WHERE[Id] = [subtasks].[i_parent_id])," +
                      "[i_progress],[idle_length],[is_manual],[priority],[s_material],[s_size],[s_subtask_name],[t_ellapsed_time],[t_list_time]," +
                      "[t_machine_time],[t_process_time],[t_setup_time],[t_start_time],[Weight] " +
                      "FROM [subtasks]",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            int ICount = 0;

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    ICount++;
                    float cut_length = float.TryParse(row[0] + "", out float fcut_length) ? fcut_length : default(float); ; 
                    DateTime @d_start_date = DateTime.TryParse(row[1] + "", out DateTime dd_start_date) ? dd_start_date : default(DateTime);
                    int @holes = int.TryParse(row[2] + "", out int iholes) ? iholes : default(int);
                    int @i_count = int.TryParse(row[3] + "", out int ii_count) ? ii_count : default(int);
                    string s_task_name = row[4] + "";
                    int @i_progress = int.TryParse(row[5] + "", out int ii_progress) ? ii_progress : default(int);
                    int @idle_length = int.TryParse(row[6] + "", out int iidle_length) ? iidle_length : default(int);
                    bool @is_manual = bool.TryParse(row[7] + "", out bool bis_manual) ? bis_manual : default(bool);
                    int @priority = int.TryParse(row[8] + "", out int ipriority) ? ipriority : default(int);
                    string @s_material = row[9] + ""; 
                    string @s_size = row[10] + "";
                    string @s_subtask_name = row[11] + "";
                    string @t_ellapsed_time = row[12] + "";
                    string @t_list_time = row[13] + "";
                    string @t_machine_time = row[14] + "";
                    string @t_process_time = row[15] + "";
                    string @t_setup_time = row[16] + "";
                    string @t_start_time = row[17] + "";
                    float @Weight = float.TryParse(row[18] + "", out float fWeight) ? fWeight : default(float);

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[subtasks] ([cut_length],[d_start_date],[holes],[i_count],[i_parent_id],[i_progress],[idle_length],[is_manual],[priority],[s_material],[s_size],[s_subtask_name],[t_ellapsed_time],[t_list_time]," +
                      "[t_machine_time],[t_process_time],[t_setup_time],[t_start_time],[Weight]) " +
                                                  "VALUES (@cut_length,@d_start_date,@holes,@i_count,(SELECT [Id] FROM[root_tasks] WHERE[s_task_name] = @s_task_name),@i_progress,@idle_length,@is_manual,@priority,@s_material,@s_size,@s_subtask_name,@t_ellapsed_time,@t_list_time," +
                      "@t_machine_time,@t_process_time,@t_setup_time,@t_start_time,@Weight)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@cut_length", cut_length, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@d_start_date", @d_start_date, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@holes", @holes, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@i_count", @i_count, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_task_name", s_task_name, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@i_progress", @i_progress, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@idle_length", @idle_length, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@is_manual", @is_manual, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@priority", @priority, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_material", @s_material, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_size", @s_size, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_subtask_name", @s_subtask_name, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_ellapsed_time", @t_ellapsed_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_list_time", @t_list_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_machine_time", @t_machine_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_process_time", @t_process_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_setup_time", @t_setup_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_start_time", @t_start_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@Weight", @Weight, MainParentClass.DefaultConnection),
                                                  });

                    Console.WriteLine("{0}/{1}", ICount, @table_Programs.Rows.Count);
                }
        }
        private static void statistic()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT [end_datetime],[expected_time],[list_name],[loading_time],[machine_time],[real_time],[setup_time],[start_datetime],[task_number]," +
                      "(SELECT [Name] FROM[Statistic_WorkPlaces] WHERE[Id] = [Fk_Id_Laser])," +
                      "[ChangedByManual],[CreatedByGenerateLog],[IsNightProgram],[IsNotSelected] " +
                      "FROM statistic",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    DateTime @end_datetime = DateTime.TryParse(row[0] + "", out DateTime Dend_datetime) ? Dend_datetime : default(DateTime);
                    string @expected_time = row[1] + "";
                    string @list_name = row[2] + "";
                    string @loading_time = row[3] + "";
                    string @machine_time = row[4] + "";
                    string @real_time = row[5] + "";
                    string @setup_time = row[6] + "";
                    DateTime @start_datetime = DateTime.TryParse(row[7] + "", out DateTime dstart_datetime) ? dstart_datetime : default(DateTime);
                    int @task_number = int.TryParse(row[8] + "", out int itask_number) ? itask_number : default(int);
                    string NameLaser = row[9] + "";
                    bool @ChangedByManual = bool.TryParse(row[10] + "", out bool bChangedByManual) ? bChangedByManual : default(bool);
                    bool @CreatedByGenerateLog = bool.TryParse(row[11] + "", out bool bCreatedByGenerateLog) ? bCreatedByGenerateLog : default(bool);
                    bool @IsNightProgram = bool.TryParse(row[12] + "", out bool bIsNightProgram) ? bIsNightProgram : default(bool);
                    bool @IsNotSelected = bool.TryParse(row[13] + "", out bool bIsNotSelected) ? bIsNotSelected : default(bool);

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[statistic] ([end_datetime],[expected_time],[list_name],[loading_time],[machine_time],[real_time],[setup_time],[start_datetime],[task_number],[Fk_Id_Laser],[ChangedByManual],[CreatedByGenerateLog],[IsNightProgram],[IsNotSelected]) " +
                                                  "VALUES (@end_datetime,@expected_time,@list_name,@loading_time,@machine_time,@real_time,@setup_time,@start_datetime,@task_number,(SELECT [Id] FROM[Statistic_WorkPlaces] WHERE[Name] = @NameLaser),@ChangedByManual,@CreatedByGenerateLog,@IsNightProgram,@IsNotSelected)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@end_datetime", @end_datetime, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@expected_time", @expected_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@list_name", @list_name, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@loading_time", @loading_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@machine_time", @machine_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@real_time", @real_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@setup_time", @setup_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@start_datetime", @start_datetime, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@task_number", @task_number, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@NameLaser", NameLaser, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@ChangedByManual", @ChangedByManual, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@CreatedByGenerateLog", @CreatedByGenerateLog, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@IsNightProgram", @IsNightProgram, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@IsNotSelected", @IsNotSelected, MainParentClass.DefaultConnection)
                                                  });
                }
        }
        private static void root_tasks()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                       "SELECT task_number,s_task_name,s_process_time,s_time_left,t_ellapsed_time,t_list_time,t_machine_time,t_setup_time,cut_length,holes,idle_length,priority " +
                       "FROM root_tasks",
                       out DataTable @table_Programs,
                       new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    int task_number = int.TryParse(row[0] + "", out int Itask) ? Itask : default(int);
                    string s_task_name = row[1] + ""; 
                    string s_process_time = row[2] + "";
                    string s_time_left = row[3] + "";
                    string t_ellapsed_time = row[4] + "";
                    string t_list_time = row[5] + "";
                    string t_machine_time = row[6] + "";
                    string t_setup_time = row[7] + "";
                    float cut_length = float.TryParse(row[8] + "", out float fCut) ? fCut : default(float);
                    int holes = int.TryParse(row[9] + "", out int Iholes) ? Iholes : default(int);
                    float idle_length = float.TryParse(row[10] + "", out float fidle_length) ? fidle_length : default(float);
                    int priority = int.TryParse(row[11] + "", out int Ipriority) ? Ipriority : default(int);

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[root_tasks] (task_number,s_task_name,s_process_time,s_time_left,t_ellapsed_time,t_list_time,t_machine_time,t_setup_time,cut_length,holes,idle_length,priority) " +
                                                  "VALUES (@task_number,@s_task_name,@s_process_time,@s_time_left,@t_ellapsed_time,@t_list_time,@t_machine_time,@t_setup_time,@cut_length,@holes,@idle_length,@priority)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@task_number", task_number, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_task_name", s_task_name, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_process_time", s_process_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@s_time_left", s_time_left, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_ellapsed_time", t_ellapsed_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_list_time", t_list_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_machine_time", t_machine_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@t_setup_time", t_setup_time, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@cut_length", cut_length, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@holes", holes, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@idle_length", idle_length, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@priority", priority, MainParentClass.DefaultConnection),
                                                  });
                }
        }
        private static void Users()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT [Surname],[MiddleName],[FirstName]," +
                      "(SELECT [NameOfProfession] FROM[Professionss] WHERE[Id] = [Fk_Id_Profession])," +
                      "[Id_User_InRegistrControlTime]," +
                      "(SELECT [TypeOfWork] FROM[TypesOfWork] WHERE[Id] = [Fk_Id_Default_TypeOfWork]) " +
                      "FROM Users",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    string @Surname = row[0] + "";
                    string @MiddleName = row[1] + "";
                    string @FirstName = row[2] + "";
                    string NameOfProfession = row[3] + "";
                    int @Id_User_InRegistrControlTime = int.TryParse(row[4] + "", out int iId_User_InRegistrControlTime) ? iId_User_InRegistrControlTime : default(int);
                    string TypeOfWork = row[5] + "";

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[Users] ([Surname],[MiddleName],[FirstName],[Fk_Id_Profession],[Id_User_InRegistrControlTime],[Fk_Id_Default_TypeOfWork]) " +
                                                  "VALUES (@Surname,@MiddleName,@FirstName,(SELECT [Id] FROM[Professionss] WHERE [NameOfProfession] = @NameOfProfession),@Id_User_InRegistrControlTime,(SELECT [Id] FROM[TypesOfWork] WHERE[TypeOfWork] = @TypeOfWork))",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@Surname", @Surname, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@MiddleName", @MiddleName, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@FirstName", @FirstName, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@NameOfProfession", NameOfProfession, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@Id_User_InRegistrControlTime", @Id_User_InRegistrControlTime, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@TypeOfWork", TypeOfWork, MainParentClass.DefaultConnection),
                                                  });
                }
        }
        private static void TypesOfWork()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT [TypeOfWork],[ShortDesignation] " +
                      "FROM TypesOfWork",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    string @TypeOfWork = row[0] + "";
                    string @ShortDesignation = row[1] + "";

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[TypesOfWork] ([TypeOfWork],[ShortDesignation]) " +
                                                  "VALUES (@TypeOfWork,@ShortDesignation)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@TypeOfWork", @TypeOfWork, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@ShortDesignation", @ShortDesignation, MainParentClass.DefaultConnection),
                                                  });
                }
        }
        private static void TypeOfRecord()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT [TypeOfRecord] " +
                      "FROM [TypeOfRecord]",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    string TypeOfRecord = row[0] + "";

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[TypeOfRecord] (TypeOfRecord) " +
                                                  "VALUES (@TypeOfRecord)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@TypeOfRecord", TypeOfRecord, MainParentClass.DefaultConnection),
                                                  });
                }
        }
        private static void Statistic_WorksArea()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT [Name],[RemarksText],[Name_RussianEquivalent] " +
                      "FROM [Statistic_WorksArea]",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    string @Name = row[0] + "";
                    string @RemarksText = row[1] + "";
                    string @Name_RussianEquivalent = row[2] + "";

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[Statistic_WorksArea]([Name],[RemarksText],[Name_RussianEquivalent]) " +
                                                  "VALUES (@Name, @RemarksText, @Name_RussianEquivalent)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@Name", @Name, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@RemarksText", @RemarksText, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@Name_RussianEquivalent", @Name_RussianEquivalent, MainParentClass.DefaultConnection),
                                                  });
                }
        }
        private static void Statistic_WorkPlaces()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT (SELECT [Name] FROM[Statistic_WorksArea] WHERE[Id] = [Fk_Id_WorkArea]),[Name] " +
                      "FROM Statistic_WorkPlaces",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    string NameArea = row[0] + "";
                    string Name = row[1] + "";

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[Statistic_WorkPlaces] ([Fk_Id_WorkArea],[Name]) " +
                                                  "VALUES ((SELECT [Id] FROM[Statistic_WorksArea] WHERE[Name] = @NameArea),@Name)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@NameArea", NameArea, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@Name", Name, MainParentClass.DefaultConnection),
                                                  });
                }
        }
        private static void Statistic_Sessions()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT (SELECT [Surname] FROM[Users] WHERE[Id] = [Fk_Id_User])," +
                      "(SELECT [Name] FROM[Statistic_WorkPlaces] WHERE[Id] = [Fk_Id_WorkPlace])," +
                      "[IpAddress],[DateStart],[DateEnd] " +
                      "FROM [Statistic_Sessions]",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    string Surname = row[0] + "";
                    string Name = row[1] + "";
                    string @IpAddress = row[2] + "";
                    DateTime @DateStart = DateTime.TryParse(row[3] + "", out DateTime DDateStart) ? DDateStart : default(DateTime);
                    DateTime @DateEnd = DateTime.TryParse(row[4] + "", out DateTime DDateEnd) ? DDateEnd : default(DateTime);

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[Statistic_Sessions] ([Fk_Id_User],[Fk_Id_WorkPlace],[IpAddress],[DateStart],[DateEnd]) " +
                                                  "VALUES ((SELECT [Id] FROM[Users] WHERE[Surname] = @Surname),(SELECT [Id] FROM[Statistic_WorkPlaces] WHERE[Name] = @Name),@IpAddress,@DateStart,@DateEnd)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@Surname", Surname, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@Name", Name, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@IpAddress", @IpAddress, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@DateStart", @DateStart, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@DateEnd", @DateEnd, MainParentClass.DefaultConnection),
                                                  });
                }
        }
        private static void Statistic_Relations_SessionsPrograms()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT (SELECT [MaskFor_KeyWord]FROM[Detail_Record] WHERE[Id] = [Fk_Id_Program_RecordArea]), " +
                      "(SELECT[KeyWord] FROM[Detail_Record] WHERE[Id] = [Fk_Id_Program_RecordArea])," +
                      "(SELECT [list_name] FROM[statistic] WHERE[Id] = [Fk_Id_Program_RecordLaser])," +
                      "(SELECT [DateStart] FROM[Statistic_Sessions] Where[Id] = [Fk_Id_Session])," +
                      "[NameOfTableOfProgram] " +
                      "FROM Statistic_Relations_SessionsPrograms",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    string MaskFor_KeyWord = row[0] + "";
                    string KeyWord = row[1] + "";
                    string list_name = row[2] + "";
                    DateTime @DateStart = DateTime.TryParse(row[3] + "", out DateTime IFk_Id_Session) ? IFk_Id_Session : default(DateTime);
                    string @NameOfTableOfProgram = row[4] + "";

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO [Statistic_Relations_SessionsPrograms] ([Fk_Id_Program_RecordArea],[Fk_Id_Program_RecordLaser],[Fk_Id_Session],[NameOfTableOfProgram]) " +
                                                  "VALUES (" +
                                                  "(SELECT [Id] FROM Detail_Record WHERE [KeyWord] = @KeyWord AND [MaskFor_KeyWord] = @MaskFor_KeyWord)," +
                                                  "(SELECT [Id] FROM [statistic] WHERE [list_name] = @list_name)," +
                                                  "(SELECT [Id] FROM[Statistic_Sessions] Where[DateStart] = @DateStart)," +
                                                  "@NameOfTableOfProgram)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@MaskFor_KeyWord", MaskFor_KeyWord, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@KeyWord", KeyWord, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@list_name", list_name, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@DateStart", @DateStart, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@NameOfTableOfProgram", @NameOfTableOfProgram, MainParentClass.DefaultConnection),
                                                  });
                }
        }
        private static void Professionss()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT [NameOfProfession] " +
                      "FROM Professionss",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    string NameOfProfession = row[0] + "";

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[Professionss] ([NameOfProfession]) " +
                                                  "VALUES (@NameOfProfession)",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@NameOfProfession", NameOfProfession, MainParentClass.DefaultConnection),
                                                  });
                }
        }
        private static void Detail_Record()
        {
            bool IsSelect = ConnectToDataBase.MainImport(MainParentClass.DefaultConnection,
                      "SELECT [MaskFor_KeyWord],[KeyWord],[Start],[End],[End_ByAccess],[Amount],(SELECT [Name] FROM [Statistic_WorkPlaces] Where[Id] = [Fk_Id_WorkPlace]) " +
                      "FROM Detail_Record",
                      out DataTable @table_Programs,
                      new ProgressOfUpdateAtStructAttribute());

            if (IsSelect)
                foreach (DataRow row in @table_Programs.Rows)
                {
                    string @MaskFor_KeyWord = row[0] + "";
                    string @KeyWord = row[1] + "";
                    DateTime @Start = DateTime.TryParse(row[2] + "", out DateTime DStart) ? DStart : default(DateTime);
                    DateTime @End = DateTime.TryParse(row[3] + "", out DateTime DEnd) ? DEnd : default(DateTime);
                    DateTime @End_ByAccess = DateTime.TryParse(row[4] + "", out DateTime DEnd_ByAccess) ? DEnd_ByAccess : default(DateTime);
                    int @Amount = int.TryParse(row[5] + "", out int IAmount) ? IAmount : default(int);
                    string NameWorkPlace = row[6] + "";

                    bool IsInsert = ConnectToDataBase.MainExport(MainParentClass.DefaultConnection,
                                                  "INSERT INTO[Detail_Record] ([MaskFor_KeyWord],[KeyWord],[Start],[End],[End_ByAccess],[Amount],[Fk_Id_WorkPlace]) " +
                                                  "VALUES (@MaskFor_KeyWord,@KeyWord,@Start,@End,@End_ByAccess,@Amount, (SELECT [Id] FROM [Statistic_WorkPlaces] Where [Name] = @NameWorkPlace))",
                                                  new ProgressOfUpdateAtStructAttribute(),
                                                  new DbParameter[]
                                                  {
                                                      ConnectToDataBase.GetParammeter("@MaskFor_KeyWord", @MaskFor_KeyWord, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@KeyWord", @KeyWord, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@Start", @Start, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@End", @End, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@End_ByAccess", @End_ByAccess, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@Amount", @Amount, MainParentClass.DefaultConnection),
                                                      ConnectToDataBase.GetParammeter("@NameWorkPlace", NameWorkPlace, MainParentClass.DefaultConnection),
                                                  });
                }
        }
    }
}
