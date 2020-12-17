namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR
{
    partial class HR_AddWorker
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
            this.labelEnrollmentDate = new System.Windows.Forms.Label();
            this.dateTimePickerEnrollmentDate = new System.Windows.Forms.DateTimePicker();
            this.labelBaseRate = new System.Windows.Forms.Label();
            this.textBoxBaseRate = new System.Windows.Forms.TextBox();
            this.comboBoxGroup = new System.Windows.Forms.ComboBox();
            this.labelGroup = new System.Windows.Forms.Label();
            this.comboBoxChief = new System.Windows.Forms.ComboBox();
            this.labelChief = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxName.Location = new System.Drawing.Point(0, 13);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(198, 20);
            this.textBoxName.TabIndex = 8;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelName.Location = new System.Drawing.Point(0, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 9;
            this.labelName.Text = "Имя: ";
            // 
            // labelEnrollmentDate
            // 
            this.labelEnrollmentDate.AutoSize = true;
            this.labelEnrollmentDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEnrollmentDate.Location = new System.Drawing.Point(0, 33);
            this.labelEnrollmentDate.Name = "labelEnrollmentDate";
            this.labelEnrollmentDate.Size = new System.Drawing.Size(158, 13);
            this.labelEnrollmentDate.TabIndex = 10;
            this.labelEnrollmentDate.Text = "Дата поступления на работу: ";
            // 
            // dateTimePickerEnrollmentDate
            // 
            this.dateTimePickerEnrollmentDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimePickerEnrollmentDate.Location = new System.Drawing.Point(0, 46);
            this.dateTimePickerEnrollmentDate.Name = "dateTimePickerEnrollmentDate";
            this.dateTimePickerEnrollmentDate.Size = new System.Drawing.Size(198, 20);
            this.dateTimePickerEnrollmentDate.TabIndex = 11;
            // 
            // labelBaseRate
            // 
            this.labelBaseRate.AutoSize = true;
            this.labelBaseRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelBaseRate.Location = new System.Drawing.Point(0, 66);
            this.labelBaseRate.Name = "labelBaseRate";
            this.labelBaseRate.Size = new System.Drawing.Size(94, 13);
            this.labelBaseRate.TabIndex = 12;
            this.labelBaseRate.Text = "Базовая ставка: ";
            // 
            // textBoxBaseRate
            // 
            this.textBoxBaseRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxBaseRate.Location = new System.Drawing.Point(0, 79);
            this.textBoxBaseRate.Name = "textBoxBaseRate";
            this.textBoxBaseRate.Size = new System.Drawing.Size(198, 20);
            this.textBoxBaseRate.TabIndex = 13;
            // 
            // comboBoxGroup
            // 
            this.comboBoxGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxGroup.FormattingEnabled = true;
            this.comboBoxGroup.Location = new System.Drawing.Point(0, 112);
            this.comboBoxGroup.Name = "comboBoxGroup";
            this.comboBoxGroup.Size = new System.Drawing.Size(198, 21);
            this.comboBoxGroup.TabIndex = 14;
            // 
            // labelGroup
            // 
            this.labelGroup.AutoSize = true;
            this.labelGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelGroup.Location = new System.Drawing.Point(0, 99);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(48, 13);
            this.labelGroup.TabIndex = 15;
            this.labelGroup.Text = "Группа: ";
            // 
            // comboBoxChief
            // 
            this.comboBoxChief.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxChief.FormattingEnabled = true;
            this.comboBoxChief.Location = new System.Drawing.Point(0, 146);
            this.comboBoxChief.Name = "comboBoxChief";
            this.comboBoxChief.Size = new System.Drawing.Size(198, 21);
            this.comboBoxChief.TabIndex = 16;
            // 
            // labelChief
            // 
            this.labelChief.AutoSize = true;
            this.labelChief.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelChief.Location = new System.Drawing.Point(0, 133);
            this.labelChief.Name = "labelChief";
            this.labelChief.Size = new System.Drawing.Size(68, 13);
            this.labelChief.TabIndex = 17;
            this.labelChief.Text = "Начальник: ";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonAdd.Location = new System.Drawing.Point(0, 182);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(198, 23);
            this.buttonAdd.TabIndex = 18;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // HR_AddWorker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 227);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.comboBoxChief);
            this.Controls.Add(this.labelChief);
            this.Controls.Add(this.comboBoxGroup);
            this.Controls.Add(this.labelGroup);
            this.Controls.Add(this.textBoxBaseRate);
            this.Controls.Add(this.labelBaseRate);
            this.Controls.Add(this.dateTimePickerEnrollmentDate);
            this.Controls.Add(this.labelEnrollmentDate);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "HR_AddWorker";
            this.Text = "AddWorker";
            this.Controls.SetChildIndex(this.labelName, 0);
            this.Controls.SetChildIndex(this.textBoxName, 0);
            this.Controls.SetChildIndex(this.labelEnrollmentDate, 0);
            this.Controls.SetChildIndex(this.dateTimePickerEnrollmentDate, 0);
            this.Controls.SetChildIndex(this.labelBaseRate, 0);
            this.Controls.SetChildIndex(this.textBoxBaseRate, 0);
            this.Controls.SetChildIndex(this.labelGroup, 0);
            this.Controls.SetChildIndex(this.comboBoxGroup, 0);
            this.Controls.SetChildIndex(this.labelChief, 0);
            this.Controls.SetChildIndex(this.comboBoxChief, 0);
            this.Controls.SetChildIndex(this.buttonAdd, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelEnrollmentDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnrollmentDate;
        private System.Windows.Forms.Label labelBaseRate;
        private System.Windows.Forms.TextBox textBoxBaseRate;
        private System.Windows.Forms.ComboBox comboBoxGroup;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.ComboBox comboBoxChief;
        private System.Windows.Forms.Label labelChief;
        private System.Windows.Forms.Button buttonAdd;
    }
}