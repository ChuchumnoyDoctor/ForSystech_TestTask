namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR
{
    partial class TreeListView_Groups
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьГруппуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.TreeListView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeListView
            // 
            this.TreeListView.ContextMenuStrip = this.contextMenuStrip1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьГруппуToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(225, 26);
            // 
            // добавитьГруппуToolStripMenuItem
            // 
            this.добавитьГруппуToolStripMenuItem.Name = "добавитьГруппуToolStripMenuItem";
            this.добавитьГруппуToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.добавитьГруппуToolStripMenuItem.Text = "Добавить/изменить группу";
            this.добавитьГруппуToolStripMenuItem.Click += new System.EventHandler(this.добавитьГруппуToolStripMenuItem_Click);
            // 
            // TreeListView_Groups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TreeListView_Groups";
            this.Controls.SetChildIndex(this.TreeListView, 0);
            ((System.ComponentModel.ISupportInitialize)(this.TreeListView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem добавитьГруппуToolStripMenuItem;
    }
}
