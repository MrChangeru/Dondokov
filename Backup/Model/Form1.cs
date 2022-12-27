using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Model
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        double a = 0, b = 0, c = -5, d = 0, zoom = 1;
        int os_x = 1, os_y = 0, os_z = 0;
        bool Wire = false;


        anModelLoader Model = null;


        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Il.ilInit();

            Il.ilEnable(Il.IL_ORIGIN_SET);


            Gl.glClearColor(255, 255, 255, 1);

            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
        
            Glu.gluPerspective(45, (float)AnT.Width / (float)AnT.Height, 0.1, 200);
        
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);

            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glLineWidth(1.0f);

            comboBox1.SelectedIndex = 0;
            
            // опиции для загрузки файла
            openFileDialog1.Filter = "ase files (*.ase)|*.ase|All files (*.*)|*.*";
        }

        private void Draw()
        {

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();
            Gl.glColor3i(255, 0, 0);

            Gl.glPushMatrix();
            Gl.glTranslated(a, b, c);
            Gl.glRotated(d, os_x, os_y, os_z);
            Gl.glScaled(zoom, zoom, zoom);

            if(Model != null)
            Model.DrawModel();

            Gl.glPopMatrix();

            Gl.glFlush();

            AnT.Invalidate();
        }

       

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
           
            Draw();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            a = (double)trackBar1.Value / 1000.0;
            label4.Text = a.ToString();
            Draw();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            b = (double)trackBar2.Value / 1000.0;
            label5.Text = b.ToString();
            Draw();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            c = (double)trackBar3.Value / 1000.0;
            label6.Text = c.ToString();
            Draw();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            d = (double)trackBar4.Value;
            label6.Text = d.ToString();
            Draw();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            zoom = (double)trackBar5.Value / 1000.0;
            label6.Text = zoom.ToString();
            Draw();
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        os_x = 1;
                        os_y = 0;
                        os_z = 0;
                        break;
                    }
                case 1:
                    {
                        os_x = 0;
                        os_y = 1;
                        os_z = 0;
                        break;
                    }
                case 2:
                    {
                        os_x = 0;
                        os_y = 0;
                        os_z = 1;
                        break;
                    }
            }

            Draw();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Draw();
        }

        // загрузка модели
        private void выбратьФайлДляЗагрузкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Model = new anModelLoader();
                Model.LoadModel(openFileDialog1.FileName);
                RenderTimer.Start();
            }
        }


    }
}
