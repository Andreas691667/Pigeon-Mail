using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Email_System.Data;

namespace Email_System
{
    public partial class Blacklist : Form
    {
        public Blacklist()
        {
            InitializeComponent();
            initializeData();
        }

        // Called upon init
        private static void initializeData ()
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem("Hey");
            emailBlackList.Items.Add(item);
        }
    }
}
