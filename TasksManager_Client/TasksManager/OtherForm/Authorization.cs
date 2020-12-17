using CommonDll.Client_Server.Client;
using CommonDll.Structs.F_Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TasksManager.LocalConfig;

namespace TasksManager.OtherForm
{
    public partial class Authorization : ImportDesignForm
    {
        public Authorization()
        {
            InitializeComponent();
        }
        private void buttonAuthorization_Click(object sender, EventArgs e)
        {
            string Login = textBoxLogin.Text;
            string Password = textBoxPassword.Text;
            Tuple<string, string, string> tuple = MethodsCall.GetUser<string>(Login, Password, out ConnectionBackInformation ConnectionBackInformatio);

            if (tuple is null ? false : !string.IsNullOrEmpty(tuple.Item1))
            {
                if (bool.TryParse(tuple.Item1, out bool Parsed))
                {
                    if (!Parsed)
                        return;
                }
                else            
                    TaskManagerForm.NameUser = tuple.Item1;

                Thread Thread = new Thread(() =>
                {
                    Application.Run(new TaskManagerForm());
                });
                Thread.Priority = ThreadPriority.Highest;
                Thread.SetApartmentState(ApartmentState.STA);
                Thread.IsBackground = false;

                try
                {
                    Thread.Start();
                    this.Close();
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
        }
    }
}
