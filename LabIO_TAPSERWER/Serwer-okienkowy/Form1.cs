using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerTCPAsync;

namespace Serwer_okienkowy
{
    public partial class Form1 : Form
    {
       


        public Form1()
        {
            InitializeComponent();
            label3.Text = DateTime.Now.ToShortDateString();
        }
       
        private async void startButton(object sender, EventArgs e)
        {
            ServerTCPAsync_TAP server;
            this.button1.Enabled = false;
            server = new ServerTCPAsync_TAP(IPAddress.Parse(textBox1.Text), Convert.ToInt32(textBox2.Text));
            var someTask = Task.Run(() => server.Start());
            await someTask;
            this.button1.Enabled = true;
        }

        private void stopButton(object sender, EventArgs e)
        {
            const string message =
                "Czy jesteś pewny, że chcesz zamknąć serwer?";
            const string caption = "Zamykanie serwera";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
       
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        
       
    }
}
