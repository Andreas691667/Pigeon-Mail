﻿namespace Email_System
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
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.wordTextBox = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.wordBlackList = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // emailBlackList
            // 
            this.emailBlackList.AccessibleName = "";
            this.emailBlackList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Blacklisted_emails});
            this.emailBlackList.FullRowSelect = true;
            this.emailBlackList.Location = new System.Drawing.Point(147, 99);
            this.emailBlackList.Name = "emailBlackList";
            this.emailBlackList.Size = new System.Drawing.Size(272, 291);
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
            this.button1.Location = new System.Drawing.Point(18, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add email";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(147, 61);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(272, 27);
            this.emailTextBox.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 29);
            this.button2.TabIndex = 4;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(532, 99);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 29);
            this.button3.TabIndex = 8;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // wordTextBox
            // 
            this.wordTextBox.Location = new System.Drawing.Point(661, 61);
            this.wordTextBox.Name = "wordTextBox";
            this.wordTextBox.Size = new System.Drawing.Size(272, 27);
            this.wordTextBox.TabIndex = 7;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(532, 61);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 29);
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
            this.wordBlackList.Location = new System.Drawing.Point(661, 99);
            this.wordBlackList.Name = "wordBlackList";
            this.wordBlackList.Size = new System.Drawing.Size(272, 291);
            this.wordBlackList.TabIndex = 5;
            this.wordBlackList.UseCompatibleStateImageBehavior = false;
            this.wordBlackList.View = System.Windows.Forms.View.Tile;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Blacklisted emails";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(532, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Blacklisted words";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 100;
            // 
            // Blacklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.wordTextBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.wordBlackList);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.emailBlackList);
            this.Name = "Blacklist";
            this.Text = "Blacklist";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView emailBlackList;
        private Button button1;
        private TextBox emailTextBox;
        private Button button2;
        private Button button3;
        private TextBox wordTextBox;
        private Button button4;
        private ListView wordBlackList;
        private Label label1;
        private Label label2;
        private ColumnHeader Blacklisted_emails;
        private ColumnHeader columnHeader1;
    }
}