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
    public partial class Form3 : Form
    {
        private Form2 main;
        public Form3(Form2 m)
        {
            main = m;
            InitializeComponent();
        }
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog picker = new OpenFileDialog();
            picker.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (picker.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(picker.FileName);
                if (pictureBox1.Image != null)
                {
                    Bitmap img = new Bitmap(pictureBox1.Image);
                    Bitmap ntsc = GetNTSC(img);
                    pictureBox2.Image = ntsc;
                    pictureBox3.Image = DrawHistogram(ntsc, pictureBox3.Width, pictureBox3.Height);
                    Bitmap hdtv = GetHDTV(img);
                    pictureBox4.Image = hdtv;
                    pictureBox5.Image = DrawHistogram(hdtv, pictureBox5.Width, pictureBox5.Height);
                    pictureBox6.Image = GetDifference(ntsc, hdtv);
                }

            }
        }
        static Bitmap GetNTSC(Bitmap bitImage)
        {
            Bitmap result = new Bitmap(bitImage.Width, bitImage.Height);
            for (int i = 0; i < bitImage.Width; i++)
            {
                for (int j = 0; j < bitImage.Height; j++)
                {
                    Color pixelColor = bitImage.GetPixel(i, j);
                    double colorValue = 0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B;
                    int adjustedColorValue = (int)(colorValue <= 255 ? colorValue : 255);
                    result.SetPixel(i, j, Color.FromArgb(pixelColor.A, adjustedColorValue, adjustedColorValue, adjustedColorValue));
                }
            }

            return result;
        }
        static Bitmap GetHDTV(Bitmap bitImage)
        {
            Bitmap result = new Bitmap(bitImage.Width, bitImage.Height);
            for (int i = 0; i < bitImage.Width; i++)
            {
                for (int j = 0; j < bitImage.Height; j++)
                {
                    Color pixelColor = bitImage.GetPixel(i, j);
                    double colorValue = 0.2126 * pixelColor.R + 0.7152 * pixelColor.G + 0.0722 * pixelColor.B;
                    int adjustedColorValue = (int)(colorValue <= 255 ? colorValue : 255);
                    result.SetPixel(i, j, Color.FromArgb(pixelColor.A, adjustedColorValue, adjustedColorValue, adjustedColorValue));
                }
            }

            return result;
        }

        static Bitmap DrawHistogram(Bitmap image, int pictureBoxWidth, int pictureBoxHeight)
        {
            int width = image.Width, height = image.Height;
            Bitmap hist = new Bitmap(pictureBoxWidth, pictureBoxHeight);
            int[] arr = new int[256];

            Color color;
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    color = image.GetPixel(i, j);
                    arr[color.B]++;
                }
            }
            int max = 0;
            for (int i = 0; i < 256; ++i)
            {
                if (arr[i] > max)
                    max = arr[i];
            }
            double maxPoint = (double)max / pictureBoxHeight;
            Color histColor = Color.Gray;
            for (int i = 0; i < pictureBoxWidth; ++i)
            {
                for (var j = pictureBoxHeight - 1; j > pictureBoxHeight - arr[i] / maxPoint; --j)
                {
                    hist.SetPixel(i, j, histColor);
                }
            }
            return hist;
        }

        static Bitmap GetDifference(Bitmap image1, Bitmap image2)
        {
            Bitmap diff = new Bitmap(image1.Width, image1.Height);
            List<List<(int, int, int)>> t = new List<List<(int, int, int)>>(image1.Width);
            for (int i = 0; i < image1.Width; i++)
            {
                t.Add(new List<(int, int, int)>(image1.Height));
                for (int j = 0; j < image1.Height; j++)
                {
                    t[i].Add((0, 0, 0));
                }
            }
            for (int i = 0; i < image1.Width; i++)
            {
                for (int j = 0; j < image1.Height; j++)
                {
                    Color color1 = image1.GetPixel(i, j);
                    Color color2 = image2.GetPixel(i, j);
                    t[i][j] = (Math.Abs(color1.R - color2.R) * 10, Math.Abs(color1.G - color2.G) * 10, Math.Abs(color1.B - color2.B) * 10);
                }
            }
            for (int i = 0; i < image1.Width; i++)
            {
                for (int j = 0; j < image1.Height; j++)
                {
                    diff.SetPixel(i, j, Color.FromArgb(t[i][j].Item1, t[i][j].Item2, t[i][j].Item3));
                }
            }

            return diff;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            main.Visible = true;
        }
    }
}
