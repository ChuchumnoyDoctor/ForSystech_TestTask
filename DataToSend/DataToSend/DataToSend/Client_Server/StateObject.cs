using System.Net;
using System.Net.Sockets;

namespace CommonDll.Client_Server
{
    /// <summary>
    /// State object for receiving data from remote device
    /// </summary>
    public class StateObject
    {
        /// <summary>
        /// Client socket
        /// </summary>
        public Socket workSocket = null;
        public IPEndPoint remoteEP;
        /// <summary>
        /// Receive buffer
        /// </summary>
        public byte[] buffer = new byte[2048];
        /// <summary>
        /// Received data string
        /// </summary>
        public string content = "";
        public StateObject()
        {

        }
        public StateObject(Socket workSocket, IPEndPoint remoteEP)
        {
            this.workSocket = workSocket;
            this.remoteEP = remoteEP;
        }
        public StateObject(Socket workSocket)
        {
            this.workSocket = workSocket;
        }
        public StateObject(Socket workSocket, byte[] buffer)
        {
            this.workSocket = workSocket;
            this.buffer = buffer;
        }
        public StateObject(StateObject @object, byte[] buffer)
        {
            this.workSocket = @object.workSocket;
            this.buffer = buffer;
            this.content = @object.content;
        }
        /// <summary>
        /// Задание буфера на прием размером с ожидаемый пакет данных
        /// </summary>
        /// <param name="bytesLength"></param>
        public void newBufferSize(int bytesLength)
        {
            buffer = new byte[bytesLength];
        }
    }
}
