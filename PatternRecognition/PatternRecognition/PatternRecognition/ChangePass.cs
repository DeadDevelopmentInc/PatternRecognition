using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class ChangePass : Form
    {
        public ChangePass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = textBox1.Text;

            if (password == null)
            {
                MessageBox.Show("Please enter password");
            }
            else
            {
                Properties.Settings.Default.Password = password;
                Properties.Settings.Default.Save();
                this.Close();
            }
        }
    }
}
