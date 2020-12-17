using CommonDll.Helps;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CommonDll.Client_Server.Server
{
    /// <summary>
    /// Модуль входящих подключений
    /// </summary>
    public static class ConnectFromServer
    {
        /// <summary>
        /// thread signal
        /// </summary>
        public static ManualResetEvent AllDone = new ManualResetEvent(false);
        /// <summary>
        /// IPAddress сервера
        /// </summary>
        public static IPAddress IPAddress { get; set; }
        /// <summary>
        /// Порт сервера
        /// </summary>
        public static int Port { get; set; }
        private static string ClassName { get; set; }
        /// <summary>
        /// Время последнего подключения
        /// </summary>
        public static TimeSpan LastUpdate { get; set; }
        /// <summary>
        /// Запуск прослушки сокета
        /// </summary>
        /// <param name="ClassName">Класс для взаимодействия запросов клиента</param>
        /// <param name="Port"></param>
        /// <param name="Parent"></param>
        public static void StartListening(string ClassName, int Port)
        {
            var Priority = Thread.CurrentThread.Priority;

            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                ConsoleWriteLine.WriteInConsole(null, null, null, "Name: " + netInterface.Name, ConsoleColor.White);
                ConsoleWriteLine.WriteInConsole(null, null, null, "Description: " + netInterface.Description, ConsoleColor.White);
                ConsoleWriteLine.WriteInConsole(null, null, null, "Addresses: ", ConsoleColor.White);
                IPInterfaceProperties ipProps = netInterface.GetIPProperties();

                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                    ConsoleWriteLine.WriteInConsole(null, null, null, " " + addr.Address.ToString(), ConsoleColor.White);
            }

            ConnectFromServer.ClassName = ClassName;
            localEndPoint = new IPEndPoint(IPAddress.Any, Port);
            ConsoleWriteLine.WriteInConsole(null, null, null, string.Format("Local IP: {0}", Helper.GetLocalIPv4(NetworkInterfaceType.Ethernet)), ConsoleColor.White);
            StartListening();
        }
        private static IPEndPoint localEndPoint { get; set; }
        private static ThreadPriority Priority { get; set; }
        private static object StartListening_LockObject = new object();
        static Socket listener { get; set; }
        public static void Dispose_SocketNStream(Socket Socket)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!(Socket is null))
                    {
                        if (Socket.Connected)
                            try
                            {
                                using (NetworkStream stream = new NetworkStream(Socket))
                                {
                                    var buffer = new byte[4096];

                                    while (stream.DataAvailable)
                                        stream.Read(buffer, 0, buffer.Length);

                                    stream.FlushAsync();
                                    stream.Flush();
                                    stream.Close();
                                    stream.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                        if (Socket is null ? false : Socket.Connected)
                        {
                            Task.Run(() =>
                            {
                                Thread Thread = new Thread(() =>
                                {
                                    try
                                    {
                                        Socket.Disconnect(true);
                                    }
                                    catch (SocketException ex)
                                    {

                                    }
                                    catch (ObjectDisposedException ex)
                                    {

                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                });
                                Thread.IsBackground = true;

                                try
                                {
                                    Thread.Start();
                                    Thread.Join(TimeSpan.FromMinutes(5));
                                }
                                catch (ThreadStateException ex)
                                {

                                }
                                catch (ThreadAbortException ex)
                                {

                                }
                                catch (NullReferenceException ex)
                                {

                                }

                                if (Thread is null ? false : Thread.IsAlive)
                                {
                                    try
                                    {
                                        Thread.Abort();
                                        Thread = null;
                                    }
                                    catch
                                    {

                                    }
                                }
                            });
                        }

                        try
                        {
                            Socket.Shutdown(SocketShutdown.Both);
                        }
                        catch (SocketException ex)
                        {

                        }
                        catch (ObjectDisposedException ex)
                        {

                        }
                        catch (Exception ex)
                        {

                        }

                        Socket.Close();
                        Socket.Dispose();
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }
        private static void StartListening()
        {
            Priority = Thread.CurrentThread.Priority;

            try
            {
                if (!Helper.IsLocked(StartListening_LockObject))
                    lock (StartListening_LockObject)
                        using (listener = new Socket(localEndPoint.AddressFamily, // Bind the socket to the local endpoint and listen for incoming connections.  
                                    SocketType.Stream, ProtocolType.Tcp)
                        {
                            ReceiveTimeout = (int)Math.Round(TimeSpan.FromMinutes(2).TotalMilliseconds),
                            SendTimeout = (int)Math.Round(TimeSpan.FromMinutes(2).TotalMilliseconds),

                        }) // Create a TCP/IP socket.  
                        {
                            listener.Bind(localEndPoint);
                            listener.Listen(5);

                            while (true)
                            {
                                AllDone.Reset(); // Set the event to nonsignaled state.  
                                ConsoleWriteLine.WriteInConsole(null, null, null, "\nWaiting for a new connection...", ConsoleColor.White);// Start an asynchronous socket to listen for connections.  
                                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                                AllDone.WaitOne();// Wait until a connection is made before continuing.
                            }
                        }
            }
            catch (ThreadAbortException ex)
            {
                StartListening();
            }
            catch (SocketException ex)
            {
                StartListening();
            }
            catch (Exception ex)
            {
                StartListening();
            }
        }
        public static int AcceptedOnline { get; set; }
        /// <summary>
        /// Асинхронное подключение
        /// </summary>
        /// <param name="ar"></param>
        private static void AcceptCallback(IAsyncResult ar)
        {
            Thread.CurrentThread.Priority = Priority;
            AcceptedOnline++;
            Socket listener = (Socket)ar.AsyncState;  // Get the socket that handles the client request. 
            Socket EndAccept = default;

            try
            {
                EndAccept = listener.EndAccept(ar);
            }
            catch (SocketException ex)
            {

            }
            catch (Exception ex)
            {

            }

            AllDone.Set();  // Signal the main thread to continue. 

            Socket handler = EndAccept;
            {
                LastUpdate = DateTime.Now.TimeOfDay;

                if (handler is null ? false : handler.Connected)
                    Sort(handler);

                ConnectFromServer.Dispose_SocketNStream(handler);
            }

            AcceptedOnline--;
        }
        /// <summary>
        /// Сортировочная. Именование подключения. Одинаковые подключения, посредством ProgressOfUpdateAtStructAttribute, чистятся
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="Parent"></param>

        private static void Sort(Socket handler)
        {
            if (handler.Connected)
            {
                string EndPoint = Convert.ToString(((IPEndPoint)handler.RemoteEndPoint).Address);
                var Priority = Thread.CurrentThread.Priority;
                Accept(handler, EndPoint);
            }
        }

        /// <summary>
        /// Обработка входных данных и обработка запроса по этим данным
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="Parent"></param>
        private static void Accept(Socket handler, string EndPoint)
        {
            {
                var Priority = Thread.CurrentThread.Priority;
                byte[] new_data = default;

                DateTime StartRecieve = DateTime.Now;

                if (handler is null ? false : handler.Connected)
                    using (ProtocolSendRecieve Recieve = new ProtocolSendRecieve(handler)) // Получение данных по сокету
                    {
                        new_data = Recieve.GetResponse;

                        TimeSpan TimeOnRecieve = DateTime.Now - StartRecieve;

                        if (!(new_data is null) & TimeOnRecieve > TimeSpan.FromSeconds(5))
                        {

                        }
                        else if (new_data is null)
                        {

                        }
                    }

                if (!(new_data is null) & (handler is null ? false : handler.Connected))
                {
                    byte[] dataSetToSend = default;

                    string sReceived = string.Format("Main package received from client({1}): {0} byte's.", new_data.Length, EndPoint);
                    ConsoleWriteLine.WriteInConsole("Network", "Listener", "In Process", sReceived, ConsoleColor.White);
                    DateTime StartCommonProcess = DateTime.Now;
                    Tuple<byte[], string> tuple = CommonLayer.CommonProcess(new_data, EndPoint, ClassName);
                    TimeSpan CommonProcess = DateTime.Now - StartCommonProcess;

                    if (CommonProcess > TimeSpan.FromSeconds(20))
                    {

                    }

                    dataSetToSend = tuple.Item1; // Обработал данные, конвертнул DataSet.

                    {
                        DateTime StartSend = DateTime.Now;

                        if (!(dataSetToSend is null) & (handler is null ? false : handler.Connected))
                            using (ProtocolSendRecieve Send = new ProtocolSendRecieve(handler, dataSetToSend)) // Отправка по сокету  
                            {
                                TimeSpan TimeOnSend = DateTime.Now - StartSend;

                                if (TimeOnSend > TimeSpan.FromSeconds(20))
                                {
                                    Task.Run(() =>
                                    {
                                        StartListening();
                                    });
                                }
                            }
                        else
                        {

                        }
                    }
                }
            }
        }
    }
}
