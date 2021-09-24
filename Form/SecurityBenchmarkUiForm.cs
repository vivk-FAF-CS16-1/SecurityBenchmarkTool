using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SBT.Audit;
using SBT.DataBase;

namespace SBT.Form
{
    public partial class SecurityBenchmarkUiForm : System.Windows.Forms.Form
    {
        private Dictionary<Control, Guid> _guids;

        private JSONSaver _jsonSaver;
        private List<DBItem> _container;

        private Control _currentItemControl;
        private bool _ignoreTextChanged;

        public SecurityBenchmarkUiForm()
        {
            InitializeComponent();

            if (_guids == null)
            {
                _guids = new Dictionary<Control, Guid>();
            }

            _currentItemControl = null;
            _ignoreTextChanged = false;

            _jsonSaver = JSONSaver.Instance;
            if (_jsonSaver.IsLoaded == false)
            {
                _jsonSaver.Load();
            }

            _container = _jsonSaver.Container;
            
            UpdateTableLayoutPanel(_container);
            
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                c.MouseClick += tableLayoutPanel1_Click;
            }
        }


        private void UpdateTableLayoutPanel(List<DBItem> container)
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Unique Name" }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Source Path" }, 1, 0);
            
            _guids.Clear();

            foreach (var item in container)
            {
                tableLayoutPanel1.RowCount += 1;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
                
                var nameTextBox = new TextBox();
                nameTextBox.Text = item.Name;
                nameTextBox.ReadOnly = true;
                nameTextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                
                _guids.Add(nameTextBox, item.GUID);

                var sourcePathTextBox = new TextBox();
                sourcePathTextBox.Text = item.SourcePath;
                sourcePathTextBox.ReadOnly = true;
                sourcePathTextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

                tableLayoutPanel1.Controls.Add(nameTextBox, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(sourcePathTextBox, 1, tableLayoutPanel1.RowCount - 1);

                nameTextBox.Click -= tableLayoutPanel1_Click;
                nameTextBox.Click += tableLayoutPanel1_Click;
            }
        }

        private void UpdateTableLayoutPanel()
        {
            UpdateTableLayoutPanel(_container);
        }

        private bool Save_handler()
        {
            if (_jsonSaver == null)
                return false;
            
            if (_jsonSaver.IsLoaded == false)
                return false;
            
            if (_container == null)
                return false;
            
            _jsonSaver.Save();
            return true;
        }

        private void UpdateTextWindow(List<Audit2Struct> audit)
        {
            richTextBox1.Nodes.Clear();

            for (var i = 0; i < audit.Count; i++)
            {
                if (audit[i].IsItem == false)
                    continue;
                
                var item = audit[i];

                var name = item.GetName();
                var node = richTextBox1.Nodes.Add(name, name);

                for (var j = 0; j < item.Fields.Count; j++)
                {
                    var field = item.Fields[j];
                    var key = field.Key;
                    var value = field.Value;
                    
                    var keyNode = node.Nodes.Add(key, key);
                    var indexOfFirst = node.Nodes.IndexOf(keyNode);
                    node.Nodes[indexOfFirst].Nodes.Add(value, value);
                }
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
            var importForm = new ImportForm();
            importForm.Show();
            importForm.Completed = UpdateTableLayoutPanel;
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            var rowIndex = tableLayoutPanel1.GetRow((Control)sender);
            if (rowIndex == 0 || rowIndex == -1)
                return;
            
            var rowControl = tableLayoutPanel1.GetControlFromPosition(0, rowIndex);

            if (_guids.ContainsKey(rowControl) == false)
                return;

            var guid = _guids[rowControl];

            _currentItemControl = rowControl;

            var dbItem = _container.Find(item => item.GUID == guid);

            UpdateTextWindow(dbItem.Audit);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save_handler();
        }

        private void exportAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentItemControl == null)
                return;

            var guid = _guids[_currentItemControl];
            var dbItem = _container.Find(item => item.GUID == guid);
            if (dbItem == null)
                return;

            var content = AuditWriter.ToString(dbItem.Audit);
            Export(content);
            
            // HINT for simple Export: Export(content, dbItem.SourcePath);
        }
    }
}
