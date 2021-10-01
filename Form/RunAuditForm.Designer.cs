
namespace SBT.Form
{
    partial class RunAuditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AuditReportTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // AuditReportTextBox
            // 
            this.AuditReportTextBox.BackColor = System.Drawing.Color.White;
            this.AuditReportTextBox.Location = new System.Drawing.Point(12, 12);
            this.AuditReportTextBox.Name = "AuditReportTextBox";
            this.AuditReportTextBox.Size = new System.Drawing.Size(670, 432);
            this.AuditReportTextBox.TabIndex = 0;
            this.AuditReportTextBox.Text = "";
            // 
            // RunAuditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AuditReportTextBox);
            this.Name = "RunAuditForm";
            this.Text = "RunAuditForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox AuditReportTextBox;
    }
}