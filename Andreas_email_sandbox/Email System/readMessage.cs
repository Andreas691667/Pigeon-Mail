using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MimeKit;

namespace Email_System
{
    public partial class readMessage : Form
    {
        MimeMessage message;
        public readMessage(MimeMessage m)
        {
            InitializeComponent();
            message = m;

            fromTb.Text = message.From.ToString();
        }
    }
}
