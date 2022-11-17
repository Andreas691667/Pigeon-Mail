namespace Email_System
{
    partial class newEmail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(newEmail));
            this.sendBt = new System.Windows.Forms.Button();
            this.messageBodyTb = new System.Windows.Forms.RichTextBox();
            this.recipientsTb = new System.Windows.Forms.TextBox();
            this.ccRecipientsTb = new System.Windows.Forms.TextBox();
            this.subjectTb = new System.Windows.Forms.TextBox();
            this.recipientLabel = new System.Windows.Forms.Label();
            this.CcLabel = new System.Windows.Forms.Label();
            this.SubjectLabel = new System.Windows.Forms.Label();
            this.exitBt = new System.Windows.Forms.Button();
            this.draftBt = new System.Windows.Forms.Button();
            this.addAttachmentBt = new System.Windows.Forms.Button();
            this.attachmentsLabel = new System.Windows.Forms.Label();
            this.attachmentsLb = new System.Windows.Forms.ListBox();
            this.removeAttachmentBt = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sendBt
            // 
            this.sendBt.Enabled = false;
            this.sendBt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.sendBt.Image = global::Email_System.Properties.Resources.icons8_email_send_32;
            this.sendBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.sendBt.Location = new System.Drawing.Point(12, 430);
            this.sendBt.Name = "sendBt";
            this.sendBt.Size = new System.Drawing.Size(74, 64);
            this.sendBt.TabIndex = 0;
            this.sendBt.Text = "Send";
            this.sendBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.sendBt.UseVisualStyleBackColor = true;
            this.sendBt.Click += new System.EventHandler(this.sendBt_Click);
            // 
            // messageBodyTb
            // 
            this.messageBodyTb.Location = new System.Drawing.Point(262, 168);
            this.messageBodyTb.Name = "messageBodyTb";
            this.messageBodyTb.Size = new System.Drawing.Size(642, 326);
            this.messageBodyTb.TabIndex = 1;
            this.messageBodyTb.Text = "";
            // 
            // recipientsTb
            // 
            this.recipientsTb.Location = new System.Drawing.Point(499, 29);
            this.recipientsTb.Name = "recipientsTb";
            this.recipientsTb.Size = new System.Drawing.Size(405, 27);
            this.recipientsTb.TabIndex = 2;
            this.recipientsTb.MouseHover += new System.EventHandler(this.recipientsTb_MouseHover);
            this.recipientsTb.Validating += new System.ComponentModel.CancelEventHandler(this.recipientsTb_Validating);
            // 
            // ccRecipientsTb
            // 
            this.ccRecipientsTb.Location = new System.Drawing.Point(499, 69);
            this.ccRecipientsTb.Name = "ccRecipientsTb";
            this.ccRecipientsTb.Size = new System.Drawing.Size(405, 27);
            this.ccRecipientsTb.TabIndex = 3;
            this.ccRecipientsTb.Validating += new System.ComponentModel.CancelEventHandler(this.ccRecipientsTb_Validating);
            // 
            // subjectTb
            // 
            this.subjectTb.Location = new System.Drawing.Point(499, 123);
            this.subjectTb.Name = "subjectTb";
            this.subjectTb.Size = new System.Drawing.Size(405, 27);
            this.subjectTb.TabIndex = 4;
            // 
            // recipientLabel
            // 
            this.recipientLabel.AutoSize = true;
            this.recipientLabel.Location = new System.Drawing.Point(407, 32);
            this.recipientLabel.Name = "recipientLabel";
            this.recipientLabel.Size = new System.Drawing.Size(71, 20);
            this.recipientLabel.TabIndex = 5;
            this.recipientLabel.Text = "Recipient";
            // 
            // CcLabel
            // 
            this.CcLabel.AutoSize = true;
            this.CcLabel.Location = new System.Drawing.Point(450, 76);
            this.CcLabel.Name = "CcLabel";
            this.CcLabel.Size = new System.Drawing.Size(28, 20);
            this.CcLabel.TabIndex = 6;
            this.CcLabel.Text = "Cc.";
            // 
            // SubjectLabel
            // 
            this.SubjectLabel.AutoSize = true;
            this.SubjectLabel.Location = new System.Drawing.Point(420, 130);
            this.SubjectLabel.Name = "SubjectLabel";
            this.SubjectLabel.Size = new System.Drawing.Size(58, 20);
            this.SubjectLabel.TabIndex = 7;
            this.SubjectLabel.Text = "Subject";
            // 
            // exitBt
            // 
            this.exitBt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.exitBt.Image = global::Email_System.Properties.Resources.icons8_close_32;
            this.exitBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.exitBt.Location = new System.Drawing.Point(101, 430);
            this.exitBt.Name = "exitBt";
            this.exitBt.Size = new System.Drawing.Size(78, 64);
            this.exitBt.TabIndex = 9;
            this.exitBt.Text = "Cancel";
            this.exitBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.exitBt.UseVisualStyleBackColor = true;
            this.exitBt.Click += new System.EventHandler(this.exitBt_Click);
            // 
            // draftBt
            // 
            this.draftBt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.draftBt.Image = global::Email_System.Properties.Resources.icons8_draft_32;
            this.draftBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.draftBt.Location = new System.Drawing.Point(12, 363);
            this.draftBt.Name = "draftBt";
            this.draftBt.Size = new System.Drawing.Size(74, 61);
            this.draftBt.TabIndex = 10;
            this.draftBt.Text = "Draft";
            this.draftBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.draftBt.UseVisualStyleBackColor = true;
            this.draftBt.Click += new System.EventHandler(this.draftBt_Click);
            // 
            // addAttachmentBt
            // 
            this.addAttachmentBt.Image = global::Email_System.Properties.Resources.icons8_attach_32;
            this.addAttachmentBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.addAttachmentBt.Location = new System.Drawing.Point(12, 304);
            this.addAttachmentBt.Name = "addAttachmentBt";
            this.addAttachmentBt.Size = new System.Drawing.Size(74, 53);
            this.addAttachmentBt.TabIndex = 11;
            this.addAttachmentBt.Text = "Attach";
            this.addAttachmentBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.addAttachmentBt.UseVisualStyleBackColor = true;
            this.addAttachmentBt.Click += new System.EventHandler(this.addAttachmentBt_Click);
            // 
            // attachmentsLabel
            // 
            this.attachmentsLabel.AutoSize = true;
            this.attachmentsLabel.Location = new System.Drawing.Point(21, 9);
            this.attachmentsLabel.Name = "attachmentsLabel";
            this.attachmentsLabel.Size = new System.Drawing.Size(95, 20);
            this.attachmentsLabel.TabIndex = 13;
            this.attachmentsLabel.Text = "Attachments:";
            this.attachmentsLabel.Visible = false;
            // 
            // attachmentsLb
            // 
            this.attachmentsLb.FormattingEnabled = true;
            this.attachmentsLb.HorizontalScrollbar = true;
            this.attachmentsLb.ItemHeight = 20;
            this.attachmentsLb.Location = new System.Drawing.Point(21, 32);
            this.attachmentsLb.Name = "attachmentsLb";
            this.attachmentsLb.Size = new System.Drawing.Size(188, 104);
            this.attachmentsLb.TabIndex = 14;
            this.attachmentsLb.Visible = false;
            // 
            // removeAttachmentBt
            // 
            this.removeAttachmentBt.Image = global::Email_System.Properties.Resources.icons8_file_delete_32;
            this.removeAttachmentBt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeAttachmentBt.Location = new System.Drawing.Point(20, 147);
            this.removeAttachmentBt.Name = "removeAttachmentBt";
            this.removeAttachmentBt.Size = new System.Drawing.Size(111, 45);
            this.removeAttachmentBt.TabIndex = 15;
            this.removeAttachmentBt.Text = "Remove";
            this.removeAttachmentBt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeAttachmentBt.UseVisualStyleBackColor = true;
            this.removeAttachmentBt.Visible = false;
            this.removeAttachmentBt.Click += new System.EventHandler(this.removeAttachmentBt_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(262, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Message:";
            this.label4.Visible = false;
            // 
            // newEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(916, 506);
            this.Controls.Add(this.removeAttachmentBt);
            this.Controls.Add(this.attachmentsLb);
            this.Controls.Add(this.attachmentsLabel);
            this.Controls.Add(this.addAttachmentBt);
            this.Controls.Add(this.draftBt);
            this.Controls.Add(this.exitBt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SubjectLabel);
            this.Controls.Add(this.CcLabel);
            this.Controls.Add(this.recipientLabel);
            this.Controls.Add(this.subjectTb);
            this.Controls.Add(this.ccRecipientsTb);
            this.Controls.Add(this.recipientsTb);
            this.Controls.Add(this.messageBodyTb);
            this.Controls.Add(this.sendBt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(934, 553);
            this.Name = "newEmail";
            this.Text = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.newEmail_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button sendBt;
        private RichTextBox messageBodyTb;
        private TextBox recipientsTb;
        private TextBox ccRecipientsTb;
        private TextBox subjectTb;
        private Label recipientLabel;
        private Label CcLabel;
        private Label SubjectLabel;
        private Button exitBt;
        private Button draftBt;
        private Button addAttachmentBt;
        private Label attachmentsLabel;
        private ListBox attachmentsLb;
        private Button removeAttachmentBt;
        private Label label4;
    }
}