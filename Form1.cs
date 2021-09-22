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
    public partial class Form1 : Form
    {
        private Form2 main;
        public Form1(Form2 form2)
        {
            main = form2;
            InitializeComponent();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private Bitmap imR, imB, imG, histR, histB, histG;

        private void button5_Click(object sender, EventArgs e)
        {
            main.Visible = true;
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = imR;
            pictureBox3.Image = histR;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = imG;
            pictureBox3.Image = histG;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = imB;
            pictureBox3.Image = histB;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog image = new OpenFileDialog();
            image.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.JPEG)|*.BMP;*.JPG;*.GIF;*.PNG;*.JPEG|All files (*.*)|*.*";
            if (image.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(image.FileName);
                    pictureBox2.Image = null;
                    pictureBox3.Image = null;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (pictureBox1.Image != null)
            {
                int[] arrR = new int[256];
                int[] arrG = new int[256];
                int[] arrB = new int[256];
                Bitmap input = new Bitmap(pictureBox1.Image);
                imR = new Bitmap(input.Width, input.Height);
                for (int i = 0; i < input.Width; i++)
                {
                    for (int j = 0; j < input.Height; j++)
                    {
                        Color color = input.GetPixel(i, j);
                        imR.SetPixel(i, j, Color.FromArgb(color.A, color.R, color.R, color.R));
                        arrR[color.R]++;
                    }
                }
                imB = new Bitmap(input.Width, input.Height);
                for (int i = 0; i < input.Width; i++)
                {
                    for (int j = 0; j < input.Height; j++)
                    {
                        Color color = input.GetPixel(i, j);
                        imB.SetPixel(i, j, Color.FromArgb(color.A, color.B, color.B, color.B));
                        arrB[color.B]++;
                    }
                }
                imG = new Bitmap(input.Width, input.Height);
                for (int i = 0; i < input.Width; i++)
                {
                    for (int j = 0; j < input.Height; j++)
                    {
                        Color color = input.GetPixel(i, j);
                        imG.SetPixel(i, j, Color.FromArgb(color.A, color.G, color.G, color.G));
                        arrG[color.G]++;
                    }
                }
                histR = new Bitmap(pictureBox3.Width, pictureBox3.Height);
                histG = new Bitmap(pictureBox3.Width, pictureBox3.Height);
                histB = new Bitmap(pictureBox3.Width, pictureBox3.Height);
                
                int maxR = 0;
                for (int i = 0; i < 256; ++i)
                {
                    if (arrR[i] > maxR)
                        maxR = arrR[i];
                }
                int maxG = 0;
                for (int i = 0; i < 256; ++i)
                {
                    if (arrG[i] > maxG)
                        maxG = arrG[i];
                }
                int maxB = 0;
                for (int i = 0; i < 256; ++i)
                {
                    if (arrB[i] > maxB)
                        maxB = arrB[i];
                }
                double pointR = (double)maxR / pictureBox3.Height;
                double pointG = (double)maxG / pictureBox3.Height;
                double pointB = (double)maxB / pictureBox3.Height;
                
                Color histColor = Color.Red;
                for (int i = 0; i < 256; ++i)
                {
                    for (var j = pictureBox3.Height - 1; j >= pictureBox3.Height - arrR[i] / pointR; j--)
                    {
                        histR.SetPixel(i, j, histColor);
                    }
                }
                histColor = Color.Green;
                for (int i = 0; i < 256; ++i)
                {
                    for (var j = pictureBox3.Height - 1; j >= pictureBox3.Height - arrG[i] / pointG; j--)
                    {
                        histG.SetPixel(i, j, histColor);
                    }
                }
                histColor = Color.Blue;
                for (int i = 0; i < 256; ++i)
                {
                    for (var j = pictureBox3.Height - 1; j >= pictureBox3.Height - arrB[i] / pointB; j--)
                    {
                        histB.SetPixel(i, j, histColor);
                    }
                }
            }
        }
    }
}
