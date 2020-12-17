using CommonDll.Helps;
using CommonDll.Structs;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CommonDll.Client_Server.Server
{
    /// <summary>
    /// Прослойка между вызываемыми методами и самим вызовом
    /// </summary>
    public static class CommonLayer
    {
        /// <summary>
        /// Прослойка/адаптер. Призван конвертировать входящий запрос, обработать на сервере и отправить обратный ответ
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="MYIpClient"></param>
        /// <param name="ClassName"></param>
        /// <param name="Parent"></param>
        /// <returns></returns>

        public static Tuple<byte[], string> CommonProcess(byte[] bytes, string MYIpClient, string ClassName)
        {
            byte[] Bytes_FromServerToClient = default;

            {
                DataToSerialize Data_FromClient = default;

                {
                    DateTime TimeDeSerialize_Start = DateTime.Now;
                    DataToBinary.Convert(bytes, out Data_FromClient, out string Exception);

                    if (!string.IsNullOrEmpty(Exception))
                    {

                    }
                    TimeSpan TimeDeSerialize = DateTime.Now - TimeDeSerialize_Start;

                    if (TimeDeSerialize > TimeSpan.FromSeconds(5))
                    {

                    }
                }

                if (!(Data_FromClient is null))
                {
                    Tuple<List<dynamic>, string, List<dynamic>> ParametersFromDataSet = Converter.GetParametersFromDataSet(Data_FromClient.dataset);
                    List<dynamic> SetParameters = ParametersFromDataSet.Item1; // Поступающие параметры

                    if (SetParameters.Count > 0 ? SetParameters[0] is StructureValueForClient : false)
                        SetParameters[0] = Data_FromClient.ReadyStructure;

                    string NameOfCallMethod = ParametersFromDataSet.Item2; // Имя метода обработчика

                    if (string.IsNullOrEmpty(NameOfCallMethod))
                        throw new Exception("Calling method not found"); // It's a exception
                    else
                    {
                        List<dynamic> ReturnParameters = ParametersFromDataSet.Item3; // Возвращаемые параметры  

                        string comment = string.Format("({0}) to access [ {1} ]", MYIpClient, NameOfCallMethod);
                        ConsoleWriteLine.WriteInConsole("Network", "Listener", "In Process", comment, ConsoleColor.White);

                        List<dynamic> ReturnItems_FromServer = null;
                        string Instruction_FromServer = null;

                        ReturnItems_FromServer = ProcessThis(SetParameters, ReturnParameters, NameOfCallMethod, ClassName);
                        ReturnItems_FromServer = ReturnItems_FromServer is null ? null : ReturnItems_FromServer.Select(x => { return x is null ? "" : x; }).ToList();

                        Instruction_FromServer = Instructions.CreateInstructions(ReturnItems_FromServer, new List<dynamic>(), ""); // Собрали инструкцию для клиента

                        DataSet Date_FromServer = Converter.ConvertFrom(ReturnItems_FromServer, new List<dynamic>(), Instruction_FromServer);

                        if (ReturnItems_FromServer.Count > 0)
                        {
                            DateTime TimeRecieve_Start = DateTime.Now;
                            dynamic Data_FromServerToClient = ReturnItems_FromServer[0];

                            if (Data_FromServerToClient is StructureValueForClient)  // GetParamatersForClientReady
                            {
                                DateTime LastTime = default;
                                string Key = "";

                                {  // Check Time and properties
                                    Dictionary<string, MainParentClass> props = Helper.GetProperties<MainParentClass, StructureValueForClient>((StructureValueForClient)Data_FromServerToClient);

                                    foreach (var From in props)
                                    {
                                        string Name = From.Key;

                                        if (Name != "Log_File")
                                        {
                                            Key += Name;
                                            var ReadyStructure_Type = ((StructureValueForClient)Data_FromServerToClient).GetType();

                                            if (!(ReadyStructure_Type is null))
                                            {
                                                var To_Property = ReadyStructure_Type.GetField("updateTime_Module_" + Name);

                                                if (!(To_Property is null))
                                                {
                                                    DateTime To_Value = (DateTime)To_Property.GetValue((StructureValueForClient)Data_FromServerToClient);
                                                    LastTime += To_Value.TimeOfDay; // сумма проверит включит все элементы
                                                }
                                            }
                                        }
                                    }
                                }

                                lock (AlreadySerialized)
                                {
                                    var Finded = AlreadySerialized.FirstOrDefault(x => x.Key.Equals(Key) || x.Key == Key);

                                    if (Finded.Value is null ? false : Finded.Value.Item2 != LastTime) // remove old serialized properties
                                    {
                                        AlreadySerialized.Remove(Finded.Key);
                                        Finded = default;
                                    }

                                    if (!(Finded.Key is null))
                                        Bytes_FromServerToClient = Finded.Value.Item1; // return already serialized
                                    else
                                    {
                                        DateTime TimeSerialize_Start = DateTime.Now;
                                        DataToBinary.Convert(Date_FromServer, (StructureValueForClient)Data_FromServerToClient, out Bytes_FromServerToClient, out string Exception);// Конвертнули датасет в байты
                                        TimeSpan TimeSerialize = DateTime.Now - TimeSerialize_Start;

                                        if (!string.IsNullOrEmpty(Exception))
                                        {

                                        }

                                        if (TimeSerialize > TimeSpan.FromSeconds(5))
                                        {

                                        }

                                        {
                                            DateTime DeserializeStart = DateTime.Now;
                                            DataToBinary.Convert(Bytes_FromServerToClient, out DataToSerialize ReturnDataSet, out string Ex); // Десериализация
                                            TimeSpan Deserialize = DateTime.Now - DeserializeStart;

                                            if (!string.IsNullOrEmpty(Ex))
                                            {
                                                DataToBinary.Convert(Date_FromServer, (StructureValueForClient)Data_FromServerToClient, out Bytes_FromServerToClient, out string Exceptionn);// Конвертнули датасет в байты
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Key))
                                            if (AlreadySerialized.FirstOrDefault(x => x.Key == Key).Key is null)
                                                AlreadySerialized.Add(Key, new Tuple<byte[], DateTime>(Bytes_FromServerToClient, LastTime)); // serialize new properties
                                    }
                                }
                            }
                            else  // Another methods
                            {
                                DataToBinary.Convert(Date_FromServer, out Bytes_FromServerToClient, out string Exception); // Конвертнули датасет в байты

                                if (!string.IsNullOrEmpty(Exception))
                                {

                                }
                            }

                            TimeSpan TimeRecieve = DateTime.Now - TimeRecieve_Start;

                            if (TimeRecieve > TimeSpan.FromSeconds(5))
                            {

                            }
                        }
                    }
                }
                else
                {
                    DataToBinary.Convert(bytes, out Data_FromClient, out string Ex);

                    if (!string.IsNullOrEmpty(Ex))
                    {

                    }
                }
            }

            return new Tuple<byte[], string>(Bytes_FromServerToClient, null);
        }
        private static Dictionary<string, Tuple<byte[], DateTime>> AlreadySerialized { get; set; } = new Dictionary<string, Tuple<byte[], DateTime>>();
        private static List<dynamic> ProcessThis(List<dynamic> SetItems, List<dynamic> DefaultReturnItems, string NameOfMethod, string ClassName) // Общий метод вызовы обработчика
        {
            List<dynamic> listReturnItems = new List<dynamic>();

            Delegate @delegate = DelegateToMethod.GetTypeOfDelegate(NameOfMethod, ClassName).Item1; // Передали в Dictionary ключ 'NameOfMethod', нашли исполняемый метод по этому ключу 

            switch (DefaultReturnItems.Count)
            {
                case 1:
                    listReturnItems = HelpTo_processThis(@delegate, SetItems, DefaultReturnItems[0]);

                    break;
                case 2:
                    listReturnItems = HelpTo_processThis(@delegate, SetItems, DefaultReturnItems[0], DefaultReturnItems[1]);

                    break;
                case 3:
                    listReturnItems = HelpTo_processThis(@delegate, SetItems, DefaultReturnItems[0], DefaultReturnItems[1], DefaultReturnItems[2]);

                    break;
                case 4:
                    listReturnItems = HelpTo_processThis(@delegate, SetItems, DefaultReturnItems[0], DefaultReturnItems[1], DefaultReturnItems[2], DefaultReturnItems[3]);

                    break;
            }

            return listReturnItems;
        }

        #region helpTo_processThis - прослойка между Tuple и List<dynamic>
        private static List<dynamic> HelpTo_processThis<Return1, Return2, Return3, Return4>(Delegate @delegate, List<dynamic> SetItems, Return1 FirstItem, Return2 SecondItem, Return3 ThirdItem, Return4 FourthItem)
        {
            List<dynamic> listToReturn = new List<dynamic>();

            Tuple<Return1, Return2, Return3, Return4> tuple = (Tuple<Return1, Return2, Return3, Return4>)@delegate.DynamicInvoke(SetItems.ToArray());
            listToReturn = new List<dynamic>() { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4 };

            return listToReturn;
        }
        private static List<dynamic> HelpTo_processThis<Return1, Return2, Return3>(Delegate @delegate, List<dynamic> SetItems, Return1 FirstItem, Return2 SecondItem, Return3 ThirdItem)
        {
            List<dynamic> listToReturn = new List<dynamic>();

            Tuple<Return1, Return2, Return3> tuple = (Tuple<Return1, Return2, Return3>)@delegate.DynamicInvoke(SetItems.ToArray());
            listToReturn = new List<dynamic>() { tuple.Item1, tuple.Item2, tuple.Item3 };

            return listToReturn;
        }
        private static List<dynamic> HelpTo_processThis<Return1, Return2>(Delegate @delegate, List<dynamic> SetItems, Return1 FirstItem, Return2 SecondItem)
        {
            List<dynamic> listToReturn = new List<dynamic>();

            Tuple<Return1, Return2> tuple = (Tuple<Return1, Return2>)@delegate.DynamicInvoke(SetItems.ToArray());

            if (!(tuple is null))
                listToReturn = new List<dynamic>() { tuple.Item1, tuple.Item2 };

            return listToReturn;
        }
        private static List<dynamic> HelpTo_processThis<Return1>(Delegate @delegate, List<dynamic> SetItems, Return1 FirstItem)
        {
            List<dynamic> listToReturn = new List<dynamic>();

            Tuple<Return1> tuple = (Tuple<Return1>)@delegate.DynamicInvoke(SetItems.ToArray());
            listToReturn = new List<dynamic>() { tuple.Item1 };

            return listToReturn;
        }
        #endregion
    }
}


