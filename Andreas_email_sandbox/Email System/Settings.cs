﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Email_System
{
    public partial class Settings : Form
    {
        private static Settings instance = null!;
        private Settings()
        {
            InitializeComponent();
            localStorageCB.Checked = Properties.Settings.Default.downloadMessagesEnabled;
            offlineModeCB.Checked = Properties.Settings.Default.offlineModeEnabled;
        }

        public static Settings GetInstance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    instance = new Settings();
                }
                return instance;
            }
        }

        private void clearCache()
        {
            Data.deleteFiles();
            Application.Restart();
            Data.deleteFiles();
        }

        private void addAccount()
        {
            //???
        }

        private void toggleLocalStorage(bool value)
        {
            Properties.Settings.Default.downloadMessagesEnabled = value;
            Properties.Settings.Default.Save();
        }

        private void clearCacheBt_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete all files? This will restart the application and you will need to login again.", "Clear cache?", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                clearCache();

            }

            else
            {
                return;
            }

        }


        private void localStorageCB_Click(object sender, EventArgs e)
        {
            toggleLocalStorage(localStorageCB.Checked);
        }

        private void offlineModeCB_CheckStateChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.offlineModeEnabled = offlineModeCB.Checked;
            Properties.Settings.Default.Save();

            if (offlineModeCB.Checked)
            {
                localStorageCB.Checked = offlineModeCB.Checked;
                toggleLocalStorage(offlineModeCB.Checked);
            }
        }
    }
}
