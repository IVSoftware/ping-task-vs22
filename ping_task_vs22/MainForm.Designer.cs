namespace ping_task_vs22
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxUri = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelCountNothingForNoReason = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxUri
            // 
            this.textBoxUri.Location = new System.Drawing.Point(47, 65);
            this.textBoxUri.Name = "textBoxUri";
            this.textBoxUri.Size = new System.Drawing.Size(355, 31);
            this.textBoxUri.TabIndex = 0;
            this.textBoxUri.Text = "https://www.google.com";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(47, 122);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(60, 25);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Status";
            // 
            // labelCountNothingForNoReason
            // 
            this.labelCountNothingForNoReason.AutoSize = true;
            this.labelCountNothingForNoReason.Location = new System.Drawing.Point(47, 170);
            this.labelCountNothingForNoReason.Name = "labelCountNothingForNoReason";
            this.labelCountNothingForNoReason.Size = new System.Drawing.Size(239, 25);
            this.labelCountNothingForNoReason.TabIndex = 2;
            this.labelCountNothingForNoReason.Text = "Count nothing for no reason";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 300);
            this.Controls.Add(this.labelCountNothingForNoReason);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.textBoxUri);
            this.Name = "MainForm";
            this.Text = "Main Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBoxUri;
        private Label labelStatus;
        private Label labelCountNothingForNoReason;
    }
}