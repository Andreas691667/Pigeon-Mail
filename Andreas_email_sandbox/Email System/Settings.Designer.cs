namespace Email_System
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.clearCacheBt = new System.Windows.Forms.Button();
            this.localStorageCB = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.offlineModeCB = new System.Windows.Forms.CheckBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clearCacheBt
            // 
            this.clearCacheBt.Location = new System.Drawing.Point(472, 96);
            this.clearCacheBt.Name = "clearCacheBt";
            this.clearCacheBt.Size = new System.Drawing.Size(206, 29);
            this.clearCacheBt.TabIndex = 0;
            this.clearCacheBt.Text = "Clear Cache";
            this.clearCacheBt.UseVisualStyleBackColor = true;
            this.clearCacheBt.Click += new System.EventHandler(this.clearCacheBt_Click);
            // 
            // localStorageCB
            // 
            this.localStorageCB.AutoSize = true;
            this.localStorageCB.Location = new System.Drawing.Point(472, 245);
            this.localStorageCB.Name = "localStorageCB";
            this.localStorageCB.Size = new System.Drawing.Size(120, 24);
            this.localStorageCB.TabIndex = 1;
            this.localStorageCB.Text = "Local storage";
            this.localStorageCB.UseVisualStyleBackColor = true;
            this.localStorageCB.Click += new System.EventHandler(this.localStorageCB_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(29, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(426, 113);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "If you are experiencing that your account is out of sync with the server, you can" +
    " manually clear the cache here. \nAt next login, it will take a while to fetch yo" +
    "ur messages once again.";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(29, 131);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(426, 138);
            this.richTextBox2.TabIndex = 3;
            this.richTextBox2.Text = resources.GetString("richTextBox2.Text");
            // 
            // offlineModeCB
            // 
            this.offlineModeCB.AutoSize = true;
            this.offlineModeCB.Location = new System.Drawing.Point(472, 402);
            this.offlineModeCB.Name = "offlineModeCB";
            this.offlineModeCB.Size = new System.Drawing.Size(177, 24);
            this.offlineModeCB.TabIndex = 4;
            this.offlineModeCB.Text = "Offline mode enabled";
            this.offlineModeCB.UseVisualStyleBackColor = true;
            this.offlineModeCB.CheckStateChanged += new System.EventHandler(this.offlineModeCB_CheckStateChanged);
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(29, 277);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(426, 149);
            this.richTextBox3.TabIndex = 5;
            this.richTextBox3.Text = resources.GetString("richTextBox3.Text");
            // 
            // richTextBox4
            // 
            this.richTextBox4.Location = new System.Drawing.Point(29, 432);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(426, 120);
            this.richTextBox4.TabIndex = 6;
            this.richTextBox4.Text = "Something about blacklists...";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(472, 523);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(206, 29);
            this.button1.TabIndex = 7;
            this.button1.Text = "Edit blacklists";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 607);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox4);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.offlineModeCB);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.localStorageCB);
            this.Controls.Add(this.clearCacheBt);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button clearCacheBt;
        private CheckBox localStorageCB;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private CheckBox offlineModeCB;
        private RichTextBox richTextBox3;
        private RichTextBox richTextBox4;
        private Button button1;
    }
}