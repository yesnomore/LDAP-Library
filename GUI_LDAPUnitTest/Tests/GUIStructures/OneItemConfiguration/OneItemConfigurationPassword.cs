using System;
using System.Windows.Forms;
using GUI_LDAPUnitTest.Tests.BusinessLogic;

namespace GUI_LDAPUnitTest.Tests.GUIStructures.OneItemConfiguration
{
    internal class OneItemConfigurationPassword : IOneItemConfiguration
    {
        private readonly TestUserRepository _userRepository;
        private const string NewPasswordMessage = "Set the New Password";

        public OneItemConfigurationPassword(TestUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void SetConfiguraionLabel(Label label)
        {
            label.Text = NewPasswordMessage;
        }

        public void SetConfiguraionTextBox(TextBox textBox)
        {
            textBox.Text = _userRepository.TestUserNewPassword;
        }

        public void SaveUserRepositoryConfiguration(TextBox sourceTextBox)
        {
            _userRepository.TestUserNewPassword = sourceTextBox.Text;
        }
    }
}