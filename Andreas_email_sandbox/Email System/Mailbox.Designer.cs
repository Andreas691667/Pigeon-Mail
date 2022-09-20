namespace Email_System
{
    partial class Mailbox
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
            this.folderLb = new System.Windows.Forms.ListBox();
            this.messageLb = new System.Windows.Forms.ListBox();
            this.newEmailBt = new System.Windows.Forms.Button();
            this.messageIdLb = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // folderLb
            // 
            this.folderLb.FormattingEnabled = true;
            this.folderLb.ItemHeight = 20;
            this.folderLb.Location = new System.Drawing.Point(12, 53);
            this.folderLb.Name = "folderLb";
            this.folderLb.Size = new System.Drawing.Size(203, 424);
            this.folderLb.TabIndex = 0;
            this.folderLb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RetrieveMessages);
            // 
            // messageLb
            // 
            this.messageLb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageLb.FormattingEnabled = true;
            this.messageLb.ItemHeight = 20;
            this.messageLb.Location = new System.Drawing.Point(221, 53);
            this.messageLb.Name = "messageLb";
            this.messageLb.Size = new System.Drawing.Size(402, 424);
            this.messageLb.TabIndex = 1;
            this.messageLb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ReadMessage);
            // 
            // newEmailBt
            // 
            this.newEmailBt.Location = new System.Drawing.Point(12, 498);
            this.newEmailBt.Name = "newEmailBt";
            this.newEmailBt.Size = new System.Drawing.Size(176, 29);
            this.newEmailBt.TabIndex = 2;
            this.newEmailBt.Text = "Create new e-mail";
            this.newEmailBt.UseVisualStyleBackColor = true;
            this.newEmailBt.Click += new System.EventHandler(this.newEmailBt_Click);
            // 
            // messageIdLb
            // 
            this.messageIdLb.FormattingEnabled = true;
            this.messageIdLb.ItemHeight = 20;
            this.messageIdLb.Location = new System.Drawing.Point(839, 171);
            this.messageIdLb.Name = "messageIdLb";
            this.messageIdLb.Size = new System.Drawing.Size(150, 104);
            this.messageIdLb.TabIndex = 3;
            this.messageIdLb.Visible = false;
            // 
            // Mailbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 775);
            this.Controls.Add(this.messageIdLb);
            this.Controls.Add(this.newEmailBt);
            this.Controls.Add(this.messageLb);
            this.Controls.Add(this.folderLb);
            this.Name = "Mailbox";
            this.Text = "Mailbox";
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox folderLb;
        private ListBox messageLb;
        private Button newEmailBt;
        private ListBox messageIdLb;
    }
}