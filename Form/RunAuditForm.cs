using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SBT.Audit;

namespace SBT.Form
{
    public partial class RunAuditForm : System.Windows.Forms.Form
    {
        public RunAuditForm(List<Audit2Struct> audit)
        {
            InitializeComponent();
            huyPoVsemuEbalu();
        }

        private void huyPoVsemuEbalu()
        {
            AuditReportTextBox.Text = "Xuy na!";
        }
    }
}
