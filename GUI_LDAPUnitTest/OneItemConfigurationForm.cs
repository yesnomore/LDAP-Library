using System;
using System.Linq;
using System.Windows.Forms;

namespace GUI_LDAPUnitTest
{
    public partial class OneItemConfigurationForm : Form
    {
        private readonly OneItemConfigurationState _oneItemConfigurationMode;
        private readonly TestManager testManager;

        public OneItemConfigurationForm(OneItemConfigurationState oneItemConfigurationMode, TestManager tf)
        {
            InitializeComponent();
            _oneItemConfigurationMode = oneItemConfigurationMode;
            testManager = tf;
            switch (oneItemConfigurationMode)
            {
                case OneItemConfigurationState.NewDescription:
                    oneItemConfigurationLabel.Text = "Set the New Description";
                    oneItemConfigurationTextBox.Text = tf.GetTestUserNewDescription();
                    break;
                case OneItemConfigurationState.NewPassword:
                    oneItemConfigurationLabel.Text = "Set the New Password";
                    oneItemConfigurationTextBox.Text = tf.GetTestUserNewPassword();
                    break;
                case OneItemConfigurationState.UserToSearch:
                    oneItemConfigurationLabel.Text = "Set the Users to search (separated by breaklines)";
                    oneItemConfigurationTextBox.Multiline = true;
                    oneItemConfigurationTextBox.Height = 60;
                    oneItemConfigurationTextBox.ScrollBars = ScrollBars.Both;
                    string users = tf.GetUserToSearch()
                        .Aggregate<string, string>(null, (current, user) => current + (user + Environment.NewLine));
                    oneItemConfigurationTextBox.Text = users;
                    break;
            }
        }

        private void oneItemConfigurationButton_Click(object sender, EventArgs e)
        {
            switch (_oneItemConfigurationMode)
            {
                case OneItemConfigurationState.NewDescription:
                    testManager.SetupTestUserNewDescription(oneItemConfigurationTextBox.Text);
                    break;
                case OneItemConfigurationState.NewPassword:
                    testManager.SetupTestUserNewPassword(oneItemConfigurationTextBox.Text);
                    break;
                case OneItemConfigurationState.UserToSearch:
                    string[] users = oneItemConfigurationTextBox.Text.Split(new[] {Environment.NewLine},
                        StringSplitOptions.None);
                    testManager.SetupUsersToSearch(users);
                    break;
            }
            Close();
        }
    }

    public enum OneItemConfigurationState
    {
        NewPassword,
        NewDescription,
        UserToSearch
    }
}