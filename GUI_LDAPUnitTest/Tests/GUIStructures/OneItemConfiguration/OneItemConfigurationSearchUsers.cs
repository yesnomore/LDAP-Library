using System;
using System.Linq;
using System.Windows.Forms;
using GUI_LDAPUnitTest.Tests.BusinessLogic;

namespace GUI_LDAPUnitTest.Tests.GUIStructures.OneItemConfiguration
{
    internal class OneItemConfigurationSearchUsers : IOneItemConfiguration
    {
        private const string NewSearchUsersMessage = "Set the Users to search (separated by breaklines)";
        private readonly TestUserRepository _userRepository;

        public OneItemConfigurationSearchUsers(TestUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public void SetConfiguraionLabel(Label label)
        {
            label.Text = NewSearchUsersMessage;
        }

        public void SetConfiguraionTextBox(TextBox textBox)
        {
            textBox.Multiline = true;
            textBox.Height = 60;
            textBox.ScrollBars = ScrollBars.Both;
            string users = _userRepository.GetUserToSearch()
                .Aggregate<string, string>(null, (current, user) => current + (user + Environment.NewLine));
            textBox.Text = users;
        }

        public void SaveUserRepositoryConfiguration(TextBox sourceTextBox)
        {
            string[] users = sourceTextBox.Text.Split(new[] {Environment.NewLine},
                StringSplitOptions.None);
            _userRepository.SetupUsersToSearch(users);
        }
    }
}