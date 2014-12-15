using System;
using System.Windows.Forms;
using GUI_LDAPUnitTest.Tests.BusinessLogic;

namespace GUI_LDAPUnitTest.Tests.GUIStructures.OneItemConfiguration
{
    internal class OneItemConfigurationSearchUsers : IOneItemConfiguration
    {
        private TestUserRepository _userRepository;

        public OneItemConfigurationSearchUsers(TestUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public void SetConfiguraionLabel(Label label)
        {
            throw new NotImplementedException();
        }

        public void SetConfiguraionTextBox(TextBox textBox)
        {
            throw new NotImplementedException();
        }

        public void SaveUserRepositoryConfiguration(TextBox sourceTextBox)
        {
            throw new NotImplementedException();
        }
    }
}