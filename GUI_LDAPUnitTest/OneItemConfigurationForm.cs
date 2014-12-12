using System;
using System.Linq;
using System.Windows.Forms;

namespace GUI_LDAPUnitTest
{
    public partial class OneItemConfigurationForm : Form
    {
        private readonly OneItemConfigurationState _oneItemConfigurationMode;
        private readonly TestUserRepository _userRepository;

        public OneItemConfigurationForm(OneItemConfigurationState oneItemConfigurationMode, TestUserRepository userRepository)
        {
            InitializeComponent();
            _oneItemConfigurationMode = oneItemConfigurationMode;
            _userRepository = userRepository;
            switch (oneItemConfigurationMode)
            {
                case OneItemConfigurationState.NewDescription:
                    oneItemConfigurationLabel.Text = "Set the New Description";
                    oneItemConfigurationTextBox.Text = _userRepository.TestUserNewDescription;
                    break;
                case OneItemConfigurationState.NewPassword:
                    oneItemConfigurationLabel.Text = "Set the New Password";
                    oneItemConfigurationTextBox.Text = _userRepository.TestUserNewPassword;
                    break;
                case OneItemConfigurationState.UserToSearch:
                    oneItemConfigurationLabel.Text = "Set the Users to search (separated by breaklines)";
                    oneItemConfigurationTextBox.Multiline = true;
                    oneItemConfigurationTextBox.Height = 60;
                    oneItemConfigurationTextBox.ScrollBars = ScrollBars.Both;
                    string users = _userRepository.GetUserToSearch()
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
                    _userRepository.TestUserNewDescription = oneItemConfigurationTextBox.Text;
                    break;
                case OneItemConfigurationState.NewPassword:
                    _userRepository.TestUserNewPassword = oneItemConfigurationTextBox.Text;
                    break;
                case OneItemConfigurationState.UserToSearch:
                    string[] users = oneItemConfigurationTextBox.Text.Split(new[] {Environment.NewLine},
                        StringSplitOptions.None);
                    _userRepository.SetupUsersToSearch(users);
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