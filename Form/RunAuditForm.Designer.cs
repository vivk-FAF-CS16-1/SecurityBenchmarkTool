
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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AuditReportView
            // 
            this.AuditReportView.BackColor = System.Drawing.Color.White;
            this.AuditReportView.Location = new System.Drawing.Point(12, 12);
            this.AuditReportView.Name = "AuditReportView";
            this.AuditReportView.Size = new System.Drawing.Size(670, 432);
            this.AuditReportView.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(688, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Enforce";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RunAuditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.AuditReportView);
            this.Name = "RunAuditForm";
            this.Text = "RunAuditForm";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button button1;

        #endregion

        private System.Windows.Forms.TreeView AuditReportView;
    }
}