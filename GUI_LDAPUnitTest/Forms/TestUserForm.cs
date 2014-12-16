using System;
using System.Windows.Forms;
using GUI_LDAPUnitTest.Tests.BusinessLogic;
using GUI_LDAPUnitTest.Tests.GUIStructures;

namespace GUI_LDAPUnitTest.Forms
{
    public partial class TestUserForm : Form
    {
        private readonly TestUserRepository _testUserRepository;

        public TestUserForm(TestUserRepository testUserRepository)
        {
            InitializeComponent();

            _testUserRepository = testUserRepository;

            Init();
        }


        private void Init()
        {
            testUserCNTextBox.Text = _testUserRepository.GetTestUserCn();
            testUserDNTextBox.Text = _testUserRepository.GetTestUserDn();
            testUserSNTextBox.Text = _testUserRepository.GetTestUserSn();

            foreach (string key in _testUserRepository.GetTestUserOtherAttributesKeys())
            {
                foreach (string value in _testUserRepository.GetTestUserOtherAttributes(key))
                    testUserOtherTextBox.Text += key + @"=" + value + Environment.NewLine;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _testUserRepository.SetupTestUser(
                    testUserDNTextBox.Text, testUserCNTextBox.Text, testUserSNTextBox.Text, TestUserAttributeStringParser.ParseTestUserAttributes(testUserOtherTextBox.Text));
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show(
                    @"Error: Unable to setup the testUser. Check the inputs.",
                    @"Error Creation Test User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}