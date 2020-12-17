using CommonDll.Client_Server.Server;
using CommonDll.Helps;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;

namespace CommonDll.Client_Server.Client
{
    /// <summary>
    /// Модуль подключения к серверу
    /// </summary>
    public static class ConnectToServer
    {
        public delegate void onConnect(Socket Connected);
        public static event onConnect OnConnect;
        private static ManualResetEvent connectDone = new ManualResetEvent(false);   // ManualResetEvent instances signal completion.
        /// <summary>
        /// Запуск подключения к серверу
        /// </summary>
        /// <param name="IPAddress">IPAddress сервера</param>
        /// <param name="Port">Порт</param>
        /// <param name="dataToSerialize">Структура на сериализацию</param>
        /// <param name="Parent">Родительский объект прогресс-аспекта</param>
        /// <returns></returns>

        public static DataToSerialize StartClient(IPAddress IPAddress, int Port, DataToSerialize dataToSerialize, out ConnectionBackInformation ConnectionBackInformation) // Для отправки DataSet
        {
            DataToSerialize ReturnDataSet = default;

            {
                TimeSpan SerializeBefore_Start = DateTime.Now.TimeOfDay;
                DataToBinary.Convert(dataToSerialize, out byte[] Bytes_ToSend, out string Exception); // Сериализация   
                dataToSerialize = null;
                TimeSpan SerializeToSend = DateTime.Now.TimeOfDay - SerializeBefore_Start;

                if (!string.IsNullOrEmpty(Exception))
                {

                }

                byte[] Bytes_FromRecieve = StartClient(IPAddress, Port, Bytes_ToSend, out ConnectionBackInformation);

                ConnectionBackInformation.Send_Serialize = SerializeToSend;
                ConnectionBackInformation.Send_Bytes = Bytes_ToSend is null ? default : Bytes_ToSend.Length;
                Bytes_ToSend = null;
                ConnectionBackInformation.Recieve_Bytes = Bytes_FromRecieve is null ? default : Bytes_FromRecieve.Length;

                if (!(Bytes_FromRecieve is null))
                {
                    TimeSpan SerializeAfter_Start = DateTime.Now.TimeOfDay;
                    DataToBinary.Convert(Bytes_FromRecieve, out ReturnDataSet, out string Ex); // Десериализация
                    ConnectionBackInformation.Recieve_Deserialize = DateTime.Now.TimeOfDay - SerializeAfter_Start;

                    if (!string.IsNullOrEmpty(Ex))
                    {

                    }

                    Bytes_FromRecieve = null;
                }
                else
                {

                }
            }

            return ReturnDataSet;
        }
        /// <summary>
        /// Lock-объект. За раз только одно подключение
        /// </summary>
        static object LockObjectToConnect = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IPAddress">IPAddress сервера</param>
        /// <param name="Port">Порт</param>
        /// <param name="SomeBytes">Сериализованные данные</param>
        /// <param name="Parent"></param>
        /// <returns></returns>
        [HandleProcessCorruptedStateExceptions()]
        [SecurityCritical]
        private static byte[] StartClient(IPAddress IPAddress, int Port, byte[] SomeBytes, out ConnectionBackInformation ConnectionBackInformation) // Для отправки DataSet
        {
            byte[] new_data = default;
            ConnectionBackInformation = new ConnectionBackInformation();

            {
                if (!(IPAddress is null))
                {
                    ProtocolSendRecieve Send = default;
                    ProtocolSendRecieve Recieve = default;
                    StateObject stateObject = new StateObject(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                    {
                        ReceiveTimeout = (int)Math.Round(TimeSpan.FromMinutes(2).TotalMilliseconds),
                        SendTimeout = (int)Math.Round(TimeSpan.FromMinutes(2).TotalMilliseconds),
                    }, new IPEndPoint(IPAddress, Port));

                    Socket handler = stateObject.workSocket;
                    {
                        {
                            DateTime StartConnect = DateTime.Now;
                            connectDone.Reset();
                            handler.BeginConnect(stateObject.remoteEP, new AsyncCallback(ConnectCallback), stateObject);
                            connectDone.WaitOne(TimeSpan.FromSeconds(30));

                            if (!(OnConnect is null))
                                OnConnect.Invoke(handler);

                            ConnectionBackInformation.Connect = DateTime.Now - StartConnect;
                        } // Connect to the remote endpoint.  

                        try
                        {
                            if (handler.Connected)
                            {
                                ConnectionBackInformation.Send_ConnectionState = true; // SEND
                                {
                                    DateTime StartSend = DateTime.Now;

                                    using (Send = new ProtocolSendRecieve(handler, SomeBytes)) // Send test DataSet to the remote device.                      
                                    {
                                        var GetResponse = Send.GetResponse;
                                        ConnectionBackInformation.Send_GetErrorMessage = (string)Send.GetErrorMessage.Clone();
                                        ConnectionBackInformation.Send_TimeSpend = DateTime.Now - StartSend;

                                        if (!string.IsNullOrEmpty(ConnectionBackInformation.Send_GetErrorMessage))
                                        {

                                        }
                                    }

                                    Send = null;
                                }

                                if (string.IsNullOrEmpty(ConnectionBackInformation.Send_GetErrorMessage)) // RECIEVE 
                                {
                                    DateTime StartRecieve = DateTime.Now;

                                    if (handler is null ? false : handler.Connected)
                                    {
                                        using (Recieve = new ProtocolSendRecieve(handler)) // Отправили в сокет новые данные: DataSet
                                        {
                                            new_data = Recieve.GetResponse;
                                            ConnectionBackInformation.Recieve_TimeSpend = DateTime.Now - StartRecieve;
                                            ConnectionBackInformation.Recieve_GetErrorMessage = (string)Recieve.GetErrorMessage.Clone();

                                            if (!string.IsNullOrEmpty(ConnectionBackInformation.Recieve_GetErrorMessage))
                                            {

                                            }

                                            ConnectionBackInformation.Recieve_ConnectionState = true;

                                            if (new_data is null)
                                                ConnectionBackInformation.Recieve_GetResponse = false;
                                            else
                                                ConnectionBackInformation.Recieve_GetResponse = true;
                                        }

                                        Recieve = null;
                                    }
                                    else
                                        ConnectionBackInformation.Recieve_ConnectionState = false;
                                }
                            }
                            else
                                ConnectionBackInformation.Send_ConnectionState = false;
                        }
                        catch (SocketException ex)
                        {
                            ConsoleWriteLine.WriteInConsole(nameof(StartClient), "", "Failed", ex.Message.ToString(), default);
                        }
                        catch (ObjectDisposedException ex)
                        {
                            ConsoleWriteLine.WriteInConsole(nameof(StartClient), "", "Failed", ex.Message.ToString(), default);
                        }
                        catch (ThreadAbortException ex)
                        {
                            ConsoleWriteLine.WriteInConsole(nameof(StartClient), "", "Failed", ex.Message.ToString(), default);
                        } // Connect to a remote device.  

                        ConnectFromServer.Dispose_SocketNStream(handler);
                    }

                    stateObject = null;
                }
            }

            return new_data;
        }
        public static bool StartClient(IPAddress IPAddress, int Port)
        {
            bool Connected = false;
            {
                lock (LockObjectToConnect)
                    if (!(IPAddress is null))
                    {
                        StateObject stateObject = new StateObject(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), new IPEndPoint(IPAddress, Port));

                        using (stateObject.workSocket)
                        {
                            lock (LockObjectToConnect)
                            {
                                connectDone.Reset();
                                stateObject.workSocket.BeginConnect(stateObject.remoteEP, new AsyncCallback(ConnectCallback), stateObject); // Connect to the remote endpoint.  
                                connectDone.WaitOne(10000);

                                if (!(OnConnect is null))
                                    OnConnect.Invoke(stateObject.workSocket);
                            }

                            try // Connect to a remote device.  
                            {
                                if (stateObject.workSocket.Connected)
                                    Connected = true;
                                else
                                {

                                }
                            }
                            catch (SocketException ex)
                            {
                                ConsoleWriteLine.WriteInConsole(nameof(StartClient), "", "Failed", ex.Message.ToString(), default);
                            }
                            catch (ObjectDisposedException ex)
                            {
                                ConsoleWriteLine.WriteInConsole(nameof(StartClient), "", "Failed", ex.Message.ToString(), default);
                            }
                            catch (ThreadAbortException ex)
                            {
                                ConsoleWriteLine.WriteInConsole(nameof(StartClient), "", "Failed", ex.Message.ToString(), default);
                            }
                        }

                        stateObject = null;
                    }
            }

            return Connected;
        } // Для отправки DataSet
        /// <summary>
        /// Попытка и проверка подключения
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectCallback(IAsyncResult ar)
        {
            StateObject stateObject = (StateObject)ar.AsyncState; // Retrieve the socket from the state object.  

            if (stateObject.workSocket is null ? false : stateObject.workSocket.Connected)
                try
                {
                    stateObject.workSocket.EndConnect(ar);  // Complete the connection. 
                    ConsoleWriteLine.WriteInConsole(nameof(ConnectCallback), "", "Done", string.Format("Socket connected to {0}", stateObject.workSocket.RemoteEndPoint.ToString()), default);
                }
                catch (SocketException ex)
                {
                    string ExceptionMessage = ex.ToString();
                    ConsoleWriteLine.WriteInConsole(nameof(ConnectCallback), "", "Failed", ExceptionMessage, default);
                }
                catch (ObjectDisposedException ex)
                {
                    string ExceptionMessage = ex.ToString();
                    ConsoleWriteLine.WriteInConsole(nameof(ConnectCallback), "", "Failed", ExceptionMessage, default);
                }
                catch (ThreadAbortException ex)
                {
                    string ExceptionMessage = ex.ToString();
                    ConsoleWriteLine.WriteInConsole(nameof(ConnectCallback), "", "Failed", ExceptionMessage, default);
                }

            connectDone.Set(); // Signal that the connection has been made.           
        }

        /// <summary>
        /// Прослойка/адаптер. Призван конвертировать входящий запрос, передать на подключение к серверу и получить обратный ответ
        /// </summary>
        /// <param name="IPAddress">IPAddress сервера</param>
        /// <param name="Port">Порт</param>
        /// <param name="SetItems">Входящие параметры</param>
        /// <param name="DefaultReturnItems">Выходящие параметры</param>
        /// <param name="MethodName">Вызываемый метод</param>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public static List<dynamic> CommonLayerConnection(IPAddress IPAddress, int Port, List<dynamic> SetItems, List<dynamic> DefaultReturnItems, string MethodName, out ConnectionBackInformation ConnectionBackInformation) // Общий коннект для вызываемых обработчиков
        {
            List<dynamic> ReturnParameters = new List<dynamic>();
            string feedBackFrom_SendMessageFromSocket = "";
            ConnectionBackInformation = default;

            {
                if (string.IsNullOrEmpty(MethodName))
                    throw new NullReferenceException("Method is null or empty");
                else
                {
                    string instructionsForServer = Instructions.CreateInstructions(SetItems, DefaultReturnItems, MethodName); //create instructionsForServer
                    DataSet dataSet = Converter.ConvertFrom(SetItems, DefaultReturnItems, instructionsForServer); // Получили DataSet.    
                    DataSet returnDataSet = new DataSet();
                    StructureValueForClient return_StructureValueForClient = null;
                    DataToSerialize dataToSerialize = null;
                    bool IsChange = false;

                    if (SetItems.Count > 0 ? (SetItems[0] is StructureValueForClient) : false)
                    {
                        dataToSerialize = new DataToSerialize() { dataset = dataSet, ReadyStructure = SetItems[0] };
                        IsChange = true;
                    }
                    else
                    {

                    }

                    if (!IsChange)
                        dataToSerialize = new DataToSerialize() { dataset = dataSet };
                    else
                    {

                    }

                    DataToSerialize feedBack = StartClient(IPAddress, Port, dataToSerialize, out ConnectionBackInformation); // Отправили один датасет на сервер и получили обратно другой   

                    if (!(feedBack is null))
                    {
                        return_StructureValueForClient = feedBack.ReadyStructure; // Обратный датасет с сервера
                        returnDataSet = feedBack.dataset; // Обратный датасет с сервера
                    }
                    else
                    {

                    }

                    if (!(feedBackFrom_SendMessageFromSocket == ProtocolSendRecieve.ExceptionMessage_NotConnected || feedBackFrom_SendMessageFromSocket == ProtocolSendRecieve.ExceptionMessage_NotRespond || feedBackFrom_SendMessageFromSocket == ProtocolSendRecieve.MessageForBytesLength_NotRecieve))
                    {
                        Tuple<List<dynamic>, string, List<dynamic>> tuple = Converter.GetParametersFromDataSet(returnDataSet); // Преобразованный ответ от сервера
                        ReturnParameters = tuple.Item1; // Возвращаемые параметры

                        if (!(return_StructureValueForClient is null))
                            for (int i = 0; i < ReturnParameters.Count; i++)
                                if (ReturnParameters[i] is StructureValueForClient)
                                    ReturnParameters[i] = return_StructureValueForClient;
                    }
                    else
                    {

                    }
                }
            }

            return ReturnParameters; // Возвращает два string'а - два кода исполнения
        }
    }
    public class ConnectionBackInformation
    {
        public TimeSpan Connect { get; set; }

        #region Send
        public bool Send_ConnectionState { get; set; }
        public int Send_Bytes { get; set; }
        public string Send_GetErrorMessage { get; set; }
        public TimeSpan Send_TimeSpend { get; set; }
        public TimeSpan Send_Serialize { get; set; }
        #endregion

        #region Recieve
        public bool Recieve_ConnectionState { get; set; }
        public string Recieve_GetErrorMessage { get; set; }
        public bool Recieve_GetResponse { get; set; }
        public TimeSpan Recieve_TimeSpend { get; set; }
        public TimeSpan Recieve_Deserialize { get; set; }
        public int Recieve_Bytes { get; set; }
        #endregion
    }
}
