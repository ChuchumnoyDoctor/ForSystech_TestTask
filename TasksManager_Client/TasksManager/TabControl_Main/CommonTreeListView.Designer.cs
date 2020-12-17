using System.Threading;

namespace TasksManager.TabControl_Main
{
    partial class CommonTreeListView
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TreeListView = new BrightIdeasSoftware.TreeListView();
            this.ToolStripStatusLabel_LastUpdateOnServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_LastUpdateOnClient = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_LastQuery = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelToExcel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusStripDataRefreshed_Bottom = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.TreeListView)).BeginInit();
            this.StatusStripDataRefreshed_Bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeListView
            // 
            this.TreeListView.CellEditUseWholeCell = false;
            this.TreeListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeListView.FullRowSelect = true;
            this.TreeListView.HideSelection = false;
            this.TreeListView.Location = new System.Drawing.Point(0, 0);
            this.TreeListView.Name = "TreeListView";
            this.TreeListView.ShowGroups = false;
            this.TreeListView.Size = new System.Drawing.Size(935, 320);
            this.TreeListView.TabIndex = 7;
            this.TreeListView.UseCellFormatEvents = true;
            this.TreeListView.UseCompatibleStateImageBehavior = false;
            this.TreeListView.UseHotControls = false;
            this.TreeListView.UseNotifyPropertyChanged = true;
            this.TreeListView.UseWaitCursorWhenExpanding = false;
            this.TreeListView.View = System.Windows.Forms.View.Details;
            this.TreeListView.VirtualMode = true;
            this.TreeListView.Expanding += new System.EventHandler<BrightIdeasSoftware.TreeBranchExpandingEventArgs>(this.TreeListView_Expanding);
            this.TreeListView.Collapsing += new System.EventHandler<BrightIdeasSoftware.TreeBranchCollapsingEventArgs>(this.TreeListView_Collapsing);
            this.TreeListView.Expanded += new System.EventHandler<BrightIdeasSoftware.TreeBranchExpandedEventArgs>(this.TreeListViewResult_Expanded);
            this.TreeListView.Collapsed += new System.EventHandler<BrightIdeasSoftware.TreeBranchCollapsedEventArgs>(this.TreeListViewResult_Collapsed);
            this.TreeListView.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.TreeListView_CellClick);
            this.TreeListView.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.FormatCell);
            this.TreeListView.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.FormatRow);
            this.TreeListView.SelectedIndexChanged += new System.EventHandler(this.TreeListView_SelectedIndexChanged);
            this.TreeListView.DoubleClick += new System.EventHandler(this.TreeListView_DoubleClick);
            // 
            // ToolStripStatusLabel_LastUpdateOnServer
            // 
            this.ToolStripStatusLabel_LastUpdateOnServer.Name = "ToolStripStatusLabel_LastUpdateOnServer";
            this.ToolStripStatusLabel_LastUpdateOnServer.Size = new System.Drawing.Size(206, 17);
            this.ToolStripStatusLabel_LastUpdateOnServer.Text = "Последнее обновление на сервере: ";
            // 
            // toolStripStatusLabel_LastUpdateOnClient
            // 
            this.toolStripStatusLabel_LastUpdateOnClient.Name = "toolStripStatusLabel_LastUpdateOnClient";
            this.toolStripStatusLabel_LastUpdateOnClient.Size = new System.Drawing.Size(206, 17);
            this.toolStripStatusLabel_LastUpdateOnClient.Text = "Последнее обновление на клиенте: ";
            // 
            // toolStripStatusLabel_LastQuery
            // 
            this.toolStripStatusLabel_LastQuery.Name = "toolStripStatusLabel_LastQuery";
            this.toolStripStatusLabel_LastQuery.Size = new System.Drawing.Size(116, 17);
            this.toolStripStatusLabel_LastQuery.Text = "Последний запрос: ";
            // 
            // toolStripStatusLabelToExcel
            // 
            this.toolStripStatusLabelToExcel.Name = "toolStripStatusLabelToExcel";
            this.toolStripStatusLabelToExcel.Size = new System.Drawing.Size(91, 17);
            this.toolStripStatusLabelToExcel.Text = "Экспорт в Excel";
            this.toolStripStatusLabelToExcel.Click += new System.EventHandler(this.toolStripStatusLabelToExcel_Click);
            // 
            // StatusStripDataRefreshed_Bottom
            // 
            this.StatusStripDataRefreshed_Bottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelToExcel,
            this.ToolStripStatusLabel_LastUpdateOnServer,
            this.toolStripStatusLabel_LastUpdateOnClient,
            this.toolStripStatusLabel_LastQuery});
            this.StatusStripDataRefreshed_Bottom.Location = new System.Drawing.Point(0, 320);
            this.StatusStripDataRefreshed_Bottom.Name = "StatusStripDataRefreshed_Bottom";
            this.StatusStripDataRefreshed_Bottom.Size = new System.Drawing.Size(935, 22);
            this.StatusStripDataRefreshed_Bottom.TabIndex = 6;
            // 
            // CommonTreeListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.TreeListView);
            this.Controls.Add(this.StatusStripDataRefreshed_Bottom);
            this.Name = "CommonTreeListView";
            this.Size = new System.Drawing.Size(935, 342);
            ((System.ComponentModel.ISupportInitialize)(this.TreeListView)).EndInit();
            this.StatusStripDataRefreshed_Bottom.ResumeLayout(false);
            this.StatusStripDataRefreshed_Bottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public BrightIdeasSoftware.TreeListView TreeListView;
        public System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel_LastUpdateOnServer;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_LastUpdateOnClient;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_LastQuery;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelToExcel;
        public System.Windows.Forms.StatusStrip StatusStripDataRefreshed_Bottom;
    }
}
