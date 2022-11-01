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
            this.components = new System.ComponentModel.Container();
            this.fromTb = new System.Windows.Forms.TextBox();
            this.bodyRtb = new System.Windows.Forms.RichTextBox();
            this.replyBt = new System.Windows.Forms.Button();
            this.replyAllBt = new System.Windows.Forms.Button();
            this.forwardBt = new System.Windows.Forms.Button();
            this.subjectTb = new System.Windows.Forms.TextBox();
            this.closeBt = new System.Windows.Forms.Button();
            this.deleteMessageBt = new System.Windows.Forms.Button();
            this.moveToTrashBT = new System.Windows.Forms.Button();
            this.attachmentsLb = new System.Windows.Forms.ListBox();
            this.downloadAttachmentBt = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.attachmentsPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toTb = new System.Windows.Forms.TextBox();
            this.dateTb = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.attachmentsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // fromTb
            // 
            this.fromTb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fromTb.ForeColor = System.Drawing.SystemColors.Highlight;
            this.fromTb.Location = new System.Drawing.Point(12, 83);
            this.fromTb.Name = "fromTb";
            this.fromTb.ReadOnly = true;
            this.fromTb.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.fromTb.Size = new System.Drawing.Size(827, 27);
            this.fromTb.TabIndex = 1;
            this.fromTb.TabStop = false;
            this.fromTb.Text = "From: ";
            // 
            // bodyRtb
            // 
            this.bodyRtb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bodyRtb.Location = new System.Drawing.Point(12, 147);
            this.bodyRtb.Name = "bodyRtb";
            this.bodyRtb.ReadOnly = true;
            this.bodyRtb.Size = new System.Drawing.Size(1065, 384);
            this.bodyRtb.TabIndex = 2;
            this.bodyRtb.TabStop = false;
            this.bodyRtb.Text = "";
            // 
            // replyBt
            // 
            this.replyBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.replyBt.Image = global::Email_System.Properties.Resources.icons8_reply_32;
            this.replyBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.replyBt.Location = new System.Drawing.Point(20, 26);
            this.replyBt.Name = "replyBt";
            this.replyBt.Size = new System.Drawing.Size(94, 58);
            this.replyBt.TabIndex = 0;
            this.replyBt.Text = "Reply";
            this.replyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.replyBt.UseVisualStyleBackColor = true;
            this.replyBt.Click += new System.EventHandler(this.replyBt_Click);
            // 
            // replyAllBt
            // 
            this.replyAllBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.replyAllBt.Image = global::Email_System.Properties.Resources.icons8_reply_all_32;
            this.replyAllBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.replyAllBt.Location = new System.Drawing.Point(120, 26);
            this.replyAllBt.Name = "replyAllBt";
            this.replyAllBt.Size = new System.Drawing.Size(94, 58);
            this.replyAllBt.TabIndex = 1;
            this.replyAllBt.Text = "Reply all";
            this.replyAllBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.replyAllBt.UseVisualStyleBackColor = true;
            this.replyAllBt.Click += new System.EventHandler(this.replyAllBt_Click);
            // 
            // forwardBt
            // 
            this.forwardBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.forwardBt.Image = global::Email_System.Properties.Resources.icons8_forward_message_32;
            this.forwardBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.forwardBt.Location = new System.Drawing.Point(220, 26);
            this.forwardBt.Name = "forwardBt";
            this.forwardBt.Size = new System.Drawing.Size(94, 58);
            this.forwardBt.TabIndex = 2;
            this.forwardBt.Text = "Forward";
            this.forwardBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.forwardBt.UseVisualStyleBackColor = true;
            this.forwardBt.Click += new System.EventHandler(this.forwardBt_Click);
            // 
            // subjectTb
            // 
            this.subjectTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subjectTb.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.subjectTb.Location = new System.Drawing.Point(12, 26);
            this.subjectTb.Name = "subjectTb";
            this.subjectTb.ReadOnly = true;
            this.subjectTb.Size = new System.Drawing.Size(1065, 51);
            this.subjectTb.TabIndex = 0;
            this.subjectTb.TabStop = false;
            // 
            // closeBt
            // 
            this.closeBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBt.Image = global::Email_System.Properties.Resources.icons8_close_32;
            this.closeBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.closeBt.Location = new System.Drawing.Point(518, 26);
            this.closeBt.Name = "closeBt";
            this.closeBt.Size = new System.Drawing.Size(94, 58);
            this.closeBt.TabIndex = 5;
            this.closeBt.Text = "Close";
            this.closeBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.closeBt.UseVisualStyleBackColor = true;
            this.closeBt.Click += new System.EventHandler(this.closeBt_Click);
            // 
            // deleteMessageBt
            // 
            this.deleteMessageBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteMessageBt.Image = global::Email_System.Properties.Resources.icons8_remove_32;
            this.deleteMessageBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.deleteMessageBt.Location = new System.Drawing.Point(419, 26);
            this.deleteMessageBt.Name = "deleteMessageBt";
            this.deleteMessageBt.Size = new System.Drawing.Size(93, 58);
            this.deleteMessageBt.TabIndex = 4;
            this.deleteMessageBt.Text = "Delete";
            this.deleteMessageBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.deleteMessageBt.UseVisualStyleBackColor = true;
            this.deleteMessageBt.Click += new System.EventHandler(this.deleteMessageBt_Click);
            // 
            // moveToTrashBT
            // 
            this.moveToTrashBT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.moveToTrashBT.Image = global::Email_System.Properties.Resources.icons8_trash_32;
            this.moveToTrashBT.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.moveToTrashBT.Location = new System.Drawing.Point(319, 26);
            this.moveToTrashBT.Name = "moveToTrashBT";
            this.moveToTrashBT.Size = new System.Drawing.Size(94, 58);
            this.moveToTrashBT.TabIndex = 3;
            this.moveToTrashBT.Text = "Trash";
            this.moveToTrashBT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.moveToTrashBT.UseVisualStyleBackColor = true;
            this.moveToTrashBT.Click += new System.EventHandler(this.moveToTrashBT_Click);
            // 
            // attachmentsLb
            // 
            this.attachmentsLb.FormattingEnabled = true;
            this.attachmentsLb.ItemHeight = 20;
            this.attachmentsLb.Location = new System.Drawing.Point(3, 30);
            this.attachmentsLb.Name = "attachmentsLb";
            this.attachmentsLb.Size = new System.Drawing.Size(251, 84);
            this.attachmentsLb.TabIndex = 0;
            // 
            // downloadAttachmentBt
            // 
            this.downloadAttachmentBt.Image = global::Email_System.Properties.Resources.icons8_download_32;
            this.downloadAttachmentBt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.downloadAttachmentBt.Location = new System.Drawing.Point(260, 75);
            this.downloadAttachmentBt.Name = "downloadAttachmentBt";
            this.downloadAttachmentBt.Size = new System.Drawing.Size(113, 36);
            this.downloadAttachmentBt.TabIndex = 1;
            this.downloadAttachmentBt.Text = "Download";
            this.downloadAttachmentBt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.downloadAttachmentBt.UseVisualStyleBackColor = true;
            this.downloadAttachmentBt.Click += new System.EventHandler(this.downloadAttachmentBt_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.deleteMessageBt);
            this.panel1.Controls.Add(this.moveToTrashBT);
            this.panel1.Controls.Add(this.closeBt);
            this.panel1.Controls.Add(this.forwardBt);
            this.panel1.Controls.Add(this.replyBt);
            this.panel1.Controls.Add(this.replyAllBt);
            this.panel1.Location = new System.Drawing.Point(459, 582);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(628, 87);
            this.panel1.TabIndex = 0;
            // 
            // attachmentsPanel
            // 
            this.attachmentsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.attachmentsPanel.Controls.Add(this.attachmentsLb);
            this.attachmentsPanel.Controls.Add(this.label1);
            this.attachmentsPanel.Controls.Add(this.downloadAttachmentBt);
            this.attachmentsPanel.Controls.Add(this.pictureBox1);
            this.attachmentsPanel.Location = new System.Drawing.Point(12, 555);
            this.attachmentsPanel.Name = "attachmentsPanel";
            this.attachmentsPanel.Size = new System.Drawing.Size(402, 117);
            this.attachmentsPanel.TabIndex = 1;
            this.attachmentsPanel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Attachments:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Email_System.Properties.Resources.icons8_attach_32;
            this.pictureBox1.Location = new System.Drawing.Point(3, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 35);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // toTb
            // 
            this.toTb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.toTb.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.toTb.Location = new System.Drawing.Point(12, 116);
            this.toTb.Name = "toTb";
            this.toTb.ReadOnly = true;
            this.toTb.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.toTb.Size = new System.Drawing.Size(827, 27);
            this.toTb.TabIndex = 6;
            this.toTb.TabStop = false;
            this.toTb.Text = "To: ";
            // 
            // dateTb
            // 
            this.dateTb.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dateTb.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.dateTb.Location = new System.Drawing.Point(845, 83);
            this.dateTb.Name = "dateTb";
            this.dateTb.ReadOnly = true;
            this.dateTb.Size = new System.Drawing.Size(232, 27);
            this.dateTb.TabIndex = 3;
            this.dateTb.TabStop = false;
            this.dateTb.Text = "Date: ";
            // 
            // readMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 698);
            this.Controls.Add(this.dateTb);
            this.Controls.Add(this.toTb);
            this.Controls.Add(this.subjectTb);
            this.Controls.Add(this.bodyRtb);
            this.Controls.Add(this.fromTb);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.attachmentsPanel);
            this.MinimumSize = new System.Drawing.Size(1117, 745);
            this.Name = "readMessage";
            this.Text = "readMessage";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.attachmentsPanel.ResumeLayout(false);
            this.attachmentsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox fromTb;
        private RichTextBox bodyRtb;
        private Button replyBt;
        private Button replyAllBt;
        private Button forwardBt;
        private TextBox subjectTb;
        private Button closeBt;
        private Button deleteMessageBt;
        private Button moveToTrashBT;
        private ListBox attachmentsLb;
        private Button downloadAttachmentBt;
        private Panel panel1;
        private BindingSource bindingSource1;
        private Panel attachmentsPanel;
        private PictureBox pictureBox1;
        private Label label1;
        private TextBox toTb;
        private TextBox dateTb;
    }
}