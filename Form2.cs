using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBT
{
    public partial class ImportForm : Form
    {
        public ImportForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
                label3.Enabled = false;
                label4.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                label3.Enabled = true;
                return ;
            }

            if (textBox1.Text == "")
            {
                label3.Enabled = false;
                label4.Enabled = true;
                return ;
            }

            label3.Enabled = false;
            label4.Enabled = false;
            
            

            Close();


        }
    }
}
