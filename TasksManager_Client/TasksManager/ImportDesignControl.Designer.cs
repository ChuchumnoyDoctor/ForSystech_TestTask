using System.Drawing;

namespace TasksManager
{
    partial class ImportDesignControl
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
            this.splitContainer_Main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_Settings = new System.Windows.Forms.SplitContainer();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.button_Hide_Settings = new System.Windows.Forms.Button();
            this.panelView = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).BeginInit();
            this.splitContainer_Main.Panel1.SuspendLayout();
            this.splitContainer_Main.Panel2.SuspendLayout();
            this.splitContainer_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Settings)).BeginInit();
            this.splitContainer_Settings.Panel1.SuspendLayout();
            this.splitContainer_Settings.Panel2.SuspendLayout();
            this.splitContainer_Settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_Main
            // 
            this.splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_Main.IsSplitterFixed = true;
            this.splitContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Main.Name = "splitContainer_Main";
            // 
            // splitContainer_Main.Panel1
            // 
            this.splitContainer_Main.Panel1.Controls.Add(this.splitContainer_Settings);
            // 
            // splitContainer_Main.Panel2
            // 
            this.splitContainer_Main.Panel2.Controls.Add(this.panelView);
            this.splitContainer_Main.Size = new System.Drawing.Size(894, 568);
            this.splitContainer_Main.SplitterDistance = 209;
            this.splitContainer_Main.TabIndex = 13;
            // 
            // splitContainer_Settings
            // 
            this.splitContainer_Settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Settings.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_Settings.IsSplitterFixed = true;
            this.splitContainer_Settings.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Settings.Name = "splitContainer_Settings";
            // 
            // splitContainer_Settings.Panel1
            // 
            this.splitContainer_Settings.Panel1.Controls.Add(this.panelSettings);
            // 
            // splitContainer_Settings.Panel2
            // 
            this.splitContainer_Settings.Panel2.Controls.Add(this.button_Hide_Settings);
            this.splitContainer_Settings.Size = new System.Drawing.Size(209, 568);
            this.splitContainer_Settings.SplitterDistance = 180;
            this.splitContainer_Settings.TabIndex = 10;
            // 
            // panelSettings
            // 
            this.panelSettings.AutoScroll = true;
            this.panelSettings.AutoSize = true;
            this.panelSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSettings.Location = new System.Drawing.Point(0, 0);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(180, 568);
            this.panelSettings.TabIndex = 1;
            // 
            // button_Hide_Settings
            // 
            this.button_Hide_Settings.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_Hide_Settings.Location = new System.Drawing.Point(0, 0);
            this.button_Hide_Settings.Name = "button_Hide_Settings";
            this.button_Hide_Settings.Size = new System.Drawing.Size(25, 568);
            this.button_Hide_Settings.TabIndex = 17;
            this.button_Hide_Settings.Text = "С\r\nк\r\nр\r\nы\r\nт\r\nь";
            this.button_Hide_Settings.UseVisualStyleBackColor = true;
            this.button_Hide_Settings.Click += new System.EventHandler(this.button_Hide_Settings_Click);
            // 
            // panelView
            // 
            this.panelView.AutoSize = true;
            this.panelView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(0, 0);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(681, 568);
            this.panelView.TabIndex = 0;
            // 
            // ImportDesignControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer_Main);
            this.Name = "ImportDesignControl";
            this.Size = new System.Drawing.Size(894, 568);
            this.Load += new System.EventHandler(this.PageDesign_Load);
            this.Resize += new System.EventHandler(this.panelDataRefreshed_Resize);
            this.splitContainer_Main.Panel1.ResumeLayout(false);
            this.splitContainer_Main.Panel2.ResumeLayout(false);
            this.splitContainer_Main.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).EndInit();
            this.splitContainer_Main.ResumeLayout(false);
            this.splitContainer_Settings.Panel1.ResumeLayout(false);
            this.splitContainer_Settings.Panel1.PerformLayout();
            this.splitContainer_Settings.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Settings)).EndInit();
            this.splitContainer_Settings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.SplitContainer splitContainer_Main;
        public System.Windows.Forms.SplitContainer splitContainer_Settings;
        public System.Windows.Forms.Panel panelSettings;
        public System.Windows.Forms.Panel panelView;
        public System.Windows.Forms.Button button_Hide_Settings;
    }
}
