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
            this.clearCacheBt = new System.Windows.Forms.Button();
            this.localStorageCB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // clearCacheBt
            // 
            this.clearCacheBt.Location = new System.Drawing.Point(474, 165);
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
            this.localStorageCB.Location = new System.Drawing.Point(474, 237);
            this.localStorageCB.Name = "localStorageCB";
            this.localStorageCB.Size = new System.Drawing.Size(120, 24);
            this.localStorageCB.TabIndex = 1;
            this.localStorageCB.Text = "Local storage";
            this.localStorageCB.UseVisualStyleBackColor = true;
            this.localStorageCB.Click += new System.EventHandler(this.localStorageCB_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}