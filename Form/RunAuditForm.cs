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
using SBT.Utils;

namespace SBT.Form
{
    public partial class RunAuditForm : System.Windows.Forms.Form
    {
        private List<Audit2Struct> _audit;

        public RunAuditForm(List<Audit2Struct> audit)
        {
            InitializeComponent();

            if (_audit == null)
                _audit = new List<Audit2Struct>();

            _audit.Clear();
            foreach (var item in audit)
            {
                if (item.IsActive == false)
                    continue;
                
                var newItem = Auditor.Audit(item);
                if (newItem == null)
                    continue;

                _audit.Add(newItem);
            }
            
            UpdateTextWindow(_audit);
        }
        
        private void UpdateTextWindow(List<Audit2Struct> audit)
        {
            AuditReportView.Nodes.Clear();

            for (var i = 0; i < audit.Count; i++)
            {
                if (audit[i].IsItem == false)
                    continue;

                var item = audit[i];

                var auditStatus = item.GetField("audit_status").Value.CustomTrim();
                var name = "[" + auditStatus + "] " + item.GetName();
                var node = AuditReportView.Nodes.Add(name);
                
                UpdateTreeNode(node, item);
            }
        }

        private void UpdateTreeNode(TreeNode node, Audit2Struct audit)
        {
            var font = AuditReportView.Font;
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

        private void huyPoVsemuEbalu()
        {
            AuditReportView.Text = "Xuy na!";
        }
    }
}
