namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR
{
    partial class TreeListView_HRMain
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
            this.panelTop_Filtes = new System.Windows.Forms.Panel();
            this.checkComboListDate = new TasksManager.TabControl_Main.Areas_Main.ReadyPages.CheckComboList();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьРаботникаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьГруппуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.TreeListView)).BeginInit();
            this.panelTop_Filtes.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeListView
            // 
            this.TreeListView.Location = new System.Drawing.Point(0, 144);
            this.TreeListView.Size = new System.Drawing.Size(933, 174);
            // 
            // panelTop_Filtes
            // 
            this.panelTop_Filtes.AutoSize = true;
            this.panelTop_Filtes.Controls.Add(this.checkComboListDate);
            this.panelTop_Filtes.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop_Filtes.Location = new System.Drawing.Point(0, 0);
            this.panelTop_Filtes.Name = "panelTop_Filtes";
            this.panelTop_Filtes.Size = new System.Drawing.Size(933, 144);
            this.panelTop_Filtes.TabIndex = 12;
            // 
            // checkComboListDate
            // 
            this.checkComboListDate.BackColor = System.Drawing.Color.Transparent;
            this.checkComboListDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkComboListDate.CheckBox_Text = "CheckBox_Text";
            this.checkComboListDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkComboListDate.Location = new System.Drawing.Point(0, 0);
            this.checkComboListDate.MinimumSize = new System.Drawing.Size(100, 23);
            this.checkComboListDate.Name = "checkComboListDate";
            this.checkComboListDate.Size = new System.Drawing.Size(933, 144);
            this.checkComboListDate.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьРаботникаToolStripMenuItem,
            this.добавитьГруппуToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(245, 48);
            // 
            // добавитьРаботникаToolStripMenuItem
            // 
            this.добавитьРаботникаToolStripMenuItem.Name = "добавитьРаботникаToolStripMenuItem";
            this.добавитьРаботникаToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.добавитьРаботникаToolStripMenuItem.Text = "Добавить/изменить работника";
            this.добавитьРаботникаToolStripMenuItem.Click += new System.EventHandler(this.добавитьРаботникаToolStripMenuItem_Click);
            // 
            // добавитьГруппуToolStripMenuItem
            // 
            this.добавитьГруппуToolStripMenuItem.Name = "добавитьГруппуToolStripMenuItem";
            this.добавитьГруппуToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.добавитьГруппуToolStripMenuItem.Text = "Добавить группу";
            this.добавитьГруппуToolStripMenuItem.Click += new System.EventHandler(this.добавитьГруппуToolStripMenuItem_Click);
            // 
            // TreeListView_HRMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.panelTop_Filtes);
            this.Name = "TreeListView_HRMain";
            this.Controls.SetChildIndex(this.panelTop_Filtes, 0);
            this.Controls.SetChildIndex(this.TreeListView, 0);
            ((System.ComponentModel.ISupportInitialize)(this.TreeListView)).EndInit();
            this.panelTop_Filtes.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop_Filtes;
        private CheckComboList checkComboListDate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem добавитьРаботникаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьГруппуToolStripMenuItem;
    }
}
