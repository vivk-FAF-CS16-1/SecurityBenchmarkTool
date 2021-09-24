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
        
        public EditItemForm(Audit2Struct audit)
        {
            InitializeComponent();

            _audit = audit;

            _fieldContainer = new List<int>();

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
                keyTextBox.TextChanged += FieldKeyModification_handler;

                valueTextBox.TextChanged -= FieldValueModification_handler;
                valueTextBox.TextChanged += FieldValueModification_handler;
            }
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
    }
}
