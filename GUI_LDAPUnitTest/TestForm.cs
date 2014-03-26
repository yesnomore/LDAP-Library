using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LDAPLibrary;
using System.Configuration;
using System.DirectoryServices.Protocols;

namespace GUI_LDAPUnitTest
{
    public partial class TestForm : Form
    {
        private List<testTriplet> testTripletList;
        private TestManager testManagerObj;
        private ILDAPManager LDAPManagerObj;

        public TestForm()
        {
            InitializeComponent();
            try
            {
                //Set up form controls
                testTripletList = new List<testTriplet>();

                //SetUp the LDAPLibrary & testManager
                setUpLDAPLibrary();
                testManagerObj = new TestManager(LDAPManagerObj);

                currentUserLabel.Text = testManagerObj.getTestUserCN();

                /*
                 * IMPORTANT METHOD: it build up the struct that correlates the interface controls 
                 * with the Controller class who run tests.
                 */
                BuildTriplets();

                setAllStateLabelText("Undefined");
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Cannot load the Config - Standard LDAP Library Init failed{0}Exception Message: {1}", Environment.NewLine, ex.Message),
                                "Error Config Loading",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        #region InterfaceEvents

        #region Menu Events

        /// <summary>
        /// If The user want to set up another config the application restart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SetUpForm setUpForm = new SetUpForm())
            {
                setUpForm.ShowDialog();
            }
            ConfigurationManager.RefreshSection("LDAPLibrary");
        }

        /// <summary>
        /// Application exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Setup Buttons

        private void configModifyUserDescriptionButton_Click(object sender, EventArgs e)
        {
            using (OneItemConfigurationForm setupModifyUserDescriptionForm = new OneItemConfigurationForm(oneItemConfigurationState.newDescription, testManagerObj))
            {

                setupModifyUserDescriptionForm.ShowDialog();
            }
        }

        private void configSearchUserButton_Click(object sender, EventArgs e)
        {
            using (OneItemConfigurationForm setupSearchUserForm = new OneItemConfigurationForm(oneItemConfigurationState.userToSearch, testManagerObj))
            {
                setupSearchUserForm.ShowDialog();
            }
        }

        private void configUserChangePasswordButton_Click(object sender, EventArgs e)
        {
            using (OneItemConfigurationForm setupNewPasswordForm = new OneItemConfigurationForm(oneItemConfigurationState.newPassword, testManagerObj))
            {
                setupNewPasswordForm.ShowDialog();
            }
        }

        /// <summary>
        /// launch the newTestUser form and update the main form at return.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setUserButton_Click(object sender, EventArgs e)
        {
            using (TestUserForm testUserForm = new TestUserForm(ref testManagerObj))
            {
                testUserForm.ShowDialog();
            }
            currentUserLabel.Text = testManagerObj.getTestUserCN();
        }

        #endregion

        #region ComboBox Visibility Events

        private void testCreateUserCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            testCreateUserLabel.Enabled = testCreateUserCheckBox.Checked;
            stateCreateUserLabel.Enabled = testCreateUserCheckBox.Checked;
        }

        private void testModifyUserDescriptionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            testModifyUserDescriptionLabel.Enabled = testModifyUserDescriptionCheckBox.Checked;
            stateModifyUserDescriptionLabel.Enabled = testModifyUserDescriptionCheckBox.Checked;
        }

        private void testSearchUserCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            testSearchUserLabel.Enabled = testSearchUsersCheckBox.Checked;
            stateSearchUsersLabel.Enabled = testSearchUsersCheckBox.Checked;
        }

        private void testConnectUserCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            testConnectUserLabel.Enabled = testConnectUserCheckBox.Checked;
            stateConnectUserLabel.Enabled = testConnectUserCheckBox.Checked;
        }

        private void testSearchUserAndConnectCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            testSearchUserAndConnectLabel.Enabled = testSearchUserAndConnectCheckBox.Checked;
            stateSearchUserAndConnectLabel.Enabled = testSearchUserAndConnectCheckBox.Checked;
        }

        private void testUserChangePasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            testUserChangePasswordLabel.Enabled = testUserChangePasswordCheckBox.Checked;
            stateUserChangePasswordLabel.Enabled = testUserChangePasswordCheckBox.Checked;
        }

        private void testDeleteUserCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            testDeleteUserLabel.Enabled = testDeleteUserCheckBox.Checked;
            stateDeleteUserLabel.Enabled = testDeleteUserCheckBox.Checked;
        }

        #endregion

        #region Other Events

        /// <summary>
        /// Manage the visibility and repositioning of the panels.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readTestsLabel_Click(object sender, EventArgs e)
        {
            if (readTestsPanel.Height == 35)
            {
                //Expand the read and move the write panel
                readTestsPanel.Height = 180;
                readTestIconLabel.Text = "-";
                writeTestsPanel.Location = new Point(writeTestsPanel.Location.X, writeTestsPanel.Location.Y + 150);
            }
            else if (readTestsPanel.Height == 180)
            {
                //Collapse the read panel and write panel repositioned.
                readTestsPanel.Height = 35;
                readTestIconLabel.Text = "+";
                writeTestsPanel.Location = new Point(writeTestsPanel.Location.X, writeTestsPanel.Location.Y - 150);
            }
        }


        private void WriteTestsLabel_Click(object sender, EventArgs e)
        {
            if (writeTestsPanel.Height == 35)
            {
                writeTestsPanel.Height = 220;
                writeTestsIconLabel.Text = "-";
            }
            else
            {
                writeTestsPanel.Height = 35;
                writeTestsIconLabel.Text = "+";
            }
        }


        #endregion

        /// <summary>
        /// Start the tests
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            testsProgressBar.Value = 0;
            setAllStateLabelText("In Progress");

            int progressBarIncrement = 100 / Enum.GetNames(typeof(tests)).Length;
            try
            {

                foreach (testTriplet t in testTripletList)
                {
                    if (t.testCheckbox.Checked)
                    {
                        t.testLabel.Text = "Started";
                        if (testManagerObj.runTest(t.testType,
                                                    Convert.ToBoolean(ConfigurationManager.AppSettings["writePermissions"])
                                                    ))
                        {
                            t.testLabel.Text = "Passed";
                            t.testLabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            t.testLabel.Text = "Failed";
                            t.testLabel.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        t.testLabel.Text = "Skipped";
                    }
                    testsProgressBar.Value += progressBarIncrement;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Qualcosa non ha funzionato nel programma di test");
            }

            testsProgressBar.Value = 100;



            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Set up the LDAPLibrary with minimal configuration.
        /// </summary>
        private void setUpLDAPLibrary()
        {

            object TEST = Config.LDAPLibrary;


            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                        Config.LDAPLibrary["LDAPAuthType"]);

            if (!string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
            {

                LDAPUser adminUser = new LDAPUser(Config.LDAPLibrary["LDAPAdminUserDN"],
                                                Config.LDAPLibrary["LDAPAdminUserCN"],
                                                Config.LDAPLibrary["LDAPAdminUserSN"],
                                                null);

                adminUser.setUserAttribute("userPassword", Config.LDAPLibrary["LDAPAdminUserPassword"]);


                LDAPManagerObj = new LDAPManager(adminUser,
                                                    Config.LDAPLibrary["LDAPServer"],
                                                    Config.LDAPLibrary["LDAPSearchBaseDN"],
                                                    authType
                                                    );
            }
            else
                LDAPManagerObj = new LDAPManager(null,
                                                    Config.LDAPLibrary["LDAPServer"],
                                                    Config.LDAPLibrary["LDAPSearchBaseDN"],
                                                    authType
                                                    );
        }

        /// <summary>
        /// add all controls in relation each other like triplets.
        /// </summary>
        private void BuildTriplets()
        {

            testTripletList.Add(new testTriplet(testStandardInitLibraryNoAdminCheckBox,
                                                tests.testStandardInitLibraryNoAdmin,
                                                stateStandardInitLibraryNoAdminLabel));

            testTripletList.Add(new testTriplet(testInitLibraryNoAdminCheckBox,
                                                tests.testInitLibraryNoAdmin,
                                                stateInitLibraryNoAdminLabel));

            testTripletList.Add(new testTriplet(testInitLibraryCheckBox,
                                                tests.testInitLibrary,
                                                stateInitLibraryLabel));

            testTripletList.Add(new testTriplet(testAdminConnectCheckBox,
                                                tests.testAdminConnection,
                                                stateAdminConnectLabel));

            testTripletList.Add(new testTriplet(testConnectUserCheckBox,
                                                tests.testConnectUser,
                                                stateConnectUserLabel));

            testTripletList.Add(new testTriplet(testSearchUserAndConnectCheckBox,
                                                tests.testSearchUserAndConnect,
                                                stateSearchUserAndConnectLabel));

            testTripletList.Add(new testTriplet(testSearchUsersCheckBox,
                                                tests.testSearchUsers,
                                                stateSearchUsersLabel));

            testTripletList.Add(new testTriplet(testCreateUserCheckBox,
                                                tests.testCreateUser,
                                                stateCreateUserLabel));

            testTripletList.Add(new testTriplet(testModifyUserDescriptionCheckBox,
                                                tests.testModifyUserDescription,
                                                stateModifyUserDescriptionLabel));

            testTripletList.Add(new testTriplet(testUserChangePasswordCheckBox,
                                                tests.testUserChangePassword,
                                                stateUserChangePasswordLabel));

            testTripletList.Add(new testTriplet(testDeleteUserCheckBox,
                                                tests.testDeleteUser,
                                                stateDeleteUserLabel));
        }

        /// <summary>
        /// SetUp Class Statement: list of all state label
        /// Set the text for all state Label in the form
        /// </summary>
        /// <param name="Enabled">Enable Value</param>
        /// <param name="checkedStatus">CheckedStatus</param>
        private void setAllStateLabelText(string text)
        {
            foreach (testTriplet t in testTripletList)
                if (!string.IsNullOrEmpty(text))
                {
                    t.testLabel.Text = text;
                    t.testLabel.ForeColor = Color.Empty;
                }
        }


        #endregion



    }

    public struct testTriplet
    {
        private CheckBox c;
        private tests t;
        private Label l;

        public CheckBox testCheckbox
        {
            get { return c; }
            set { c = value; }
        }
        public tests testType
        {
            get { return t; }
            set { t = value; }
        }
        public Label testLabel
        {
            get { return l; }
            set { l = value; }
        }

        public testTriplet(CheckBox c, tests t, Label l)
        {
            this.c = c;
            this.t = t;
            this.l = l;
        }
    }
}
