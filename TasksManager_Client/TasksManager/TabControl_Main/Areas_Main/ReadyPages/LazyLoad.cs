using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TasksManager.TabControl_Main.Areas_Main.ReadyPages
{
    public partial class LazyLoad : UserControl
    {
        public LazyLoad()
        {
            InitializeComponent();
        }
        private static Dictionary<Tuple<UserControl, string>, Tuple<Thread, ManualResetEvent>> Threads { get; set; } = new Dictionary<Tuple<UserControl, string>, Tuple<Thread, ManualResetEvent>>();
        public static void LazyLoading(UserControl UserControl, string Method, Action Action)
        {
            if (!(Action is null) & !(UserControl is null) & !(Method is null))
                lock (Threads)
                {
                    var Finded = Threads.FirstOrDefault(x => x.Key.Item1 == UserControl & x.Key.Item2 == Method);

                    if (!(Finded.Key is null))
                    {
                        try
                        {
                            Threads[Finded.Key].Item1.Abort();
                        }
                        catch
                        {

                        }

                        Threads.Remove(Finded.Key);
                    }

                    ManualResetEvent LazyLoading_Start = default;
                    Finded = Threads.FirstOrDefault(x => x.Key.Item1 == UserControl);

                    if (Finded.Key is null)
                        LazyLoading_Start = new ManualResetEvent(UserControl.Visible);
                    else
                        LazyLoading_Start = Finded.Value.Item2;

                    var Key = new Tuple<UserControl, string>(UserControl, Method);

                    Thread Thread = new Thread(() =>
                    {
                        if (Thread.CurrentThread.Name is null)
                            Thread.CurrentThread.Name = UserControl.Name + ": " + Method;

                        try
                        {
                            LazyLoading_Start.WaitOne();

                            if (UserControl.InvokeRequired)
                                UserControl.Invoke(new Action(() => Action.Invoke()));
                            else
                                Action.Invoke();

                            lock (Threads)
                                Threads.Remove(Key);
                        }
                        catch (ThreadAbortException ex)
                        {

                        }
                    });
                    Threads.Add(Key, new Tuple<Thread, ManualResetEvent>(Thread, LazyLoading_Start));
                    Thread.Priority = ThreadPriority.Highest;
                    Thread.Start();
                }
        }
        public static void VisibleCh(Control Control, object sender, EventArgs e)
        {
            if (!(Control is null))
                lock (Threads)
                {
                    var Where = Threads.Where(x => x.Key.Item1 == Control);

                    if (Where is null ? false : Where.Count() > 0)
                        foreach (var Finded in Where.ToList())
                            if (Control.Enabled)
                                Finded.Value.Item2.Set();
                            else
                            {

                            }
                    /*else
                        Threads[Finded.Key] = new Tuple<Thread, ManualResetEvent>(Finded.Value.Item1, new ManualResetEvent(false));*/
                }
        }
        private void LazyLoad_VisibleChanged(object sender, EventArgs e)
        {
            VisibleCh(this, sender, e);
        }
    }
}
