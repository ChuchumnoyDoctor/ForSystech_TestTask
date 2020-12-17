namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages
{
    partial class PageExport
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
            this.buttonToExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonToExport
            // 
            this.buttonToExport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonToExport.Location = new System.Drawing.Point(0, 373);
            this.buttonToExport.Name = "buttonToExport";
            this.buttonToExport.Size = new System.Drawing.Size(475, 23);
            this.buttonToExport.TabIndex = 2;
            this.buttonToExport.Text = "Сохранить";
            this.buttonToExport.UseVisualStyleBackColor = true;
            this.buttonToExport.Click += new System.EventHandler(this.buttonToExport_Click);
            // 
            // PageExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonToExport);
            this.Name = "PageExport";
            this.Size = new System.Drawing.Size(475, 396);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button buttonToExport;
    }
}
