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
            this.replyBt = new System.Windows.Forms.Button();
            this.replyAllBt = new System.Windows.Forms.Button();
            this.forwardBt = new System.Windows.Forms.Button();
            this.subjectTb = new System.Windows.Forms.TextBox();
            this.closeBt = new System.Windows.Forms.Button();
            this.ccRecipientsTb = new System.Windows.Forms.TextBox();
            this.deleteMessageBt = new System.Windows.Forms.Button();
            this.moveToTrashBT = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fromTb
            // 
            this.fromTb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fromTb.Location = new System.Drawing.Point(147, 56);
            this.fromTb.Name = "fromTb";
            this.fromTb.ReadOnly = true;
            this.fromTb.Size = new System.Drawing.Size(896, 27);
            this.fromTb.TabIndex = 0;
            // 
            // bodyRtb
            // 
            this.bodyRtb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bodyRtb.Location = new System.Drawing.Point(73, 132);
            this.bodyRtb.Name = "bodyRtb";
            this.bodyRtb.ReadOnly = true;
            this.bodyRtb.Size = new System.Drawing.Size(970, 367);
            this.bodyRtb.TabIndex = 1;
            this.bodyRtb.Text = "";
            // 
            // replyBt
            // 
            this.replyBt.Location = new System.Drawing.Point(72, 543);
            this.replyBt.Name = "replyBt";
            this.replyBt.Size = new System.Drawing.Size(94, 29);
            this.replyBt.TabIndex = 2;
            this.replyBt.Text = "Reply";
            this.replyBt.UseVisualStyleBackColor = true;
            this.replyBt.Click += new System.EventHandler(this.replyBt_Click);
            // 
            // replyAllBt
            // 
            this.replyAllBt.Location = new System.Drawing.Point(212, 543);
            this.replyAllBt.Name = "replyAllBt";
            this.replyAllBt.Size = new System.Drawing.Size(94, 29);
            this.replyAllBt.TabIndex = 3;
            this.replyAllBt.Text = "Reply all";
            this.replyAllBt.UseVisualStyleBackColor = true;
            this.replyAllBt.Click += new System.EventHandler(this.replyAllBt_Click);
            // 
            // forwardBt
            // 
            this.forwardBt.Location = new System.Drawing.Point(344, 543);
            this.forwardBt.Name = "forwardBt";
            this.forwardBt.Size = new System.Drawing.Size(94, 29);
            this.forwardBt.TabIndex = 4;
            this.forwardBt.Text = "Forward";
            this.forwardBt.UseVisualStyleBackColor = true;
            this.forwardBt.Click += new System.EventHandler(this.forwardBt_Click);
            // 
            // subjectTb
            // 
            this.subjectTb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.subjectTb.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.subjectTb.Location = new System.Drawing.Point(147, 12);
            this.subjectTb.Name = "subjectTb";
            this.subjectTb.ReadOnly = true;
            this.subjectTb.Size = new System.Drawing.Size(898, 38);
            this.subjectTb.TabIndex = 5;
            // 
            // closeBt
            // 
            this.closeBt.Location = new System.Drawing.Point(490, 543);
            this.closeBt.Name = "closeBt";
            this.closeBt.Size = new System.Drawing.Size(94, 29);
            this.closeBt.TabIndex = 6;
            this.closeBt.Text = "Close";
            this.closeBt.UseVisualStyleBackColor = true;
            this.closeBt.Click += new System.EventHandler(this.closeBt_Click);
            // 
            // ccRecipientsTb
            // 
            this.ccRecipientsTb.Location = new System.Drawing.Point(147, 99);
            this.ccRecipientsTb.Name = "ccRecipientsTb";
            this.ccRecipientsTb.ReadOnly = true;
            this.ccRecipientsTb.Size = new System.Drawing.Size(898, 27);
            this.ccRecipientsTb.TabIndex = 7;
            // 
            // deleteMessageBt
            // 
            this.deleteMessageBt.Location = new System.Drawing.Point(862, 528);
            this.deleteMessageBt.Name = "deleteMessageBt";
            this.deleteMessageBt.Size = new System.Drawing.Size(93, 58);
            this.deleteMessageBt.TabIndex = 8;
            this.deleteMessageBt.Text = "Delete message";
            this.deleteMessageBt.UseVisualStyleBackColor = true;
            this.deleteMessageBt.Click += new System.EventHandler(this.deleteMessageBt_Click);
            // 
            // moveToTrashBT
            // 
            this.moveToTrashBT.Location = new System.Drawing.Point(735, 528);
            this.moveToTrashBT.Name = "moveToTrashBT";
            this.moveToTrashBT.Size = new System.Drawing.Size(94, 58);
            this.moveToTrashBT.TabIndex = 9;
            this.moveToTrashBT.Text = "Move to trash";
            this.moveToTrashBT.UseVisualStyleBackColor = true;
            this.moveToTrashBT.Click += new System.EventHandler(this.moveToTrashBT_Click);
            // 
            // readMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 698);
            this.Controls.Add(this.moveToTrashBT);
            this.Controls.Add(this.deleteMessageBt);
            this.Controls.Add(this.ccRecipientsTb);
            this.Controls.Add(this.closeBt);
            this.Controls.Add(this.subjectTb);
            this.Controls.Add(this.forwardBt);
            this.Controls.Add(this.replyAllBt);
            this.Controls.Add(this.replyBt);
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
        private Button replyBt;
        private Button replyAllBt;
        private Button forwardBt;
        private TextBox subjectTb;
        private Button closeBt;
        private TextBox ccRecipientsTb;
        private Button deleteMessageBt;
        private Button moveToTrashBT;
    }
}