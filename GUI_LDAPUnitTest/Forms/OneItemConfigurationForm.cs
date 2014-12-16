using System;
using System.Windows.Forms;
using GUI_LDAPUnitTest.Tests.BusinessLogic;
using GUI_LDAPUnitTest.Tests.GUIStructures;
using GUI_LDAPUnitTest.Tests.GUIStructures.OneItemConfiguration;

namespace GUI_LDAPUnitTest.Forms
{
    public partial class OneItemConfigurationForm : Form
    {
        private readonly IOneItemConfiguration _oneItemConfiguration;

        public OneItemConfigurationForm(OneItemConfigurationState oneItemConfigurationMode,
            TestUserRepository userRepository)
        {
            InitializeComponent();
            _oneItemConfiguration = OneItemConfigurationFactory.GetOneItemConfiguration(oneItemConfigurationMode,
                userRepository);
            _oneItemConfiguration.SetConfiguraionLabel(oneItemConfigurationLabel);
            _oneItemConfiguration.SetConfiguraionTextBox(oneItemConfigurationTextBox);
        }

        private void oneItemConfigurationButton_Click(object sender, EventArgs e)
        {
            _oneItemConfiguration.SaveUserRepositoryConfiguration(oneItemConfigurationTextBox);
            Close();
        }
    }
}