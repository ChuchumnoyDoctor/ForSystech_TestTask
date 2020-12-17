namespace TasksManager.TabControl_Main.F_ServerProcess
{
    partial class ViewServerProcess
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
            this.contextMenuStrip_TreeListViewResult = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.обновитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.автообновлениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выполненоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customProgressBarTemperary = new TasksManager.TabControl_Main.F_ServerProcess.CustomProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.TreeListView)).BeginInit();
            this.contextMenuStrip_TreeListViewResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeListView
            // 
            this.TreeListView.ContextMenuStrip = this.contextMenuStrip_TreeListViewResult;
            this.TreeListView.Size = new System.Drawing.Size(939, 448);
            // 
            // contextMenuStrip_TreeListViewResult
            // 
            this.contextMenuStrip_TreeListViewResult.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обновитьToolStripMenuItem,
            this.выполненоToolStripMenuItem});
            this.contextMenuStrip_TreeListViewResult.Name = "contextMenuStrip_TreeListViewResult";
            this.contextMenuStrip_TreeListViewResult.Size = new System.Drawing.Size(192, 48);
            // 
            // обновитьToolStripMenuItem
            // 
            this.обновитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.автообновлениеToolStripMenuItem});
            this.обновитьToolStripMenuItem.Name = "обновитьToolStripMenuItem";
            this.обновитьToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.обновитьToolStripMenuItem.Text = "Обновить на клиенте";
            // 
            // автообновлениеToolStripMenuItem
            // 
            this.автообновлениеToolStripMenuItem.Checked = true;
            this.автообновлениеToolStripMenuItem.CheckOnClick = true;
            this.автообновлениеToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.автообновлениеToolStripMenuItem.Name = "автообновлениеToolStripMenuItem";
            this.автообновлениеToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.автообновлениеToolStripMenuItem.Text = "Автообновление";
            // 
            // выполненоToolStripMenuItem
            // 
            this.выполненоToolStripMenuItem.Checked = true;
            this.выполненоToolStripMenuItem.CheckOnClick = true;
            this.выполненоToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.выполненоToolStripMenuItem.Name = "выполненоToolStripMenuItem";
            this.выполненоToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.выполненоToolStripMenuItem.Text = "Выполнено";
            this.выполненоToolStripMenuItem.Click += new System.EventHandler(this.ВыполненоToolStripMenuItem_Click);
            // 
            // customProgressBarTemperary
            // 
            this.customProgressBarTemperary.Location = new System.Drawing.Point(803, 379);
            this.customProgressBarTemperary.Name = "customProgressBarTemperary";
            this.customProgressBarTemperary.Size = new System.Drawing.Size(66, 22);
            this.customProgressBarTemperary.TabIndex = 9;
            // 
            // ViewServerProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customProgressBarTemperary);
            this.Name = "ViewServerProcess";
            this.Size = new System.Drawing.Size(939, 470);
            this.Load += new System.EventHandler(this.ViewServerProcess_Load);
            this.Controls.SetChildIndex(this.TreeListView, 0);
            this.Controls.SetChildIndex(this.customProgressBarTemperary, 0);
            ((System.ComponentModel.ISupportInitialize)(this.TreeListView)).EndInit();
            this.contextMenuStrip_TreeListViewResult.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip_TreeListViewResult;
        private System.Windows.Forms.ToolStripMenuItem обновитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem автообновлениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выполненоToolStripMenuItem;
        private CustomProgressBar customProgressBarTemperary;
    }
}
