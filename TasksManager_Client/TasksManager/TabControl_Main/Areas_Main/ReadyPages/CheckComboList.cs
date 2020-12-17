using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages
{
    public partial class CheckComboList : UserControl
    {
        public CheckComboList()
        {
            InitializeComponent();

            checkBoxComboBox1.CheckBoxCheckedChanged += OnChanged;
        }
        private void CheckComboList_Load(object sender, EventArgs e)
        {
            CheckComboList_Resize(sender, e);
        }
        private void HideFilters()
        {
            checkBoxComboBox1.Hide();
            panelFloatMoreLow.Hide();
            comboBoxOnSelected.Hide();
            panelDateTimeMoreLow.Hide();
            textBoxSearch.Hide();
        }

        #region SetConfiguration
        public void SetConfiguration(List<string> Collection, string CheckBox_Text) // For combobox select
        {
            HideFilters();

            if (Collection is null)
                Collection = new List<string>();

            this.CheckBox_Text = CheckBox_Text;
            comboBoxOnSelected.Show();
            int index = comboBoxOnSelected.SelectedIndex;
            comboBoxOnSelected.DataSource = Collection;
            comboBoxOnSelected.SelectedIndex = index == -1 ? Collection.Count > 0 ? 0 : -1 : index < Collection.Count ? index : Collection.Count > 0 ? 0 : -1;
        }
        public void SetConfiguration(DateTime Value, string CheckBox_Text) // Date more or low
        {
            HideFilters();

            this.CheckBox_Text = CheckBox_Text;
            panelDateTimeMoreLow.Show();
        }
        public void SetConfiguration(float Value, string CheckBox_Text) // Float more or low
        {
            HideFilters();

            this.CheckBox_Text = CheckBox_Text;
            panelFloatMoreLow.Show();
        }
        public void SetConfiguration(string Value, string CheckBox_Text) // Search
        {
            HideFilters();

            this.CheckBox_Text = CheckBox_Text;
            textBoxSearch.Show();
        }
        public void SetConfiguration(bool Value, string CheckBox_Text) // Search
        {
            HideFilters();

            this.CheckBox_Text = CheckBox_Text;
        }
        public void SetConfiguration(List<string> Value, bool b, string CheckBox_Text) // Selected check
        {
            HideFilters();

            checkBoxComboBox1.Show();
            this.CheckBox_Text = CheckBox_Text;
            checkBoxComboBox1.Items.Clear();
            checkBoxComboBox1.Items.AddRange(Value.ToArray());
        }
        #endregion

        private List<string> MoreLow = new List<string>() { ">", "<" };
        public delegate void onSelectedValueChanged(object sender, EventArgs e);
        public event onSelectedValueChanged OnSelectedValueChanged;

        public bool IsCheck { get { return checkBoxCheck.Checked; } }
        public bool Check(float Value)
        {
            if (IsCheck ? panelFloatMoreLow.Visible : false)
                return (float.TryParse(textBoxFloatMoreThan.Text, out float More) ?
                    float.TryParse(textBoxFloatLessThan.Text, out float Less) ?
                        (Value >= More & Value <= Less) || (Value >= Less & Value <= More) || (Value >= More & Value <= Less) || (Value >= Less & Value <= More) :
                        (Value <= More) :
                    float.TryParse(textBoxFloatLessThan.Text, out Less) ?
                        (Value >= Less) :
                        true);

            return true;
        }
        public bool Check(float Value, float Value2)
        {
            if (IsCheck ? panelFloatMoreLow.Visible : false)
                return (float.TryParse(textBoxFloatMoreThan.Text, out float More) ?
                    float.TryParse(textBoxFloatLessThan.Text, out float Less) ?
                        ((Value >= More - 5 & Value <= More + 5) & (Value2 >= Less - 5 & Value2 <= Less + 5)) || ((Value2 >= More - 5 & Value2 <= More + 5) & (Value >= Less - 5 & Value <= Less + 5)) :
                        ((Value >= More - 5 & Value <= More + 5) || (Value2 >= More - 5 & Value2 <= More + 5)) :
                    float.TryParse(textBoxFloatLessThan.Text, out Less) ?
                        ((Value >= Less - 5 & Value <= Less + 5) || (Value2 >= Less - 5 & Value2 <= Less + 5)) :
                        true);

            return true;
        }
        public bool Check(DateTime Value)
        {
            if (Value != default(DateTime))
                if (IsCheck ? panelDateTimeMoreLow.Visible : false)
                    return Value >= GetStartDate & Value <= GetEndDate;
                else
                {

                }
            else
                return false;

            return true;
        }
        public DateTime GetStartDate { get { return (dateTimePickerLessThan.Value > dateTimePickerMoreThan.Value ? dateTimePickerMoreThan.Value : dateTimePickerLessThan.Value).Date; } }
        public DateTime GetEndDate { get { return (dateTimePickerLessThan.Value > dateTimePickerMoreThan.Value ? dateTimePickerLessThan.Value : dateTimePickerMoreThan.Value).Date + new TimeSpan(23, 59, 59); } }
        public bool Check(string Value)
        {
            if (!string.IsNullOrEmpty(Value))
                if (IsCheck)
                {
                    string From = Value.ToUpper();
                    From = From.Replace(",", ".");

                    if (comboBoxOnSelected.Visible)
                    {
                        string To = comboBoxOnSelected.Text.ToUpper();
                        To = To.Replace(",", ".");

                        if (!From.ToUpper().Contains(To))
                            return (false);
                        else
                            return (true);
                    }
                    else if (textBoxSearch.Visible)
                    {
                        string To = textBoxSearch.Text.ToUpper();
                        To = To.Replace(",", ".");

                        if (!From.ToUpper().Contains(To))
                            return (false);
                        else
                            return (true);
                    }
                    else if (checkBoxComboBox1.Visible)
                    {
                        var Item = checkBoxComboBox1.CheckBoxItems.FirstOrDefault(x => From.ToUpper().Contains((x.ComboBoxItem + "").Replace(",", ".").ToUpper()));

                        if ((Item is null ? false : !Item.Checked) & !(checkBoxComboBox1.CheckBoxItems.FirstOrDefault(x => x.Checked) is null))
                            return (false);
                        else
                            return (true);
                    }
                    else
                    {

                    }
                }

            return true;
        }
        public bool Check(bool Value)
        {
            return IsCheck ? (Value) : true;
        }
        public string CheckBox_Text { get { return checkBoxCheck.Text; } set { checkBoxCheck.Text = value; } }
        private void ValueChanged(object sender, EventArgs e)
        {
            if (!(OnSelectedValueChanged is null))
                if (!IsOnSelectedValueChanged)
                {
                    IsOnSelectedValueChanged = true;
                    OnSelectedValueChanged.Invoke(sender, e);
                    IsOnSelectedValueChanged = false;
                }
        }
        bool IsOnSelectedValueChanged { get; set; }
        private void checkBoxCheck_CheckedChanged(object sender, EventArgs e)
        {
            ValueChanged(sender, e);
        }
        private void OnChanged(object sender, EventArgs e)
        {
            if (checkBoxCheck.Checked)
                ValueChanged(sender, e);
        }
        private void CheckComboList_Resize(object sender, EventArgs e)
        {
            if (this.Height > 0)
            {
                int OnSelected = (comboBoxOnSelected.Visible ? comboBoxOnSelected.Height : 0);
                int FloatMoreLow = (panelFloatMoreLow.Visible ? panelFloatMoreLow.Height : 0);
                int Search = (textBoxSearch.Visible ? textBoxSearch.Height : 0);
                int TimeMoreLow = (panelDateTimeMoreLow.Visible ? panelDateTimeMoreLow.Height : 0);

                panelValues.Height = OnSelected +
                    FloatMoreLow +
                    Search +
                    TimeMoreLow;

                this.Height = checkBoxCheck.Height + panelValues.Height;
            }
        }
    }
}
