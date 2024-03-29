﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb; //ole datababese acess

namespace Kitaplik_Projesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection(@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source =C:\Users\Mustafa\Desktop\Kişisel\Udemy Projeler\Kitaplik_Projesi\Kitaplik.mdb");

        void listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("Select * from Kitaplar",baglanti);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
            //radioButton1.Checked = true; //otamatik seçili getirdikki veri tabanında hata almayalım
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            listele();
        }

        string durum = "";

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if
                (
                string.IsNullOrEmpty(TxtKitapAd.Text) || string.IsNullOrEmpty(TxtYazar.Text) || string.IsNullOrEmpty(TxtSayfa.Text)
                || string.IsNullOrEmpty(CmbTur.Text)
                )
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz doldurunuz eksiksiz doldurunuz");
                return;
            }

            if (rdBtnIkıncıEl.Checked == false && rdBtnPakette.Checked == false) //en az biri seçilmek zorunda
            {
                MessageBox.Show("Lütfen kitap durumunu seçiniz");
                return;
            }


            baglanti.Open();
            OleDbCommand komut1 = new OleDbCommand("insert into Kitaplar (KitapAd,Yazar,Tur,Sayfa,Durum) values(@p1,@p2,@p3,@p4,@p5)",baglanti);
            komut1.Parameters.AddWithValue("@p1",TxtKitapAd.Text);
            komut1.Parameters.AddWithValue("@p2",TxtYazar.Text);
            komut1.Parameters.AddWithValue("@p3",CmbTur.Text);
            komut1.Parameters.AddWithValue("@p4",TxtSayfa.Text);
            komut1.Parameters.AddWithValue("@p5",durum);
            komut1.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kitap Eklendi","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            listele();    
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            durum = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            durum = "1";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            TxtKitapid.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtKitapAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtYazar.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            CmbTur.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtSayfa.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            if (dataGridView1.Rows[secilen].Cells[5].Value.ToString() == "True")
            {
                rdBtnPakette.Checked = true;
            }
            else
            {
                rdBtnIkıncıEl.Checked = true;
            }
           
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if
               (
               string.IsNullOrEmpty(TxtKitapid.Text) 
               )
            {
                MessageBox.Show("Lütfen id alanını doldurunuz eksiksiz doldurunuz");
                return;
            }

            baglanti.Open();
            OleDbCommand komutsil = new OleDbCommand(
               "Delete From Kitaplar where Kitapid = @k1", baglanti);
            komutsil.Parameters.AddWithValue("k1", TxtKitapid.Text);
            komutsil.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıt Silindi");
            listele();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if
               (
                string.IsNullOrEmpty(TxtKitapid.Text) ||  string.IsNullOrEmpty(TxtKitapAd.Text) || string.IsNullOrEmpty(TxtYazar.Text)
                || string.IsNullOrEmpty(TxtSayfa.Text) || string.IsNullOrEmpty(CmbTur.Text)
               )
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz doldurunuz eksiksiz doldurunuz");
                return;
            }

            baglanti.Open();
            OleDbCommand komut = new OleDbCommand(
               "Update Kitaplar set KitapAd=@p1,Yazar=@p2,Tur=@p3,Sayfa=@p4,Durum=@p5 where Kitapid = @p6", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtKitapAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtYazar.Text);
            komut.Parameters.AddWithValue("@p3", CmbTur.Text);
            komut.Parameters.AddWithValue("@p4", TxtSayfa.Text);

            if (rdBtnIkıncıEl.Checked == true)
            {
                komut.Parameters.AddWithValue("@p5", durum);
            }
            else
            {
                komut.Parameters.AddWithValue("@p5", durum);
            }

            komut.Parameters.AddWithValue("@p6", TxtKitapid.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıt Güncellendi");
            listele();
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            if
                (
                string.IsNullOrEmpty(TxtKitapBul.Text)
                )
            {
                MessageBox.Show("Lütfen Kitap arama alanını doldurunuz eksiksiz doldurunuz");
                return;
            }

            OleDbCommand komut = new OleDbCommand(
               "Select * from Kitaplar where KitapAd = @k1", baglanti);
            komut.Parameters.AddWithValue("@k1", TxtKitapBul.Text);

            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(komut);
            da.Fill(dt);
            dataGridView1.DataSource = dt;         
        }
        

        private void BtnKitapAra_Click(object sender, EventArgs e)
        {
            if
                (
                string.IsNullOrEmpty(TxtKitapBul.Text)
                )
            {
                MessageBox.Show("Lütfen Kitap arama alanını doldurunuz eksiksiz doldurunuz");
                return;
            }

            OleDbCommand komut = new OleDbCommand(
               "Select * from Kitaplar where KitapAd like '%" + TxtKitapBul.Text + "%' ", baglanti);
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(komut);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
