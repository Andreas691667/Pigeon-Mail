namespace Email_System
{
    partial class readMessage
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
            this.fromTb = new System.Windows.Forms.TextBox();
            this.bodyRtb = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // fromTb
            // 
            this.fromTb.Location = new System.Drawing.Point(73, 41);
            this.fromTb.Name = "fromTb";
            this.fromTb.ReadOnly = true;
            this.fromTb.Size = new System.Drawing.Size(548, 27);
            this.fromTb.TabIndex = 0;
            // 
            // bodyRtb
            // 
            this.bodyRtb.Location = new System.Drawing.Point(73, 93);
            this.bodyRtb.Name = "bodyRtb";
            this.bodyRtb.ReadOnly = true;
            this.bodyRtb.Size = new System.Drawing.Size(548, 406);
            this.bodyRtb.TabIndex = 1;
            this.bodyRtb.Text = "";
            // 
            // readMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 698);
            this.Controls.Add(this.bodyRtb);
            this.Controls.Add(this.fromTb);
            this.Name = "readMessage";
            this.Text = "readMessage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox fromTb;
        private RichTextBox bodyRtb;
    }
}