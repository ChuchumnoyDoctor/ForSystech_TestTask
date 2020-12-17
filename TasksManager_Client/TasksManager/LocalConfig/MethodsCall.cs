using CommonDll.Client_Server.Client;
using CommonDll.Structs.F_StructDataOnServer;
using CommonDll.Structs.F_Users;
using System;
using System.Collections.Generic;
using System.Reflection;
using Converter = CommonDll.Client_Server.Converter;

namespace TasksManager.LocalConfig
{
    static class MethodsCall
    {
        #region Клиентская прослойка между вызванными методами и подключением к серверу        
        public static Tuple<List<dynamic>, string, string> Common<T>(List<dynamic> SetItems, List<dynamic> ReturnItems, string MethodName, out ConnectionBackInformation ConnectionBackInformation)
        {
            Tuple<List<dynamic>, string, string> tuple = default;
            ConnectionBackInformation = default;

            {
                var r = CommonLayer(SetItems, ReturnItems, MethodName, out ConnectionBackInformation);
                tuple = new Tuple<List<dynamic>, string, string>(r.Item1, r.Item2, null);
            }

            return tuple;
        }
        public static Tuple<T, string, string> SetToServer<T>(StructureValueForClient result, out ConnectionBackInformation ConnectionBackInformation)
        {
            string MethodName = MethodBase.GetCurrentMethod().Name;
            List<dynamic> SetItems = new List<dynamic>() { result };
            List<dynamic> ReturnItems = new List<dynamic>() { Converter.CreateInstance<T>(), Converter.CreateInstance<string>() };
            Tuple<List<dynamic>, string, string> tuple = Common<T>(SetItems, ReturnItems, MethodName, out ConnectionBackInformation);

            if (tuple.Item1.Count == 0)
                return new Tuple<T, string, string>(Converter.CreateInstance<T>(), tuple.Item2, tuple.Item3);
            else
                return new Tuple<T, string, string>(tuple.Item1[0], tuple.Item2, tuple.Item3);
        }
        public static Tuple<T, string, string> GetUser<T>(string Login, string Password, out ConnectionBackInformation ConnectionBackInformation)
        {
            string MethodName = MethodBase.GetCurrentMethod().Name;
            List<dynamic> SetItems = new List<dynamic>() { Login, Password };
            List<dynamic> ReturnItems = new List<dynamic>() { Converter.CreateInstance<T>(), Converter.CreateInstance<string>() };
            Tuple<List<dynamic>, string, string> tuple = Common<T>(SetItems, ReturnItems, MethodName, out ConnectionBackInformation);

            if (tuple.Item1.Count == 0)
                return new Tuple<T, string, string>(Converter.CreateInstance<T>(), tuple.Item2, tuple.Item3);
            else
                return new Tuple<T, string, string>(tuple.Item1[0], tuple.Item2, tuple.Item3);
        }
        public static Tuple<StructureValueForClient, string, string> GetParamatersForClientReady(List<string> ListNameOfValue, out ConnectionBackInformation ConnectionBackInformation)
        {
            string MethodName = MethodBase.GetCurrentMethod().Name;
            List<dynamic> SetItems = new List<dynamic>() { ListNameOfValue };
            List<dynamic> ReturnItems = new List<dynamic>() { Converter.CreateInstance<StructureValueForClient>(), Converter.CreateInstance<string>() };
            Tuple<List<dynamic>, string, string> tuple = Common<StructureValueForClient>(SetItems, ReturnItems, MethodName, out ConnectionBackInformation);

            if (tuple.Item1.Count == 0)
                return new Tuple<StructureValueForClient, string, string>(null, tuple.Item2, tuple.Item3);
            else
                return new Tuple<StructureValueForClient, string, string>(tuple.Item1[0], tuple.Item2, tuple.Item3);
        }
        public static Tuple<List<dynamic>, string> CommonLayer(List<dynamic> SetItems, List<dynamic> ReturnItems, string MethodName, out ConnectionBackInformation ConnectionBackInformation)
        {
            string messageFromServer1 = "";
            List<dynamic> messageFromServer = new List<dynamic>();
            ConnectionBackInformation ConnectionBackInformation_Local = default;

            {
                if (string.IsNullOrEmpty(MethodName))
                    throw new NullReferenceException(nameof(MethodName) + " is null or empty");
                else
                {
                    if (ImportDesignForm.IPAddress is null)
                        throw new NullReferenceException(nameof(ImportDesignForm.IPAddress) + " is null");

                    if (ImportDesignForm.Port == default)
                        throw new NullReferenceException(nameof(ImportDesignForm.Port) + " == default");

                    List<dynamic> returnParametersAndMessageFromSocket = ConnectToServer.CommonLayerConnection(ImportDesignForm.IPAddress, ImportDesignForm.Port, SetItems, ReturnItems, MethodName, out ConnectionBackInformation_Local);

                    messageFromServer = returnParametersAndMessageFromSocket;

                    if (messageFromServer.Count > 1)
                        messageFromServer1 = messageFromServer[messageFromServer.Count - 1];
                }
            }

            ConnectionBackInformation = ConnectionBackInformation_Local;

            return new Tuple<List<dynamic>, string>(messageFromServer, messageFromServer1);
        }
        #endregion
    }
}
