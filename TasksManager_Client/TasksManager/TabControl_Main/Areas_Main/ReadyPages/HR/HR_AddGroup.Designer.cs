namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR
{
    partial class HR_AddGroup
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
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxPercent = new System.Windows.Forms.TextBox();
            this.labelPercent = new System.Windows.Forms.Label();
            this.textBoxMaxPercent = new System.Windows.Forms.TextBox();
            this.labelMaxPercent = new System.Windows.Forms.Label();
            this.textBoxDeepLevelSubWorkers = new System.Windows.Forms.TextBox();
            this.labelDeepLevelSubWorkers = new System.Windows.Forms.Label();
            this.textBoxPercentSubWorkers = new System.Windows.Forms.TextBox();
            this.labelPercentSubWorkers = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxName.Location = new System.Drawing.Point(0, 13);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(249, 20);
            this.textBoxName.TabIndex = 10;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelName.Location = new System.Drawing.Point(0, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(63, 13);
            this.labelName.TabIndex = 11;
            this.labelName.Text = "Название: ";
            // 
            // textBoxPercent
            // 
            this.textBoxPercent.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxPercent.Location = new System.Drawing.Point(0, 46);
            this.textBoxPercent.Name = "textBoxPercent";
            this.textBoxPercent.Size = new System.Drawing.Size(249, 20);
            this.textBoxPercent.TabIndex = 12;
            // 
            // labelPercent
            // 
            this.labelPercent.AutoSize = true;
            this.labelPercent.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPercent.Location = new System.Drawing.Point(0, 33);
            this.labelPercent.Name = "labelPercent";
            this.labelPercent.Size = new System.Drawing.Size(56, 13);
            this.labelPercent.TabIndex = 13;
            this.labelPercent.Text = "% за год: ";
            // 
            // textBoxMaxPercent
            // 
            this.textBoxMaxPercent.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxMaxPercent.Location = new System.Drawing.Point(0, 79);
            this.textBoxMaxPercent.Name = "textBoxMaxPercent";
            this.textBoxMaxPercent.Size = new System.Drawing.Size(249, 20);
            this.textBoxMaxPercent.TabIndex = 14;
            // 
            // labelMaxPercent
            // 
            this.labelMaxPercent.AutoSize = true;
            this.labelMaxPercent.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelMaxPercent.Location = new System.Drawing.Point(0, 66);
            this.labelMaxPercent.Name = "labelMaxPercent";
            this.labelMaxPercent.Size = new System.Drawing.Size(103, 13);
            this.labelMaxPercent.TabIndex = 15;
            this.labelMaxPercent.Text = "Максимальный %: ";
            // 
            // textBoxDeepLevelSubWorkers
            // 
            this.textBoxDeepLevelSubWorkers.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxDeepLevelSubWorkers.Location = new System.Drawing.Point(0, 112);
            this.textBoxDeepLevelSubWorkers.Name = "textBoxDeepLevelSubWorkers";
            this.textBoxDeepLevelSubWorkers.Size = new System.Drawing.Size(249, 20);
            this.textBoxDeepLevelSubWorkers.TabIndex = 16;
            // 
            // labelDeepLevelSubWorkers
            // 
            this.labelDeepLevelSubWorkers.AutoSize = true;
            this.labelDeepLevelSubWorkers.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDeepLevelSubWorkers.Location = new System.Drawing.Point(0, 99);
            this.labelDeepLevelSubWorkers.Name = "labelDeepLevelSubWorkers";
            this.labelDeepLevelSubWorkers.Size = new System.Drawing.Size(126, 13);
            this.labelDeepLevelSubWorkers.TabIndex = 17;
            this.labelDeepLevelSubWorkers.Text = "Уровень подчиненных: ";
            // 
            // textBoxPercentSubWorkers
            // 
            this.textBoxPercentSubWorkers.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxPercentSubWorkers.Location = new System.Drawing.Point(0, 145);
            this.textBoxPercentSubWorkers.Name = "textBoxPercentSubWorkers";
            this.textBoxPercentSubWorkers.Size = new System.Drawing.Size(249, 20);
            this.textBoxPercentSubWorkers.TabIndex = 18;
            // 
            // labelPercentSubWorkers
            // 
            this.labelPercentSubWorkers.AutoSize = true;
            this.labelPercentSubWorkers.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPercentSubWorkers.Location = new System.Drawing.Point(0, 132);
            this.labelPercentSubWorkers.Name = "labelPercentSubWorkers";
            this.labelPercentSubWorkers.Size = new System.Drawing.Size(156, 13);
            this.labelPercentSubWorkers.TabIndex = 19;
            this.labelPercentSubWorkers.Text = "% от зарплаты подчиненных: ";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonAdd.Location = new System.Drawing.Point(0, 180);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(249, 23);
            this.buttonAdd.TabIndex = 20;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // HR_AddGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 225);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxPercentSubWorkers);
            this.Controls.Add(this.labelPercentSubWorkers);
            this.Controls.Add(this.textBoxDeepLevelSubWorkers);
            this.Controls.Add(this.labelDeepLevelSubWorkers);
            this.Controls.Add(this.textBoxMaxPercent);
            this.Controls.Add(this.labelMaxPercent);
            this.Controls.Add(this.textBoxPercent);
            this.Controls.Add(this.labelPercent);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "HR_AddGroup";
            this.Text = "AddGroup";
            this.Controls.SetChildIndex(this.labelName, 0);
            this.Controls.SetChildIndex(this.textBoxName, 0);
            this.Controls.SetChildIndex(this.labelPercent, 0);
            this.Controls.SetChildIndex(this.textBoxPercent, 0);
            this.Controls.SetChildIndex(this.labelMaxPercent, 0);
            this.Controls.SetChildIndex(this.textBoxMaxPercent, 0);
            this.Controls.SetChildIndex(this.labelDeepLevelSubWorkers, 0);
            this.Controls.SetChildIndex(this.textBoxDeepLevelSubWorkers, 0);
            this.Controls.SetChildIndex(this.labelPercentSubWorkers, 0);
            this.Controls.SetChildIndex(this.textBoxPercentSubWorkers, 0);
            this.Controls.SetChildIndex(this.buttonAdd, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxPercent;
        private System.Windows.Forms.Label labelPercent;
        private System.Windows.Forms.TextBox textBoxMaxPercent;
        private System.Windows.Forms.Label labelMaxPercent;
        private System.Windows.Forms.TextBox textBoxDeepLevelSubWorkers;
        private System.Windows.Forms.Label labelDeepLevelSubWorkers;
        private System.Windows.Forms.TextBox textBoxPercentSubWorkers;
        private System.Windows.Forms.Label labelPercentSubWorkers;
        private System.Windows.Forms.Button buttonAdd;
    }
}