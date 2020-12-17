namespace TasksManager.TabControl_Main.F_ServerProcess
{
    partial class CustomProgressBar
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
            this.picboxPB = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picboxPB)).BeginInit();
            this.SuspendLayout();
            // 
            // picboxPB
            // 
            this.picboxPB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picboxPB.Location = new System.Drawing.Point(0, 0);
            this.picboxPB.Name = "picboxPB";
            this.picboxPB.Size = new System.Drawing.Size(150, 150);
            this.picboxPB.TabIndex = 0;
            this.picboxPB.TabStop = false;
            // 
            // CustomProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picboxPB);
            this.Name = "CustomProgressBar";
            this.Resize += new System.EventHandler(this.PicboxPB_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picboxPB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picboxPB;
    }
}
