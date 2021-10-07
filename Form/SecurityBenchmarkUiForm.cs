using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SBT.Audit;
using SBT.DataBase;

namespace SBT.Form
{
    public partial class SecurityBenchmarkUiForm : System.Windows.Forms.Form
    {
        private FindItemForm _findItemForm;

        private Dictionary<Control, Guid> _controlGuids;
        private Dictionary<TreeNode, Audit2Struct> _treeNodeDict;

        private JSONSaver _jsonSaver;
        private List<DBItem> _container;

        private Control _currentItemControl;
        private TreeNode _currentTreeNode;
        private List<Audit2Struct> _currentAudit;
        
        private bool _ignoreTextChanged;

        



        public SecurityBenchmarkUiForm()
        {
            InitializeComponent();

            if (_controlGuids == null)
            {
                _controlGuids = new Dictionary<Control, Guid>();
            }

            if (_treeNodeDict == null)
            {
                _treeNodeDict = new Dictionary<TreeNode, Audit2Struct>();
            }

            _currentItemControl = null;
            _currentTreeNode = null;
            _currentAudit = null;
            
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
            
            richTextBox1.NodeMouseClick += richTextBox1_NodeMouseClick;
        }


        private void UpdateTableLayoutPanel(List<DBItem> container)
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Unique Name" }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Source Path" }, 1, 0);
            
            _controlGuids.Clear();

            foreach (var item in container)
            {
                tableLayoutPanel1.RowCount += 1;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
                
                var nameTextBox = new TextBox();
                nameTextBox.Text = item.Name;
                nameTextBox.ReadOnly = true;
                nameTextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                
                _controlGuids.Add(nameTextBox, item.GUID);

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
            _treeNodeDict.Clear();

            for (var i = 0; i < audit.Count; i++)
            {
                if (audit[i].IsItem == false)
                    continue;
                
                var item = audit[i];

                var name = item.GetName();
                var node = richTextBox1.Nodes.Add(name);
                
                _treeNodeDict.Add(node, item);
                UpdateTreeNode(node, item);
            }
        }

        private void UpdateTreeNode(TreeNode node, Audit2Struct audit)
        {
            var font = richTextBox1.Font;
            if (audit.IsActive == false)
            {
                font = new Font(font, FontStyle.Strikeout);
            }

            node.NodeFont = font;
            
            var name = audit.GetName();
            node.Name = name;
            
            node.Nodes.Clear();
            for (var index = 0; index < audit.Fields.Count; index++)
            {
                var field = audit.Fields[index];
                var key = field.Key;
                var value = field.Value;
                    
                var keyNode = node.Nodes.Add(key, key);
                var indexOfFirst = node.Nodes.IndexOf(keyNode);
                node.Nodes[indexOfFirst].Nodes.Add(value, value);
            }
        }


        void richTextBox1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            richTextBox1.SelectedNode = e.Node;

            if (e.Node.Level != 0)
                return;   

            _currentTreeNode = e.Node;

            var contextMenuStrip = new ContextMenuStrip();

            contextMenuStrip.Items.Add("Edit", null, EditItemContextMenuStrip_Click);

            var auditItem = _treeNodeDict[_currentTreeNode];
            var changeStateText = !auditItem.IsActive 
                ? "Activate" 
                : "Deactivate";
            contextMenuStrip.Items.Add(changeStateText, null, ChangeStateItemContextMenuStrip_Click);

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
            var node = _currentTreeNode;
            var audit = _treeNodeDict[node];
            
            var editItemForm = new EditItemForm(audit);
            editItemForm.Show();

            var onUpdate = new Action(() =>
            {
                UpdateTreeNode(node, audit);
            });
            editItemForm.OnUpdate = onUpdate;
        }
        
        private void ChangeStateItemContextMenuStrip_Click(object sender, EventArgs e)
        {
            var auditItem = _treeNodeDict[_currentTreeNode];
            auditItem.IsActive = !auditItem.IsActive;
            
            UpdateTreeNode(_currentTreeNode, auditItem);
        }


        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            var rowIndex = tableLayoutPanel1.GetRow((Control)sender);
            if (rowIndex == 0 || rowIndex == -1)
                return;
            
            var rowControl = tableLayoutPanel1.GetControlFromPosition(0, rowIndex);

            if (_controlGuids.ContainsKey(rowControl) == false)
                return;

            var guid = _controlGuids[rowControl];

            _currentItemControl = rowControl; 

            var dbItem = _container.Find(item => item.GUID == guid);

            _currentAudit = dbItem.Audit;
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

            var guid = _controlGuids[_currentItemControl];
            var dbItem = _container.Find(item => item.GUID == guid);
            if (dbItem == null)
                return;

            var content = AuditWriter.ToString(dbItem.Audit);
            Export(content);
            
            // HINT for simple Export: Export(content, dbItem.SourcePath);
        }

        private void runAuditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentAudit == null)
                return;
            var runAuditForm = new RunAuditForm(_currentAudit);
            runAuditForm.Show();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_findItemForm == null || _findItemForm.IsDisposed)
                _findItemForm = new FindItemForm();

            

            _findItemForm.Show();
            _findItemForm.Focus();
        }

        public void FindItemsByName(string itemName)
        {
            if (_currentAudit == null)
                return;

            var copyCurrentAudit = new List<Audit2Struct>(_currentAudit);

            int i = 0;
            while (i < copyCurrentAudit.Count)
            {
                if (!copyCurrentAudit[i].GetName().ToUpper().Contains(itemName.ToUpper()))
                {
                    copyCurrentAudit.Remove(copyCurrentAudit[i]);
                    continue;
                }
                i++;
            }

            UpdateTextWindow(copyCurrentAudit);
        }

        private void activateAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var node in _treeNodeDict.Keys)
            {
                _treeNodeDict[node].IsActive = true;
                UpdateTreeNode(node, _treeNodeDict[node]);
            }
        }

        private void deactivateAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var node in _treeNodeDict.Keys)
            {
                _treeNodeDict[node].IsActive = false;
                UpdateTreeNode(node, _treeNodeDict[node]);
            }
        }
    }
}
