using CommonDll.Helps;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CommonDll.Client_Server.Client
{
    /// <summary>
    /// Модуль автообновления данных клиента. Клиентская часть
    /// </summary>
    public class AutoRefresh_ClientPart
    {
        private static Dictionary<string, DateTime> CheckProperties_With_TimeLastUpdate = new Dictionary<string, DateTime>();
        private static Dictionary<string, DateTime> ListNameModuleOnServer = new Dictionary<string, DateTime>();
        private static object CheckAutoRefresh_OnServer_lockObject = new object();
        /// <summary>
        /// Проверка времени на обновленные данные с сервера
        /// </summary>
        /// <param name="NameOfModuleToUpdate">Название модуля для обновления</param>
        /// <param name="timeLastUpdate">Время последнего обновления</param>
        /// <param name="InNetWorkProtocol">Задействован сетевой протокол. На случай, есль взаимодействие внутри сервера</param>
        /// <param name="IPAddress">IPAddress сервера</param>
        /// <param name="Thisport">Порт</param>
        /// <param name="Parent">Родительский объект прогресс-аспекта</param>
        /// <returns>Возвращает логическое значение, можно ли отправлять запрос на обновление</returns>

        public static bool CheckAutoRefresh(string NameOfModuleToUpdate, DateTime timeLastUpdate, bool InNetWorkProtocol, IPAddress IPAddress, int Thisport, out ConnectionBackInformation ConnectionBackInformation)
        {
            bool Returned = false;
            ConnectionBackInformation = default;

            {
                if (!string.IsNullOrEmpty(NameOfModuleToUpdate))
                {
                    lock (CheckProperties_With_TimeLastUpdate) // lock на добавление и изменение
                        if (CheckProperties_With_TimeLastUpdate.ContainsKey(NameOfModuleToUpdate))
                        {
                            if (CheckProperties_With_TimeLastUpdate[NameOfModuleToUpdate] != timeLastUpdate)
                                CheckProperties_With_TimeLastUpdate[NameOfModuleToUpdate] = timeLastUpdate;
                        }
                        else
                            CheckProperties_With_TimeLastUpdate.Add(NameOfModuleToUpdate, new DateTime());

                    lock (ListNameModuleOnServer)
                        if (ListNameModuleOnServer.ContainsKey(NameOfModuleToUpdate))
                            lock (CheckProperties_With_TimeLastUpdate)  // lock на взятие значения
                                if (ListNameModuleOnServer[NameOfModuleToUpdate] > CheckProperties_With_TimeLastUpdate[NameOfModuleToUpdate])
                                {
                                    Returned = true;

                                    return Returned;
                                }

                    if (!Helper.IsLocked(CheckAutoRefresh_OnServer_lockObject))
                        lock (CheckAutoRefresh_OnServer_lockObject)
                        {
                            TimeSpan StartLoad = DateTime.Now.TimeOfDay;
                            CheckAutoRefresh(InNetWorkProtocol, IPAddress, Thisport, out ConnectionBackInformation);
                            TimeSpan TimeOnLoad = DateTime.Now.TimeOfDay - StartLoad;

                            if (ConnectionBackInformation.Recieve_GetResponse & TimeOnLoad > TimeSpan.FromSeconds(20))
                            {

                            }
                        }
                }
            }
            return Returned;
        }
        /// <summary>
        /// Запрос информации о обновленных данных
        /// </summary>
        /// <param name="InNetWorkProtocol">Задействован сетевой протокол. На случай, есль взаимодействие внутри сервера</param>
        /// <param name="IPAddress">IPAddress сервера</param>
        /// <param name="Thisport">Порт</param>
        /// <param name="Parent">Родительский объект прогресс-аспекта</param>

        public static void CheckAutoRefresh(bool InNetWorkProtocol, IPAddress IPAddress, int Thisport, out ConnectionBackInformation ConnectionBackInformation) // Метод на постоянный апдейт
        {
            ConnectionBackInformation = default;

            {
                Dictionary<string, DateTime> CheckProperties_With_TimeLastUpdate_Copy = default;

                lock (CheckProperties_With_TimeLastUpdate)  // lock на взятие коллекции
                    CheckProperties_With_TimeLastUpdate_Copy = CheckProperties_With_TimeLastUpdate.ToDictionary(x => x.Key, y => y.Value);

                if (CheckProperties_With_TimeLastUpdate_Copy.Count != 0)
                {
                    StructureValueForClient StructureValueForClient = new StructureValueForClient() { CheckProperties_With_TimeLastUpdate = CheckProperties_With_TimeLastUpdate_Copy };
                    Dictionary<string, DateTime> dictFromServer = new Dictionary<string, DateTime>();

                    if (InNetWorkProtocol)
                    {
                        List<dynamic> SetItems = new List<dynamic>() { StructureValueForClient };
                        List<dynamic> ReturnItems = new List<dynamic>() { Converter.CreateInstance<StructureValueForClient>(), Converter.CreateInstance<string>() };

                        TimeSpan StartLoad = DateTime.Now.TimeOfDay;
                        List<dynamic> returnParametersAndMessageFromSocket = ConnectToServer.CommonLayerConnection(IPAddress, Thisport, SetItems, ReturnItems, nameof(CheckAutoRefresh), out ConnectionBackInformation);
                        TimeSpan TimeOnLoad = DateTime.Now.TimeOfDay - StartLoad;

                        if (TimeOnLoad > TimeSpan.FromSeconds(20))
                        {

                        }
                        else if (TimeOnLoad > TimeSpan.FromSeconds(10))
                        {

                        }
                        else if (TimeOnLoad > TimeSpan.FromSeconds(5))
                        {

                        }
                        else if (TimeOnLoad > TimeSpan.FromSeconds(3))
                        {

                        }

                        try
                        {
                            if (returnParametersAndMessageFromSocket.Count > 0)
                                dictFromServer = ((StructureValueForClient)returnParametersAndMessageFromSocket[0]).CheckProperties_With_TimeLastUpdate;
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        StructureValueForClient Struct = Server.AutoRefresh_ServerPart.CheckAutoRefresh(StructureValueForClient);
                        Dictionary<string, DateTime> returnParametersAndMessageFromSocket = Struct.CheckProperties_With_TimeLastUpdate;
                        string messageFromSocket = default;
                        dictFromServer = returnParametersAndMessageFromSocket;
                    }

                    if (dictFromServer is null ? false : dictFromServer.Count > 0)
                        lock (ListNameModuleOnServer)
                            foreach (var s in dictFromServer)
                                if (ListNameModuleOnServer.ContainsKey(s.Key))
                                {
                                    if (ListNameModuleOnServer[s.Key] != s.Value)
                                        ListNameModuleOnServer[s.Key] = s.Value;
                                }
                                else
                                {
                                    ListNameModuleOnServer.Add(s.Key, s.Value);
                                }
                }
            }
        }
        /// <summary>
        /// Запрос времени обновления по названию модуля/структуры
        /// </summary>
        /// <param name="NameModule"></param>
        /// <returns></returns>
        public static TimeSpan GetTimeFromServer(string NameModule)
        {
            if (!string.IsNullOrEmpty(NameModule))
                lock (ListNameModuleOnServer)
                    if (ListNameModuleOnServer.ContainsKey(NameModule))
                    {
                        TimeSpan Time = ListNameModuleOnServer[NameModule].TimeOfDay;

                        return new TimeSpan(Time.Hours, Time.Minutes, Time.Seconds);
                    }

            return new TimeSpan();
        }
    }
}
