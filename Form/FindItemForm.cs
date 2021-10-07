using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBT.Form
{
    public partial class FindItemForm : System.Windows.Forms.Form
    {
        private SecurityBenchmarkUiForm _mainForm;

        public FindItemForm()
        {
            InitializeComponent();
            _mainForm = Application.OpenForms.OfType<SecurityBenchmarkUiForm>().First();
            _mainForm.FormClosed += CloseForm;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            _mainForm.Focus();
            _mainForm.FindItemsByName(textBox1.Text);
            
        }

        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            _mainForm.FindItemsByName("");
        }
    }
}
