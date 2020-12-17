using System;
using System.Windows.Forms;

namespace TasksManager.TabControl_Main.F_Loading
{
    public partial class LoadDialog : UserControl
    {
        public LoadDialog()
        {
            InitializeComponent();

            Timer.Tick += Timer_Tick;
        }
        private void Start()
        {
            Timer.Start();
            Started = DateTime.Now;
        }
        private void Stop()
        {
            Timer.Stop();
            this.Hide();
        }
        private DateTime Started { get; set; }
        Timer Timer { get; set; } = new Timer() { Interval = 1000 };
        private void Timer_Tick(object sender, EventArgs e) // Timer
        {
            var Seconds = Math.Round((DateTime.Now - Started).TotalSeconds);

            if (!this.Visible & Seconds > 3)
            {
                this.BringToFront();
                this.Show();
            }

            labelLoading.Text = "Loading..." + Seconds;
        }
        public void Set(string ModuleDone, TimeSpan Time)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() =>
                 labelPleaseWait.Text += "\n" + string.Format("{0}: {1}", ModuleDone, Math.Round(Time.TotalSeconds))));
            else
                labelPleaseWait.Text += "\n" + string.Format("{0}: {1}", ModuleDone, Math.Round(Time.TotalSeconds));
        }
        bool working;
        public bool Working
        {
            get
            {
                return working;
            }
            set
            {
                if (!working & value)
                    Start();
                else if (working & !value)
                    Stop();

                working = value;
            }
        }
        private void LoadDialog_Shown(object sender, EventArgs e)
        {
            Working = true;
        }
    }
}
