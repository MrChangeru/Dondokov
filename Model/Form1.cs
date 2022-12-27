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

        // Данные по умолчанию
        double X = -0.015, Y = -0.07, Z = -0.35, zoom = 1;//координаты 3д модели и приближение
        double capturedX = 0, capturedY = 0;//координаты мыши в момент захвата
        int index=0;
        double[] angle = new double[3] { 280, 0, 0 };//угол перспективы
        bool captured = false;//захват мышью
        anModelLoader Model = null;

        //Главная функция
        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Il.ilInit();

            Il.ilEnable(Il.IL_ORIGIN_SET);


            Gl.glClearColor(255, 255, 255, 1);

            Gl.glViewport(0, 0, 2*AnT.Width, 2*AnT.Height);

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
            
            // загрузка модели
            Model = new anModelLoader();
            Model.LoadModel("C:/Users/chang/Desktop/ВСГУТУ/Геометрия и топология 2/Курсовая/hirdoscooter.ase");
            RenderTimer.Start();
        }

        //Функция для определения координат и угла перспективы модели
        private void Draw()
        {

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();
            Gl.glColor3i(255, 0, 0);

            Gl.glPushMatrix();
            Gl.glTranslated(X, Y, Z);
            Gl.glRotated(angle[0], 1, 0, 0);
            Gl.glRotated(angle[1], 0, 1, 0);
            Gl.glRotated(angle[2], 0, 0, 1);
            Gl.glScaled(zoom, zoom, zoom);

            if(Model != null)
            Model.DrawModel();

            Gl.glPopMatrix();

            Gl.glFlush();

            AnT.Invalidate();
        }
               
        //Функция таймера. через определенное время запускает функцию Draw
        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            Draw();
        }

        //Функция изменения координаты Х для модели 
        private void trackBarX_Scroll(object sender, EventArgs e)
        {
            X = (double)trackBarX.Value / 100000.0;
            label4.Text = X.ToString();
            Draw();
        }

        //Функция изменения координаты Y для модели 
        private void trackBarY_Scroll(object sender, EventArgs e)
        {
            Y = (double)trackBarY.Value / 100000.0;
            label5.Text = Y.ToString();
            Draw();
        }

        //Функция изменения координаты Z для модели
        private void trackBarZ_Scroll(object sender, EventArgs e)
        {
            Z = (double)trackBarZ.Value / 100000.0;
            label6.Text = Z.ToString();
            Draw();
        }
                
        //Функция определяет нажатие кнопки мыши
        private void AnT_MouseDown(object sender, MouseEventArgs e)
        {
            capturedX = (double)e.X;
            capturedY = (double)e.Y;
            captured = true;
        }

        //Функция определяет опускание кнопки мыши
        private void AnT_MouseUp(object sender, MouseEventArgs e)
        {
            captured = false;
        }

        //Функция определяет передвижение мыши
        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured == true)
            {
                angle[2] += -(capturedX - (double)e.X) / 1000;
                angle[0] += -(capturedY - (double)e.Y) / 1000;

            }
            Draw();
        }

        //Функция изменения угла перспективы
        private void trackBarAngle_Scroll(object sender, EventArgs e)
        {
            angle[index] = (double)trackBarAngle.Value;
            label10.Text = angle[index].ToString();
            Draw();
        }

        //Функция зума
        private void trackBarZoom_Scroll(object sender, EventArgs e)
        {
            zoom = (double)trackBarZoom.Value / 1000.0;
            label11.Text = zoom.ToString();
            Draw();
        }

        //Функция определения оси вращения
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        index = 0;
                        break;
                    }
                case 1:
                    {
                        index = 1;
                        break;
                    }
                case 2:
                    {
                        index = 2;
                        break;
                    }
            }

            Draw();

        }     
    }
}
