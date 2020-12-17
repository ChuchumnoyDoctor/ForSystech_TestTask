using System.Drawing;

namespace TasksManager
{
    partial class ImportDesignForm
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
            this.StatusStripGlobal = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelTextForConnectToServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelStatusForConnectToServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusStripGlobal.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStripGlobal
            // 
            this.StatusStripGlobal.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusStripGlobal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelTextForConnectToServer,
            this.toolStripStatusLabelStatusForConnectToServer});
            this.StatusStripGlobal.Location = new System.Drawing.Point(0, 428);
            this.StatusStripGlobal.Name = "StatusStripGlobal";
            this.StatusStripGlobal.Size = new System.Drawing.Size(800, 22);
            this.StatusStripGlobal.TabIndex = 7;
            this.StatusStripGlobal.Text = "statusStripBottom";
            // 
            // toolStripStatusLabelTextForConnectToServer
            // 
            this.toolStripStatusLabelTextForConnectToServer.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelTextForConnectToServer.Name = "toolStripStatusLabelTextForConnectToServer";
            this.toolStripStatusLabelTextForConnectToServer.Size = new System.Drawing.Size(143, 17);
            this.toolStripStatusLabelTextForConnectToServer.Text = "Подключение к серверу:";
            // 
            // toolStripStatusLabelStatusForConnectToServer
            // 
            this.toolStripStatusLabelStatusForConnectToServer.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelStatusForConnectToServer.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelStatusForConnectToServer.Name = "toolStripStatusLabelStatusForConnectToServer";
            this.toolStripStatusLabelStatusForConnectToServer.Size = new System.Drawing.Size(43, 17);
            this.toolStripStatusLabelStatusForConnectToServer.Text = "Status";
            // 
            // ImportDesignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.StatusStripGlobal);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "ImportDesignForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportDesignForm_FormClosing);
            this.Load += new System.EventHandler(this.FormDesign_Load);
            this.StatusStripGlobal.ResumeLayout(false);
            this.StatusStripGlobal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.StatusStrip StatusStripGlobal;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelTextForConnectToServer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStatusForConnectToServer;
    }
}