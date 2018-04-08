using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Tomogramm
{
    public partial class Form1 : Form { 

         private Bin binaries = new Bin();
         private View view = new View();
         private bool loaded = false;
         private int currentLayer = 0;
         private int FrameCount = 0;
         private bool needReload = false;
         DateTime NextFpsUpdate = DateTime.Now.AddSeconds(1);

     

        public Form1()
        {
            InitializeComponent();
        }

        void displayFPS()
        {
            if (DateTime.Now >= NextFpsUpdate)
            {
                this.Text = String.Format("CT Visualiser (fps={0})", FrameCount);
                NextFpsUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
       {
           OpenFileDialog dialog = new OpenFileDialog();

               if (dialog.ShowDialog() == DialogResult.OK)
               {
                   string str = dialog.FileName;
                   binaries.readBIN(str);
                   view.SetupView(glControl1.Width, glControl1.Height);
                   loaded = true;
                   trackBar2.Maximum = Bin.Z - 1;
                   glControl1.Invalidate();
               }
           }

        private void glControl1_Paint(object sender, EventArgs e)
        {
            if (loaded)
            {
                if (radioButton1.Checked)
                    view.DrawQuads(currentLayer);
                if (radioButton3.Checked)
                    view.DrawQuadsStrip(currentLayer);
                    if (radioButton2.Checked)
                {
                    if (needReload)
                    {
                        view.generateTextureImage(currentLayer);
                        view.Load2DTexture();
                        needReload = false;
                    }
                    view.DrawTexture();
                }
                glControl1.SwapBuffers();
            }
        }

      private void trackBar2_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar2.Value;
            if (radioButton2.Checked) needReload = true;
            glControl1_Paint(sender, e);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }

        private void glControl1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            view.min = trackBar1.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            view.width = trackBar3.Value;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

