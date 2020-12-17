using System;
using System.Drawing;
using System.Windows.Forms;

namespace TasksManager.TabControl_Main.F_ServerProcess
{
    public partial class CustomProgressBar : UserControl
    {
        //pb = ProgressBar
        double pbUnit;
        int pbWIDTH, pbHEIGHT, pbComplete;

        Bitmap bmp;

        public void Set(int Complete)
        {
            pbComplete = Complete;
            Draw();
        }
        void Draw()
        {
            //graphics
            g = Graphics.FromImage(bmp);

            //clear graphics
            g.Clear(Color.LightSkyBlue);

            //draw progressbar
            g.FillRectangle(Brushes.CornflowerBlue, new Rectangle(0, 0, (int)(pbComplete * pbUnit), pbHEIGHT));

            //draw % complete
            g.DrawString(pbComplete + "%", new Font("Arial", pbHEIGHT / 2), Brushes.Black, new PointF(pbWIDTH / 2 - pbHEIGHT, pbHEIGHT / 10));

            //load bitmap in picturebox picboxPB
            picboxPB.Image = bmp;
            this.Refresh();
        }

        private void PicboxPB_Resize(object sender, EventArgs e)
        {
            SetResize();
        }
        void SetResize()
        {
            picboxPB.Width = this.Width;
            picboxPB.Height = this.Height;
            pbWIDTH = picboxPB.Width;
            pbHEIGHT = picboxPB.Height;
            pbUnit = pbWIDTH / 100.0;
            bmp = new Bitmap(pbWIDTH, pbHEIGHT);
            Draw();
        }
        public Bitmap GetDrawnBitmap()
        {
            return bmp;
        }

        Graphics g;
        public CustomProgressBar()
        {
            InitializeComponent();
            SetResize();
        }
    }
}
