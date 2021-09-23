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

        private List<Audit2Struct> _containerOfItems;

        private Control _currentItemControl;
        private bool _ignoreTextChanged;

        



        public SecurityBenchmarkUiForm()
        {
            InitializeComponent();



            if (_guids == null)
            {
                _guids = new Dictionary<Control, Guid>();
            }

            if (_containerOfItems == null)
            {
                _containerOfItems = new List<Audit2Struct>();
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

        private void UpdateTextWindow(List<Audit2Struct> container)
        {
            richTextBox1.Nodes.Clear();

            for (var i = 0; i < container.Count; i++)
            {
                if (container[i].IsItem == false)
                    continue;
                
                var item = container[i];

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

                _containerOfItems.Add(item);
            }
        }

        void richTextBox1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Button != MouseButtons.Right)
                return;

            richTextBox1.SelectedNode = e.Node;

            Console.WriteLine(e.Node.Level + " " + e.Node.Index);

            if (e.Node.Level != 0)
            {
                return;   
            }

            var index = e.Node.Index;

            if (_containerOfItems.Count <= index)
                return;



            var contextMenuStrip = new ContextMenuStrip();

            contextMenuStrip.Items.Add("Edit", null, EditItemContextMenuStrip_Click);

            if (_containerOfItems[index].IsActive)
            {
                contextMenuStrip.Items.Add("Deactivate", null, null);
                e.Node.NodeFont = new System.Drawing.Font(richTextBox1.Font, System.Drawing.FontStyle.Strikeout);
                
            }
            else
            {
                contextMenuStrip.Items.Add("Activate", null, null);
                e.Node.NodeFont = richTextBox1.Font;
            }

            e.Node.ContextMenuStrip = contextMenuStrip;
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

        private void EditItemContextMenuStrip_Click(object sender, EventArgs e)
        {
            var editItemForm = new EditItemForm();
            editItemForm.Show();
            //editItemForm.Completed = UpdateSmthng; //todo: to implement the idea of this action in form EditItemForm
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

            var container = _container.Find(item => item.GUID == guid);

            UpdateTextWindow(container.Audit);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save_handler();
        }
    }
}
