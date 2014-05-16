using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GUI_LDAPUnitTest
{
    public partial class TestUserForm : Form
    {
        private readonly TestManager _testManagerObj;

        public TestUserForm(ref TestManager testManagerObj)
        {
            InitializeComponent();

            _testManagerObj = testManagerObj;

            Init();

        }


        private void Init()
        {
            testUserCNTextBox.Text = _testManagerObj.GetTestUserCn();
            testUserDNTextBox.Text = _testManagerObj.GetTestUserDn();
            testUserSNTextBox.Text = _testManagerObj.GetTestUserSn();

            foreach (string key in _testManagerObj.GetTestUserOtherAttributesKeys())
            {
                foreach (string value in _testManagerObj.GetTestUserOtherAttributes(key))
                    testUserOtherTextBox.Text += key + @"=" + value + Environment.NewLine;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] attributes = testUserOtherTextBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var testUserAttribute = new Dictionary<string, List<string>>();
            attributes = attributes.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            try
            {
                foreach (string attribute in attributes)
                {
                    string[] temp = attribute.Split(new[] { '=' });
                    testUserAttribute.Add(temp[0], new List<string> { temp[1] });
                }

                _testManagerObj.SetupTestUser(
                    testUserDNTextBox.Text, testUserCNTextBox.Text, testUserSNTextBox.Text, testUserAttribute);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show(@"Error: Unable to setup the testUser. Check the inputs. Application will be restarted", @"Error Creation Test User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }

        }
    }
}
