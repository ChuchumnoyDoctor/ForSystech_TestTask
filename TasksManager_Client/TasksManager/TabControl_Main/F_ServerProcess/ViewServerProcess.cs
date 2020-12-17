using BrightIdeasSoftware;
using CommonDll.Structs.F_ProgressOfUpdate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TasksManager.TabControl_Main.Areas_Main.ReadyPages;

namespace TasksManager.TabControl_Main.F_ServerProcess
{
    public partial class ViewServerProcess : CommonTreeListView
    {
        public ViewServerProcess()
        {
            InitializeComponent();
        }
        #region Override 
        #region TreeListView         
        public override void FillResultTree()
        {
            TreeListView.Visible = false; // невидимый итем прогружается быстрее
            TreeListView.AllColumns.Clear();
            TreeListView.RowHeight = 20;
            TreeListView.CellPadding = new Rectangle(3, 3, 3, 3);
            TreeListView.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;

            TreeListView.CanExpandGetter = delegate (object x)
            {
                dynamic value = x is ProgressOfUpdateAtStructAttribute ? выполненоToolStripMenuItem.Checked ?
                            ((ProgressOfUpdateAtStructAttribute)x).Submodules is null ? 0 : ((ProgressOfUpdateAtStructAttribute)x).Submodules.Count :
                            ((ProgressOfUpdateAtStructAttribute)x).Submodules is null ? 0 : ((ProgressOfUpdateAtStructAttribute)x).Submodules.Where(z => z.Status != "Выполнено").Count() :
                    0;

                return value > 0;
            };
            TreeListView.ChildrenGetter = delegate (object x)
            {
                dynamic Key = x;

                return
                x is ProgressOfUpdateAtStructAttribute ? выполненоToolStripMenuItem.Checked ?
                        ((ProgressOfUpdateAtStructAttribute)x).Submodules :
                        ((ProgressOfUpdateAtStructAttribute)x).Submodules is null ? null : ((ProgressOfUpdateAtStructAttribute)x).Submodules.Where(z => z.Status != "Выполнено") :
                null;
            };

            OLVColumn Name = new OLVColumn("Именование", nameof(Name));
            Name.AspectGetter = delegate (object x)
            {
                dynamic value = x is ProgressOfUpdateAtStructAttribute ? (string.IsNullOrEmpty(((ProgressOfUpdateAtStructAttribute)x).Name_RussianEquivalent) ? ((ProgressOfUpdateAtStructAttribute)x).Name : ((ProgressOfUpdateAtStructAttribute)x).Name_RussianEquivalent) : default;

                return value;
            };
            Name.Width = 500;
            Name.IsEditable = false;
            Name.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Name);

            OLVColumn Count = new OLVColumn("Планируемое количество", nameof(Count));
            Count.AspectGetter = delegate (object x)
            {
                dynamic value = x is ProgressOfUpdateAtStructAttribute ? ((ProgressOfUpdateAtStructAttribute)x).Count : default;

                return value;
            };
            Count.Width = 100;
            Count.IsEditable = false;
            Count.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Count);

            OLVColumn SubmodulesCount = new OLVColumn("Фактическое количество", nameof(SubmodulesCount));
            SubmodulesCount.AspectGetter = delegate (object x)
            {
                dynamic value = x is ProgressOfUpdateAtStructAttribute ? ((ProgressOfUpdateAtStructAttribute)x).Submodules is null ? default : ((ProgressOfUpdateAtStructAttribute)x).Submodules.Count : default;

                return value;
            };
            SubmodulesCount.Width = 100;
            SubmodulesCount.IsEditable = false;
            SubmodulesCount.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(SubmodulesCount);

            OLVColumn Submodules_Done = new OLVColumn("Выполнено", nameof(Submodules_Done));
            Submodules_Done.AspectGetter = delegate (object x)
            {
                dynamic value = x is ProgressOfUpdateAtStructAttribute ? ((ProgressOfUpdateAtStructAttribute)x).Submodules_Done(0) : default;

                return value;
            };
            Submodules_Done.Width = 100;
            Submodules_Done.IsEditable = false;
            Submodules_Done.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Submodules_Done);

            OLVColumn ProgressBar = new OLVColumn("Прогресс", "ProgressBar");
            ProgressBar.ImageGetter = delegate (object x)
            {
                int Val = x is ProgressOfUpdateAtStructAttribute ? ((ProgressOfUpdateAtStructAttribute)x).Progress : default;
                dynamic value = default;

                lock (customProgressBarTemperary)
                {
                    customProgressBarTemperary.Set(Val);
                    Image largeImage = (Image)customProgressBarTemperary.GetDrawnBitmap().Clone();
                    value = largeImage;
                }

                return value;
            };
            ProgressBar.Width = 100;
            ProgressBar.IsEditable = false;
            ProgressBar.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(ProgressBar);

            if (customProgressBarTemperary is null)
                customProgressBarTemperary = new CustomProgressBar();

            customProgressBarTemperary.Visible = false;
            customProgressBarTemperary.Width = ProgressBar.Width;
            customProgressBarTemperary.Height = TreeListView.RowHeight == -1 ? 15 : TreeListView.RowHeight;

            OLVColumn LastChange_TimeSpan = new OLVColumn("Последнее изменение", nameof(LastChange_TimeSpan));
            LastChange_TimeSpan.AspectGetter = delegate (object x)
            {
                TimeSpan value = x is ProgressOfUpdateAtStructAttribute ? ((ProgressOfUpdateAtStructAttribute)x).LastChange_DateTime.TimeOfDay : default;
                value = TimeSpan.FromSeconds(Math.Round(value.TotalSeconds, 1));
                value = new TimeSpan(value.Days, value.Hours, value.Minutes, value.Seconds);

                return value;
            };
            LastChange_TimeSpan.Width = 100;
            LastChange_TimeSpan.IsEditable = false;
            LastChange_TimeSpan.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(LastChange_TimeSpan);

            OLVColumn TimeSpend = new OLVColumn("Затрачено", nameof(TimeSpend));
            TimeSpend.AspectGetter = delegate (object x)
            {
                TimeSpan value = x is ProgressOfUpdateAtStructAttribute ? ((ProgressOfUpdateAtStructAttribute)x).TimeSpend : default;
                value = TimeSpan.FromSeconds(Math.Round(value.TotalSeconds, 1));
                value = new TimeSpan(value.Days, value.Hours, value.Minutes, value.Seconds);

                return value;
            };
            TimeSpend.Width = 100;
            TimeSpend.IsEditable = false;
            TimeSpend.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(TimeSpend);

            OLVColumn Status = new OLVColumn("Статус", nameof(Status));
            Status.AspectGetter = delegate (object x)
            {
                dynamic value = x is ProgressOfUpdateAtStructAttribute ? ((ProgressOfUpdateAtStructAttribute)x).Status : default;

                return value;
            };
            Status.Width = 100;
            Status.IsEditable = false;
            Status.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Status);

            OLVColumn Comment = new OLVColumn("Комментарий", nameof(Comment));
            Comment.AspectGetter = delegate (object x)
            {
                dynamic value = x is ProgressOfUpdateAtStructAttribute ? ((ProgressOfUpdateAtStructAttribute)x).Comment : default;

                return value;
            };
            Comment.Width = 150;
            Comment.IsEditable = false;
            Comment.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Comment);

            OLVColumn ExceptionMessage = new OLVColumn("Сообщение ошибки", nameof(ExceptionMessage));
            ExceptionMessage.AspectGetter = delegate (object x)
            {
                dynamic value = x is ProgressOfUpdateAtStructAttribute ? ((ProgressOfUpdateAtStructAttribute)x).ExceptionMessage : default;

                return value;
            };
            ExceptionMessage.Width = 600;
            ExceptionMessage.IsEditable = false;
            ExceptionMessage.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(ExceptionMessage);

            TreeListView.RebuildColumns();
            TreeListView.GridLines = true; // Отображение сетки.

            TreeListView.Visible = true;
        }

        #region Format Row & Cell
        public override FormatCellEventArgs Virtual_GetCellObject_Value(FormatCellEventArgs e)
        {
            if (e.Model is ProgressOfUpdateAtStructAttribute)
            {
                ProgressOfUpdateAtStructAttribute row = (ProgressOfUpdateAtStructAttribute)e.Model;

                if (e.Column.AspectName == "Status")
                {
                    if (row.Status == ProgressOfUpdateAtStructAttribute.Status_Done)
                        e.SubItem.BackColor = Color.LightGreen;
                    else if (row.Status == ProgressOfUpdateAtStructAttribute.Status_Failed)
                        e.SubItem.BackColor = Color.Orange;
                }
                else if (e.Column.AspectName == "LastChange_TimeSpan")
                {
                    if (DateTime.Now - row.LastChange_DateTime < TimeSpan.FromMinutes(20))
                        e.SubItem.BackColor = Color.Yellow;
                }
            }

            return e;
        }
        #endregion
        #endregion
        #endregion

        #region new
        List<ProgressOfUpdateAtStructAttribute> List_ProgressOfUpdateAtStructAttribute { get; set; } // Main struct
        public void Set(List<ProgressOfUpdateAtStructAttribute> List_ProgressOfUpdateAtStructAttribute)
        {
            LazyLoad.LazyLoading(this, MethodBase.GetCurrentMethod().Name, new Action(() =>
            {
                if (List_ProgressOfUpdateAtStructAttribute is null ? false : List_ProgressOfUpdateAtStructAttribute.Count > 0)
                {
                    // ProgressOfUpdateAtStructAttribute_Screenshot - облегченная версия ProgressOfUpdateAtStructAttribute
                    this.List_ProgressOfUpdateAtStructAttribute = ProgressOfUpdateAtStructAttribute_Screenshot.CreateScreenShot(List_ProgressOfUpdateAtStructAttribute, true);

                    if (TreeListView.InvokeRequired)
                        try
                        {
                            TreeListView.Invoke(new Action(() => SetObjects(this.List_ProgressOfUpdateAtStructAttribute, null)));
                            TreeListView.Invoke(new Action(() => TreeListView.SelectedObjects = null));
                        }
                        catch
                        {

                        }
                    else
                    {
                        SetObjects(this.List_ProgressOfUpdateAtStructAttribute, null);
                        TreeListView.SelectedObjects = null;
                    }
                }
            }));
        }
        private void ВыполненоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Set(List_ProgressOfUpdateAtStructAttribute);
        }
        #endregion

        private void ViewServerProcess_Load(object sender, EventArgs e)
        {
            customProgressBarTemperary.Hide();
        }
    }
}
