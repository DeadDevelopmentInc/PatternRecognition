using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatternRecognition
{
    public partial class Compute : Form
    {
        //Said about scsfl login
        bool truepas = false;

        /// <summary>
        /// Standart constructor
        /// </summary>
        public Compute()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method for getting psswrd
        /// </summary>
        /// <returns></returns>
        public bool GetPass()
        {
            return truepas;
        }

        /// <summary>
        /// Method for input psswrd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Read textBox value
            string password = textBox1.Text;

            //Chech pass
            if (password == null)
            {
                //If pass empty
                MessageBox.Show("Please enter password");
            }
            else if (password != Properties.Settings.Default.Password)
            {
                //If it pass incorrect
                MessageBox.Show("Please enter correct password");
                textBox1.Clear();
            }
            else
            {
                //If pass correct
                //Change value
                truepas = true;
                this.Close();
            }
        }
    }
}
