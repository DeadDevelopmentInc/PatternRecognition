using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PatternRecognition
{
    public partial class ChangePass : Form
    {
        /// <summary>
        /// Standart constructor
        /// </summary>
        public ChangePass()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method for change password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //read textBox value
            string password = textBox1.Text;

            //Check value
            if (password == null)
            {
                //If user don't write pass
                MessageBox.Show("Please enter password");
            }
            else
            {
                //Apply changes
                Properties.Settings.Default.Password = password;
                Properties.Settings.Default.Save();
                this.Close();
            }
        }
    }
}
