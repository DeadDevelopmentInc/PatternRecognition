using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Compute : Form
    {
        bool truepas = false;

        public Compute()
        {
            InitializeComponent();
        }

        public bool GetPass()
        {
            return truepas;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password;

            password = textBox1.Text;

            if (password == null)
            {
                MessageBox.Show("Please enter password");
            }
            else if (password != Properties.Settings.Default.Password)
            {
                MessageBox.Show("Please enter correct password");
                textBox1.Clear();
            }
            else
            {
                truepas = true;
                this.Close();
            }
        }
    }
}
