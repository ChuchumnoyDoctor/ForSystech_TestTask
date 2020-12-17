namespace TasksManager.TabControl_Main.F_Loading
{
    partial class LoadDialog
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
            this.labelLoading = new System.Windows.Forms.Label();
            this.labelPleaseWait = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelLoading
            // 
            this.labelLoading.AutoSize = true;
            this.labelLoading.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLoading.Location = new System.Drawing.Point(0, 0);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(78, 20);
            this.labelLoading.TabIndex = 0;
            this.labelLoading.Text = "Loading...";
            // 
            // labelPleaseWait
            // 
            this.labelPleaseWait.AutoSize = true;
            this.labelPleaseWait.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPleaseWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPleaseWait.Location = new System.Drawing.Point(0, 20);
            this.labelPleaseWait.Name = "labelPleaseWait";
            this.labelPleaseWait.Size = new System.Drawing.Size(93, 20);
            this.labelPleaseWait.TabIndex = 1;
            this.labelPleaseWait.Text = "Please wait!";
            // 
            // LoadDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelPleaseWait);
            this.Controls.Add(this.labelLoading);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "LoadDialog";
            this.Size = new System.Drawing.Size(266, 194);
            this.Load += new System.EventHandler(this.LoadDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.Label labelPleaseWait;
    }
}