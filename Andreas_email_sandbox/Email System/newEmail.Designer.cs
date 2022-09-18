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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.exitBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sendBt
            // 
            this.sendBt.Location = new System.Drawing.Point(220, 433);
            this.sendBt.Name = "sendBt";
            this.sendBt.Size = new System.Drawing.Size(137, 29);
            this.sendBt.TabIndex = 0;
            this.sendBt.Text = "Send e-mail";
            this.sendBt.UseVisualStyleBackColor = true;
            this.sendBt.Click += new System.EventHandler(this.sendBt_Click);
            // 
            // messageBodyTb
            // 
            this.messageBodyTb.Location = new System.Drawing.Point(398, 199);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(407, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Recipient";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(450, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Cc.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(420, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Subject";
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
            this.exitBt.Location = new System.Drawing.Point(90, 433);
            this.exitBt.Name = "exitBt";
            this.exitBt.Size = new System.Drawing.Size(94, 29);
            this.exitBt.TabIndex = 9;
            this.exitBt.Text = "Exit";
            this.exitBt.UseVisualStyleBackColor = true;
            this.exitBt.Click += new System.EventHandler(this.exitBt_Click);
            // 
            // newEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(916, 506);
            this.Controls.Add(this.exitBt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.subjectTb);
            this.Controls.Add(this.ccRecipientsTb);
            this.Controls.Add(this.recipientsTb);
            this.Controls.Add(this.messageBodyTb);
            this.Controls.Add(this.sendBt);
            this.Name = "newEmail";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button sendBt;
        private RichTextBox messageBodyTb;
        private TextBox recipientsTb;
        private TextBox ccRecipientsTb;
        private TextBox subjectTb;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button exitBt;
    }
}