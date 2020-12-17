using CommonDll.Structs.F_HR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages.HR
{
    public partial class HR_AddGroup : ImportDesignForm
    {
        public HR_AddGroup()
        {
            InitializeComponent();
        }
        public HR_Group HR_Group { get; set; }
        private HR_Group UpdateObject_Original { get; set; }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxName.Text) &
                int.TryParse(textBoxPercent.Text, out int Percent) &
                int.TryParse(textBoxMaxPercent.Text, out int MaxPercent) &
                int.TryParse(textBoxDeepLevelSubWorkers.Text, out int DeepLevelSubWorkers) &
                float.TryParse(textBoxPercentSubWorkers.Text, out float PercentSubWorkers))
            {
                HR_Group = new HR_Group()
                {
                    Name = textBoxName.Text,
                    Percent = Percent,
                    MaxPercent = MaxPercent,
                    DeepLevelSubWorkers = DeepLevelSubWorkers,
                    PercentSubWorkers = PercentSubWorkers,
                    UpdateObject_Original = UpdateObject_Original
                };
                this.Close();
            }
            else
                buttonAdd.BackColor = Color.Yellow;
        }
        public void Set(HR_Group HR_Group)
        {
            UpdateObject_Original = HR_Group;
            textBoxName.Text = HR_Group.Name;
            textBoxPercent.Text = HR_Group.Percent + "";
            textBoxMaxPercent.Text = HR_Group.MaxPercent + "";
            textBoxDeepLevelSubWorkers.Text = HR_Group.DeepLevelSubWorkers + "";
            textBoxPercentSubWorkers.Text = HR_Group.PercentSubWorkers + "";
        }
    }
}
