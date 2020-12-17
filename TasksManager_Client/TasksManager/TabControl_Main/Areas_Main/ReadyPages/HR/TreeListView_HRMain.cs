using BrightIdeasSoftware;
using CommonDll.Structs;
using CommonDll.Structs.F_HR;
using CommonDll.Structs.F_LogFile.F_LogRecord;
using CommonDll.Structs.F_LogFile.F_LogRecord.F_TypeRecord;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TasksManager.OtherForm;

namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR
{
    public partial class TreeListView_HRMain : CommonTreeListView
    {
        public TreeListView_HRMain()
        {
            InitializeComponent();

            this.checkComboListDate.SetConfiguration(default(DateTime), "Период");
            this.checkComboListDate.OnSelectedValueChanged += OnSelectedValueChanged;
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

            TreeListView.CanExpandGetter = delegate (object x)
            {
                return
                    x is CommonDll.Structs.F_HR.HR ? ((CommonDll.Structs.F_HR.HR)x).Workers_All.Where(HR_Worker => CheckFilters(HR_Worker, TaskManagerForm.NameUser)).ToList().Count() > 0 :
                    x is HR_Worker ? ((HR_Worker)x).SubWorkers.Count() > 0 :
                    default;
            };
            TreeListView.ChildrenGetter = delegate (object x)
            {
                return
                    x is CommonDll.Structs.F_HR.HR ? ((CommonDll.Structs.F_HR.HR)x).Workers_All.Where(HR_Worker => CheckFilters(HR_Worker, TaskManagerForm.NameUser)).ToList().ToList() :
                    x is HR_Worker ? ((HR_Worker)x).SubWorkers.ToList() :
                    default;
            };

            OLVColumn Name = new OLVColumn("Имя", nameof(Name));
            Name.AspectGetter = delegate (object x)
            {
                return
                    x is CommonDll.Structs.F_HR.HR ? ((CommonDll.Structs.F_HR.HR)x).Base_Name :
                    x is HR_Worker ? ((HR_Worker)x).Name :
                    default;
            };
            Name.AutoCompleteEditorMode = AutoCompleteMode.None;
            Name.Width = 200;
            Name.IsEditable = false;
            Name.TextAlign = HorizontalAlignment.Left;
            TreeListView.AllColumns.Add(Name);

            OLVColumn EnrollmentDate = new OLVColumn("Дата поступления на работу", nameof(EnrollmentDate));
            EnrollmentDate.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Worker ? ((HR_Worker)x).EnrollmentDate :
                    default;
            };
            EnrollmentDate.AutoCompleteEditorMode = AutoCompleteMode.None;
            EnrollmentDate.Width = 80;
            EnrollmentDate.IsEditable = false;
            EnrollmentDate.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(EnrollmentDate);

            OLVColumn BaseRate = new OLVColumn("Базовая ставка", nameof(BaseRate));
            BaseRate.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Worker ? ((HR_Worker)x).BaseRate :
                    default;
            };
            BaseRate.AutoCompleteEditorMode = AutoCompleteMode.None;
            BaseRate.Width = 100;
            BaseRate.IsEditable = false;
            BaseRate.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(BaseRate);

            OLVColumn Group = new OLVColumn("Группа", nameof(Group));
            Group.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Worker ? ((HR_Worker)x).Group.Name :
                    default;
            };
            Group.AutoCompleteEditorMode = AutoCompleteMode.None;
            Group.Width = 100;
            Group.IsEditable = false;
            Group.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Group);

            OLVColumn PercentSubWorkers = new OLVColumn("% с подчиненных", nameof(PercentSubWorkers));
            PercentSubWorkers.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Worker ? ((HR_Worker)x).Group is null ? default : ((HR_Worker)x).Group.PercentSubWorkers :
                    default;
            };
            PercentSubWorkers.AutoCompleteEditorMode = AutoCompleteMode.None;
            PercentSubWorkers.Width = 100;
            PercentSubWorkers.IsEditable = false;
            PercentSubWorkers.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(PercentSubWorkers);

            OLVColumn CurrentPercent = new OLVColumn("Текущий %", nameof(CurrentPercent));
            CurrentPercent.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Worker ? ((HR_Worker)x).CurrentPercent(DateTime.Now) :
                    default;
            };
            CurrentPercent.AutoCompleteEditorMode = AutoCompleteMode.None;
            CurrentPercent.Width = 100;
            CurrentPercent.IsEditable = false;
            CurrentPercent.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(CurrentPercent);

            OLVColumn PaymentPeriod = new OLVColumn("Зарплата", nameof(PaymentPeriod));
            PaymentPeriod.AspectGetter = delegate (object x)
            {
                return
                    x is HR_Worker ? this.checkComboListDate.IsCheck ? 
                    ((HR_Worker)x).PaymentPeriod(this.checkComboListDate.GetStartDate, this.checkComboListDate.GetEndDate) :
                    default :
                    x is CommonDll.Structs.F_HR.HR ? ((CommonDll.Structs.F_HR.HR)x).PaymentPeriod(this.checkComboListDate.GetStartDate, this.checkComboListDate.GetEndDate) :
                    default;
            };
            PaymentPeriod.AutoCompleteEditorMode = AutoCompleteMode.None;
            PaymentPeriod.Width = 100;
            PaymentPeriod.IsEditable = false;
            PaymentPeriod.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(PaymentPeriod);

            TreeListView.RebuildColumns();
            TreeListView.GridLines = true; // Отображение сетки.
        }
        #endregion

        #region Format Row & Cell
        public override FormatRowEventArgs Virtual_GetRowObject_Value(FormatRowEventArgs e)
        {
            if (e.Item.RowObject is CommonDll.Structs.F_HR.HR)
            {
                int ColorN = 240;

                if (e.Item.BackColor != Color.FromArgb(ColorN, ColorN, ColorN))
                    e.Item.BackColor = Color.FromArgb(ColorN, ColorN, ColorN); // Серый
            }
            else if (e.Item.RowObject is HR_Worker)
            {
                if (!ImportProtocol.HR.Workers_Hierarchy.Contains((HR_Worker)e.Item.RowObject))
                    if (e.Item.BackColor != Color.AliceBlue)
                        e.Item.BackColor = Color.AliceBlue;
            }

            return e;
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
                    SetObjects(new List<CommonDll.Structs.F_HR.HR>() { ImportProtocol.HR }, new Action(() => ExpandOrColapse<CommonDll.Structs.F_HR.HR>(true)));
                }
            }));
        }
        public bool CheckFilters(HR_Worker HR_Worker, string Name)
        {
            if (!string.IsNullOrEmpty(Name))
                if (HR_Worker.Name == Name)
                    return true;
                else if (HR_Worker.GetParent<HR_Worker>() is HR_Worker)
                    return CheckFilters(HR_Worker.GetParent<HR_Worker>(), Name);
                else
                    return false;

            return true;
        }
        private void добавитьРаботникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(ImportProtocol.HR is null))
            {
                HR_AddWorker HR_AddWorker = new HR_AddWorker();
                HR_AddWorker.Set(ImportProtocol.HR.Groups, ImportProtocol.HR.Workers_All);

                if (this.TreeListView.SelectedObject is HR_Worker)
                    HR_AddWorker.Set((HR_Worker)this.TreeListView.SelectedObject);

                HR_AddWorker.ShowDialog();

                if (!(HR_AddWorker.HR_Worker is null))
                {
                    LogRecord.LogFile_Modify(new TypeRecord() { Text = TypeRecord.Update }, TypeRecord.Update, HR_AddWorker.HR_Worker);

                    HR_AddWorker.HR_Worker.ClonePart();

                    if (HR_AddWorker.HR_Worker.Parent is HR_Worker)
                    {
                        if (MainParentClass.ToFindObject_AtMainStruct(ImportProtocol.HR, HR_AddWorker.HR_Worker.Parent, out TimeSpan Time, out MainParentClass FindedHR_Worker))
                            if (FindedHR_Worker is HR_Worker)
                                ((HR_Worker)FindedHR_Worker).SubWorkers.Add(HR_AddWorker.HR_Worker);
                    }
                    else
                        ImportProtocol.HR.Workers_Hierarchy.Add(HR_AddWorker.HR_Worker);

                    Changed();
                }
            }                
        }
        private void добавитьГруппуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(ImportProtocol.HR is null))
            {
                HR_AddGroup HR_AddGroup = new HR_AddGroup();
                HR_AddGroup.ShowDialog();

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
