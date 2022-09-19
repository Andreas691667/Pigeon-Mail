namespace Email_System
{
    partial class login
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
            this.usernameTb = new System.Windows.Forms.TextBox();
            this.passwordTb = new System.Windows.Forms.TextBox();
            this.loginBt = new System.Windows.Forms.Button();
            this.exitBt = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // usernameTb
            // 
            this.usernameTb.Location = new System.Drawing.Point(177, 40);
            this.usernameTb.Name = "usernameTb";
            this.usernameTb.Size = new System.Drawing.Size(329, 27);
            this.usernameTb.TabIndex = 0;
            // 
            // passwordTb
            // 
            this.passwordTb.Location = new System.Drawing.Point(177, 89);
            this.passwordTb.Name = "passwordTb";
            this.passwordTb.Size = new System.Drawing.Size(329, 27);
            this.passwordTb.TabIndex = 1;
            this.passwordTb.UseSystemPasswordChar = true;
            // 
            // loginBt
            // 
            this.loginBt.Location = new System.Drawing.Point(177, 160);
            this.loginBt.Name = "loginBt";
            this.loginBt.Size = new System.Drawing.Size(94, 29);
            this.loginBt.TabIndex = 2;
            this.loginBt.Text = "Login";
            this.loginBt.UseVisualStyleBackColor = true;
            this.loginBt.Click += new System.EventHandler(this.loginBt_Click);
            // 
            // exitBt
            // 
            this.exitBt.Location = new System.Drawing.Point(177, 204);
            this.exitBt.Name = "exitBt";
            this.exitBt.Size = new System.Drawing.Size(94, 29);
            this.exitBt.TabIndex = 3;
            this.exitBt.Text = "Exit";
            this.exitBt.UseVisualStyleBackColor = true;
            this.exitBt.Click += new System.EventHandler(this.exitBt_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 263);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exitBt);
            this.Controls.Add(this.loginBt);
            this.Controls.Add(this.passwordTb);
            this.Controls.Add(this.usernameTb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox usernameTb;
        private TextBox passwordTb;
        private Button loginBt;
        private Button exitBt;
        private Label label1;
        private Label label2;
    }
}