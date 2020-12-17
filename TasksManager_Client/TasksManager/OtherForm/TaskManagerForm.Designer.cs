namespace TasksManager.OtherForm
{
    partial class TaskManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageHR = new System.Windows.Forms.TabPage();
            this.hR_Page1 = new TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR.HR_Page();
            this.tabPage_ServerProcess = new System.Windows.Forms.TabPage();
            this.viewServerProcess1 = new TasksManager.TabControl_Main.F_ServerProcess.ViewServerProcess();
            this.tabControlMain.SuspendLayout();
            this.tabPageHR.SuspendLayout();
            this.tabPage_ServerProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageHR);
            this.tabControlMain.Controls.Add(this.tabPage_ServerProcess);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1137, 633);
            this.tabControlMain.TabIndex = 11;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageHR
            // 
            this.tabPageHR.Controls.Add(this.hR_Page1);
            this.tabPageHR.Location = new System.Drawing.Point(4, 22);
            this.tabPageHR.Name = "tabPageHR";
            this.tabPageHR.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHR.Size = new System.Drawing.Size(1129, 607);
            this.tabPageHR.TabIndex = 7;
            this.tabPageHR.Text = "HR";
            this.tabPageHR.UseVisualStyleBackColor = true;
            // 
            // hR_Page1
            // 
            this.hR_Page1.CollectedChangesToExport = null;
            this.hR_Page1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hR_Page1.Location = new System.Drawing.Point(3, 3);
            this.hR_Page1.Name = "hR_Page1";
            this.hR_Page1.Size = new System.Drawing.Size(1123, 601);
            this.hR_Page1.TabIndex = 0;
            // 
            // tabPage_ServerProcess
            // 
            this.tabPage_ServerProcess.Controls.Add(this.viewServerProcess1);
            this.tabPage_ServerProcess.Location = new System.Drawing.Point(4, 22);
            this.tabPage_ServerProcess.Name = "tabPage_ServerProcess";
            this.tabPage_ServerProcess.Size = new System.Drawing.Size(1129, 607);
            this.tabPage_ServerProcess.TabIndex = 6;
            this.tabPage_ServerProcess.Text = "Сервер - прогресс обработки";
            this.tabPage_ServerProcess.UseVisualStyleBackColor = true;
            // 
            // viewServerProcess1
            // 
            this.viewServerProcess1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewServerProcess1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewServerProcess1.IndexClick = 0;
            this.viewServerProcess1.IsSetted = false;
            this.viewServerProcess1.LastQuery = "";
            this.viewServerProcess1.LastTimeOnServer = new System.DateTime(((long)(0)));
            this.viewServerProcess1.LastTimeUpdate = new System.DateTime(((long)(0)));
            this.viewServerProcess1.Location = new System.Drawing.Point(0, 0);
            this.viewServerProcess1.Name = "viewServerProcess1";
            this.viewServerProcess1.Size = new System.Drawing.Size(1129, 607);
            this.viewServerProcess1.TabIndex = 0;
            // 
            // TaskManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 655);
            this.Controls.Add(this.tabControlMain);
            this.Name = "TaskManagerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskManagerForm_FormClosing);
            this.Shown += new System.EventHandler(this.TaskManagerForm_Shown);
            this.Controls.SetChildIndex(this.tabControlMain, 0);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageHR.ResumeLayout(false);
            this.tabPage_ServerProcess.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPage_ServerProcess;
        private TabControl_Main.F_ServerProcess.ViewServerProcess viewServerProcess1;
        private System.Windows.Forms.TabPage tabPageHR;
        private TabControl_Main.Areas_Main.ReadyPages.HR.HR_Page hR_Page1;
    }
}

