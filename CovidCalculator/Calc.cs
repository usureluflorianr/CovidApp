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

namespace CovidCalculator
{
    public partial class Calc : Form
    {
        Thread th;
        private void usu2(object obj)
        {
            Application.Run(new Form1());
        }
        public Calc()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(usu2);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        //butonul de calcul

        double r = 0; 

        private void solve(string nume, int varsta, int greutate, int inaltime, string diabet, string cardiac, string hiv)
        {
            double bmi = 1.0 * greutate / inaltime / inaltime * 100 * 100;

            double ad = bmi - 25;
            if (bmi >= 19 && bmi <= 25) ad = 0;
            else if (bmi < 19) ad = 19 - bmi;
            else ad = bmi - 25;
            double ad2 = varsta - 20;
            if (ad2 < 0) ad2 = 0;
            if (ad < 0) ad = -ad;

            r = 10.0 + ad * (ad + 1) / 2.0 / 2 / 2 + ad2 * (ad2 + 1) / 2.0 / 6 / 6;

            double cat = (100.0 - r) / 3.0;
            if (cat > 5) cat = 5;

            if (diabet == "DA") r += cat;
            if (cardiac == "DA") r += cat;
            if (hiv == "DA") r += cat;

            if (r >= 100) r = 99.2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nume = textBox1.Text;
            int varsta = Int32.Parse(textBox2.Text);
            int greutate = Int32.Parse(textBox3.Text);
            int inaltime = Int32.Parse(textBox4.Text); 
            string diabet = textBox5.Text;
            string cardiac = textBox6.Text;
            string hiv = textBox7.Text;

            solve(nume, varsta, greutate, inaltime, diabet, cardiac, hiv); 

            MessageBox.Show(nume + ", ai " + r.ToString() + " sanse sa faci o forma grava de covid"); 
        }

        private void Calc_Load(object sender, EventArgs e)
        {

        }
    }
}
