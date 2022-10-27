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
            this.sendBt = new System.Windows.Forms.Button();
            this.messageBodyTb = new System.Windows.Forms.RichTextBox();
            this.recipientsTb = new System.Windows.Forms.TextBox();
            this.ccRecipientsTb = new System.Windows.Forms.TextBox();
            this.subjectTb = new System.Windows.Forms.TextBox();
            this.recipientLabel = new System.Windows.Forms.Label();
            this.CcLabel = new System.Windows.Forms.Label();
            this.SubjectLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.exitBt = new System.Windows.Forms.Button();
            this.draftBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sendBt
            // 
            this.sendBt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.sendBt.Location = new System.Drawing.Point(234, 415);
            this.sendBt.Name = "sendBt";
            this.sendBt.Size = new System.Drawing.Size(137, 64);
            this.sendBt.TabIndex = 0;
            this.sendBt.Text = "Send e-mail";
            this.sendBt.UseVisualStyleBackColor = true;
            this.sendBt.Click += new System.EventHandler(this.sendBt_Click);
            // 
            // messageBodyTb
            // 
            this.messageBodyTb.Location = new System.Drawing.Point(400, 195);
            this.messageBodyTb.Name = "messageBodyTb";
            this.messageBodyTb.Size = new System.Drawing.Size(506, 284);
            this.messageBodyTb.TabIndex = 1;
            this.messageBodyTb.Text = "";
            // 
            // recipientsTb
            // 
            this.recipientsTb.Location = new System.Drawing.Point(499, 29);
            this.recipientsTb.Name = "recipientsTb";
            this.recipientsTb.Size = new System.Drawing.Size(405, 27);
            this.recipientsTb.TabIndex = 2;
            // 
            // ccRecipientsTb
            // 
            this.ccRecipientsTb.Location = new System.Drawing.Point(499, 69);
            this.ccRecipientsTb.Name = "ccRecipientsTb";
            this.ccRecipientsTb.Size = new System.Drawing.Size(405, 27);
            this.ccRecipientsTb.TabIndex = 3;
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
            this.recipientLabel.Location = new System.Drawing.Point(407, 36);
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
            this.SubjectLabel.Location = new System.Drawing.Point(420, 126);
            this.SubjectLabel.Name = "SubjectLabel";
            this.SubjectLabel.Size = new System.Drawing.Size(58, 20);
            this.SubjectLabel.TabIndex = 7;
            this.SubjectLabel.Text = "Subject";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(400, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Message:";
            // 
            // exitBt
            // 
            this.exitBt.Location = new System.Drawing.Point(12, 450);
            this.exitBt.Name = "exitBt";
            this.exitBt.Size = new System.Drawing.Size(94, 29);
            this.exitBt.TabIndex = 9;
            this.exitBt.Text = "Exit";
            this.exitBt.UseVisualStyleBackColor = true;
            this.exitBt.Click += new System.EventHandler(this.exitBt_Click);
            // 
            // draftBt
            // 
            this.draftBt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.draftBt.Location = new System.Drawing.Point(112, 415);
            this.draftBt.Name = "draftBt";
            this.draftBt.Size = new System.Drawing.Size(116, 64);
            this.draftBt.TabIndex = 10;
            this.draftBt.Text = "Save as draft";
            this.draftBt.UseVisualStyleBackColor = true;
            this.draftBt.Click += new System.EventHandler(this.draftBt_Click);
            // 
            // newEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(916, 506);
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
        private Label label4;
        private Button exitBt;
        private Button draftBt;
    }
}