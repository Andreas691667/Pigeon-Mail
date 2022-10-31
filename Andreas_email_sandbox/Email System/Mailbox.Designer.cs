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
            this.moveToTrashBt = new System.Windows.Forms.Button();
            this.deleteBt = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logoutBt = new System.Windows.Forms.Button();
            this.settingsBt = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderLb
            // 
            this.folderLb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.folderLb.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.folderLb.FormattingEnabled = true;
            this.folderLb.ItemHeight = 28;
            this.folderLb.Location = new System.Drawing.Point(15, 89);
            this.folderLb.Name = "folderLb";
            this.folderLb.Size = new System.Drawing.Size(237, 480);
            this.folderLb.TabIndex = 0;
            this.folderLb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RetrieveMessages);
            // 
            // messageLb
            // 
            this.messageLb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageLb.BackColor = System.Drawing.SystemColors.Window;
            this.messageLb.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.messageLb.FormattingEnabled = true;
            this.messageLb.ItemHeight = 25;
            this.messageLb.Location = new System.Drawing.Point(255, 89);
            this.messageLb.Name = "messageLb";
            this.messageLb.Size = new System.Drawing.Size(752, 479);
            this.messageLb.TabIndex = 1;
            this.messageLb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ReadMessage);
            // 
            // newEmailBt
            // 
            this.newEmailBt.Image = global::Email_System.Properties.Resources.icons8_secured_letter_32;
            this.newEmailBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.newEmailBt.Location = new System.Drawing.Point(3, 0);
            this.newEmailBt.Name = "newEmailBt";
            this.newEmailBt.Size = new System.Drawing.Size(95, 64);
            this.newEmailBt.TabIndex = 2;
            this.newEmailBt.Text = "New e-mail";
            this.newEmailBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.newEmailBt.UseVisualStyleBackColor = true;
            this.newEmailBt.Click += new System.EventHandler(this.newEmailBt_Click);
            // 
            // refreshBt
            // 
            this.refreshBt.Image = global::Email_System.Properties.Resources.refresh;
            this.refreshBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.refreshBt.Location = new System.Drawing.Point(106, 0);
            this.refreshBt.Name = "refreshBt";
            this.refreshBt.Size = new System.Drawing.Size(70, 64);
            this.refreshBt.TabIndex = 3;
            this.refreshBt.Text = "Refresh";
            this.refreshBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.refreshBt.UseVisualStyleBackColor = true;
            this.refreshBt.Click += new System.EventHandler(this.refreshBt_Click);
            // 
            // refreshTimer
            // 
            this.refreshTimer.Enabled = true;
            this.refreshTimer.Interval = 60000;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // addFlagBt
            // 
            this.addFlagBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addFlagBt.Image = global::Email_System.Properties.Resources.icons8_flag_32;
            this.addFlagBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.addFlagBt.Location = new System.Drawing.Point(556, 0);
            this.addFlagBt.Name = "addFlagBt";
            this.addFlagBt.Size = new System.Drawing.Size(105, 64);
            this.addFlagBt.TabIndex = 4;
            this.addFlagBt.Text = "Flag/unflag";
            this.addFlagBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.addFlagBt.UseVisualStyleBackColor = true;
            this.addFlagBt.Visible = false;
            this.addFlagBt.Click += new System.EventHandler(this.addFlagBt_Click);
            // 
            // removeFlagBt
            // 
            this.removeFlagBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeFlagBt.Location = new System.Drawing.Point(445, 3);
            this.removeFlagBt.Name = "removeFlagBt";
            this.removeFlagBt.Size = new System.Drawing.Size(105, 55);
            this.removeFlagBt.TabIndex = 5;
            this.removeFlagBt.Text = "Remove flag";
            this.removeFlagBt.UseVisualStyleBackColor = true;
            this.removeFlagBt.Visible = false;
            this.removeFlagBt.Click += new System.EventHandler(this.removeFlagBt_Click);
            // 
            // moveToTrashBt
            // 
            this.moveToTrashBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveToTrashBt.Image = global::Email_System.Properties.Resources.icons8_trash_32;
            this.moveToTrashBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.moveToTrashBt.Location = new System.Drawing.Point(749, 0);
            this.moveToTrashBt.Name = "moveToTrashBt";
            this.moveToTrashBt.Size = new System.Drawing.Size(56, 64);
            this.moveToTrashBt.TabIndex = 6;
            this.moveToTrashBt.Text = "Trash";
            this.moveToTrashBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.moveToTrashBt.UseVisualStyleBackColor = true;
            this.moveToTrashBt.Visible = false;
            this.moveToTrashBt.Click += new System.EventHandler(this.moveToTrashBt_Click);
            // 
            // deleteBt
            // 
            this.deleteBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteBt.Image = global::Email_System.Properties.Resources.icons8_remove_32;
            this.deleteBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.deleteBt.Location = new System.Drawing.Point(667, 0);
            this.deleteBt.Name = "deleteBt";
            this.deleteBt.Size = new System.Drawing.Size(76, 64);
            this.deleteBt.TabIndex = 7;
            this.deleteBt.Text = "Delete";
            this.deleteBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.deleteBt.UseVisualStyleBackColor = true;
            this.deleteBt.Visible = false;
            this.deleteBt.Click += new System.EventHandler(this.deleteBt_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.logoutBt);
            this.panel1.Controls.Add(this.settingsBt);
            this.panel1.Controls.Add(this.refreshBt);
            this.panel1.Controls.Add(this.removeFlagBt);
            this.panel1.Controls.Add(this.deleteBt);
            this.panel1.Controls.Add(this.addFlagBt);
            this.panel1.Controls.Add(this.newEmailBt);
            this.panel1.Controls.Add(this.moveToTrashBt);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(992, 64);
            this.panel1.TabIndex = 8;
            // 
            // logoutBt
            // 
            this.logoutBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logoutBt.Image = global::Email_System.Properties.Resources.icons8_logout_32;
            this.logoutBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.logoutBt.Location = new System.Drawing.Point(908, 0);
            this.logoutBt.Name = "logoutBt";
            this.logoutBt.Size = new System.Drawing.Size(81, 64);
            this.logoutBt.TabIndex = 9;
            this.logoutBt.Text = "Sign out";
            this.logoutBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.logoutBt.UseVisualStyleBackColor = true;
            this.logoutBt.Click += new System.EventHandler(this.logoutBt_Click);
            // 
            // settingsBt
            // 
            this.settingsBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsBt.Image = global::Email_System.Properties.Resources.icons8_settings_32;
            this.settingsBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.settingsBt.Location = new System.Drawing.Point(811, 0);
            this.settingsBt.Name = "settingsBt";
            this.settingsBt.Size = new System.Drawing.Size(91, 64);
            this.settingsBt.TabIndex = 8;
            this.settingsBt.Text = "Settings";
            this.settingsBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.settingsBt.UseVisualStyleBackColor = true;
            // 
            // Mailbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 588);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.messageLb);
            this.Controls.Add(this.folderLb);
            this.Name = "Mailbox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Mailbox";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Mailbox_FormClosed);
            this.panel1.ResumeLayout(false);
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
        private Button moveToTrashBt;
        private Button deleteBt;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Button settingsBt;
        private Button logoutBt;
    }
}