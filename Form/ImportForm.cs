using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using SBT.Audit;
using SBT.DataBase;

namespace SBT.Form
{
    public partial class ImportForm : System.Windows.Forms.Form
    {
        #region Public fields

        public Action Completed = null;
        
        #endregion
        
        #region Private feilds
        
        private JSONSaver _jsonSaver;
        private List<DBItem> _container;
        
        #endregion
        
        public ImportForm()
        {
            InitializeComponent();
            // TODO: Add filter only for audit file
            
            _jsonSaver = JSONSaver.Instance;
            if (_jsonSaver.IsLoaded == false)
            {
                _jsonSaver.Load();
            }

            _container = _jsonSaver.Container;
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
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                label3.Enabled = true;
                label4.Enabled = false;
                return ;
            }

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                label3.Enabled = false;
                label4.Enabled = true;
                return ;
            }

            var name = textBox1.Text;
            var sourcePath = textBox2.Text;

            using (var sr = new StreamReader(@sourcePath))
            {
                var sourceContent = sr.ReadToEnd();
                var (err, parseData) = AuditParser.Parse(sourceContent);
                var (err2, parseTest) = AuditParser.ParseV2(sourceContent);
                var test = AuditParser.ParseItems(parseData);
                var testJson = JsonConvert.SerializeObject(test);
                var testDeserialize = JsonConvert.DeserializeObject<List<AuditItem>>(testJson);
                var content = err ?? AuditWriter.ToString(parseData);
                var newItem = new DBItem
                {
                    GUID = Guid.NewGuid(),
                    Name = name,
                    SourcePath = sourcePath,
                    
                    Audit = new List<Audit2Struct>(parseTest)
                };

                _container.Add(newItem);

                _jsonSaver.Save();
            }

            label3.Enabled = false; 
            label4.Enabled = false;
            
            Completed?.Invoke();
            Completed = null;
            
            Close();
        }
    }
}
