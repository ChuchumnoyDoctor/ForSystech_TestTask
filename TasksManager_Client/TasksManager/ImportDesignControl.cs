using System;
using System.Linq;
using System.Net;
using TasksManager.TabControl_Main.Areas_Main.ReadyPages;

namespace TasksManager
{
    public partial class ImportDesignControl : LazyLoad
    {
        public ImportDesignControl()
        {
            try
            {
                InitializeComponent();
            }
            catch
            {

            }
        } // Оставить try{}catch{} у всех методов, т.к. компилится в runtime, но не кампилится как дезайн контрол
        private void PageDesign_Load(object sender, EventArgs e)
        {
            #region Data refreshed
            {
                try
                {
                    temp_splitContainer_Settings_Split = splitContainer_Settings.SplitterDistance;
                    temp_button_Hide_Settings_Width = 25;
                    temp_splitContainer_Main_Split = temp_splitContainer_Settings_Split + temp_button_Hide_Settings_Width;
                }
                catch
                {

                }
            }
            #endregion         
        }
        public IPAddress IPAddress_Server { get { return ImportDesignForm.IPAddress; } }
        public IPAddress IPAddress_Client { get { return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork); } }
        public const string Empty = "Отсутствует";
        public readonly string ToolStrip_LastUpdateOnServer;
        public readonly string ToolStrip_LastUpdateOnClient;
        public readonly string ToolStrip_LastQuery;
        public object InvokeRequired_LockObject = new object();
        #region Hide settings
        int temp_splitContainer_Settings_Split { get; set; }
        int temp_button_Hide_Settings_Width { get; set; }
        int temp_splitContainer_Main_Split { get; set; }
        public void button_Hide_Settings_Click(object sender, EventArgs e)
        {
            try
            {
                splitContainer_Main.Visible = false;

                if (splitContainer_Settings.Panel1Collapsed)
                {
                    splitContainer_Settings.Panel1Collapsed = false;

                    splitContainer_Main.SplitterDistance = temp_splitContainer_Main_Split;
                    splitContainer_Settings.SplitterDistance = temp_splitContainer_Settings_Split;

                    button_Hide_Settings.Text =
                        "\nС" +
                        "\nк" +
                        "\nр" +
                        "\nы" +
                        "\nт" +
                        "\nь";
                }
                else
                {
                    splitContainer_Settings.Panel1Collapsed = true;

                    splitContainer_Main.SplitterDistance = temp_button_Hide_Settings_Width;
                    splitContainer_Settings.SplitterDistance = 0;

                    button_Hide_Settings.Text =
                       "\nП" +
                       "\nо" +
                       "\nк" +
                       "\nа" +
                       "\nз" +
                       "\nа" +
                       "\nт" +
                       "\nь";
                }

                splitContainer_Main.Visible = true;
            }
            catch
            {

            }
        }
        #endregion
        private void panelDataRefreshed_Resize(object sender, EventArgs e)
        {
            try
            {
                if (splitContainer_Settings.Panel1Collapsed)
                {
                    splitContainer_Settings.Panel1Collapsed = false;
                    splitContainer_Settings.Panel1Collapsed = true;
                }
            }
            catch
            {

            }
        }
    }
}
