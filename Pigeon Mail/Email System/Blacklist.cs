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
        private void initializeData()
        {
            // get black listed emails and words
            if (black_list_emails.Count > 0)
            {
                foreach (string email in black_list_emails)
                {
                    emailBlackList.Items.Add(email);
                }
            }

            if (black_list_words.Count > 0)
            {
                foreach (string word in black_list_words)
                {
                    wordBlackList.Items.Add(word);
                }
            }
        }

        private string BLACK_EMAIL_TYPE = "BLACK_EMAIL";
        private string BLACK_WORD_TYPE = "BLACK_WORD";

        // Add black email button
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(emailTextBox.Text)) return;
            addItemToBlackList(BLACK_EMAIL_TYPE, emailTextBox.Text);
            emailTextBox.Clear();
        }

        // Add back word button
        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(wordTextBox.Text)) return;
            addItemToBlackList(BLACK_WORD_TYPE, wordTextBox.Text);
            wordTextBox.Clear();
        }

        private void addItemToBlackList(string type, string item)
        {
            if (type == BLACK_EMAIL_TYPE)
            {
                emailBlackList.Items.Add(item);
                black_list_emails.Add(item);
            }

            if (type == BLACK_WORD_TYPE)
            {
                wordBlackList.Items.Add(item);
                black_list_words.Add(item);
            }

        }

        // Delete word from blacklist clicked
        private void button3_Click(object sender, EventArgs e)
        {

            var selectedItem = wordBlackList.SelectedItems[0];
            if (selectedItem != null)
            {
                Data.black_list_words.Remove(selectedItem.Text);
                wordBlackList.Items.Remove(selectedItem);
            }
        }

        // Delete email from blacklist cliced
        private void button2_Click(object sender, EventArgs e)
        {
            var selectedItem = emailBlackList.SelectedItems[0];
            if (selectedItem != null)
            {
                Data.black_list_emails.Remove(selectedItem.Text);
                emailBlackList.Items.Remove(selectedItem);
            }
        }
    }
}
