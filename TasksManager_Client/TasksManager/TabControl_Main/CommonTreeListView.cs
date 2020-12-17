using BrightIdeasSoftware;
using CommonDll.Helps;
using CommonDll.Structs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using TasksManager.TabControl_Main.Areas_Main.ReadyPages;

namespace TasksManager.TabControl_Main
{
    public partial class CommonTreeListView : LazyLoad
    {
        /// <summary>
        /// Common parent for TreeListView'es.
        /// </summary>
        public CommonTreeListView()
        {
            InitializeComponent();

            FillResultTree();
        }

        public delegate void selected(dynamic e);
        public event selected Selected;
        public List<dynamic> GetObjects_From_TreeListView()
        {
            return TreeListView.Objects.Cast<dynamic>().ToList();
        }

        #region FillResultTree
        [MTAThread]
        /// <summary>
        /// Use Override. Example in parent.
        /// </summary>
        /// <param name="tree">This is current TreeListView.</param>
        public virtual void FillResultTree() // Пример
        {
            TreeListView.AllColumns.Clear();
            TreeListView.RowHeight = 30;
            TreeListView.CellPadding = new Rectangle(3, 3, 3, 3);
            TreeListView.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;

            TreeListView.CanExpandGetter = delegate (object x)
            {
                return default;
            };
            TreeListView.ChildrenGetter = delegate (object x)
            {
                return default;
            };

            OLVColumn Attribute = new OLVColumn("Attribute", nameof(Attribute));
            Attribute.AspectGetter = delegate (object x)
            {
                return default;
            };
            Attribute.Width = 190;
            Attribute.IsEditable = false;
            Attribute.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Attribute);

            OLVColumn WorkPlace = new OLVColumn("WorkPlace", nameof(WorkPlace));
            WorkPlace.AspectGetter = delegate (object x)
            {
                return default;
            };
            WorkPlace.Width = 140;
            WorkPlace.IsEditable = false;
            WorkPlace.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(WorkPlace);

            OLVColumn IPAddress = new OLVColumn("IP Address", nameof(IPAddress));
            IPAddress.AspectGetter = delegate (object x)
            {
                return default;
            };
            IPAddress.Width = 140;
            IPAddress.IsEditable = false;
            IPAddress.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(IPAddress);

            OLVColumn Start = new OLVColumn("Дата начала", nameof(Start));
            Start.AspectGetter = delegate (object x)
            {
                return default;
            };
            Start.Width = 140;
            Start.IsEditable = false;
            Start.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Start);

            OLVColumn End = new OLVColumn("Дата окончания", nameof(End));
            End.AspectGetter = delegate (object x)
            {
                return default;
            };
            End.Width = 140;
            End.IsEditable = false;
            End.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(End);

            OLVColumn Helper = new OLVColumn("Помощник", nameof(Helper));
            Helper.AspectGetter = delegate (object x)
            {
                return default;
            };
            Helper.Width = 140;
            Helper.IsEditable = false;
            Helper.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Helper);

            OLVColumn Amount = new OLVColumn("Количество", nameof(Amount));
            Amount.AspectGetter = delegate (object x)
            {
                return default;
            };
            Amount.Width = 140;
            Amount.IsEditable = false;
            Amount.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Amount);

            OLVColumn Name = new OLVColumn("Название программы", nameof(Name));
            Name.AspectGetter = delegate (object x)
            {
                return default;
            };
            Name.Width = 140;
            Name.IsEditable = false;
            Name.TextAlign = HorizontalAlignment.Center;
            TreeListView.AllColumns.Add(Name);

            TreeListView.RebuildColumns();
            TreeListView.GridLines = true; // Отображение сетки.
        }
        #endregion

        #region Format Row & Cell  
        #region Row
        /// <summary>
        /// Use override for method 'Virtual_GetRowObject_Key(FormatRowEventArgs e)' for building a 'Key'.
        /// Use override for method 'Virtual_GetRowObject_Value(FormatRowEventArgs e)' for building a 'Value'.
        /// </summary>
        private void FormatRow(object sender, FormatRowEventArgs e)
        {
            e = Virtual_GetRowObject_Value(e);
        }

        /// <summary>
        /// Use override for method 'Virtual_GetRowObject_Value(FormatRowEventArgs e)' for building a 'Value'.
        /// </summary>
        public virtual FormatRowEventArgs Virtual_GetRowObject_Value(FormatRowEventArgs e)
        {
            return e;
        }
        #endregion

        #region Cell
        /// <summary>
        /// Use override for method 'Virtual_GetRowObject_Key(FormatRowEventArgs e)' for building a 'Key'.
        /// Use override for method 'Virtual_GetRowObject_Value(FormatRowEventArgs e)' for building a 'Value'.
        /// </summary>
        private void FormatCell(object sender, FormatCellEventArgs e)
        {
            e = Virtual_GetCellObject_Value(e);

            if (!(e.CellValue is null))
            {
                var defaul = Helper.ReturnDefault(e.CellValue);

                if (e.CellValue.Equals(defaul) || e.CellValue.Equals(default(TimeSpan).ToString()))
                    e.SubItem.ForeColor = Color.Transparent;
            }
        }

        /// <summary>        
        /// Use override for method 'Virtual_GetRowObject_Value(FormatRowEventArgs e)' for building a 'Value'.
        /// </summary>
        public virtual FormatCellEventArgs Virtual_GetCellObject_Value(FormatCellEventArgs e)
        {
            return e;
        }
        #endregion
        #endregion

        #region Collapse, Expand
        public bool IsSetted { get; set; }
        private List<string> lastExpand = new List<string>();
        private List<string> LastExpand
        {
            get
            {
                if (lastExpand is null)
                    lastExpand = new List<string>();

                return lastExpand;
            }
            set
            {
                lastExpand = value;
            }
        }
        public virtual bool ExpandOrColapse_Сondition<T>(T Model)
        {
            return true;
        }
        public void ExpandOrColapse<T>(bool Expand)
        {
            IsSetted = true;

            if (TreeListView.InvokeRequired)
                TreeListView.Invoke(new Action(() =>
                {
                    ExpandOrColapse_InvokerRequired<T>(Expand);
                    TreeListView.TopItemIndex = TopItemIndex;
                }
                ));
            else
            {
                ExpandOrColapse_InvokerRequired<T>(Expand);
                TreeListView.TopItemIndex = TopItemIndex;
            };

            IsSetted = false;
        }
        public void ExpandOrColapse<T>(bool Expand, T Object)
        {
            IsSetted = true;

            if (TreeListView.InvokeRequired)
                TreeListView.Invoke(new Action(() =>
                {
                    ExpandOrColapse_InvokerRequired(Expand, Object);
                    TreeListView.TopItemIndex = TopItemIndex;
                }));
            else
            {
                ExpandOrColapse_InvokerRequired(Expand, Object);
                TreeListView.TopItemIndex = TopItemIndex;
            }

            IsSetted = false;
        }
        public void ExpandOrColapse_InvokerRequired<T>(bool Expand)
        {
            dynamic Model = null;
            int Index = 0;
            Model = TryGetModel(Index);

            while (!(Model is null))
            {
                if (Model is T & ExpandOrColapse_Сondition(Model))
                {
                    bool IsExpanded = false;
                    IsExpanded = TreeListView.IsExpanded(Model);

                    if (Expand)
                    {
                        if (!IsExpanded)
                            TreeListView.Expand(Model);
                    }
                    else
                    {
                        if (IsExpanded)
                            TreeListView.Collapse(Model);
                    }
                }

                Index++;
                Model = TryGetModel(Index);
            }
        }
        public void ExpandOrColapse_InvokerRequired<T>(bool Expand, T Object)
        {
            dynamic Model = null;
            int Index = 0;
            Model = TryGetModel(Index);

            while (!(Model is null))
            {
                if (CheckItemContains(Object is MainParentClass ? ((MainParentClass)(dynamic)Object).Base_Name : (dynamic)Object, Model is MainParentClass ? ((MainParentClass)Model).Base_Name : Model))
                {
                    bool IsExpanded = false;
                    IsExpanded = TreeListView.IsExpanded(Model);

                    if (Expand)
                    {
                        if (!IsExpanded)
                            TreeListView.Expand(Model);
                    }
                    else
                    {
                        if (IsExpanded)
                            TreeListView.Collapse(Model);
                    }
                }

                Index++;

                Model = TryGetModel(Index);
            }
        }
        public object TryGetModel(int Index)
        {
            object Model = null;

            try
            {
                Model = TreeListView.GetModelObject(Index);
            }
            catch
            {
                TreeListView.Update();
                TreeListView.Invoke(new Action(() => TreeListView.Update()));

                try
                {
                    Model = TreeListView.GetModelObject(Index);
                }
                catch
                {
                    var GetObjectCount = TreeListView.TreeModel.GetObjectCount();
                    var ItemsCount = TreeListView.Items.Count;
                    TreeListView.Update();
                }
            }

            return Model;
        }
        private void TreeListViewResult_Collapsed(object sender, TreeBranchCollapsedEventArgs e)
        {
            if (e.Item != null)
                lock (LastExpand)
                    if (CheckItemContains(out int Index, e.Item.RowObject))
                        LastExpand.RemoveAt(Index);
        }
        private void TreeListViewResult_Expanded(object sender, TreeBranchExpandedEventArgs e)
        {
            if (e.Item != null)
                lock (LastExpand)
                    if (!CheckItemContains(out int Index, e.Item.RowObject))
                        LastExpand.Add(e.Item.RowObject is MainParentClass ? ((MainParentClass)e.Item.RowObject).Base_Name : e.Item.RowObject + "");
        }
        private void TreeListView_Expanding(object sender, TreeBranchExpandingEventArgs e)
        {
            if (!IsSetted)
            {
                e.Canceled = true;
                TopItemIndex = TreeListView.TopItemIndex;
                ExpandOrColapse(true, e.Item.RowObject);
            }
        }
        private void TreeListView_Collapsing(object sender, TreeBranchCollapsingEventArgs e)
        {
            if (!IsSetted)
            {
                e.Canceled = true;
                TopItemIndex = TreeListView.TopItemIndex;
                ExpandOrColapse(false, e.Item.RowObject);
            }
        }
        public bool CheckItemContains(out int Index, object RowObject)
        {
            Index = RowObject is null ? -1 : LastExpand.FindIndex(x => CheckItemContains(x, RowObject is MainParentClass ? ((MainParentClass)RowObject).Base_Name : RowObject + ""));

            return Index == -1 ? false : true;
        }
        public bool CheckItemContains(object Object, object Model)
        {
            bool Returned = false;

            if (Object is null ? Model is null : Object.GetType() == Model.GetType())
            {
                var ravno = Object == Model;
                var RefEq = object.ReferenceEquals(Object, Model);
                var Eq = Object is null ? false : Object.Equals(Model);

                if (ravno & RefEq & Eq)
                    Returned = true;
                else if (ravno & !RefEq & Eq)
                    Returned = true;
                else if (!ravno & RefEq & Eq)
                    Returned = true;
                else if (ravno & !RefEq & !Eq)
                    Returned = true;
                else if (!ravno & RefEq & !Eq)
                    Returned = true;
                else if (!ravno & !RefEq & Eq)
                    Returned = true;
                else if (ravno & RefEq & !Eq)
                    Returned = true;
            }

            return Returned;
        }
        #endregion

        #region Selected Object
        public int IndexClick { get; set; }
        private void TreeListView_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.RowIndex != -1)
                if (Selected != null)
                    Selected.Invoke(e.Model);
        }
        #region Next, Previous 
        public dynamic SelectNext_Previous(bool IsNext, List<Type> ByType)
        {
            int Index = IndexClick;
            dynamic Model = TreeListView.GetModelObject(Index);

            if (Model is null)
            {
                Model = TreeListView.GetModelObject(0);

                if (Model is null)
                    return null;
            }

            TreeListView.SelectedIndex = -1;
            TreeListView.Items[IndexClick].Selected = false;
            TreeListView.Items[IndexClick].Focused = false;

            while (!(Model is null))
            {
                if (IsNext)
                {
                    Index++;
                }
                else
                {
                    Index--;
                }

                Model = TreeListView.GetModelObject(Index);

                if (Model is null ? false : ByType is null ? false : ByType.Contains(Model.GetType()))
                {
                    IndexClick = Index;

                    break;
                }
            }

            TreeListView.SelectObject(TreeListView.GetModelObject(IndexClick));
            TreeListView.SelectedIndex = IndexClick;
            TreeListView.FocusedItem = TreeListView.Items[IndexClick];

            return Model;
        }
        #endregion 
        #endregion

        #region Статусная строка
        private readonly string ToolStrip_LastUpdateOnServer = "П.о. на сервере: ";
        private readonly string ToolStrip_LastUpdateOnClient = "П.о. на клиенте: ";
        private readonly string ToolStrip_LastQuery = "П. запрос: ";
        public string LastQuery { get { return toolStripStatusLabel_LastQuery.Text.Replace(ToolStrip_LastQuery, ""); } set { toolStripStatusLabel_LastQuery.Text = ToolStrip_LastQuery + value; } }
        #endregion     

        public delegate void onSelected(object SelectedObject);
        public event onSelected OnSelected;
        public void TreeListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndexClick = TreeListView.SelectedIndex;

            if (this.InvokeRequired)
                this.Invoke(new Action(() =>
                {
                    if (!(OnSelected is null))
                        OnSelected.Invoke(TreeListView.SelectedObject);
                }));
            else
            {
                if (!(OnSelected is null))
                    OnSelected.Invoke(TreeListView.SelectedObject);
            }
        }
        int TopItemIndex { get; set; }
        public void SetObjects<T>(List<T> collection, Action ExpandOrColapse)
        {
            LazyLoad.LazyLoading(this, MethodBase.GetCurrentMethod().Name, new Action(() =>
            {
                TimeSpan Time_Expand = default;

                if (Thread.CurrentThread.Name is null)
                    Thread.CurrentThread.Name = nameof(SetObjects);

                if (this.InvokeRequired)
                    this.Invoke(new Action(() => SetObjects(collection)));
                else
                    SetObjects(collection);

                if (!(ExpandOrColapse is null))
                {
                    DateTime StartExpand = DateTime.Now;
                    ExpandOrColapse.Invoke();
                    Time_Expand = DateTime.Now - StartExpand;
                }
            }));
        }
        private void SetObjects<T>(List<T> collection)
        {
            TopItemIndex = TreeListView.TopItemIndex;

            try
            {
                TreeListView.Objects = null;
            }
            catch // проблема индексации при асинхронном исполнении
            {

            }

            TreeListView.SetObjects(collection);
            TreeListView.SelectedObjects = null;
            TreeListView.ClearCachedInfo();

            if (LastExpand is null ? false : LastExpand.Count > 0)
                foreach (var r in LastExpand.ToList())
                    ExpandOrColapse(true, r);

            TreeListView.TopItemIndex = TopItemIndex;
        }
        private void SetObjects(List<string> collection)
        {
            try
            {
                TreeListView.Objects = null;
            }
            catch // проблема индексации при асинхронном исполнении
            {

            }

            TreeListView.SetObjects(collection);
            TreeListView.SelectedObjects = null;
            TreeListView.ClearCachedInfo();
        }
        private void toolStripStatusLabelToExcel_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
            Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];

            int i = 1;
            int i2 = 1;

            foreach (OLVColumn ch in TreeListView.AllColumns)
            {
                ws.Cells[i2, i] = ch.Text;
                i++;
            }

            i2++;

            for (int j = 0; j < TreeListView.Items.Count; j++)
            {
                ListViewItem lvi = TreeListView.GetItem(j);
                i = 1;

                foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                {
                    ws.Cells[i2, i] = lvs.ForeColor == Color.Transparent ? null : lvs.Text;
                    i++;
                }

                i2++;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            saveFileDialog1.FileName = this.ToString() + " " + DateTime.Now;
            saveFileDialog1.Title = "Export to Excel";
            DialogResult dr = saveFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
                wb.SaveAs(saveFileDialog1.FileName);

            wb.Close();
            app.Quit();
        }
        private void TreeListView_DoubleClick(object sender, EventArgs e)
        {
            var SelectedObject = this.TreeListView.SelectedObject;

            if (TreeListView.CellEditActivation != ObjectListView.CellEditActivateMode.DoubleClick)
                if (!(SelectedObject is null))
                    ExpandOrColapse(!this.TreeListView.IsExpanded(SelectedObject), SelectedObject);
                else
                {

                }
            else
            {

            }
        }

        #region Обновление/Импорт
        private DateTime lastTimeUpdate;
        public DateTime LastTimeUpdate
        {
            get
            {
                return lastTimeUpdate;
            }
            set
            {
                lastTimeUpdate = value;

                if (this.InvokeRequired)
                    StatusStripDataRefreshed_Bottom.Invoke(new Action(() =>
                    {
                        TimeSpan TIme = new TimeSpan(LastTimeUpdate.TimeOfDay.Hours, LastTimeUpdate.TimeOfDay.Minutes, LastTimeUpdate.TimeOfDay.Seconds);
                        toolStripStatusLabel_LastUpdateOnClient.Text = ToolStrip_LastUpdateOnClient + TIme;

                        if (DateTime.Now.TimeOfDay - TIme < TimeSpan.FromMinutes(1))
                            toolStripStatusLabel_LastUpdateOnClient.BackColor = Color.LightGreen;
                        else
                            toolStripStatusLabel_LastUpdateOnClient.BackColor = Color.Transparent;
                    }
                    ));
                else
                {
                    TimeSpan TIme = new TimeSpan(LastTimeUpdate.TimeOfDay.Hours, LastTimeUpdate.TimeOfDay.Minutes, LastTimeUpdate.TimeOfDay.Seconds);
                    toolStripStatusLabel_LastUpdateOnClient.Text = ToolStrip_LastUpdateOnClient + TIme;

                    if (DateTime.Now.TimeOfDay - TIme < TimeSpan.FromMinutes(1))
                        toolStripStatusLabel_LastUpdateOnClient.BackColor = Color.LightGreen;
                    else
                        toolStripStatusLabel_LastUpdateOnClient.BackColor = Color.Transparent;
                }
            }
        }
        private DateTime lastTimeOnServer;
        public DateTime LastTimeOnServer
        {
            get
            {
                return lastTimeOnServer;
            }
            set
            {
                lastTimeOnServer = value;

                if (this.InvokeRequired)
                    StatusStripDataRefreshed_Bottom.Invoke(new Action(() =>
                    {
                        TimeSpan TIme = new TimeSpan(LastTimeOnServer.TimeOfDay.Hours, LastTimeOnServer.TimeOfDay.Minutes, LastTimeOnServer.TimeOfDay.Seconds);
                        ToolStripStatusLabel_LastUpdateOnServer.Text = ToolStrip_LastUpdateOnServer + TIme;

                        if (DateTime.Now.TimeOfDay - TIme < TimeSpan.FromMinutes(1))
                            ToolStripStatusLabel_LastUpdateOnServer.BackColor = Color.LightGreen;
                        else
                            ToolStripStatusLabel_LastUpdateOnServer.BackColor = Color.Transparent;
                    }
                    ));
                else
                {
                    TimeSpan TIme = new TimeSpan(LastTimeOnServer.TimeOfDay.Hours, LastTimeOnServer.TimeOfDay.Minutes, LastTimeOnServer.TimeOfDay.Seconds);
                    ToolStripStatusLabel_LastUpdateOnServer.Text = ToolStrip_LastUpdateOnServer + TIme;

                    if (DateTime.Now.TimeOfDay - TIme < TimeSpan.FromMinutes(1))
                        ToolStripStatusLabel_LastUpdateOnServer.BackColor = Color.LightGreen;
                    else
                        ToolStripStatusLabel_LastUpdateOnServer.BackColor = Color.Transparent;
                }
            }
        }
        #endregion
    }
}
