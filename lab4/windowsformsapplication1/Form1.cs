using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Object thisLock = new Object();
        HtmlNodeCollection nazwa;
        HtmlNodeCollection n_cena;
        HtmlNodeCollection s_prom;

        private void button1_Click(object sender, EventArgs e)
        {
            pobieranie2();
        }

        public Task wyszukiwanie()
        {
            return Task.Factory.StartNew(() =>
                {

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    HtmlWeb hw = new HtmlWeb();
                    doc = hw.Load(textBox1.Text);
                    nazwa = doc.DocumentNode.SelectNodes("//ul[@class=\"product_list float\"]/li/div[3]/a[2]/text()");
                    n_cena = doc.DocumentNode.SelectNodes("//ul[@class=\"product_list float\"]/li/div[3]/div[1]/span[3]/text()");
                    s_prom = doc.DocumentNode.SelectNodes("//ul[@class=\"product_list float\"]/li/div[3]/div[1]/span[2]/span[2]/text()");

                });            
        }
        
        public void pobieranie2()
        {

            int i = 0;
            int j = 0;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb hw = new HtmlWeb();
            doc = hw.Load(textBox1.Text);
            nazwa = doc.DocumentNode.SelectNodes("//ul[@class=\"product_list float\"]/li/div[3]/a[2]/text()");
            n_cena = doc.DocumentNode.SelectNodes("//ul[@class=\"product_list float\"]/li/div[3]/div[1]/span[3]/text()");
            s_prom = doc.DocumentNode.SelectNodes("//ul[@class=\"product_list float\"]/li/div[3]/div[1]/span[2]/span[2]/text()");       

            string wynik_nazwa = "";
            foreach (var name in nazwa)
            {
                wynik_nazwa = name.InnerText + Environment.NewLine;
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = wynik_nazwa;
            }
            string wynik_n_cena = "";
            foreach (var n_price in n_cena)
            {

                wynik_n_cena = n_price.InnerText;
                dataGridView1.Rows[i].Cells[1].Value = wynik_n_cena;
                i++;

            }
            string wynik_s_cena = "";
            foreach (var s_price in s_prom)
            {
                wynik_s_cena = s_price.InnerText;
                dataGridView1.Rows[j].Cells[2].Value = wynik_s_cena;
                j++;
            }
        }
        
        
        public async void pobieranie()
        {
            int i = 0;
            int j = 0;
            await wyszukiwanie();

            string wynik_nazwa = "";
            foreach (var name in nazwa)
            {
                wynik_nazwa = name.InnerText + Environment.NewLine;
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = wynik_nazwa;
            }
            string wynik_n_cena = "";
            foreach (var n_price in n_cena)
            {

                wynik_n_cena = n_price.InnerText;
                dataGridView1.Rows[i].Cells[1].Value = wynik_n_cena;
                i++;

            }
            string wynik_s_cena = "";
            foreach (var s_price in s_prom)
            {
                wynik_s_cena = s_price.InnerText;
                dataGridView1.Rows[j].Cells[2].Value = wynik_s_cena;
                j++;
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            zapis();
        }

        public void zapis()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.TableName = "Promocje";
            dt.Columns.Add("Nazwa");
            dt.Columns.Add("Cena");
            dt.Columns.Add("Procent");
            ds.Tables.Add(dt);

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                DataRow row = ds.Tables["Promocje"].NewRow();

                row["Nazwa"] = r.Cells[0].Value;
                row["Cena"] = r.Cells[1].Value;
                row["Procent"] = r.Cells[2].Value;

                ds.Tables["Promocje"].Rows.Add(row);
            }
            try
            {
                ds.WriteXml("D:\\Data.xml");
                MessageBox.Show("Plik zapisany");
            }
            catch
            {
                MessageBox.Show("Nie moge zapisać pliku");
            }
        }
    }
}
