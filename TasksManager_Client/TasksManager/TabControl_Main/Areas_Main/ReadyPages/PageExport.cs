using CommonDll.Structs;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages
{
    public partial class PageExport : LazyLoad
    {
        public PageExport()
        {
            InitializeComponent();
        }
        public MainParentClass CollectedChangesToExport { get; set; }
        public MainParentClass Set(MainParentClass Before, MainParentClass SettedStruct, bool Exported, out TimeSpan TimeOnSetted)
        {
            DateTime TimeOnSetted_Start = DateTime.Now;
            TimeSpan MainParentClass_Changes_ToChanged_Time = default;
            TimeSpan CollectedChangesToExport_ToChanged_Time = default;
            TimeSpan Set_MainParentClass_ToChanged_Time = default;
            TimeSpan Before_ToChanged_Time = default;
            bool IsClient = false;

            if (SettedStruct is null ? false : MainParentClass.ToFindTypeRecord(SettedStruct, out TimeSpan TimeS, out MainParentClass FindedObject))
            { // From Client
                IsChanged = true;
                IsClient = true;

                if (CollectedChangesToExport is null)
                {
                    CollectedChangesToExport = (MainParentClass)SettedStruct.Clone();
                    MainParentClass.ToChanged(CollectedChangesToExport, CollectedChangesToExport, out CollectedChangesToExport_ToChanged_Time, true, out string Exception);
                }
                else
                {
                    MainParentClass.ToChanged((MainParentClass)SettedStruct.Clone(), CollectedChangesToExport, out MainParentClass_Changes_ToChanged_Time, true, out string Exception);

                    if (!string.IsNullOrEmpty(Exception))
                    {

                    }
                }

                if (!(Before is null))
                {
                    MainParentClass.ToChanged(SettedStruct, Before, out Before_ToChanged_Time, false, out string Exception);

                    if (!string.IsNullOrEmpty(Exception))
                    {

                    }
                }

            }
            else
            {  // From Server0
                IsClient = false;

                if (IsChanged)
                {
                    MainParentClass.ToChanged(CollectedChangesToExport, SettedStruct, out Set_MainParentClass_ToChanged_Time, false, out string Exception);

                    if (!string.IsNullOrEmpty(Exception))
                    {

                    }
                }

                if (Clear_CollectedChangesToExport)  // Обнулить экспортируемые данные
                {
                    Clear_CollectedChangesToExport = false;
                    CollectedChangesToExport = null;
                }
            }

            if (IsChanged & Exported)
            {
                buttonToExport_Click(default, default); // сразу экспорт
            }

            TimeOnSetted = DateTime.Now - TimeOnSetted_Start;

            if (TimeOnSetted > TimeSpan.FromSeconds(10))
            {

            }
            else if (TimeOnSetted > TimeSpan.FromSeconds(5))
            {

            }
            else if (TimeOnSetted > TimeSpan.FromSeconds(2))
            {

            }

            return IsClient ? Before : SettedStruct;
        }
        public virtual StructureValueForClient StructureValueForClient { get; }
        bool Clear_CollectedChangesToExport { get; set; } = false;
        public void buttonToExport_Click(object sender, EventArgs e)
        {
            if (!(StructureValueForClient is null))
                if (CollectedChangesToExport is null ? false : MainParentClass.ToFindTypeRecord(CollectedChangesToExport, out TimeSpan TimeS, out MainParentClass FindedObject))
                {
                    Task.Run(() =>
                    {
                        if (new ExportDesignForm().SetToServer(sender, e, StructureValueForClient, out string Exception))
                        {
                            Clear_CollectedChangesToExport = true;  // Обнулить экспортируемые данные
                            IsChanged = false;
                        }
                        else
                        {

                        }
                    });
                }
                else
                {

                }
            else
            {

            }
        }
        bool isChanged = false;
        private bool IsChanged
        {
            get
            {
                return isChanged;
            }
            set
            {
                isChanged = value;

                if (this.InvokeRequired)
                    this.Invoke(new Action(() =>
                    {
                        if (isChanged)
                        {
                            buttonToExport.Enabled = true;
                            buttonToExport.BackColor = Color.Yellow;
                        }
                        else
                        {
                            buttonToExport.Enabled = false;
                            buttonToExport.BackColor = Color.Transparent;
                        }
                    }));
                else
                {
                    if (isChanged)
                    {
                        buttonToExport.Enabled = true;
                        buttonToExport.BackColor = Color.Yellow;
                    }
                    else
                    {
                        buttonToExport.Enabled = false;
                        buttonToExport.BackColor = Color.Transparent;
                    }
                }
            }
        }
    }
}
