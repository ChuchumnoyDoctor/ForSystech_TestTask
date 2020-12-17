namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR
{
    partial class HR_Page
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageHR = new System.Windows.Forms.TabPage();
            this.treeListView_HRMain1 = new TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR.TreeListView_HRMain();
            this.tabPageGroups = new System.Windows.Forms.TabPage();
            this.treeListView_Groups1 = new TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR.TreeListView_Groups();
            this.tabControl1.SuspendLayout();
            this.tabPageHR.SuspendLayout();
            this.tabPageGroups.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageHR);
            this.tabControl1.Controls.Add(this.tabPageGroups);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(475, 373);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageHR
            // 
            this.tabPageHR.Controls.Add(this.treeListView_HRMain1);
            this.tabPageHR.Location = new System.Drawing.Point(4, 22);
            this.tabPageHR.Name = "tabPageHR";
            this.tabPageHR.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHR.Size = new System.Drawing.Size(467, 347);
            this.tabPageHR.TabIndex = 0;
            this.tabPageHR.Text = "HR";
            this.tabPageHR.UseVisualStyleBackColor = true;
            // 
            // treeListView_HRMain1
            // 
            this.treeListView_HRMain1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeListView_HRMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView_HRMain1.IndexClick = 0;
            this.treeListView_HRMain1.IsSetted = false;
            this.treeListView_HRMain1.LastQuery = "Последний запрос: ";
            this.treeListView_HRMain1.LastTimeOnServer = new System.DateTime(((long)(0)));
            this.treeListView_HRMain1.LastTimeUpdate = new System.DateTime(((long)(0)));
            this.treeListView_HRMain1.Location = new System.Drawing.Point(3, 3);
            this.treeListView_HRMain1.Name = "treeListView_HRMain1";
            this.treeListView_HRMain1.Size = new System.Drawing.Size(461, 341);
            this.treeListView_HRMain1.TabIndex = 0;
            // 
            // tabPageGroups
            // 
            this.tabPageGroups.Controls.Add(this.treeListView_Groups1);
            this.tabPageGroups.Location = new System.Drawing.Point(4, 22);
            this.tabPageGroups.Name = "tabPageGroups";
            this.tabPageGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGroups.Size = new System.Drawing.Size(467, 347);
            this.tabPageGroups.TabIndex = 1;
            this.tabPageGroups.Text = "Группы";
            this.tabPageGroups.UseVisualStyleBackColor = true;
            // 
            // treeListView_Groups1
            // 
            this.treeListView_Groups1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeListView_Groups1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView_Groups1.IndexClick = 0;
            this.treeListView_Groups1.IsSetted = false;
            this.treeListView_Groups1.LastQuery = "Последний запрос: ";
            this.treeListView_Groups1.LastTimeOnServer = new System.DateTime(((long)(0)));
            this.treeListView_Groups1.LastTimeUpdate = new System.DateTime(((long)(0)));
            this.treeListView_Groups1.Location = new System.Drawing.Point(3, 3);
            this.treeListView_Groups1.Name = "treeListView_Groups1";
            this.treeListView_Groups1.Size = new System.Drawing.Size(461, 341);
            this.treeListView_Groups1.TabIndex = 0;
            // 
            // HR_Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "HR_Page";
            this.Controls.SetChildIndex(this.buttonToExport, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPageHR.ResumeLayout(false);
            this.tabPageGroups.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageHR;
        private System.Windows.Forms.TabPage tabPageGroups;
        private TreeListView_HRMain treeListView_HRMain1;
        private TreeListView_Groups treeListView_Groups1;
    }
}
