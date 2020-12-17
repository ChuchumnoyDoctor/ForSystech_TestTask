using CommonDll.Client_Server;
using CommonDll.Client_Server.Server;
using CommonDll.Helps;
using CommonDll.Structs;
using CommonDll.Structs.F_ProgressOfUpdate;
using CommonDll.Structs.F_StructDataOnServer;
using CommonDll.Structs.F_Users;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading;
using TasksManager_Server.Methods.MethodsProcess;
using TasksManager_Server.MethodsProcess;
using Action = System.Action;

namespace TasksManager_Server
{
    public static class MethodsCall // Вызываемые методы-обработчики
    {
        static MethodsCall()
        {
            Helper_WINWORD.Clear();
            Helper_EXCEL.Clear();
            AppDomain.CurrentDomain.ProcessExit += delegate (object sender, EventArgs e)
            {
                Helper_WINWORD.Clear();
                Helper_EXCEL.Clear();
            };
        }
        public static Tuple<string, string> SetToServer(StructureValueForClient Obj)
        {
            string Text = ParentMethods.SetToServer(Obj, Program.Progress);

            return new Tuple<string, string>(null, Text);
        }
        public static Tuple<string, string> GetUser(string Login, string Password)
        {
            var User = ValueForClient.ReadyStructure.Users.User_s.FirstOrDefault(x => x.Login == Login & x.Password == Password);

            return new Tuple<string, string>(User is null ? "False" : User.HR_Worker is null ? "True" : User.HR_Worker.Name, User is null ? "Нет совпадений" : "");
        }

        #region Протокол авто-обновления клиент-сервер
        #region Проверка обновленных элементов
        public static Tuple<StructureValueForClient, string> CheckAutoRefresh(StructureValueForClient StructureValueForClient)
        {
            StructureValueForClient Text = default;
            ProgressOfUpdateAtStructAttribute Progress = (ProgressOfUpdateAtStructAttribute)ParentMethods.GetStandart().Clone_Full();
            Progress.NonSerializedConfig.IsInTask = false;
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                Text = AutoRefresh_ServerPart.CheckAutoRefresh(StructureValueForClient);
            });
            Progress.Start();

            return new Tuple<StructureValueForClient, string>(Text, Progress.ExceptionMessage);
        }
        #endregion

        #region Получить параметры по имени параметров
        public static Tuple<StructureValueForClient, string> GetParamatersForClientReady(List<string> ListNameOfValue) // Для запроса по нажатию, получает уже готовый результат
        {
            StructureValueForClient ReturnStruct = default;
            ProgressOfUpdateAtStructAttribute Progress = (ProgressOfUpdateAtStructAttribute)ParentMethods.GetStandart().Clone_Full();
            Progress.NonSerializedConfig.IsInTask = false;
            Progress.NonSerializedConfig.Method = new Action(() =>
            {
                DateTime Start = DateTime.Now;

                try
                {
                    ReturnStruct = AutoRefresh_ServerPart.GetParamatersForClientReady(ListNameOfValue);
                }
                catch (Exception ex)
                {
                    try
                    {
                        ReturnStruct = AutoRefresh_ServerPart.GetParamatersForClientReady(ListNameOfValue);
                    }
                    catch (Exception exx)
                    {

                    }
                }

                TimeSpan TimeSpend = DateTime.Now - Start;
                ReturnStruct.TimeServerOnGetProperties = TimeSpend;
                ReturnStruct.TimeServerWhenGetProperties = DateTime.Now.TimeOfDay;
            });
            Progress.Start();

            return new Tuple<StructureValueForClient, string>(ReturnStruct, Progress.ExceptionMessage);
        }
        #endregion
        #endregion
    }
    public class MethodsCall_Temp : ParentMethods
    {
        public override MainParentClass Menu_Virtual(ProgressOfUpdateAtStructAttribute Parent)
        {
            throw new NotImplementedException();
        }
    }
}

