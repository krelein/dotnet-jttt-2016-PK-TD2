using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemotMail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.DataSource = ListaZadan.lista;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "" && textBox3.Text != "")
            {
                Zadanie tmp=new Zadanie(textBox3.Text, textBox1.Text, textBox4.Text, textBox2.Text);
                ListaZadan.lista.Add(tmp);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Message M = new Message("demotmailtest@gmail.com", "dotNetC#");

            foreach (var task in ListaZadan.lista)
            {              
                M.SetUrl(task.url);
                M.Send(task.adres, task.tekst);        
            }
            MessageBox.Show("Wykonano");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListaZadan.Serialize();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListaZadan.lista.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ListaZadan.Deserialize();
            listBox1.DataSource = ListaZadan.lista;
        }
    }
}
