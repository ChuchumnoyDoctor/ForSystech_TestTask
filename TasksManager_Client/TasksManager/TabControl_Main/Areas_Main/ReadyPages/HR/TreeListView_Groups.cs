using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonDll.Structs.F_HR;
using CommonDll.Structs.F_LogFile.F_LogRecord;
using CommonDll.Structs.F_LogFile.F_LogRecord.F_TypeRecord;
using System.Reflection;

namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR
{
    public partial class TreeListView_Groups : CommonTreeListView
    {
        public TreeListView_Groups()
        {
            InitializeComponent();
        }

        public delegate void onChanged();
        public event onChanged OnChanged;
        private void Changed()
        {
            if (!(OnChanged is null))
                OnChanged.Invoke();
        }
        private void OnSelectedValueChanged(object sender, EventArgs e)
        {
            Set(this.LastTimeUpdate, this.LastTimeOnServer);
        }
        #region Override 
        #region TreeListView         
        public override void FillResultTree()
        {
            TreeListView.AllColumns.Clear();
            TreeListView.RowHeight = 20;
            TreeListView.CellPadding = new Rectangle(3, 3, 3, 3);
            TreeListView.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
            TreeListView.CellEditStarting += delegate (object sender, CellEditEventArgs e)
            {
                e.Control.Text = default;
            };

            OLVColumn Name = new OLVColumn("Название", nameof(Name));
            Name.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Group ? ((HR_Group)x).Name :
                    default;
            };
            Name.AutoCompleteEditorMode = AutoCompleteMode.None;
            Name.Width = 200;
            Name.IsEditable = false;
            Name.TextAlign = HorizontalAlignment.Left;
            TreeListView.AllColumns.Add(Name);

            OLVColumn Percent = new OLVColumn("% за год", nameof(Percent));
            Percent.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Group ? ((HR_Group)x).Percent :
                    default;
            };
            Percent.AutoCompleteEditorMode = AutoCompleteMode.None;
            Percent.Width = 80;
            Percent.IsEditable = false;
            Percent.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Percent);

            OLVColumn MaxPercent = new OLVColumn("Суммарная надбавка", nameof(MaxPercent));
            MaxPercent.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Group ? ((HR_Group)x).MaxPercent :
                    default;
            };
            MaxPercent.AutoCompleteEditorMode = AutoCompleteMode.None;
            MaxPercent.Width = 200;
            MaxPercent.IsEditable = false;
            MaxPercent.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(MaxPercent);

            OLVColumn DeepLevelSubWorkers = new OLVColumn("Уровень подчиненных", nameof(DeepLevelSubWorkers));
            DeepLevelSubWorkers.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Group ? ((HR_Group)x).DeepLevelSubWorkers :
                    default;
            };
            DeepLevelSubWorkers.AutoCompleteEditorMode = AutoCompleteMode.None;
            DeepLevelSubWorkers.Width = 200;
            DeepLevelSubWorkers.IsEditable = false;
            DeepLevelSubWorkers.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(DeepLevelSubWorkers);

            OLVColumn PercentSubWorkers = new OLVColumn("% от зарплаты подчиненных", nameof(PercentSubWorkers));
            PercentSubWorkers.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Group ? ((HR_Group)x).PercentSubWorkers :
                    default;
            };
            PercentSubWorkers.AutoCompleteEditorMode = AutoCompleteMode.None;
            PercentSubWorkers.Width = 200;
            PercentSubWorkers.IsEditable = false;
            PercentSubWorkers.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(PercentSubWorkers);

            TreeListView.RebuildColumns();
            TreeListView.GridLines = true; // Отображение сетки.
        }
        #endregion
        #endregion
        public void Set(DateTime LastTimeUpdate, DateTime LastTimeOnServer)
        {
            LazyLoad.LazyLoading(this, MethodBase.GetCurrentMethod().Name, new Action(() =>
            {
                this.LastTimeUpdate = LastTimeUpdate;
                this.LastTimeOnServer = LastTimeOnServer;
                ImportProtocol.HR.ClonePart();

                if (!(ImportProtocol.HR is null))
                {
                    SetObjects(ImportProtocol.HR.Groups, null);
                }
            }));
        }
        private void добавитьГруппуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(ImportProtocol.HR is null))
            {
                HR_AddGroup HR_AddGroup = new HR_AddGroup();
                HR_AddGroup.ShowDialog();

                if (this.TreeListView.SelectedObject is HR_Group)
                    HR_AddGroup.Set((HR_Group)this.TreeListView.SelectedObject);

                if (!(HR_AddGroup.HR_Group is null))
                {
                    LogRecord.LogFile_Modify(new TypeRecord() { Text = TypeRecord.Update }, TypeRecord.Update, HR_AddGroup.HR_Group);
                    ImportProtocol.HR.Groups.Add(HR_AddGroup.HR_Group);
                    Changed();
                }
            }
        }
    }
}
