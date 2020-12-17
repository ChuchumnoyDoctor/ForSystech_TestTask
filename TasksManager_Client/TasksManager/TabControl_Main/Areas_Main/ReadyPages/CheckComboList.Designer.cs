namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages
{
    partial class CheckComboList
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
            PresentationControls.CheckBoxProperties checkBoxProperties1 = new PresentationControls.CheckBoxProperties();
            this.checkBoxCheck = new System.Windows.Forms.CheckBox();
            this.panelValues = new System.Windows.Forms.Panel();
            this.comboBoxOnSelected = new System.Windows.Forms.ComboBox();
            this.panelDateTimeMoreLow = new System.Windows.Forms.Panel();
            this.dateTimePickerLessThan = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerMoreThan = new System.Windows.Forms.DateTimePicker();
            this.panelFloatMoreLow = new System.Windows.Forms.Panel();
            this.textBoxFloatLessThan = new System.Windows.Forms.TextBox();
            this.textBoxFloatMoreThan = new System.Windows.Forms.TextBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.checkBoxComboBox1 = new PresentationControls.CheckBoxComboBox();
            this.panelValues.SuspendLayout();
            this.panelDateTimeMoreLow.SuspendLayout();
            this.panelFloatMoreLow.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxCheck
            // 
            this.checkBoxCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCheck.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxCheck.Location = new System.Drawing.Point(0, 0);
            this.checkBoxCheck.Name = "checkBoxCheck";
            this.checkBoxCheck.Size = new System.Drawing.Size(642, 23);
            this.checkBoxCheck.TabIndex = 0;
            this.checkBoxCheck.Text = "CheckBox_Text";
            this.checkBoxCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCheck.UseVisualStyleBackColor = true;
            this.checkBoxCheck.CheckedChanged += new System.EventHandler(this.checkBoxCheck_CheckedChanged);
            // 
            // panelValues
            // 
            this.panelValues.Controls.Add(this.comboBoxOnSelected);
            this.panelValues.Controls.Add(this.panelDateTimeMoreLow);
            this.panelValues.Controls.Add(this.panelFloatMoreLow);
            this.panelValues.Controls.Add(this.textBoxSearch);
            this.panelValues.Controls.Add(this.checkBoxComboBox1);
            this.panelValues.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelValues.Location = new System.Drawing.Point(0, 23);
            this.panelValues.MinimumSize = new System.Drawing.Size(0, 21);
            this.panelValues.Name = "panelValues";
            this.panelValues.Size = new System.Drawing.Size(642, 261);
            this.panelValues.TabIndex = 7;
            // 
            // comboBoxOnSelected
            // 
            this.comboBoxOnSelected.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxOnSelected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOnSelected.Location = new System.Drawing.Point(0, 121);
            this.comboBoxOnSelected.Name = "comboBoxOnSelected";
            this.comboBoxOnSelected.Size = new System.Drawing.Size(642, 21);
            this.comboBoxOnSelected.TabIndex = 7;
            this.comboBoxOnSelected.SelectedValueChanged += new System.EventHandler(this.OnChanged);
            // 
            // panelDateTimeMoreLow
            // 
            this.panelDateTimeMoreLow.AutoSize = true;
            this.panelDateTimeMoreLow.Controls.Add(this.dateTimePickerLessThan);
            this.panelDateTimeMoreLow.Controls.Add(this.dateTimePickerMoreThan);
            this.panelDateTimeMoreLow.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDateTimeMoreLow.Location = new System.Drawing.Point(0, 81);
            this.panelDateTimeMoreLow.Name = "panelDateTimeMoreLow";
            this.panelDateTimeMoreLow.Size = new System.Drawing.Size(642, 40);
            this.panelDateTimeMoreLow.TabIndex = 9;
            // 
            // dateTimePickerLessThan
            // 
            this.dateTimePickerLessThan.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimePickerLessThan.Location = new System.Drawing.Point(0, 20);
            this.dateTimePickerLessThan.Name = "dateTimePickerLessThan";
            this.dateTimePickerLessThan.Size = new System.Drawing.Size(642, 20);
            this.dateTimePickerLessThan.TabIndex = 1;
            this.dateTimePickerLessThan.ValueChanged += new System.EventHandler(this.OnChanged);
            // 
            // dateTimePickerMoreThan
            // 
            this.dateTimePickerMoreThan.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimePickerMoreThan.Location = new System.Drawing.Point(0, 0);
            this.dateTimePickerMoreThan.Name = "dateTimePickerMoreThan";
            this.dateTimePickerMoreThan.Size = new System.Drawing.Size(642, 20);
            this.dateTimePickerMoreThan.TabIndex = 0;
            this.dateTimePickerMoreThan.ValueChanged += new System.EventHandler(this.OnChanged);
            // 
            // panelFloatMoreLow
            // 
            this.panelFloatMoreLow.AutoSize = true;
            this.panelFloatMoreLow.Controls.Add(this.textBoxFloatLessThan);
            this.panelFloatMoreLow.Controls.Add(this.textBoxFloatMoreThan);
            this.panelFloatMoreLow.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFloatMoreLow.Location = new System.Drawing.Point(0, 41);
            this.panelFloatMoreLow.Name = "panelFloatMoreLow";
            this.panelFloatMoreLow.Size = new System.Drawing.Size(642, 40);
            this.panelFloatMoreLow.TabIndex = 8;
            // 
            // textBoxFloatLessThan
            // 
            this.textBoxFloatLessThan.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxFloatLessThan.Location = new System.Drawing.Point(0, 20);
            this.textBoxFloatLessThan.Name = "textBoxFloatLessThan";
            this.textBoxFloatLessThan.Size = new System.Drawing.Size(642, 20);
            this.textBoxFloatLessThan.TabIndex = 7;
            this.textBoxFloatLessThan.TextChanged += new System.EventHandler(this.OnChanged);
            // 
            // textBoxFloatMoreThan
            // 
            this.textBoxFloatMoreThan.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxFloatMoreThan.Location = new System.Drawing.Point(0, 0);
            this.textBoxFloatMoreThan.Name = "textBoxFloatMoreThan";
            this.textBoxFloatMoreThan.Size = new System.Drawing.Size(642, 20);
            this.textBoxFloatMoreThan.TabIndex = 6;
            this.textBoxFloatMoreThan.TextChanged += new System.EventHandler(this.OnChanged);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxSearch.Location = new System.Drawing.Point(0, 21);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(642, 20);
            this.textBoxSearch.TabIndex = 10;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.OnChanged);
            // 
            // checkBoxComboBox1
            // 
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxComboBox1.CheckBoxProperties = checkBoxProperties1;
            this.checkBoxComboBox1.DisplayMemberSingleItem = "";
            this.checkBoxComboBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxComboBox1.FormattingEnabled = true;
            this.checkBoxComboBox1.Location = new System.Drawing.Point(0, 0);
            this.checkBoxComboBox1.Margin = new System.Windows.Forms.Padding(5);
            this.checkBoxComboBox1.Name = "checkBoxComboBox1";
            this.checkBoxComboBox1.Size = new System.Drawing.Size(642, 21);
            this.checkBoxComboBox1.TabIndex = 13;
            // 
            // CheckComboList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelValues);
            this.Controls.Add(this.checkBoxCheck);
            this.MinimumSize = new System.Drawing.Size(100, 23);
            this.Name = "CheckComboList";
            this.Size = new System.Drawing.Size(642, 450);
            this.Load += new System.EventHandler(this.CheckComboList_Load);
            this.Resize += new System.EventHandler(this.CheckComboList_Resize);
            this.panelValues.ResumeLayout(false);
            this.panelValues.PerformLayout();
            this.panelDateTimeMoreLow.ResumeLayout(false);
            this.panelFloatMoreLow.ResumeLayout(false);
            this.panelFloatMoreLow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelValues;
        private System.Windows.Forms.Panel panelDateTimeMoreLow;
        private System.Windows.Forms.DateTimePicker dateTimePickerLessThan;
        private System.Windows.Forms.DateTimePicker dateTimePickerMoreThan;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Panel panelFloatMoreLow;
        private System.Windows.Forms.TextBox textBoxFloatLessThan;
        private System.Windows.Forms.TextBox textBoxFloatMoreThan;
        private System.Windows.Forms.ComboBox comboBoxOnSelected;
        public System.Windows.Forms.CheckBox checkBoxCheck;
        private PresentationControls.CheckBoxComboBox checkBoxComboBox1;
    }
}
