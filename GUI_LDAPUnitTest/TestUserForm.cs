using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GUI_LDAPUnitTest
{
    public partial class TestUserForm : Form
    {
        private TestManager testManagerObj;

        public TestUserForm(ref TestManager testManagerObj)
        {
            InitializeComponent();

            this.testManagerObj = testManagerObj;

            Init();

        }


        private void Init()
        {
            testUserCNTextBox.Text = testManagerObj.GetTestUserCn();
            testUserDNTextBox.Text = testManagerObj.GetTestUserDn();
            testUserSNTextBox.Text = testManagerObj.GetTestUserSn();

            foreach (string key in testManagerObj.GetTestUserOtherAttributesKeys())
            {
                foreach (string value in testManagerObj.GetTestUserOtherAttributes(key))
                    testUserOtherTextBox.Text += key + "=" + value + Environment.NewLine;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] attributes = testUserOtherTextBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Dictionary<string, List<string>> testUserAttribute = new Dictionary<string, List<string>>();
            attributes = attributes.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            try
            {
                foreach (string attribute in attributes)
                {
                    string[] temp = new string[2];
                    temp = attribute.Split(new[] { '=' });
                    testUserAttribute.Add(temp[0], new List<string> { temp[1] });
                }

                testManagerObj.SetupTestUser(
                    testUserDNTextBox.Text, testUserCNTextBox.Text, testUserSNTextBox.Text, testUserAttribute);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show("Error: Unable to setup the testUser. Check the inputs. Application will be restarted", "Error Creation Test User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }

        }
    }
}
