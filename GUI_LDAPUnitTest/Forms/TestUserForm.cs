using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            string[] attributes = testUserOtherTextBox.Text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            var testUserAttribute = new Dictionary<string, List<string>>();
            attributes = attributes.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            try
            {
                foreach (string attribute in attributes)
                {
                    string[] temp = attribute.Split(new[] {'='});
                    testUserAttribute.Add(temp[0], new List<string> {temp[1]});
                }

                _testUserRepository.SetupTestUser(
                    testUserDNTextBox.Text, testUserCNTextBox.Text, testUserSNTextBox.Text, testUserAttribute);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show(
                    @"Error: Unable to setup the testUser. Check the inputs. Application will be restarted",
                    @"Error Creation Test User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}