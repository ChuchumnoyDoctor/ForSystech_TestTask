using CommonDll.Structs;
using System;
using System.Drawing;

namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages
{
    public partial class ControlToCreateChanged : LazyLoad
    {
        public ControlToCreateChanged()
        {
            InitializeComponent();

            this.Enabled = false;
            Default = buttonCreate.BackColor;
        }
        public Color Default { get; set; }
        public void Set(MainParentClass Child)
        {
            this.Enabled = true;
            buttonCreate.BackColor = Default;

            if (Child is null)
                buttonCreate.Text = "Добавить";
            else
                buttonCreate.Text = "Сохранить";
        }
        public virtual void buttonAdd_Click(object sender, EventArgs e)
        {
            throw new Exception("override this method");
        }
    }
}
