using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompGraf2
{
    public partial class Form4 : Form
    {
        private Form2 main;
        public Form4(Form2 m)
        {
            main = m;
            InitializeComponent();
        }
        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            main.Visible = true;
        }
        double my_H = 0;
        double my_S = 0;
        double my_V = 0;
        Bitmap pict;


        List<double> HSV_Transformer_From_RGB(double R, double G, double B)
        {


            List<double> l = new List<double>();

            double maximum = Math.Max(Math.Max(R, G), B);
            double minimum = Math.Min(Math.Min(R, G), B);
            double difference = maximum - minimum;

            double H;
            if (Math.Abs(maximum - minimum) < 0.0001)
                H = 0;
            else
            if (Math.Abs(maximum - R) < 0.0001 && G >= B)
                H = 60 * (G - B) / difference;
            else
            if (Math.Abs(maximum - B) < 0.0001 && G < B)
                H = 60 * (G - B) / difference + 360;
            else
            if (Math.Abs(maximum - G) < 0.0001)
                H = 60 * (B - R) / difference + 120;
            else
                H = 60 * (R - G) / difference + 240;
            double S;

            if (Math.Abs(maximum - 0) < 0.0001)
                S = 0;
            else
                S = 1 - minimum / maximum;

            double V = maximum;

            l.Add(H);
            l.Add(S);
            l.Add(V);

            return l;
        }



        List<double> RGB_Transformer_From_HSV(double H, double S, double V)
        {

            double helper = Math.Floor(H / 60.0) % 6;
            double x = H / 60.0 - Math.Floor(H / 60.0);
            double y = (1 - S) * V;
            double z = (1 - x * S) * V;
            double q = (1 - (1 - x) * S) * V;
            List<double> l = new List<double>();
            switch (helper)
            {
                case 0:
                    l.Add(V);
                    l.Add(q);
                    l.Add(y);
                    return l;
                case 1:
                    l.Add(z);
                    l.Add(V);
                    l.Add(y);
                    return l;
                case 2:
                    l.Add(y);
                    l.Add(V);
                    l.Add(q);
                    return l;
                case 3:
                    l.Add(y);
                    l.Add(z);
                    l.Add(V);
                    return l;
                case 4:
                    l.Add(q);
                    l.Add(y);
                    l.Add(V);
                    return l;
                case 5:
                    l.Add(V);
                    l.Add(y);
                    l.Add(z);
                    return l;
                default:

                    return l;
            }
        }


        void Draw_and_Save()
        {
            double loc_H = trackBar1.Value;
            double loc_S = trackBar2.Value / 100.0;
            double loc_V = trackBar3.Value / 100.0;

            Bitmap myBitmap = (Bitmap)pict.Clone();

            for (int i = 0; i < myBitmap.Width; ++i)
                for (int j = 0; j < myBitmap.Height; ++j)
                {
                    var pixel_color = myBitmap.GetPixel(i, j);
                    var res = HSV_Transformer_From_RGB(pixel_color.R / 255.0, pixel_color.G / 255.0, pixel_color.B / 255.0);


                    res[0] += loc_H;

                    if (res[0] > 360)
                        res[0] -= 360;
                    if (res[0] < 0)
                        res[0] += 360;
                    res[1] += loc_S;
                    res[1] = Math.Min(1, res[1]);
                    res[1] = Math.Max(0, res[1]);
                    res[2] += loc_V;
                    res[2] = Math.Min(1, res[2]);
                    res[2] = Math.Max(0, res[2]);

                    var res_back = RGB_Transformer_From_HSV(res[0], res[1], res[2]);
                    myBitmap.SetPixel(i, j, Color.FromArgb(pixel_color.A, (int)(res_back[0] * 255), (int)(res_back[1] * 255), (int)(res_back[2] * 255)));
                }

            pictureBox1.Image = myBitmap;
            myBitmap.Save("my_final.jpg");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openner = new OpenFileDialog
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"
            };
            if (openner.ShowDialog() == DialogResult.OK)
            {
                pict = new Bitmap(openner.FileName);
                pictureBox1.Image = pict;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            my_H = trackBar1.Value * 1.5;
            Draw_and_Save();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            my_S = trackBar2.Value / 100.0;
            Draw_and_Save();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            my_V = trackBar3.Value / 100.0;
            Draw_and_Save();
        }
    }
}
