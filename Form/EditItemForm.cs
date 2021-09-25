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
    public partial class EditItemForm : System.Windows.Forms.Form
    {
        public Action OnUpdate = null;

        private List<int> _fieldContainer;

        private Audit2Struct _audit;

        private int _selectedRowId;
        private int _uniqueNumber;

        public EditItemForm(Audit2Struct audit)
        {
            InitializeComponent();

            _audit = audit;

            _fieldContainer = new List<int>();

            _selectedRowId = -1; //  audit.Fields.Count != 0 ? audit.Fields.Count - 1 : 0
            UpdateTableLayoutPanel(audit);
        }

        private void UpdateTableLayoutPanel(Audit2Struct audit)
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Key" }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Value" }, 1, 0);
            for (var index = 0; index < audit.Fields.Count; index++)
            {
                var field = audit.Fields[index];
                tableLayoutPanel1.RowCount += 1;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));

                
                var keyTextBox = new RichTextBox();
                keyTextBox.Text = field.Key;
                keyTextBox.ReadOnly = false;
                keyTextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

                var valueTextBox = new RichTextBox();
                valueTextBox.Text = field.Value;
                valueTextBox.ReadOnly = false;
                valueTextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

                tableLayoutPanel1.Controls.Add(keyTextBox, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(valueTextBox, 1, tableLayoutPanel1.RowCount - 1);

                _fieldContainer.Add(index);

                keyTextBox.TextChanged -= FieldKeyModification_handler;
                keyTextBox.MouseClick -= selectRow_Click;
                keyTextBox.TextChanged += FieldKeyModification_handler;
                keyTextBox.MouseClick += selectRow_Click;

                valueTextBox.TextChanged -= FieldValueModification_handler;
                valueTextBox.MouseClick -= selectRow_Click;
                valueTextBox.TextChanged += FieldValueModification_handler;
                valueTextBox.MouseClick += selectRow_Click;
            }

            tableLayoutPanel1.RowCount += 1;

            var buttonsPanel = new TableLayoutPanel();

            var addButton = new Button();
            addButton.Text = "+";
            addButton.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            addButton.MouseClick += addButton_Click;

            var deleteButton = new Button();
            deleteButton.Text = "-";
            deleteButton.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            deleteButton.MouseClick += deleteButton_Click;

            buttonsPanel.RowCount = 1;
            buttonsPanel.ColumnCount = 2;
            buttonsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));

            buttonsPanel.Controls.Add(deleteButton, 0, buttonsPanel.RowCount - 1);
            buttonsPanel.Controls.Add(addButton, 1, buttonsPanel.RowCount - 1);

            tableLayoutPanel1.Controls.Add(buttonsPanel, 1, tableLayoutPanel1.RowCount - 1);
        }

        private void FieldKeyModification_handler(object sender, EventArgs e)
        {
            var rowIndex = tableLayoutPanel1.GetRow((Control)sender);
            if (rowIndex == 0 || rowIndex == -1)
                return;

            var control = (Control) sender;
            var fieldIndex = _fieldContainer[rowIndex - 1];

            _audit.Fields[fieldIndex].Key = control.Text;
        }
        
        private void FieldValueModification_handler(object sender, EventArgs e)
        {
            var rowIndex = tableLayoutPanel1.GetRow((Control)sender);
            if (rowIndex == 0 || rowIndex == -1)
                return;

            var control = (Control) sender;
            var fieldIndex = _fieldContainer[rowIndex - 1];

            _audit.Fields[fieldIndex].Value = control.Text;
        }
        
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnUpdate?.Invoke();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("+");

            _audit.Fields.Add(new Audit2Field("New key", "New value"));
            UpdateTableLayoutPanel(_audit);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("-");
            
            if (_selectedRowId == -1 || _selectedRowId >= _audit.Fields.Count)
                return;

            _audit.Fields.RemoveAt(_selectedRowId);

            Console.Write(_audit.Fields);
            UpdateTableLayoutPanel(_audit);
        }

        private void selectRow_Click(object sender, EventArgs e)
        {
            _selectedRowId = tableLayoutPanel1.GetPositionFromControl((Control)sender).Row - 1;

            foreach (Control control in tableLayoutPanel1.Controls)
            {
                control.BackColor = Color.White;
            }

            tableLayoutPanel1.GetControlFromPosition(0, _selectedRowId + 1).BackColor = Color.LightBlue;
            tableLayoutPanel1.GetControlFromPosition(1, _selectedRowId + 1).BackColor = Color.LightBlue;
        }
    }
}
