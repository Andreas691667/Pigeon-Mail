namespace Email_System
{
    partial class Blacklist
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
            this.emailBlackList = new System.Windows.Forms.ListView();
            this.Blacklisted_emails = new System.Windows.Forms.ColumnHeader();
            this.button1 = new System.Windows.Forms.Button();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.DeleteEmailBtn = new System.Windows.Forms.Button();
            this.deleteWordBtn = new System.Windows.Forms.Button();
            this.wordTextBox = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.wordBlackList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // emailBlackList
            // 
            this.emailBlackList.AccessibleName = "";
            this.emailBlackList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Blacklisted_emails});
            this.emailBlackList.FullRowSelect = true;
            this.emailBlackList.Location = new System.Drawing.Point(184, 124);
            this.emailBlackList.Margin = new System.Windows.Forms.Padding(4);
            this.emailBlackList.Name = "emailBlackList";
            this.emailBlackList.Size = new System.Drawing.Size(339, 363);
            this.emailBlackList.TabIndex = 0;
            this.emailBlackList.UseCompatibleStateImageBehavior = false;
            this.emailBlackList.View = System.Windows.Forms.View.Tile;
            // 
            // Blacklisted_emails
            // 
            this.Blacklisted_emails.Width = 100;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 76);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 36);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add email";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(184, 76);
            this.emailTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(339, 31);
            this.emailTextBox.TabIndex = 3;
            // 
            // DeleteEmailBtn
            // 
            this.DeleteEmailBtn.Location = new System.Drawing.Point(22, 124);
            this.DeleteEmailBtn.Margin = new System.Windows.Forms.Padding(4);
            this.DeleteEmailBtn.Name = "DeleteEmailBtn";
            this.DeleteEmailBtn.Size = new System.Drawing.Size(118, 36);
            this.DeleteEmailBtn.TabIndex = 4;
            this.DeleteEmailBtn.Text = "Delete";
            this.DeleteEmailBtn.UseVisualStyleBackColor = true;
            this.DeleteEmailBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // deleteWordBtn
            // 
            this.deleteWordBtn.Location = new System.Drawing.Point(665, 124);
            this.deleteWordBtn.Margin = new System.Windows.Forms.Padding(4);
            this.deleteWordBtn.Name = "deleteWordBtn";
            this.deleteWordBtn.Size = new System.Drawing.Size(118, 36);
            this.deleteWordBtn.TabIndex = 8;
            this.deleteWordBtn.Text = "Delete";
            this.deleteWordBtn.UseVisualStyleBackColor = true;
            this.deleteWordBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // wordTextBox
            // 
            this.wordTextBox.Location = new System.Drawing.Point(826, 76);
            this.wordTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.wordTextBox.Name = "wordTextBox";
            this.wordTextBox.Size = new System.Drawing.Size(339, 31);
            this.wordTextBox.TabIndex = 7;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(665, 76);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(118, 36);
            this.button4.TabIndex = 6;
            this.button4.Text = "Add word";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // wordBlackList
            // 
            this.wordBlackList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.wordBlackList.FullRowSelect = true;
            this.wordBlackList.Location = new System.Drawing.Point(826, 124);
            this.wordBlackList.Margin = new System.Windows.Forms.Padding(4);
            this.wordBlackList.Name = "wordBlackList";
            this.wordBlackList.Size = new System.Drawing.Size(339, 363);
            this.wordBlackList.TabIndex = 5;
            this.wordBlackList.UseCompatibleStateImageBehavior = false;
            this.wordBlackList.View = System.Windows.Forms.View.Tile;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "Blacklisted emails";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(665, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Blacklisted words";
            // 
            // Blacklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1406, 562);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deleteWordBtn);
            this.Controls.Add(this.wordTextBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.wordBlackList);
            this.Controls.Add(this.DeleteEmailBtn);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.emailBlackList);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Blacklist";
            this.Text = "Blacklist";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView emailBlackList;
        private Button button1;
        private TextBox emailTextBox;
        private Button DeleteEmailBtn;
        private Button deleteWordBtn;
        private TextBox wordTextBox;
        private Button button4;
        private ListView wordBlackList;
        private Label label1;
        private Label label2;
        private ColumnHeader Blacklisted_emails;
        private ColumnHeader columnHeader1;
    }
}