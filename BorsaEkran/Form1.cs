using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace BorsaEkran
{
    public partial class Form1 : Form
    {
        List<Dovizler> doviz = new List<Dovizler>();
        string bg = "https://www.tcmb.gov.tr/kurlar/today.xml";
        XmlDocument xmlVerisi = new XmlDocument();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Fill();
            #region Data
            //string Usd = xmlVerisi.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;
            //var Ussd = xmlVerisi.SelectNodes("Tarih_Date/Currency/Isim").Item(2).InnerText;
            //var FiyatSatis = xmlVerisi.SelectNodes("Tarih_Date/Currency/ForexSelling").Item(1).InnerText;
            //var FiyatAlis = xmlVerisi.SelectNodes("Tarih_Date/Currency/ForexBuying").Item(1).InnerText;

            #endregion
            CBFILL();

        }
        public void Fill()
        {
            xmlVerisi.Load(bg);
            DateTime Tarih = Convert.ToDateTime(xmlVerisi.SelectSingleNode("Tarih_Date").Attributes["Tarih"].Value);

            int sayi = 0;
            for (int i = 0; i < 21; i++)
            {
                doviz.Add(new Dovizler
                {
                    Isim = xmlVerisi.SelectNodes("Tarih_Date/Currency/Isim").Item(sayi).InnerText,
                    AlisFiyati = xmlVerisi.SelectNodes("Tarih_Date/Currency/ForexBuying").Item(sayi).InnerText,
                    SatisFiyati = xmlVerisi.SelectNodes("Tarih_Date/Currency/ForexSelling").Item(sayi).InnerText
                });
                sayi++;

            }
            DataTable dt = new DataTable();
            dt.Columns.Add("Döviz Cinsi");
            dt.Columns.Add("Alış");
            dt.Columns.Add("Satış");
            foreach (var item in doviz)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.Isim;
                dr[1] = item.AlisFiyati;
                dr[2] = item.SatisFiyati;
                dt.Rows.Add(dr);
            }
            dataGridView1.DataSource = dt;
        }
        private void Guncel()
        {
            doviz.Clear();



        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Guncel();
            Fill();
        }
        private void CBFILL()
        {

            CB1.ValueMember = "AlisFiyati";
            CB1.DisplayMember = "Isim";
            CB1.DataSource = doviz.ToList();
            //CB2.ValueMember = "AlisFiyati";
            //CB2.DisplayMember = "Isim";
            //CB2.DataSource = doviz.ToList();
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            //txtCB1.Text = CB1.SelectedValue.ToString();
            //txtCB2.Text = CB2.SelectedValue.ToString();
        }

        private void btnhesap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty ( lblHesap.Text)&& string.IsNullOrEmpty(CB1.SelectedValue.ToString()))
            {
                decimal a = Convert.ToDecimal(CB1.SelectedValue.ToString().Replace(".", ","));
                //decimal b = Convert.ToDecimal(txtCB2.Text.Replace(".", ","));
                lblHesap.Text = (a * Convert.ToDecimal(txtCB1.Text.Replace(".", ","))).ToString() + "TL ";
            }
            else
            {
                MessageBox.Show("Lütfen Veri Giriniz");
            }


        }
    }
    
}
