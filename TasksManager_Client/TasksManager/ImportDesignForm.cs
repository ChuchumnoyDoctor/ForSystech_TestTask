using CommonDll.Client_Server.Client;
using CommonDll.Helps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using TasksManager.LocalConfig;
using Task = System.Threading.Tasks.Task;

namespace TasksManager
{
    public partial class ImportDesignForm : Form
    {
        public ImportDesignForm() // Оставить try{}catch{} у всех методов, т.к. компилится в runtime, но не кампилится как дезайн контрол
        {
            InitializeComponent();
        }
        private void ImportDesignForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (FormDesignes.Contains(this))
                    FormDesignes.Remove(this);
            }
            catch
            {

            }
        }
        public object InvokeRequired_LockObject = new object();
        public string PathMyDocuments { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); } }
        public static IPAddress IPAddress { get; set; } // Ip adress server'а
        public static int Port { get; set; } // Port server'а
        public static bool ServerIsRunning { get; set; }
        public static List<ImportDesignForm> FormDesignes { get; set; }
        static bool Loaded { get; set; }
        static object Loaded_Lock = new object();
        public void Connect_Click(object sender, EventArgs e)
        {
            try
            {
                lock (Loaded_Lock)
                    if (!Loaded)
                    {
                        TryConnectToServer();
                        Loaded = true;
                    }
                    else
                    {
                        UpdateStatus(this, ServerIsRunning, InvokeRequired_LockObject);
                    }
            }
            catch
            {

            }
        }

        private void TryConnectToServer()  // Обработчик настройки подключения к серверу
        {
            try
            {
                string Address = Helper.GetLocalIPv4(NetworkInterfaceType.Ethernet);
                IPAddress = IPAddress.TryParse(Address, out IPAddress IP) ? IP : default;
                Port = 11000;

                if (IPAddress == default) // Пустой Ip-адрес?
                {
                    // Настройка подключения к серверу
                }
                else
                    ConnectToServer.StartClient(IPAddress, Port);
            }
            catch
            {

            }
        }
        object OnConnected_Lock = new object();
        private void OnConnected(Socket Socket_Connected)
        {
            bool Connected = Socket_Connected.Connected;

            Task Task = Task.Run(() =>
            {
                if (!Helper.IsLocked(OnConnected_Lock))
                    lock (OnConnected_Lock)
                        //if (Socket_Connected.Connected != ServerIsRunning)
                        foreach (var form in FormDesignes)
                            UpdateStatus(form, ServerIsRunning = Connected, InvokeRequired_LockObject);
            });
        }
        private static void UpdateStatus(ImportDesignForm FormDesign, bool ServerIsRunning, object InvokeRequired_LockObject)
        {
            try
            {
                if (ServerIsRunning)
                {
                    if (FormDesign.toolStripStatusLabelStatusForConnectToServer.Text != "(" + IPAddress + ") " + "Online")
                        if (FormDesign.InvokeRequired)
                        {
                            FormDesign.Invoke(new Action(() => FormDesign.toolStripStatusLabelStatusForConnectToServer.Text = "(" + IPAddress + ") " + "Online"));
                            FormDesign.Invoke(new Action(() => FormDesign.toolStripStatusLabelStatusForConnectToServer.BackColor = Color.LightGreen));
                        }
                        else
                        {
                            FormDesign.toolStripStatusLabelStatusForConnectToServer.Text = "(" + IPAddress + ") " + "Online";
                            FormDesign.toolStripStatusLabelStatusForConnectToServer.BackColor = Color.LightGreen;
                        }
                }
                else
                {
                    if (FormDesign.toolStripStatusLabelStatusForConnectToServer.Text != "Offline")
                        if (FormDesign.InvokeRequired)
                        {
                            FormDesign.Invoke(new Action(() => FormDesign.toolStripStatusLabelStatusForConnectToServer.Text = "Offline"));
                            FormDesign.Invoke(new Action(() => FormDesign.toolStripStatusLabelStatusForConnectToServer.BackColor = Color.Yellow));
                        }
                        else
                        {
                            FormDesign.toolStripStatusLabelStatusForConnectToServer.Text = "Offline";
                            FormDesign.toolStripStatusLabelStatusForConnectToServer.BackColor = Color.Yellow;
                        }
                }
            }
            catch
            {

            }
        }
        public bool IsExitButton { get; set; }
        private void FormDesign_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            IsExitButton = false;

            try
            {
                ConnectToServer.OnConnect += OnConnected;

                if (FormDesignes is null)
                    FormDesignes = new List<ImportDesignForm>();

                if (!FormDesignes.Contains(this))
                    FormDesignes.Add(this);
            }
            catch
            {

            }

            try
            {
                this.TopMost = true;
                this.BringToFront();
                this.TopMost = false;
                Connect_Click(sender, e);
            }
            catch
            {

            }

            UpdateStatus(this, ServerIsRunning, InvokeRequired_LockObject);
        }
    }
}
