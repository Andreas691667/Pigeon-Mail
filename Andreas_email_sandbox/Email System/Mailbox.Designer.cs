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
            this.components = new System.ComponentModel.Container();
            this.folderLb = new System.Windows.Forms.ListBox();
            this.messageLb = new System.Windows.Forms.ListBox();
            this.newEmailBt = new System.Windows.Forms.Button();
            this.refreshBt = new System.Windows.Forms.Button();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.addFlagBt = new System.Windows.Forms.Button();
            this.removeFlagBt = new System.Windows.Forms.Button();
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
            this.messageLb.BackColor = System.Drawing.SystemColors.Window;
            this.messageLb.FormattingEnabled = true;
            this.messageLb.ItemHeight = 20;
            this.messageLb.Location = new System.Drawing.Point(221, 53);
            this.messageLb.Name = "messageLb";
            this.messageLb.Size = new System.Drawing.Size(670, 424);
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
            // refreshBt
            // 
            this.refreshBt.Location = new System.Drawing.Point(29, 20);
            this.refreshBt.Name = "refreshBt";
            this.refreshBt.Size = new System.Drawing.Size(94, 29);
            this.refreshBt.TabIndex = 3;
            this.refreshBt.Text = "Refresh";
            this.refreshBt.UseVisualStyleBackColor = true;
            this.refreshBt.Click += new System.EventHandler(this.refreshBt_Click);
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 10000;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // addFlagBt
            // 
            this.addFlagBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addFlagBt.Location = new System.Drawing.Point(1049, 53);
            this.addFlagBt.Name = "addFlagBt";
            this.addFlagBt.Size = new System.Drawing.Size(138, 29);
            this.addFlagBt.TabIndex = 4;
            this.addFlagBt.Text = "Add flag";
            this.addFlagBt.UseVisualStyleBackColor = true;
            this.addFlagBt.Visible = false;
            this.addFlagBt.Click += new System.EventHandler(this.addFlagBt_Click);
            // 
            // removeFlagBt
            // 
            this.removeFlagBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeFlagBt.Location = new System.Drawing.Point(1049, 88);
            this.removeFlagBt.Name = "removeFlagBt";
            this.removeFlagBt.Size = new System.Drawing.Size(138, 29);
            this.removeFlagBt.TabIndex = 5;
            this.removeFlagBt.Text = "Remove flag";
            this.removeFlagBt.UseVisualStyleBackColor = true;
            this.removeFlagBt.Visible = false;
            this.removeFlagBt.Click += new System.EventHandler(this.removeFlagBt_Click);
            // 
            // Mailbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 775);
            this.Controls.Add(this.removeFlagBt);
            this.Controls.Add(this.addFlagBt);
            this.Controls.Add(this.refreshBt);
            this.Controls.Add(this.newEmailBt);
            this.Controls.Add(this.messageLb);
            this.Controls.Add(this.folderLb);
            this.Name = "Mailbox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Mailbox";
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox folderLb;
        private ListBox messageLb;
        private Button newEmailBt;
        private Button refreshBt;
        private System.Windows.Forms.Timer refreshTimer;
        private Button addFlagBt;
        private Button removeFlagBt;
    }
}