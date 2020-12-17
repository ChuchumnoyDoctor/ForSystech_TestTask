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
    public partial class HR_AddWorker : ImportDesignForm
    {
        public HR_AddWorker()
        {
            InitializeComponent();
        }
        public HR_Worker HR_Worker { get; set; }
        private HR_Worker UpdateObject_Original { get; set; }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxName.Text) &
                float.TryParse(textBoxBaseRate.Text, out float BaseRate) &
                comboBoxGroup.SelectedValue is HR_Group)
            {
                HR_Worker = new HR_Worker()
                {
                    Name = textBoxName.Text,
                    EnrollmentDate = dateTimePickerEnrollmentDate.Value,
                    BaseRate = BaseRate,
                    Group = (HR_Group)comboBoxGroup.SelectedValue,
                    Parent = (HR_Worker)comboBoxChief.SelectedValue,
                    UpdateObject_Original = UpdateObject_Original
                };
                this.Close();
            }
            else
                buttonAdd.BackColor = Color.Yellow;
        }
        public void Set(List<HR_Group> Groups, List<HR_Worker> Workers_All)
        {
            comboBoxGroup.DataSource = Groups;
            comboBoxGroup.DisplayMember = nameof(HR_Group.Name);

            comboBoxChief.DataSource = Workers_All;
            comboBoxChief.DisplayMember = nameof(CommonDll.Structs.F_HR.HR_Worker.Name);
        }
        public void Set(HR_Worker HR_Worker)
        {
            ((List<HR_Worker>)comboBoxChief.DataSource).Remove(HR_Worker);
            UpdateObject_Original = HR_Worker;
            textBoxName.Text = HR_Worker.Name;
            dateTimePickerEnrollmentDate.Value = HR_Worker.EnrollmentDate;
            textBoxBaseRate.Text = HR_Worker.BaseRate + "";
            comboBoxGroup.SelectedIndex = ((List<HR_Group>)comboBoxGroup.DataSource).FindIndex(x => x.Name == HR_Worker.Group.Name);
            comboBoxChief.SelectedIndex = ((List<HR_Worker>)comboBoxChief.DataSource).FindIndex(x => 
            x.Name == (HR_Worker.Parent is HR_Worker ? ((HR_Worker)HR_Worker.Parent).Name : null));
        }
    }
}
