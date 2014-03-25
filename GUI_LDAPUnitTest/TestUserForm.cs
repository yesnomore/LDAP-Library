using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LDAPLibrary;
using System.Configuration;

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
            testUserCNTextBox.Text = testManagerObj.getTestUserCN();
            testUserDNTextBox.Text = testManagerObj.getTestUserDN();
            testUserSNTextBox.Text = testManagerObj.getTestUserSN();

            foreach (string key in testManagerObj.getTestUserOtherAttributesKeys())
            {
                foreach (string value in testManagerObj.getTestUserOtherAttributes(key))
                    testUserOtherTextBox.Text += key + "=" + value + Environment.NewLine;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] attributes = testUserOtherTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Dictionary<string, List<string>> testUserAttribute = new Dictionary<string, List<string>>();
            attributes = attributes.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            try
            {
                foreach (string attribute in attributes)
                {
                    string[] temp = new string[2];
                    temp = attribute.Split(new char[] { '=' });
                    testUserAttribute.Add(temp[0], new List<string>() { temp[1] });
                }

                testManagerObj.setupTestUser(
                    testUserDNTextBox.Text, testUserCNTextBox.Text, testUserSNTextBox.Text, testUserAttribute);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Error: Unable to setup the testUser. Check the inputs. Application will be restarted", "Error Creation Test User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
            }

        }
    }
}
