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
    public partial class SBT : Form
    {
        private AuditFilesDatabaseController _auditFilesDatabaseController;

        public SBT()
        {
            InitializeComponent();

            _auditFilesDatabaseController = AuditFilesDatabaseController.GetInstance();

            UpdateTableLayoutPanel(_auditFilesDatabaseController.GetDatabaseNamesFilenames());

            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                c.MouseClick += new MouseEventHandler(tableLayoutPanel1_Click);
            }

            _auditFilesDatabaseController.OnDatabaseChanged += UpdateTableLayoutPanel;

            
        }



        public void UpdateTableLayoutPanel(Dictionary<string, string> newData)
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Unique Name" }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Path" }, 1, 0);



            foreach (var uniqueName in newData.Keys)
            {
                tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
                
                var uniqueNameTextbox = new TextBox();
                uniqueNameTextbox.Text = uniqueName;
                uniqueNameTextbox.ReadOnly = true;
                uniqueNameTextbox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                

                var pathTextbox = new TextBox();
                pathTextbox.Text = newData[uniqueName];
                pathTextbox.ReadOnly = true;
                pathTextbox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

                tableLayoutPanel1.Controls.Add(uniqueNameTextbox, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(pathTextbox, 1, tableLayoutPanel1.RowCount - 1);


            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void SBT_Load(object sender, EventArgs e)
        {

        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportForm importForm = new ImportForm();

            importForm.Show();
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            var rowIndex = tableLayoutPanel1.GetRow((Control)sender);

            if (rowIndex == 0)
                return;

            var rowControl = tableLayoutPanel1.GetControlFromPosition(0, rowIndex);

            textBox1.Text = _auditFilesDatabaseController.ReadDatabaseFileByName(rowControl.Text);

        }
    }
}
