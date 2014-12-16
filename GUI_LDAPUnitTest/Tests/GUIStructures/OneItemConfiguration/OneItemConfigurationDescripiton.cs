using System.Windows.Forms;
using GUI_LDAPUnitTest.Tests.BusinessLogic;

namespace GUI_LDAPUnitTest.Tests.GUIStructures.OneItemConfiguration
{
    internal class OneItemConfigurationDescripiton : IOneItemConfiguration
    {
        private const string NewDescriptionMessage = "Set the New Description";
        private readonly TestUserRepository _userRepository;

        public OneItemConfigurationDescripiton(TestUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void SetConfiguraionLabel(Label label)
        {
            label.Text = NewDescriptionMessage;
        }

        public void SetConfiguraionTextBox(TextBox textBox)
        {
            textBox.Text = _userRepository.TestUserNewDescription;
        }

        public void SaveUserRepositoryConfiguration(TextBox sourceTextBox)
        {
            _userRepository.TestUserNewDescription = sourceTextBox.Text;
        }
    }
}