using CommonDll.Client_Server.Client;
using CommonDll.Helps;
using CommonDll.Structs;
using CommonDll.Structs.F_ProgressOfUpdate;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TasksManager.TabControl_Main;
using TasksManager.TabControl_Main.F_Loading;
using Task = System.Threading.Tasks.Task;

namespace TasksManager.OtherForm
{
    public partial class TaskManagerForm : ImportDesignForm
    {
        public TaskManagerForm()
        {
            InitializeComponent();

            StatusStripGlobal.Items.Add(toolStripStatusLabelExport);
            StatusStripGlobal.Items.Add(ToolStripStatusLabel);
            StatusStrip = this.StatusStripGlobal;
        }
        public static string NameUser { get; set; }
        ToolStripStatusLabel ToolStripStatusLabel { get; set; } = new ToolStripStatusLabel() { BorderSides = ToolStripStatusLabelBorderSides.All };
        public static StatusStrip StatusStrip { get; set; }
        public static ToolStripStatusLabel toolStripStatusLabelExport { get; set; } = new ToolStripStatusLabel() { BorderSides = ToolStripStatusLabelBorderSides.All };
        #region Main start events || OnLoad
        bool OperatorLaser { get; set; }
        private void TaskManagerForm_Shown(object sender, EventArgs e)
        {
            if (Thread.CurrentThread.Name is null)
                Thread.CurrentThread.Name = nameof(TaskManagerForm_Shown);

            if (true)
            {

            } // HR
            else
            {
                tabControlMain.TabPages.Remove(tabPageHR);
            }

            if (false & !OperatorLaser)
            {

            } // Отладка сервера
            else
            {
                tabControlMain.TabPages.Remove(tabPage_ServerProcess);
            }

            UpdateModules();
            Updated = true;
        }
        bool updated;
        bool Updated
        {
            get
            {
                return updated;
            }
            set
            {
                updated = value;

                if (updated)
                    if (LoadDialog is null)
                    {
                        Task.Run(() =>
                        {
                            Thread.CurrentThread.Priority = ThreadPriority.Highest;

                            if (this.InvokeRequired)
                                this.Invoke(new Action(() =>
                                {
                                    LoadDialog = new LoadDialog();
                                    this.Controls.Add(LoadDialog);

                                    if (this.Width > 0 & this.Height > 0)
                                        LoadDialog.Location = new Point() { X = this.Width / 2 - LoadDialog.Width / 2, Y = this.Height / 2 - LoadDialog.Height / 2 };

                                    LoadDialog.BackColor = Color.Transparent;
                                }
                                ));
                        });
                    }
                    else
                    {

                    }
                else if (!updated)
                    if (!(LoadDialog is null))
                        if (LoadDialog.InvokeRequired)
                            LoadDialog.Invoke(new Action(() =>
                            {
                                LoadDialog.Working = false;
                                this.Controls.Remove(LoadDialog);
                                LoadDialog = null;
                            }));
                        else
                        {
                            LoadDialog.Working = false;
                            this.Controls.Remove(LoadDialog);
                            LoadDialog = null;
                        }
            }
        }
        static LoadDialog LoadDialog { get; set; }
        public void UpdateModules()
        {
            List<string> ListNameOfValue = new List<string>();

            if (tabControlMain.TabPages.Contains(tabPage_ServerProcess))  // отладка сервера
                ListNameOfValue.Add(nameof(StructureValueForClient.ProgressOfUpdate));

            if (tabControlMain.TabPages.Contains(tabPageHR))
                ListNameOfValue.Add(nameof(StructureValueForClient.HR));

            ImportProtocol.Set(ListNameOfValue);
            ImportProtocol.OnUpdate += OnUpdate;
        }
        private void OnUpdate(bool Update, TimeSpan TimeFromServer, StructureValueForClient StructureValueForClient_Setted, ConnectionBackInformation ConnectionBackInformationLoad)
        {
            DateTime LastTimeUpdate = DateTime.Now;

            if (Update)
            {
                Updated = false;

                if (true) //Load
                {
                    #region Pages load
                    #region HR
                    DateTime HRStart = DateTime.Now;

                    if (tabControlMain.TabPages.Contains(tabPageHR))
                        if (StructureValueForClient_Setted.UpdateTime_Module_HR > default(DateTime))
                            if (this.StructureValueForClient is null ? true :
                                    StructureValueForClient_Setted.UpdateTime_Module_HR >
                                    this.StructureValueForClient.UpdateTime_Module_HR)
                            {
                                Updated = true;
                                CommonDll.Structs.F_HR.HR HR = (CommonDll.Structs.F_HR.HR)StructureValueForClient_Setted.HR.Clone();

                                if (this.InvokeRequired)
                                    this.Invoke(new Action(() =>
                                    {
                                        hR_Page1.Set(LastTimeUpdate, StructureValueForClient_Setted.UpdateTime_Module_HR, HR);
                                    }));

                                HR = null;
                            }

                    if (!(StructureValueForClient_Setted.HR is null) & !(LoadDialog is null))
                        LoadDialog.Set(nameof(StructureValueForClient_Setted.HR), DateTime.Now - HRStart);
                    #endregion

                    #region отладка сервера
                    DateTime ProgressOfUpdateStart = DateTime.Now;

                    if (this.InvokeRequired)
                        this.Invoke(new Action(() =>
                         viewServerProcess1.LastTimeUpdate = LastTimeUpdate));
                    else
                        viewServerProcess1.LastTimeUpdate = LastTimeUpdate;

                    if (tabControlMain.TabPages.Contains(tabPage_ServerProcess))
                        if (StructureValueForClient_Setted.UpdateTime_Module_ProgressOfUpdate > default(DateTime))
                            if (this.StructureValueForClient is null ? true :
                                    StructureValueForClient_Setted.UpdateTime_Module_ProgressOfUpdate >
                                    this.StructureValueForClient.UpdateTime_Module_ProgressOfUpdate)
                                if (viewServerProcess1.LastTimeOnServer == default(DateTime))
                                {
                                    Updated = true;
                                    List<ProgressOfUpdateAtStructAttribute> List_ProgressOfUpdateAtStructAttribute = StructureValueForClient_Setted.ProgressOfUpdate.List_ProgressOfUpdateAtStructAttribute.Select(x => (ProgressOfUpdateAtStructAttribute)x.Clone()).ToList();

                                    if (this.InvokeRequired)
                                        this.Invoke(new Action(() =>
                                        {
                                            viewServerProcess1.LastTimeOnServer = StructureValueForClient_Setted.UpdateTime_Module_ProgressOfUpdate;
                                            viewServerProcess1.Set(List_ProgressOfUpdateAtStructAttribute);
                                        }));

                                    List_ProgressOfUpdateAtStructAttribute = null;
                                }

                    if (!(StructureValueForClient_Setted.ProgressOfUpdate is null) & !(LoadDialog is null))
                        LoadDialog.Set(nameof(StructureValueForClient_Setted.ProgressOfUpdate), DateTime.Now - ProgressOfUpdateStart);
                    #endregion
                    #endregion
                }

                Dictionary<string, MainParentClass> props = Helper.GetProperties<MainParentClass, StructureValueForClient>(StructureValueForClient_Setted);

                foreach (var From in props) // Обнулить не задействованные структуры.
                {
                    string Name = From.Key;
                    var ReadyStructure_Type = StructureValueForClient_Setted.GetType();

                    if (!(ReadyStructure_Type is null))
                    {
                        var To_Property = ReadyStructure_Type.GetProperty(Name);

                        if (!(To_Property is null))
                            To_Property.SetValue(StructureValueForClient_Setted, null);
                    }
                }

                this.StructureValueForClient = StructureValueForClient_Setted;
                StructureValueForClient_Setted = null;
                TimeSpan TimeLoadOnClient = DateTime.Now - LastTimeUpdate;

                if (Updated)
                    Updated = false;
                else
                {

                }

                TimeFromServer = new TimeSpan(TimeFromServer.Hours, TimeFromServer.Minutes, TimeFromServer.Seconds);
                TimeLoadOnClient = new TimeSpan(TimeLoadOnClient.Hours, TimeLoadOnClient.Minutes, TimeLoadOnClient.Seconds);

                if (StatusStripGlobal.InvokeRequired)
                    StatusStripGlobal.Invoke(new Action(() =>
                    {
                        ToolStripStatusLabel.BackColor = Color.Transparent;
                        ToolStripStatusLabel.Text = string.Format("Load from server: {0}, on client: {1}", TimeFromServer, TimeLoadOnClient);
                    }));
            }
            else
            {
                if (StatusStripGlobal.InvokeRequired)
                    StatusStripGlobal.Invoke(new Action(() =>
                    {
                        ToolStripStatusLabel.BackColor = Color.Orange;
                        ToolStripStatusLabel.Text = string.Format("Load from server: {0}, on client: {1}", "Failed", "at " + LastTimeUpdate);
                    }));
            }
        }
        StructureValueForClient StructureValueForClient { get; set; }
        #endregion

        private void TaskManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsExitButton)
                Process.GetCurrentProcess().Kill();
        }
        int BeforeIndex { get; set; }
        int LastIndex { get; set; }
        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LastIndex != tabControlMain.SelectedIndex)
            {
                BeforeIndex = LastIndex;
                LastIndex = tabControlMain.SelectedIndex;
            }
        }
    }
}