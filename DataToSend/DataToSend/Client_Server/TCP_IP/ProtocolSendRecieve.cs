using CommonDll.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;

[assembly: RuntimeCompatibility(WrapNonExceptionThrows = false)]

namespace CommonDll.Client_Server
{
    /// <summary>
    /// Протокол передачи данных между клиентом и сервером: методы Send и Recieve
    /// </summary>
    public class ProtocolSendRecieve : IDisposable
    {
        /// <summary>
        /// Статус о получении или отправке всех необходимых данных
        /// </summary>
        private ManualResetEvent StatusReceieve { get; set; }
        private ManualResetEvent StatusSendBytesLength { get; set; }
        private ManualResetEvent SendCallbackStatus { get; set; }
        private ManualResetEvent RecieveSomeBytesLength { get; set; }
        bool BytesIsRecieve { get; set; }
        /// <summary>
        /// Выходной массив байтов
        /// </summary>
        private byte[] responseByBytes { get; set; }
        public static readonly string MessageToConnect = "Try connect to server.";
        public static readonly string MessageFromConnected = "<Connected>";
        private static readonly string MessageForBytesLengthRecieve = "<Byte's length received>";
        public static readonly string MessageForBytesLength_NotRecieve = "<Byte's length not recieve>";
        public static readonly string ExceptionMessage_NotConnected = "<Not connected>";
        public static readonly string ExceptionMessage_NotRespond = "<Not respond>";
        private string ExceptionMessage { get; set; }
        /// <summary>
        /// Структура-стек вызовов протокола отправки данных 
        /// </summary>
        public Dictionary<string, Tuple<bool, string, TimeSpan>> StructSetData { get; set; }
        /// <summary>
        /// Структура-стек вызовов протокола получения данных 
        /// </summary>
        public Dictionary<string, Tuple<bool, string, TimeSpan>> StructGetData { get; set; }
        /// <summary>
        /// Время ожидания
        /// </summary>
        private TimeSpan WaitTime = TimeSpan.FromMinutes(2); // Timeout
        private DateTime StartConnection { get; set; }
        /// <summary>
        /// Конструктор для отправки данных
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="SomeBytes"></param>
        public ProtocolSendRecieve(Socket handler, byte[] SomeBytes)
        {
            StartConnection = DateTime.Now;

            if (SomeBytes.Count() == 0)
                throw new Exception("SomeBytes.Count == 0");

            if (handler is null ? false : handler.Connected)
            {
                StructSetData = new Dictionary<string, Tuple<bool, string, TimeSpan>>()
                {
                    {string.Format("Stage: '{0}'", "Start"), null},
                    {string.Format("Stage: '{0}'", "Send bytes length"), null},
                    {string.Format("Stage: '{0}'", "Get message about bytes length"), null},
                    {string.Format("Stage: '{0}'", "Send bytes"), null},
                    {string.Format("Stage: '{0}'", "Done"), null},
                    {string.Format("Stage: '{0}'", string.Format("Set '{0}'", MessageToConnect)), null},
                    {string.Format("Stage: '{0}'", string.Format("Exception: '{0}'", ExceptionMessage_NotRespond)), null},
                };

                ExceptionMessage = "";
                NewIvents();

                Thread Thread = new Thread(() =>
                {
                    if (Thread.CurrentThread.Name is null)
                        Thread.CurrentThread.Name = nameof(ProtocolSendRecieve) + "_Send";

                    try
                    {
                        StructSetData[string.Format("Stage: '{0}'", "Start")] = new Tuple<bool, string, TimeSpan>(true, string.Format("Send start. Bytes: {0}", SomeBytes.Length), DateTime.Now - StartConnection);
                        StatusReceieve = new ManualResetEvent(false);
                        Send(new StateObject(handler, SomeBytes));
                        StatusReceieve.WaitOne(WaitTime);
                        StructSetData[string.Format("Stage: '{0}'", "Done")] = new Tuple<bool, string, TimeSpan>(true, "Send done", DateTime.Now - StartConnection);
                    }
                    catch (ThreadAbortException ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Send", "Failed", ExceptionMessage, default);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Send", "Failed", ExceptionMessage, default);
                    }
                    catch (SocketException ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Send", "Failed", ExceptionMessage, default);
                    }
                    catch (RuntimeWrappedException ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Send", "Failed", ExceptionMessage, default);
                    }
                    catch (Exception ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Send", "Failed", ExceptionMessage, default);
                    }
                    catch
                    {
                        ExceptionMessage = MethodBase.GetCurrentMethod().Name + " catch";
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Send", "Failed", ExceptionMessage, default);
                    }
                });

                try
                {
                    Thread.Priority = ThreadPriority.Highest;
                    Thread.Start();
                    Thread.Join(WaitTime);
                }
                catch (ThreadStateException ex)
                {
                    ExceptionMessage = ex.ToString();
                }
                catch (ThreadStartException ex)
                {
                    ExceptionMessage = ex.ToString();
                }
                catch (ThreadAbortException ex)
                {
                    ExceptionMessage = ex.ToString();
                }
                catch (NullReferenceException ex)
                {
                    ExceptionMessage = ex.ToString();
                }

                if (Thread is null ? false : Thread.IsAlive)
                    try
                    {
                        ExceptionMessage = "Time is out";
                        Thread.Abort();
                        Thread = null;
                    }
                    catch
                    {

                    }
            }
        }
        /// <summary>
        /// Конструктор для получения данных
        /// </summary>
        /// <param name="handler"></param>
        public ProtocolSendRecieve(Socket handler)
        {
            StartConnection = DateTime.Now;

            if (handler is null ? false : handler.Connected)
            {
                StructGetData = new Dictionary<string, Tuple<bool, string, TimeSpan>>()
                {
                    {string.Format("Stage: '{0}'", "Start"), null},
                    {string.Format("Stage: '{0}'", "Get bytes length"), null},
                    {string.Format("Stage: '{0}'", "Send message about bytes length"), null},
                    {string.Format("Stage: '{0}'", "Get bytes"), null},
                    {string.Format("Stage: '{0}'", "Done"), null},
                    {string.Format("Stage: '{0}'", string.Format("Get '{0}'", MessageToConnect)), null},
                    {string.Format("Stage: '{0}'", string.Format("Set '{0}'", MessageFromConnected)), null},
                    {string.Format("Stage: '{0}'", string.Format("Get '{0}'", MessageFromConnected)), null},
                    {string.Format("Stage: '{0}'", string.Format("Exception: '{0}'", ExceptionMessage_NotRespond)), null},
                };

                ExceptionMessage = "";
                NewIvents();

                Thread Thread = new Thread(() =>
                {
                    if (Thread.CurrentThread.Name is null)
                        Thread.CurrentThread.Name = nameof(ProtocolSendRecieve) + "_Recieve";

                    try
                    {
                        StructGetData[string.Format("Stage: '{0}'", "Start")] = new Tuple<bool, string, TimeSpan>(true, "Recieve start", DateTime.Now - StartConnection);
                        StatusReceieve = new ManualResetEvent(false);
                        Receive(new StateObject(handler));
                        StatusReceieve.WaitOne(WaitTime);
                        StructGetData[string.Format("Stage: '{0}'", "Done")] = new Tuple<bool, string, TimeSpan>(true, "Recieve done", DateTime.Now - StartConnection);
                    }
                    catch (ThreadAbortException ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Recieve", "Failed", ExceptionMessage, default);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Recieve", "Failed", ExceptionMessage, default);
                    }
                    catch (SocketException ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Recieve", "Failed", ExceptionMessage, default);
                    }
                    catch (RuntimeWrappedException ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Recieve", "Failed", ExceptionMessage, default);
                    }
                    catch (Exception ex)
                    {
                        ExceptionMessage = ex.Message.ToString();
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Recieve", "Failed", ExceptionMessage, default);
                    }
                    catch
                    {
                        ExceptionMessage = MethodBase.GetCurrentMethod().Name + " catch";
                        ConsoleWriteLine.WriteInConsole(nameof(ProtocolSendRecieve), "Recieve", "Failed", ExceptionMessage, default);
                    }
                });

                try
                {
                    Thread.Priority = ThreadPriority.Highest;
                    Thread.Start();
                    Thread.Join(WaitTime);
                }
                catch (ThreadStateException ex)
                {
                    ExceptionMessage = ex.ToString();
                }
                catch (ThreadStartException ex)
                {
                    ExceptionMessage = ex.ToString();
                }
                catch (ThreadAbortException ex)
                {
                    ExceptionMessage = ex.ToString();
                }
                catch (NullReferenceException ex)
                {
                    ExceptionMessage = ex.ToString();
                }

                if (Thread is null ? false : Thread.IsAlive)
                    try
                    {
                        ExceptionMessage = "Time is out";
                        Thread.Abort();
                        Thread = null;
                    }
                    catch
                    {

                    }
            }
        }
        /// <summary>
        /// Обновление ивентов
        /// </summary>
        private void NewIvents()
        {
            StatusReceieve = new ManualResetEvent(false);
            StatusReceieve = new ManualResetEvent(false);
            StatusSendBytesLength = new ManualResetEvent(false);
            SendCallbackStatus = new ManualResetEvent(false);
            RecieveSomeBytesLength = new ManualResetEvent(false);
            BytesIsRecieve = false;
        }
        public string GetErrorMessage
        {
            get
            {
                return ExceptionMessage;
            }
        }
        /// <summary>
        /// Ответ от класса Sockets на получение массива байтов
        /// </summary>
        public byte[] GetResponse
        {
            get
            {
                return responseByBytes;
            }
        }
        /// <summary>
        /// Отправка массива byte[] по сокету
        /// </summary>
        /// <param name="stateObjectOriginal"></param>
        [HandleProcessCorruptedStateExceptions()]
        [SecurityCritical]
        private void Send(StateObject stateObjectOriginal)
        {
            {
                StatusSendBytesLength = new ManualResetEvent(false); // Ивент на инфо о получении размерности
                Send(stateObjectOriginal, true); // Отправка размерности массива            
                StatusSendBytesLength.WaitOne(WaitTime); // Ожидание ответа
            }

            {
                if (stateObjectOriginal.workSocket is null ? false : stateObjectOriginal.workSocket.Connected)
                {
                    SendCallbackStatus = new ManualResetEvent(false); // Ивент, что информация о размерности доставлена

                    AsyncCallback AsyncCallback = new AsyncCallback(SendCallback);
                    stateObjectOriginal.workSocket.BeginSend(stateObjectOriginal.buffer, 0, stateObjectOriginal.buffer.Length
                    , 0, AsyncCallback, stateObjectOriginal); // new AsyncCallback на отправку основного массива байтов

                    if (!(StructSetData is null))
                        StructSetData[string.Format("Stage: '{0}'", "Send bytes")] = new Tuple<bool, string, TimeSpan>(true, string.Format("Bytes length: {0}", stateObjectOriginal.buffer.Length), DateTime.Now - StartConnection);
                    else
                    {

                    }

                    SendCallbackStatus.WaitOne(WaitTime); // Ожидание ответа.
                }
                else
                {
                    ExceptionMessage = MethodBase.GetCurrentMethod().Name + " stateObjectOriginal.workSocket.Connected => else";
                    ConsoleWriteLine.WriteInConsole(nameof(Send), "", "Failed", ExceptionMessage, default);
                }
            }

            if (!StatusReceieve.SafeWaitHandle.IsClosed)
                StatusReceieve.Set();   // Signal that all bytes have been sent. 
        }
        [HandleProcessCorruptedStateExceptions()]
        [SecurityCritical]
        private void Send(StateObject stateObjectOriginal, bool SendBytesLength)
        {
            if (stateObjectOriginal.workSocket is null ? false : stateObjectOriginal.workSocket.Connected)
                if (SendBytesLength) // {Отправка размерности массива}
                {
                    {
                        string bufferLength = stateObjectOriginal.buffer.Length.ToString(); // Строка с размерностью буфера.
                        byte[] bytesLength = Encoding.ASCII.GetBytes(stateObjectOriginal.buffer.Length.ToString()); // Отправили строку с информацией о буфере в байты.

                        {
                            SendCallbackStatus = new ManualResetEvent(false);

                            AsyncCallback AsyncCallback = new AsyncCallback(SendCallback);
                            stateObjectOriginal.workSocket.BeginSend(bytesLength, 0, bytesLength.Length, 0,
                                AsyncCallback, stateObjectOriginal); // new AsyncCallback на отправку информации о размерности  
                            if (!(StructSetData is null))
                                StructSetData[string.Format("Stage: '{0}'", "Send bytes length")] = new Tuple<bool, string, TimeSpan>(true, string.Format("Bytes length: {0}", bytesLength.Length), DateTime.Now - StartConnection);
                            else
                            {

                            }

                            SendCallbackStatus.WaitOne(WaitTime); // Ивент, что информация о размерности доставлена. Ожидание ответа.

                            RecieveSomeBytesLength = new ManualResetEvent(false);
                            Receive(new StateObject(stateObjectOriginal.workSocket));// Recieve на обратный ответ о получении размерности с уже стандартным сокетом                      
                            RecieveSomeBytesLength.WaitOne(WaitTime);

                            if (!BytesIsRecieve)
                                Send(stateObjectOriginal, SendBytesLength);
                        }

                        if (!StatusSendBytesLength.SafeWaitHandle.IsClosed)
                            StatusSendBytesLength.Set();
                    }
                }
                else // {Отправка промежуточного кода}
                {
                    SendCallbackStatus = new ManualResetEvent(false);
                    stateObjectOriginal.workSocket.BeginSend(stateObjectOriginal.buffer, 0, stateObjectOriginal.buffer.Length
                       , 0, new AsyncCallback(SendCallback), stateObjectOriginal); // new AsyncCallback на отправку, что информация о размерности получена  
                    SendCallbackStatus.WaitOne(WaitTime);
                }
            else
            {
                ExceptionMessage = MethodBase.GetCurrentMethod().Name + " else";
                ConsoleWriteLine.WriteInConsole(nameof(Send), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
            }
        }
        [HandleProcessCorruptedStateExceptions()]
        [SecurityCritical]
        private void Send(Socket handler, string SomeCode)
        {
            byte[] bytesCode = Encoding.ASCII.GetBytes(SomeCode);  // Конверт кода в byte[]      
            Send(new StateObject(handler, bytesCode), false); // Отправка кода о полученном массиве байтов, false - условие для ветки внутри метода Send    
        }
        [HandleProcessCorruptedStateExceptions()]
        [SecurityCritical]
        private void SendCallback(IAsyncResult ar) // Создает новый поток
        {
            if (Thread.CurrentThread.Name is null)
                Thread.CurrentThread.Name = nameof(SendCallback);

            Thread.CurrentThread.Priority = ThreadPriority.Highest;  // Приоритет для нового потока

            try
            {
                StateObject stateObject = (StateObject)ar.AsyncState;  // Retrieve the socket from the state object.           

                if (stateObject.workSocket is null ? false : stateObject.workSocket.Connected)
                {
                    int bytesSent = stateObject.workSocket.EndSend(ar); // Complete sending the data to the remote device.  
                    ConsoleWriteLine.WriteInConsole(nameof(SendCallback), "", "Done", string.Format("Sent {0} bytes.", bytesSent), default);
                }

                if (!SendCallbackStatus.SafeWaitHandle.IsClosed)
                    SendCallbackStatus.Set();
            }
            catch (ThreadAbortException ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(SendCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
            }
            catch (ObjectDisposedException ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(SendCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
            }
            catch (SocketException ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(SendCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
            }
            catch (RuntimeWrappedException ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(SendCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(SendCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
            }
            catch
            {
                ExceptionMessage = MethodBase.GetCurrentMethod().Name + " catch";
                ConsoleWriteLine.WriteInConsole(nameof(SendCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
            }
        }
        /// <summary>
        /// Получение byte[] по сокету
        /// </summary>
        /// <param name="stateObjectOriginal"></param>
        [HandleProcessCorruptedStateExceptions()]
        [SecurityCritical]
        private void Receive(StateObject stateObjectOriginal)
        {
            if (stateObjectOriginal.workSocket is null ? false : stateObjectOriginal.workSocket.Connected)
            {
                AsyncCallback asyncCallback = new AsyncCallback(ReceiveCallback);
                stateObjectOriginal.workSocket.BeginReceive(stateObjectOriginal.buffer, 0, stateObjectOriginal.buffer.Length, 0, asyncCallback, stateObjectOriginal); // Begin receiving the data from the remote device.
            }
            else
            {
                ExceptionMessage = MethodBase.GetCurrentMethod().Name + " stateObjectOriginal.workSocket.Connected => else";
                ConsoleWriteLine.WriteInConsole(nameof(Receive), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
            }
        }
        [HandleProcessCorruptedStateExceptions()]
        [SecurityCritical]
        private void ReceiveCallback(IAsyncResult ar)  // Создает новый поток
        {
            if (Thread.CurrentThread.Name is null)
                Thread.CurrentThread.Name = nameof(ReceiveCallback);

            Thread.CurrentThread.Priority = ThreadPriority.Highest;  // Приоритет для нового потока
            StateObject stateObjectOriginal = (StateObject)ar.AsyncState; // Retrieve the state object and the client socket from the asynchronous state object.          

            try
            {
                int bytesRead = 0;
                stateObjectOriginal.content = "";

                if (stateObjectOriginal.workSocket is null ? false : stateObjectOriginal.workSocket.Connected)
                {
                    {
                        bytesRead = stateObjectOriginal.workSocket.EndReceive(ar); // Read data from the remote device. 
                        stateObjectOriginal.content = (Encoding.ASCII.GetString(stateObjectOriginal.buffer, 0, bytesRead));  // Попробовали конвертнуть в строку. string конвертится.  
                    }

                    if (bytesRead == stateObjectOriginal.buffer.Length) // Получили массив байтов равнозначный с буфером StateObject - значит можно с ним работать.
                    {
                        if (stateObjectOriginal.content.Length > 1)  // All the data has arrived; put it in response.  
                            if (stateObjectOriginal.content.IndexOf(MessageToConnect) > -1) // Обратный код выполнения для принимающей стороны
                            {
                                stateObjectOriginal.content = "";

                                if (!(StructGetData is null))
                                {
                                    StructGetData[string.Format("Stage: '{0}'", string.Format("Get '{0}'", MessageToConnect))] = new Tuple<bool, string, TimeSpan>(true, MessageToConnect, DateTime.Now - StartConnection);
                                    StructGetData[string.Format("Stage: '{0}'", string.Format("Set '{0}'", MessageFromConnected))] = new Tuple<bool, string, TimeSpan>(true, MessageFromConnected, DateTime.Now - StartConnection);
                                }
                                else
                                {

                                }

                                Send(stateObjectOriginal.workSocket, MessageFromConnected);

                                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                                {
                                    StatusReceieve.Set(); // Signal that all bytes have been received. 
                                }
                            }
                            else // Массив байтов для принимающей стороны
                            {
                                {
                                    if (!(StructGetData is null))
                                        StructGetData[string.Format("Stage: '{0}'", "Get bytes")] = new Tuple<bool, string, TimeSpan>(true, string.Format("Bytes: '{0}'", bytesRead), DateTime.Now - StartConnection);
                                    else
                                    {

                                    }

                                    stateObjectOriginal.content = "";

                                    byte[] bytes = new byte[bytesRead];
                                    Array.Copy(stateObjectOriginal.buffer, bytes, bytesRead);

                                    if (responseByBytes is null)
                                        responseByBytes = bytes;
                                    else
                                        responseByBytes = responseByBytes.Concat(bytes).ToArray();

                                    if (!StatusReceieve.SafeWaitHandle.IsClosed)
                                    {
                                        StatusReceieve.Set(); // Signal that all bytes have been received. 
                                    }
                                }
                            }
                    }
                    else if (stateObjectOriginal.content.IndexOf(MessageFromConnected) > -1) // Обратный код подключения для принимающей стороны
                    {
                        if (!(StructGetData is null))
                            StructGetData[string.Format("Stage: '{0}'", string.Format("Get '{0}'", MessageFromConnected))] = new Tuple<bool, string, TimeSpan>(true, MessageFromConnected, DateTime.Now - StartConnection);
                        else
                        {

                        }

                        stateObjectOriginal.content = "";
                        responseByBytes = stateObjectOriginal.buffer;

                        if (!StatusReceieve.SafeWaitHandle.IsClosed)
                        {
                            StatusReceieve.Set(); // Signal that all bytes have been received.
                        }
                    }
                    else if (stateObjectOriginal.content.IndexOf(MessageForBytesLength_NotRecieve) > -1) // Обратный код исполнения о получении размерности
                    {
                        if (!(StructSetData is null))
                            StructSetData[string.Format("Stage: '{0}'", "Get message about bytes length")] = new Tuple<bool, string, TimeSpan>(false, null, DateTime.Now - StartConnection);
                        else
                        {
                            ExceptionMessage = MessageForBytesLength_NotRecieve;
                            ConsoleWriteLine.WriteInConsole(nameof(ReceiveCallback), "", "Failed", ExceptionMessage, default);

                            if (!StatusReceieve.SafeWaitHandle.IsClosed)
                            {
                                StatusReceieve.Set(); // Signal that all bytes have been received. 
                            }

                            return;
                        }

                        stateObjectOriginal.content = "";
                        BytesIsRecieve = false;

                        if (!StatusReceieve.SafeWaitHandle.IsClosed)
                            RecieveSomeBytesLength.Set(); // Сигнал, что BytesLength получен  
                    }
                    else if (stateObjectOriginal.content.IndexOf(MessageForBytesLengthRecieve) > -1) // Обратный код исполнения о получении размерности
                    {
                        if (!(StructSetData is null))
                            StructSetData[string.Format("Stage: '{0}'", "Get message about bytes length")] = new Tuple<bool, string, TimeSpan>(true, MessageForBytesLengthRecieve, DateTime.Now - StartConnection);
                        else
                        {

                        }

                        BytesIsRecieve = true;

                        if (!RecieveSomeBytesLength.SafeWaitHandle.IsClosed)
                            RecieveSomeBytesLength.Set(); // Сигнал, что BytesLength получен  
                    }
                    else if (bytesRead == 0) // return;
                    {
                        if (!StatusReceieve.SafeWaitHandle.IsClosed)
                        {
                            StatusReceieve.Set(); // Signal that all bytes have been received. 
                        }
                    }
                    else // ветка на изменение размерности
                    {
                        int bytesLength = 0;
                        bool tryBytesLengthRead = false;

                        if (string.IsNullOrEmpty(stateObjectOriginal.content.ToString()))
                        {
                            if (!(StructGetData is null))
                                StructGetData[string.Format("Stage: '{0}'", string.Format("Exception: '{0}'", ExceptionMessage_NotRespond))] = new Tuple<bool, string, TimeSpan>(true, ExceptionMessage_NotRespond, DateTime.Now - StartConnection);
                            else
                            {

                            }

                            if (!(StructSetData is null))
                                StructSetData[string.Format("Stage: '{0}'", string.Format("Exception: '{0}'", ExceptionMessage_NotRespond))] = new Tuple<bool, string, TimeSpan>(true, ExceptionMessage_NotRespond, DateTime.Now - StartConnection);
                            else
                            {

                            }

                            ExceptionMessage = ExceptionMessage_NotRespond;
                            ConsoleWriteLine.WriteInConsole(nameof(ReceiveCallback), "", "Failed", ExceptionMessage, default);

                            if (!StatusReceieve.SafeWaitHandle.IsClosed)
                            {
                                StatusReceieve.Set(); // Signal that all bytes have been received. 
                            }
                        }
                        else
                        {
                            string Messaga = stateObjectOriginal.content.ToString();

                            if (Int32.TryParse(Messaga, out int Length))
                            {
                                if (!(StructGetData is null))
                                {
                                    StructGetData[string.Format("Stage: 'Get bytes length'")] = new Tuple<bool, string, TimeSpan>(true, string.Format("Byte's length getted: '{0}'", Length), DateTime.Now - StartConnection);
                                    StructGetData[string.Format("Stage: '{0}'", "Send message about bytes length")] = new Tuple<bool, string, TimeSpan>(true, MessageForBytesLengthRecieve, DateTime.Now - StartConnection);
                                }
                                else
                                {

                                }

                                stateObjectOriginal.content = "";
                                Send(stateObjectOriginal.workSocket, MessageForBytesLengthRecieve); // Отправка кода о полученой размерности
                                bytesLength = Length; // Прочитали размерность    
                                tryBytesLengthRead = true;
                            }
                            else  // Дополнительное чтение неполного массива
                            {
                                byte[] bytes = new byte[bytesRead];
                                Array.Copy(stateObjectOriginal.buffer, bytes, bytesRead);

                                if (responseByBytes is null)
                                    responseByBytes = bytes;
                                else
                                    responseByBytes = responseByBytes.Concat(bytes).ToArray();

                                stateObjectOriginal.content = "";
                                stateObjectOriginal.newBufferSize(stateObjectOriginal.buffer.Length - bytesRead); // Создаем массив новой размерности.  
                                Receive(stateObjectOriginal); // Еще один вызов на получение данных.
                            }
                        }

                        if (tryBytesLengthRead)
                        {
                            stateObjectOriginal.content = "";
                            stateObjectOriginal.newBufferSize(bytesLength); // Создаем массив новой размерности.  
                            Receive(stateObjectOriginal); // Еще один вызов на получение данных.
                        }
                    }
                }
                else
                {
                    ExceptionMessage = MethodBase.GetCurrentMethod().Name + " stateObjectOriginal.workSocket.Connected => else";
                    ConsoleWriteLine.WriteInConsole(nameof(ReceiveCallback), "", "Failed", ExceptionMessage, default);

                    if (!StatusReceieve.SafeWaitHandle.IsClosed)
                    {
                        StatusReceieve.Set(); // Signal that all bytes have been received. 
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(ReceiveCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                {
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
                }
            }
            catch (ObjectDisposedException ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(ReceiveCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                {
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
                }
            }
            catch (SocketException ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(ReceiveCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                {
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
                }
            }
            catch (RuntimeWrappedException ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(ReceiveCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                {
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message.ToString();
                ConsoleWriteLine.WriteInConsole(nameof(ReceiveCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                {
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
                }
            }
            catch
            {
                ExceptionMessage = MethodBase.GetCurrentMethod().Name + " catch";
                ConsoleWriteLine.WriteInConsole(nameof(ReceiveCallback), "", "Failed", ExceptionMessage, default);

                if (!StatusReceieve.SafeWaitHandle.IsClosed)
                {
                    StatusReceieve.Set(); // Signal that all bytes have been received. 
                }
            }
        }
        public void Dispose()
        {
            if (!(StatusReceieve is null))
                StatusReceieve.Dispose();

            if (!(StatusSendBytesLength is null))
                StatusSendBytesLength.Dispose();

            if (!(SendCallbackStatus is null))
                SendCallbackStatus.Dispose();

            if (!(RecieveSomeBytesLength is null))
                RecieveSomeBytesLength.Dispose();
        }
    }
}
