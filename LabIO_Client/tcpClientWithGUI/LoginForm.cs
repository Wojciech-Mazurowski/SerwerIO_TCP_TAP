using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tcpLogin_Client_LIB;

namespace tcpClientWithGUI
{
    public partial class LoginForm : Form
    {

        private NetworkStream _stream;

        public LoginForm(NetworkStream stream)
        {
            _stream = stream;
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {

            StringBuilder readable = Operations.Login(_stream, userTextBox.Text, passTextBox.Text);
            if (readable[0] == '1')
            {
                MessageBox.Show("Login Succesful.");
                this.Hide();
                FuncForm ff = new FuncForm(this, _stream);
                ff.Show();

            }
            else { MessageBox.Show("Wrong username or password.");}
        }

        private void Register_Click(object sender, EventArgs e)
        {
            StringBuilder readable = Operations.Register(_stream, userTextBox.Text, passTextBox.Text);
            if (readable[0] == '1')
            {
                MessageBox.Show("Registration Succesful. You can now login.");
            }
            else MessageBox.Show("This user already exists!");
        }

            private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
      
    }
}
