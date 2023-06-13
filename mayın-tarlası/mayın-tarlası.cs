using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;


namespace mayın_tarlası
{

    public partial class mayın_tarlası : Form
    {
        OleDbConnection con;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        string bilgilerim = "EREN TAN 17 UYDUSUNDAN SELAMLAR 7 KITA 124 ÜLKEYE YAYIN YAPMAKTAYIZ";

        public mayın_tarlası()
        {
            InitializeComponent();
        }

        int Puan = 0;
        Random rnd = new Random();
        List<int> Mayın = new List<int>();
        int Katsayi = 1;

        private void mayın_tarlası_Load(object sender, EventArgs e)
        {
            
            gridwievDolur();
            dataGrid.Rows[0].Cells[0].Selected = false;

            bilgileriYaz();
        }
        public void gridwievDolur()
        {
            dataGrid.Rows.Clear();
            con = new OleDbConnection("Provider=Microsoft.ACE.OleDb.12.0;Data Source=mayintarlasi.accdb");
            da = new OleDbDataAdapter("SElect userName,dTarih,puan from Oyuncu", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "Oyuncu");
            dataGrid.DataSource = ds.Tables["Oyuncu"];
            con.Close();
            dataGrid.Rows[0].Cells[0].Selected = true;
        }
        private bool isimKontrol(string isim)
        {
            con.Open();
            OleDbCommand komut = new OleDbCommand("SELECT COUNT(*) FROM Oyuncu where userName=@adi", con);
            komut.Parameters.Add("@adi", isim);

            int ks = int.Parse(komut.ExecuteScalar().ToString());
            if (ks == 1)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
                

            }



        }

        /*mayın tarlası oluşturma */
        int Msayisi = 0;
        private void btndoldur(int mayinSayisi)
        {
            int sayı = 0;
            for (int m = 0; m < mayinSayisi; m++)
            {
                sayı = rnd.Next(1, 100);
                for (int g = 0; g < Mayın.Count; g++)
                {
                    if (sayı == Mayın[g])
                    {
                        sayı = rnd.Next(1, 100);
                        g = 0;
                    }
                }

                Mayın.Add(sayı);
            }

            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.Controls.Clear();

            int kutu = 1;



            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.RowCount = 10;



            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

                for (int x = 0; x < tableLayoutPanel1.RowCount; x++)
                {
                    if (i == 0)
                    {
                        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                    }
                    Button cmd = new Button();
                    cmd.BackColor = Color.Blue;
                    cmd.Dock = DockStyle.Fill;
                    cmd.Name = kutu.ToString();
                    // cmd.Text = kutu.ToString();
                    tableLayoutPanel1.Controls.Add(cmd, i, x);
                    kutu++;
                }
            }

            ButtonClickAyarla();
        }
        private void kayıt()
        {
            cmd = new OleDbCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "insert into Oyuncu (userName,dTarih,puan) values ('" + textBox1.Text + "','" + String.Format("{0:d}", DateTime.Now) + "','" + Convert.ToInt32(puantext.Text) + "')";
            cmd.ExecuteNonQuery();
            con.Close();
        }
        /*------------------------------------------------*/
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                label2.Text = "10";
                Katsayi = 1;
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                label2.Text = "25";
                Katsayi = 3;
            }
        }


        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                label2.Text = "40";
                Katsayi = 5;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Maximum = 100 - Convert.ToInt32(label2.Text);
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Step = 100 - (Convert.ToInt32(label2.Text));
            Mayın.Clear();

            if (string.IsNullOrEmpty(textBox1.Text))
                MessageBox.Show("Oyuncu adı kısmı boş geçilemez");
            else if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false)
                MessageBox.Show("Lütfen oyun seviyesini seçiniz");
            else if (isimKontrol(textBox1.Text))
            {
                MessageBox.Show("kullanıcı zaten var.");

            }
            else
            {
                btndoldur(Convert.ToInt32(label2.Text));
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;

            }
            Dtiklanan = 1;

            //hile();

        }
        private void ButtonClickAyarla()
        {
            foreach (Control ctl in this.tableLayoutPanel1.Controls)
            {
                ctl.MouseClick += new MouseEventHandler(mayın_tarlası_MouseClick);

            }

        }

        private void mayın_tarlası_MouseClick(object sender, MouseEventArgs e)
        {
            Event(sender);

        }
        int Dtiklanan = 1;
        private void Event(object sender)
        {

            if (sender.GetType().Name == "Button")
            {
                Msayisi = 0;
                Button tiklanan = (Button)sender;
                if (tiklanan.BackColor != Color.Green && tiklanan.BackColor != Color.Red)
                {
                    string isim = tiklanan.Name;
                    if (Mayın.IndexOf(Convert.ToInt32(isim)) != -1) // Yandın
                    {
                        tiklanan.BackColor = Color.Red;
                        HepsiniAc();
                        MessageBox.Show("Kaybettiniz. \nToplam Puan: " + Puan);
                        Puan = 0;
                    }
                    else // Kazandın
                    {


                        tiklanan.BackColor = Color.Green;


                        Puan += Katsayi;
                        //tiklanan.Text = Puan.ToString();
                        puantext.Text = Puan.ToString();


                        if (Dtiklanan == 100 - Convert.ToInt32(label2.Text))
                        {
                            HepsiniAc();
                            MessageBox.Show("Kazandınız \nToplam Puan: " + Puan);
                            Puan = 0;
                        }
                        else
                        {
                            Dtiklanan++;
                            toolStripProgressBar1.Value += 1;
                        }
                        foreach (int item in mayınTara(Convert.ToInt32(tiklanan.Name)))
                        {
                            for (int k = 0; k < Mayın.Count; k++)
                            {
                                if (item == Mayın[k])
                                {
                                    Msayisi++;
                                }
                            }
                        }
                        tiklanan.Text = Msayisi.ToString();

                    }
                }
            }

        }
        /*herhangi oyun sonunu gösteren kod bloğu */
        private void HepsiniAc()
        {
            for (int i = 0; i <= (tableLayoutPanel1.ColumnCount * tableLayoutPanel1.RowCount) - 1; i++)
            {
                Button btn = (Button)tableLayoutPanel1.Controls[i];
                if (Mayın.IndexOf(Convert.ToInt32(btn.Name)) != -1)
                {
                    btn.BackColor = Color.Red;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                    btn.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\mayin.png");
                }
                else
                {
                    btn.BackColor = Color.Green;
                }


            }
            kayıt();


            this.Controls.Clear();
            this.InitializeComponent();
            gridwievDolur();
            bilgileriYaz();
            

        }
        private void bilgileriYaz()
        {
            toolStripStatusLabel1.Text = bilgilerim;
            toolStripStatusLabel2.Text = DateTime.Now.ToString();
        }

        //hileee- -------------------------------------

        //private void hile()
        //{
        //    for (int i = 0; i <= (tableLayoutPanel1.ColumnCount * tableLayoutPanel1.RowCount) - 1; i++)
        //    {
        //        Button btn = (Button)tableLayoutPanel1.Controls[i];
        //        if (Mayın.IndexOf(Convert.ToInt32(btn.Name)) != -1)
        //        {
        //            btn.BackColor = Color.Red;
        //            btn.BackgroundImageLayout = ImageLayout.Stretch;
        //            btn.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\mayin.png");
        //        }
        //    }
        //}

        List<int> taranacak = new List<int>();
        private List<int> mayınTara(int tıklananid)
        {
            taranacak.Clear();
            if (tıklananid == 1)
            {
                taranacak.Add(tıklananid + 11);
                taranacak.Add(tıklananid + 10);
                taranacak.Add(tıklananid + 1);
            }
            else if (tıklananid == 10)
            {
                taranacak.Add(tıklananid + 9);
                taranacak.Add(tıklananid + 10);
                taranacak.Add(tıklananid - 1);
            }
            else if (tıklananid == 100)
            {
                taranacak.Add(tıklananid - 1);
                taranacak.Add(tıklananid - 10);
                taranacak.Add(tıklananid - 11);
            }
            else if (tıklananid == 91)
            {
                taranacak.Add(tıklananid + 1);
                taranacak.Add(tıklananid - 9);
                taranacak.Add(tıklananid - 10);
            }
            else if (tıklananid != 1 && tıklananid != 91 && tıklananid % 10 == 1)
            {
                taranacak.Add(tıklananid + 10);
                taranacak.Add(tıklananid - 10);
                taranacak.Add(tıklananid - 9);
                taranacak.Add(tıklananid + 11);
                taranacak.Add(tıklananid + 1);
            }
            else if (tıklananid != 1 && tıklananid < 10)
            {
                taranacak.Add(tıklananid + 10);
                taranacak.Add(tıklananid - 1);
                taranacak.Add(tıklananid + 1);
                taranacak.Add(tıklananid + 9);
                taranacak.Add(tıklananid + 11);
            }
            else if (tıklananid != 10 && tıklananid != 100 && tıklananid % 10 == 0)
            {
                taranacak.Add(tıklananid + 10);
                taranacak.Add(tıklananid - 10);
                taranacak.Add(tıklananid + 9);
                taranacak.Add(tıklananid - 11);
                taranacak.Add(tıklananid - 1);
            }
            else if (tıklananid != 91 && tıklananid != 100 && tıklananid / 10 == 9)
            {
                taranacak.Add(tıklananid - 10);
                taranacak.Add(tıklananid + 1);
                taranacak.Add(tıklananid - 1);
                taranacak.Add(tıklananid - 11);
                taranacak.Add(tıklananid - 9);
            }
            else
            {
                taranacak.Add(tıklananid + 10);
                taranacak.Add(tıklananid + 1);
                taranacak.Add(tıklananid - 1);
                taranacak.Add(tıklananid - 10);
                taranacak.Add(tıklananid - 11);
                taranacak.Add(tıklananid + 11);
                taranacak.Add(tıklananid - 9);
                taranacak.Add(tıklananid + 9);
            }
            return taranacak;



        }

      
    }
}
