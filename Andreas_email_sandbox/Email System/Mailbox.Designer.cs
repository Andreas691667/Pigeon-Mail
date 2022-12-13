namespace Email_System
{
    partial class Mailbox
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mailbox));
            this.folderLb = new System.Windows.Forms.ListBox();
            this.messageLb = new System.Windows.Forms.ListBox();
            this.newEmailBt = new System.Windows.Forms.Button();
            this.refreshBt = new System.Windows.Forms.Button();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.addFlagBt = new System.Windows.Forms.Button();
            this.removeFlagBt = new System.Windows.Forms.Button();
            this.moveToTrashBt = new System.Windows.Forms.Button();
            this.deleteBt = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.folderDropDown = new System.Windows.Forms.ComboBox();
            this.moveMessageBt = new System.Windows.Forms.Button();
            this.senderRBT = new System.Windows.Forms.RadioButton();
            this.contentRBT = new System.Windows.Forms.RadioButton();
            this.subjectRBT = new System.Windows.Forms.RadioButton();
            this.searchTb = new System.Windows.Forms.TextBox();
            this.searchBt = new System.Windows.Forms.Button();
            this.logoutBt = new System.Windows.Forms.Button();
            this.settingsBt = new System.Windows.Forms.Button();
            this.logLabel = new System.Windows.Forms.Label();
            this.messagesDGV = new System.Windows.Forms.DataGridView();
            this.Folder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Body = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.folderDGV = new System.Windows.Forms.DataGridView();
            this.FolderView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addFolderBt = new System.Windows.Forms.Button();
            this.newFolderTB = new System.Windows.Forms.TextBox();
            this.loadIconPB = new System.Windows.Forms.PictureBox();
            this.deleteFolderBt = new System.Windows.Forms.Button();
            this.markMessageBt = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.messagesDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadIconPB)).BeginInit();
            this.SuspendLayout();
            // 
            // folderLb
            // 
            this.folderLb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.folderLb.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.folderLb.FormattingEnabled = true;
            this.folderLb.ItemHeight = 21;
            this.folderLb.Location = new System.Drawing.Point(13, 56);
            this.folderLb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.folderLb.Name = "folderLb";
            this.folderLb.Size = new System.Drawing.Size(241, 802);
            this.folderLb.TabIndex = 0;
            this.folderLb.Visible = false;
            // 
            // messageLb
            // 
            this.messageLb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageLb.BackColor = System.Drawing.SystemColors.Window;
            this.messageLb.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.messageLb.FormattingEnabled = true;
            this.messageLb.ItemHeight = 19;
            this.messageLb.Location = new System.Drawing.Point(338, 67);
            this.messageLb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.messageLb.Name = "messageLb";
            this.messageLb.Size = new System.Drawing.Size(1546, 764);
            this.messageLb.TabIndex = 1;
            this.messageLb.Visible = false;
            // 
            // newEmailBt
            // 
            this.newEmailBt.Image = global::Email_System.Properties.Resources.icons8_secured_letter_32;
            this.newEmailBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.newEmailBt.Location = new System.Drawing.Point(3, 0);
            this.newEmailBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.newEmailBt.Name = "newEmailBt";
            this.newEmailBt.Size = new System.Drawing.Size(83, 48);
            this.newEmailBt.TabIndex = 2;
            this.newEmailBt.Text = "New e-mail";
            this.newEmailBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.newEmailBt.UseVisualStyleBackColor = true;
            this.newEmailBt.Click += new System.EventHandler(this.newEmailBt_Click);
            // 
            // refreshBt
            // 
            this.refreshBt.Image = global::Email_System.Properties.Resources.refresh;
            this.refreshBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.refreshBt.Location = new System.Drawing.Point(93, 0);
            this.refreshBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.refreshBt.Name = "refreshBt";
            this.refreshBt.Size = new System.Drawing.Size(61, 48);
            this.refreshBt.TabIndex = 3;
            this.refreshBt.Text = "Refresh";
            this.refreshBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.refreshBt.UseVisualStyleBackColor = true;
            this.refreshBt.Click += new System.EventHandler(this.refreshBt_Click);
            // 
            // refreshTimer
            // 
            this.refreshTimer.Enabled = true;
            this.refreshTimer.Interval = 10000;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // addFlagBt
            // 
            this.addFlagBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addFlagBt.Image = global::Email_System.Properties.Resources.icons8_flag_32;
            this.addFlagBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.addFlagBt.Location = new System.Drawing.Point(1489, 0);
            this.addFlagBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addFlagBt.Name = "addFlagBt";
            this.addFlagBt.Size = new System.Drawing.Size(92, 48);
            this.addFlagBt.TabIndex = 4;
            this.addFlagBt.Text = "Flag/unflag";
            this.addFlagBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.addFlagBt.UseVisualStyleBackColor = true;
            this.addFlagBt.Visible = false;
            this.addFlagBt.Click += new System.EventHandler(this.toggleFlag);
            // 
            // removeFlagBt
            // 
            this.removeFlagBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeFlagBt.Location = new System.Drawing.Point(1489, 2);
            this.removeFlagBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.removeFlagBt.Name = "removeFlagBt";
            this.removeFlagBt.Size = new System.Drawing.Size(92, 41);
            this.removeFlagBt.TabIndex = 5;
            this.removeFlagBt.Text = "Remove flag";
            this.removeFlagBt.UseVisualStyleBackColor = true;
            this.removeFlagBt.Visible = false;
            // 
            // moveToTrashBt
            // 
            this.moveToTrashBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveToTrashBt.Image = global::Email_System.Properties.Resources.icons8_trash_32;
            this.moveToTrashBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.moveToTrashBt.Location = new System.Drawing.Point(1659, 0);
            this.moveToTrashBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.moveToTrashBt.Name = "moveToTrashBt";
            this.moveToTrashBt.Size = new System.Drawing.Size(49, 48);
            this.moveToTrashBt.TabIndex = 6;
            this.moveToTrashBt.Text = "Trash";
            this.moveToTrashBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.moveToTrashBt.UseVisualStyleBackColor = true;
            this.moveToTrashBt.Visible = false;
            this.moveToTrashBt.Click += new System.EventHandler(this.moveToTrashBt_Click);
            // 
            // deleteBt
            // 
            this.deleteBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteBt.Image = global::Email_System.Properties.Resources.icons8_remove_32;
            this.deleteBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.deleteBt.Location = new System.Drawing.Point(1586, 0);
            this.deleteBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.deleteBt.Name = "deleteBt";
            this.deleteBt.Size = new System.Drawing.Size(66, 48);
            this.deleteBt.TabIndex = 7;
            this.deleteBt.Text = "Delete";
            this.deleteBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.deleteBt.UseVisualStyleBackColor = true;
            this.deleteBt.Visible = false;
            this.deleteBt.Click += new System.EventHandler(this.deleteBt_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.markMessageBt);
            this.panel1.Controls.Add(this.folderDropDown);
            this.panel1.Controls.Add(this.moveMessageBt);
            this.panel1.Controls.Add(this.senderRBT);
            this.panel1.Controls.Add(this.contentRBT);
            this.panel1.Controls.Add(this.subjectRBT);
            this.panel1.Controls.Add(this.searchTb);
            this.panel1.Controls.Add(this.searchBt);
            this.panel1.Controls.Add(this.logoutBt);
            this.panel1.Controls.Add(this.settingsBt);
            this.panel1.Controls.Add(this.refreshBt);
            this.panel1.Controls.Add(this.deleteBt);
            this.panel1.Controls.Add(this.addFlagBt);
            this.panel1.Controls.Add(this.newEmailBt);
            this.panel1.Controls.Add(this.moveToTrashBt);
            this.panel1.Controls.Add(this.removeFlagBt);
            this.panel1.Location = new System.Drawing.Point(10, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1870, 61);
            this.panel1.TabIndex = 8;
            // 
            // folderDropDown
            // 
            this.folderDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.folderDropDown.FormattingEnabled = true;
            this.folderDropDown.Location = new System.Drawing.Point(1310, 26);
            this.folderDropDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.folderDropDown.Name = "folderDropDown";
            this.folderDropDown.Size = new System.Drawing.Size(173, 23);
            this.folderDropDown.TabIndex = 16;
            // 
            // moveMessageBt
            // 
            this.moveMessageBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveMessageBt.Image = global::Email_System.Properties.Resources.icons8_live_folder_32;
            this.moveMessageBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.moveMessageBt.Location = new System.Drawing.Point(1223, 2);
            this.moveMessageBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.moveMessageBt.Name = "moveMessageBt";
            this.moveMessageBt.Size = new System.Drawing.Size(82, 46);
            this.moveMessageBt.TabIndex = 15;
            this.moveMessageBt.Text = "Move";
            this.moveMessageBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.moveMessageBt.UseVisualStyleBackColor = true;
            this.moveMessageBt.Click += new System.EventHandler(this.moveMessageBt_Click);
            // 
            // senderRBT
            // 
            this.senderRBT.AutoSize = true;
            this.senderRBT.Location = new System.Drawing.Point(223, 3);
            this.senderRBT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.senderRBT.Name = "senderRBT";
            this.senderRBT.Size = new System.Drawing.Size(61, 19);
            this.senderRBT.TabIndex = 14;
            this.senderRBT.Text = "Sender";
            this.senderRBT.UseVisualStyleBackColor = true;
            // 
            // contentRBT
            // 
            this.contentRBT.AutoSize = true;
            this.contentRBT.Location = new System.Drawing.Point(295, 3);
            this.contentRBT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.contentRBT.Name = "contentRBT";
            this.contentRBT.Size = new System.Drawing.Size(68, 19);
            this.contentRBT.TabIndex = 13;
            this.contentRBT.Text = "Content";
            this.contentRBT.UseVisualStyleBackColor = true;
            // 
            // subjectRBT
            // 
            this.subjectRBT.AutoSize = true;
            this.subjectRBT.Checked = true;
            this.subjectRBT.Location = new System.Drawing.Point(372, 3);
            this.subjectRBT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.subjectRBT.Name = "subjectRBT";
            this.subjectRBT.Size = new System.Drawing.Size(64, 19);
            this.subjectRBT.TabIndex = 12;
            this.subjectRBT.TabStop = true;
            this.subjectRBT.Text = "Subject";
            this.subjectRBT.UseVisualStyleBackColor = true;
            // 
            // searchTb
            // 
            this.searchTb.Location = new System.Drawing.Point(223, 26);
            this.searchTb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.searchTb.Name = "searchTb";
            this.searchTb.PlaceholderText = "Search in all folders...";
            this.searchTb.Size = new System.Drawing.Size(218, 23);
            this.searchTb.TabIndex = 11;
            this.searchTb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTb_KeyDown);
            // 
            // searchBt
            // 
            this.searchBt.Image = global::Email_System.Properties.Resources.icons8_search_32;
            this.searchBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.searchBt.Location = new System.Drawing.Point(159, 0);
            this.searchBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.searchBt.Name = "searchBt";
            this.searchBt.Size = new System.Drawing.Size(59, 48);
            this.searchBt.TabIndex = 10;
            this.searchBt.Text = "Search";
            this.searchBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.searchBt.UseVisualStyleBackColor = true;
            this.searchBt.Click += new System.EventHandler(this.searchBt_Click);
            // 
            // logoutBt
            // 
            this.logoutBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logoutBt.Image = global::Email_System.Properties.Resources.icons8_logout_32;
            this.logoutBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.logoutBt.Location = new System.Drawing.Point(1797, 0);
            this.logoutBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logoutBt.Name = "logoutBt";
            this.logoutBt.Size = new System.Drawing.Size(71, 48);
            this.logoutBt.TabIndex = 9;
            this.logoutBt.Text = "Sign out";
            this.logoutBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.logoutBt.UseVisualStyleBackColor = true;
            this.logoutBt.Click += new System.EventHandler(this.logoutBt_Click);
            // 
            // settingsBt
            // 
            this.settingsBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsBt.Image = global::Email_System.Properties.Resources.icons8_settings_32;
            this.settingsBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.settingsBt.Location = new System.Drawing.Point(1712, 0);
            this.settingsBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.settingsBt.Name = "settingsBt";
            this.settingsBt.Size = new System.Drawing.Size(80, 48);
            this.settingsBt.TabIndex = 8;
            this.settingsBt.Text = "Settings";
            this.settingsBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.settingsBt.UseVisualStyleBackColor = true;
            this.settingsBt.Click += new System.EventHandler(this.settingsBt_Click);
            // 
            // logLabel
            // 
            this.logLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.logLabel.Enabled = false;
            this.logLabel.Location = new System.Drawing.Point(1155, 886);
            this.logLabel.Name = "logLabel";
            this.logLabel.Size = new System.Drawing.Size(670, 20);
            this.logLabel.TabIndex = 9;
            this.logLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // messagesDGV
            // 
            this.messagesDGV.AllowUserToAddRows = false;
            this.messagesDGV.AllowUserToDeleteRows = false;
            this.messagesDGV.AllowUserToResizeColumns = false;
            this.messagesDGV.AllowUserToResizeRows = false;
            this.messagesDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messagesDGV.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.messagesDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.messagesDGV.ColumnHeadersHeight = 40;
            this.messagesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.messagesDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Folder,
            this.From,
            this.Subject,
            this.Body,
            this.Date});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.messagesDGV.DefaultCellStyle = dataGridViewCellStyle4;
            this.messagesDGV.Location = new System.Drawing.Point(259, 67);
            this.messagesDGV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.messagesDGV.MultiSelect = false;
            this.messagesDGV.Name = "messagesDGV";
            this.messagesDGV.ReadOnly = true;
            this.messagesDGV.RowHeadersVisible = false;
            this.messagesDGV.RowHeadersWidth = 51;
            this.messagesDGV.RowTemplate.Height = 29;
            this.messagesDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.messagesDGV.Size = new System.Drawing.Size(1624, 805);
            this.messagesDGV.TabIndex = 10;
            this.messagesDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.messagesDGV_CellDoubleClick);
            // 
            // Folder
            // 
            this.Folder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Folder.DefaultCellStyle = dataGridViewCellStyle2;
            this.Folder.HeaderText = "Folder";
            this.Folder.MinimumWidth = 130;
            this.Folder.Name = "Folder";
            this.Folder.ReadOnly = true;
            this.Folder.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Folder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Folder.Width = 130;
            // 
            // From
            // 
            this.From.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.From.HeaderText = "From";
            this.From.MinimumWidth = 6;
            this.From.Name = "From";
            this.From.ReadOnly = true;
            this.From.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.From.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.From.Width = 95;
            // 
            // Subject
            // 
            this.Subject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Subject.HeaderText = "Subject";
            this.Subject.MinimumWidth = 6;
            this.Subject.Name = "Subject";
            this.Subject.ReadOnly = true;
            this.Subject.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Subject.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Body
            // 
            this.Body.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Body.HeaderText = "Message";
            this.Body.MinimumWidth = 6;
            this.Body.Name = "Body";
            this.Body.ReadOnly = true;
            this.Body.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Body.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Date
            // 
            this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle3.Format = "F";
            dataGridViewCellStyle3.NullValue = null;
            this.Date.DefaultCellStyle = dataGridViewCellStyle3;
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 6;
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Date.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Date.Width = 48;
            // 
            // folderDGV
            // 
            this.folderDGV.AllowUserToAddRows = false;
            this.folderDGV.AllowUserToDeleteRows = false;
            this.folderDGV.AllowUserToResizeColumns = false;
            this.folderDGV.AllowUserToResizeRows = false;
            this.folderDGV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.folderDGV.BackgroundColor = System.Drawing.SystemColors.Control;
            this.folderDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.folderDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FolderView});
            this.folderDGV.Location = new System.Drawing.Point(13, 67);
            this.folderDGV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.folderDGV.MultiSelect = false;
            this.folderDGV.Name = "folderDGV";
            this.folderDGV.ReadOnly = true;
            this.folderDGV.RowHeadersVisible = false;
            this.folderDGV.RowHeadersWidth = 51;
            this.folderDGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.folderDGV.RowTemplate.Height = 29;
            this.folderDGV.Size = new System.Drawing.Size(241, 767);
            this.folderDGV.TabIndex = 11;
            this.folderDGV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.folderDGV_CellClick);
            // 
            // FolderView
            // 
            this.FolderView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FolderView.HeaderText = "Column1";
            this.FolderView.MinimumWidth = 6;
            this.FolderView.Name = "FolderView";
            this.FolderView.ReadOnly = true;
            this.FolderView.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FolderView.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // addFolderBt
            // 
            this.addFolderBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addFolderBt.Image = global::Email_System.Properties.Resources.icons8_add_folder_32;
            this.addFolderBt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addFolderBt.Location = new System.Drawing.Point(13, 876);
            this.addFolderBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addFolderBt.Name = "addFolderBt";
            this.addFolderBt.Size = new System.Drawing.Size(120, 31);
            this.addFolderBt.TabIndex = 12;
            this.addFolderBt.Text = "Add folder";
            this.addFolderBt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addFolderBt.UseVisualStyleBackColor = true;
            this.addFolderBt.Click += new System.EventHandler(this.button1_Click);
            // 
            // newFolderTB
            // 
            this.newFolderTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newFolderTB.Location = new System.Drawing.Point(138, 886);
            this.newFolderTB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.newFolderTB.Name = "newFolderTB";
            this.newFolderTB.PlaceholderText = "Enter name...";
            this.newFolderTB.Size = new System.Drawing.Size(110, 23);
            this.newFolderTB.TabIndex = 13;
            // 
            // loadIconPB
            // 
            this.loadIconPB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadIconPB.Image = global::Email_System.Properties.Resources.giphy;
            this.loadIconPB.Location = new System.Drawing.Point(1839, 876);
            this.loadIconPB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadIconPB.Name = "loadIconPB";
            this.loadIconPB.Size = new System.Drawing.Size(45, 34);
            this.loadIconPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadIconPB.TabIndex = 14;
            this.loadIconPB.TabStop = false;
            this.loadIconPB.Visible = false;
            // 
            // deleteFolderBt
            // 
            this.deleteFolderBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteFolderBt.Image = global::Email_System.Properties.Resources.icons8_delete_folder_32;
            this.deleteFolderBt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteFolderBt.Location = new System.Drawing.Point(12, 842);
            this.deleteFolderBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.deleteFolderBt.Name = "deleteFolderBt";
            this.deleteFolderBt.Size = new System.Drawing.Size(121, 30);
            this.deleteFolderBt.TabIndex = 16;
            this.deleteFolderBt.Text = "Delete folder";
            this.deleteFolderBt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteFolderBt.UseVisualStyleBackColor = true;
            this.deleteFolderBt.Click += new System.EventHandler(this.deleteFolderBt_Click);
            // 
            // markMessageBt
            // 
            this.markMessageBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.markMessageBt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.markMessageBt.Location = new System.Drawing.Point(1135, 2);
            this.markMessageBt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.markMessageBt.Name = "markMessageBt";
            this.markMessageBt.Size = new System.Drawing.Size(82, 46);
            this.markMessageBt.TabIndex = 17;
            this.markMessageBt.Text = "Mark as read/unread";
            this.markMessageBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.markMessageBt.UseVisualStyleBackColor = true;
            this.markMessageBt.Click += new System.EventHandler(this.markMessageBt_Click);
            // 
            // Mailbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 913);
            this.Controls.Add(this.deleteFolderBt);
            this.Controls.Add(this.loadIconPB);
            this.Controls.Add(this.logLabel);
            this.Controls.Add(this.newFolderTB);
            this.Controls.Add(this.addFolderBt);
            this.Controls.Add(this.folderDGV);
            this.Controls.Add(this.messagesDGV);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.messageLb);
            this.Controls.Add(this.folderLb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(1160, 484);
            this.Name = "Mailbox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Mailbox";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Mailbox_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.messagesDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadIconPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox folderLb;
        private ListBox messageLb;
        private Button newEmailBt;
        private Button refreshBt;
        private System.Windows.Forms.Timer refreshTimer;
        private Button addFlagBt;
        private Button removeFlagBt;
        private Button moveToTrashBt;
        private Button deleteBt;
        private Panel panel1;
        private Button settingsBt;
        private Button logoutBt;
        private Button searchBt;
        private TextBox searchTb;
        private RadioButton senderRBT;
        private RadioButton contentRBT;
        private RadioButton subjectRBT;
        private Label logLabel;
        private DataGridView messagesDGV;
        private DataGridView folderDGV;
        private DataGridViewTextBoxColumn FolderView;
        private Button addFolderBt;
        private TextBox newFolderTB;
        private Button moveMessageBt;
        private Button deleteFolderBt;
        public ComboBox folderDropDown;
        public PictureBox loadIconPB;
        private DataGridViewTextBoxColumn Folder;
        private DataGridViewTextBoxColumn From;
        private DataGridViewTextBoxColumn Subject;
        private DataGridViewTextBoxColumn Body;
        private DataGridViewTextBoxColumn Date;
        private Button markMessageBt;
    }
}