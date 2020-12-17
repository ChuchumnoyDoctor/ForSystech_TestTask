using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace TasksManager.TabControl_Main.Areas_Main
{
    public partial class buttonSNextPrevious : UserControl
    {
        public buttonSNextPrevious()
        {
            InitializeComponent();
        }

        #region buttonS Next, Previous  
        public delegate void check();
        public event check Event_Check;
        public void SetIsCheck(bool IsCheck)
        {
            this.IsCheck = IsCheck;
            @event.Set();
        }
        bool IsCheck { get; set; }
        ManualResetEvent @event { get; set; }
        public List<Type> NextPreviousByType { get; set; }
        public delegate void getInfoFromSelected(bool IsNext);
        public event getInfoFromSelected Event_GetInfoFromSelected;
        private void buttonPrevious_MouseDown(object sender, MouseEventArgs e)
        {
            button_MouseDown(false);
        }
        private void buttonNext_MouseDown(object sender, MouseEventArgs e)
        {
            button_MouseDown(true);
        }
        private void button_MouseDown(bool IsNext)
        {
            if (!(Event_Check is null))
            {
                @event = new ManualResetEvent(false);
                Event_Check.Invoke();
                @event.WaitOne();

                if (IsCheck)
                    if (!(Event_GetInfoFromSelected is null))
                        Event_GetInfoFromSelected.Invoke(IsNext);
            }
            else
            {
                if (!(Event_GetInfoFromSelected is null))
                    Event_GetInfoFromSelected.Invoke(IsNext);
            }
        }
        #endregion
    }
}
