using System.IO;
using System.Windows.Forms;

namespace SBT.Form
{
    partial class SecurityBenchmarkUiForm
    {
        private void Export(string content)
        {
            var sdf = new SaveFileDialog();
            
            sdf.Filter = "Audit files (*.audit)|*.audit";
            sdf.FilterIndex = 2;

            if (sdf.ShowDialog() == DialogResult.OK)
            {
                Export(content, sdf.FileName);
            }
        }

        private void Export(string content, string filePath)
        {
            File.WriteAllText(filePath, content);
        }
    }
}