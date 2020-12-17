using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Reflection;

namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR
{
    public partial class HR_Page : PageExport
    {
        public HR_Page()
        {
            InitializeComponent();

            buttonToExport.Text += " - HR";
            buttonToExport.Hide();
            treeListView_HRMain1.OnChanged += delegate { Set(treeListView_HRMain1.LastTimeUpdate, treeListView_HRMain1.LastTimeOnServer, ImportProtocol.HR); };
            treeListView_Groups1.OnChanged += delegate { Set(treeListView_HRMain1.LastTimeUpdate, treeListView_HRMain1.LastTimeOnServer, ImportProtocol.HR); };
        }
        public void Set(DateTime LastTimeUpdate, DateTime LastTimeOnServer, CommonDll.Structs.F_HR.HR Set_HR)
        {
            Set_HR = (CommonDll.Structs.F_HR.HR)base.Set(ImportProtocol.HR, Set_HR, true, out TimeSpan TimeOnSetted);
            ImportProtocol.HR = Set_HR;

            LazyLoad.LazyLoading(this, MethodBase.GetCurrentMethod().Name, new Action(() =>
            {
                treeListView_HRMain1.Set(LastTimeUpdate, LastTimeOnServer);
                treeListView_Groups1.Set(LastTimeUpdate, LastTimeOnServer);
            }));
        }
        public override StructureValueForClient StructureValueForClient { get { return new StructureValueForClient() { HR = (CommonDll.Structs.F_HR.HR)CollectedChangesToExport }; } }
    }
}
