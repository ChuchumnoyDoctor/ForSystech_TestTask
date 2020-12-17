using CommonDll.Client_Server.Client;
using CommonDll.Helps;
using CommonDll.Structs.F_StructDataOnServer;
using System;
using System.Drawing;
using System.Threading;
using TasksManager.LocalConfig;
using TasksManager.OtherForm;

namespace TasksManager
{
    public partial class ExportDesignForm : ImportDesignForm
    {
        public ExportDesignForm() // Оставить try{}catch{} у всех методов, т.к. компилится в runtime, но не кампилится как дезайн контрол
        {
            InitializeComponent();
        }
        private bool CheckToAbort(Thread Thread)
        {
            bool IsAlive_AndAborted = false;

            if (Thread is null ? false : Thread.IsAlive)
            {
                IsAlive_AndAborted = true;

                try
                {
                    Thread.Abort();
                    Thread = null;
                }
                catch (ThreadStateException ex)
                {

                }
                catch (ThreadAbortException ex)
                {

                }
                catch (NullReferenceException ex)
                {

                }
            }

            return IsAlive_AndAborted;
        }
        public bool SetToServer(object sender, EventArgs e, StructureValueForClient StructureValueForClient, out string Exception) // Event
        {
            DateTime Start = DateTime.Now;
            bool Done = false;
            string Exception_Local = null;

            Thread Thread = new Thread(() =>
            {
                try
                {
                    if (sender is null)
                        sender = new object();

                    lock (sender)
                    {
                        if (Thread.CurrentThread.Name is null)
                            Thread.CurrentThread.Name = nameof(SetToServer);

                        if (StructureValueForClient != null)
                        {
                            if (TaskManagerForm.StatusStrip.InvokeRequired)
                                TaskManagerForm.StatusStrip.Invoke(new Action(() =>
                                {
                                    TaskManagerForm.toolStripStatusLabelExport.BackColor = Color.Yellow;
                                    TaskManagerForm.toolStripStatusLabelExport.Text = string.Format("Export: {0}", "Started");
                                }));
                            else
                            {
                                TaskManagerForm.toolStripStatusLabelExport.BackColor = Color.Yellow;
                                TaskManagerForm.toolStripStatusLabelExport.Text = string.Format("Export: {0}", "Started");
                            }

                            DateTime Started = DateTime.Now;
                            Tuple<string, string, string> tuple1 = MethodsCall.SetToServer<string>(StructureValueForClient, out ConnectionBackInformation ConnectionBackInformation);
                            TimeSpan Time = DateTime.Now - Started;

                            if (tuple1 is null ? true : (!string.IsNullOrEmpty(tuple1.Item1) || !string.IsNullOrEmpty(tuple1.Item2) || !string.IsNullOrEmpty(tuple1.Item3)))
                            {
                                Started = DateTime.Now;
                                tuple1 = MethodsCall.SetToServer<string>(StructureValueForClient, out ConnectionBackInformation);
                                Time = DateTime.Now - Started;
                            }

                            if (tuple1 is null ? false : !string.IsNullOrEmpty(tuple1.Item1))
                            {
                                Done = false;
                                Exception_Local = tuple1.Item1;
                            }
                            else if (tuple1 is null ? false : !string.IsNullOrEmpty(tuple1.Item2))
                            {
                                Done = false;
                                Exception_Local = tuple1.Item2;
                            }
                            else if (tuple1 is null ? false : !string.IsNullOrEmpty(tuple1.Item3))
                            {
                                Done = false;
                                Exception_Local = tuple1.Item3;
                            }
                            else if (tuple1 is null)
                            {
                                Exception_Local = "tuple1 is null";
                                Done = false;
                            }
                            else
                            {
                                Done = true;
                            }
                        }
                        else
                            Exception_Local = "StructureValueForClient is null";
                    }
                }
                catch (ThreadAbortException ex)
                {

                }
            });
            Thread.IsBackground = true;

            try
            {
                Thread.Start();
                Thread.Join((int)Math.Round(TimeSpan.FromMinutes(2).TotalMilliseconds));
            }
            catch (ThreadStateException ex)
            {

            }
            catch (ThreadAbortException ex)
            {

            }
            catch (NullReferenceException ex)
            {

            }

            if (CheckToAbort(Thread))
                if (string.IsNullOrEmpty(Exception_Local))
                    Exception_Local = "Aborted";

            Exception = Exception_Local;

            if (TaskManagerForm.StatusStrip.InvokeRequired)
                TaskManagerForm.StatusStrip.Invoke(new Action(() =>
                {
                    TaskManagerForm.toolStripStatusLabelExport.BackColor = Done ? Color.LightGreen : Color.Orange;
                    TaskManagerForm.toolStripStatusLabelExport.Text = string.Format("Export: {0}", Done ? string.Format("Done ({0})", Helper.ToString(DateTime.Now - Start)) : Exception_Local);
                }));
            else
            {
                TaskManagerForm.toolStripStatusLabelExport.BackColor = Done ? Color.LightGreen : Color.Orange;
                TaskManagerForm.toolStripStatusLabelExport.Text = string.Format("Export: {0}", Done ? string.Format("Done ({0})", Helper.ToString(DateTime.Now - Start)) : Exception_Local);
            }

            return Done;
        }
    }
}
