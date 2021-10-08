
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
            this.AuditReportView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // AuditReportTextBox
            // 
            this.AuditReportView.BackColor = System.Drawing.Color.White;
            this.AuditReportView.Location = new System.Drawing.Point(12, 12);
            this.AuditReportView.Name = "AuditReportView";
            this.AuditReportView.Size = new System.Drawing.Size(670, 432);
            this.AuditReportView.TabIndex = 0;
            this.AuditReportView.Text = "";
            // 
            // RunAuditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AuditReportView);
            this.Name = "RunAuditForm";
            this.Text = "RunAuditForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView AuditReportView;
    }
}