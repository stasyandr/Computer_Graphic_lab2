﻿using System;
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
    public partial class Form2 : Form
    {
        private Form1 form1;
        //private Form3 form3;
        //private Form4 form4;
        public Form2()
        {
            InitializeComponent();
            form1 = new Form1(this);
            form1.Visible = false;
            //form3 = new Form3(this);
            //form3.Visible = false;
            //form4 = new Form4(this);
            //form4.Visible = false;
        }        
        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            form1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            //form3.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            //form4.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
