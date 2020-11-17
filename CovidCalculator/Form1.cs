using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using iTextSharp.text.pdf.parser;
using System.IO;
using ExcelDataReader;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace CovidCalculator
{
    public partial class Form1 : Form
    {
        Thread th;

        public Form1()
        {
            InitializeComponent();
        }

        private void usu2(object obj)
        {
            Application.Run(new About());
        }

        private void usu3(object obj)
        {
            Application.Run(new Calc());
        }

        //butonu cu about
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(usu2);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(usu3);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        double r = 0;

        private void solve(string nume, double varsta, double greutate, double inaltime, string diabet, string cardiac, string hiv)
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

            if (r >= 100) r = 99; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "PDF files|*.xlsx", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    
                    try
                    {

                        FileStream stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        DataSet result = excelReader.AsDataSet();

                        ArrayList fname = new ArrayList();
                        ArrayList lname = new ArrayList();
                        ArrayList age = new ArrayList();
                        ArrayList height = new ArrayList();
                        ArrayList weight = new ArrayList();
                        ArrayList diab = new ArrayList();
                        ArrayList card = new ArrayList();
                        ArrayList hiv = new ArrayList();


                        int count = 0;
                        while (excelReader.Read())
                        {
                            fname.Add(excelReader.GetString(0));
                            lname.Add(excelReader.GetString(1));
                            age.Add(excelReader.GetString(2));
                            height.Add(excelReader.GetString(3));
                            weight.Add(excelReader.GetString(4));
                            diab.Add(excelReader.GetString(5));
                            card.Add(excelReader.GetString(6));
                            hiv.Add(excelReader.GetString(7));
                            count++;
                        }
                        count--;

                        string ans = "";

                        for (int i = 1; i < count; i++)
                        {
                            r = 0;
                            string nume = fname[i].ToString();
                            double varsta = Double.Parse(age[i].ToString());
                            double greutate = Double.Parse(height[i].ToString());
                            double inaltime = Double.Parse(weight[i].ToString());
                            string diabet = diab[i].ToString();
                            string cardiac = card[i].ToString();
                            string hivus = hiv[i].ToString();
                            solve(nume, varsta, inaltime, greutate, diabet, cardiac, hivus);
                            
                            
                            ans += fname[i].ToString() + " " + lname[i].ToString() + " are " + r + " sanse sa faca o forma grava de covid. \n\n"; 
                

                        }

                        MessageBox.Show(ans);

                        string path = @"C:\Users\Usurelu Florian\Desktop\Rezultat.txt";
                        if (!File.Exists(path))
                        {
                            File.Create(path);
                            TextWriter tw = new StreamWriter(path);
                            tw.WriteLine(ans);
                            tw.Close();
                        }
                        else if (File.Exists(path))
                        {
                            using (var tw = new StreamWriter(path, true))
                            {
                                tw.WriteLine(ans);
                                tw.WriteLine("\n#############################################################\n");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
            }
        }
    }
}
