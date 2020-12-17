namespace TasksManager.TabControl_Main.Areas_Main
{
    partial class buttonSNextPrevious
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
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Location = new System.Drawing.Point(2, 3);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(105, 23);
            this.buttonPrevious.TabIndex = 3;
            this.buttonPrevious.Text = "Предыдущая";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonPrevious_MouseDown);
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNext.Location = new System.Drawing.Point(107, 3);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(107, 23);
            this.buttonNext.TabIndex = 4;
            this.buttonNext.Text = "Следующая";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonNext_MouseDown);
            // 
            // buttonSNextPrevious
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonNext);
            this.Name = "buttonSNextPrevious";
            this.Size = new System.Drawing.Size(217, 29);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonNext;
    }
}
