namespace GUI_LDAPUnitTest
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.runButton = new System.Windows.Forms.Button();
            this.testsProgressBar = new System.Windows.Forms.ProgressBar();
            this.testsLabel = new System.Windows.Forms.Label();
            this.testInitLibraryLabel = new System.Windows.Forms.Label();
            this.testAdminConnectLabel = new System.Windows.Forms.Label();
            this.testCreateUserLabel = new System.Windows.Forms.Label();
            this.testModifyUserDescriptionLabel = new System.Windows.Forms.Label();
            this.testSearchUserLabel = new System.Windows.Forms.Label();
            this.testConnectUserLabel = new System.Windows.Forms.Label();
            this.testSearchUserAndConnectLabel = new System.Windows.Forms.Label();
            this.testUserChangePasswordLabel = new System.Windows.Forms.Label();
            this.testDeleteUserLabel = new System.Windows.Forms.Label();
            this.stateInitLibraryLabel = new System.Windows.Forms.Label();
            this.stateAdminConnectLabel = new System.Windows.Forms.Label();
            this.stateCreateUserLabel = new System.Windows.Forms.Label();
            this.stateModifyUserDescriptionLabel = new System.Windows.Forms.Label();
            this.stateSearchUsersLabel = new System.Windows.Forms.Label();
            this.stateConnectUserLabel = new System.Windows.Forms.Label();
            this.stateSearchUserAndConnectLabel = new System.Windows.Forms.Label();
            this.stateUserChangePasswordLabel = new System.Windows.Forms.Label();
            this.stateDeleteUserLabel = new System.Windows.Forms.Label();
            this.testCreateUserCheckBox = new System.Windows.Forms.CheckBox();
            this.testModifyUserDescriptionCheckBox = new System.Windows.Forms.CheckBox();
            this.testSearchUsersCheckBox = new System.Windows.Forms.CheckBox();
            this.testConnectUserCheckBox = new System.Windows.Forms.CheckBox();
            this.testSearchUserAndConnectCheckBox = new System.Windows.Forms.CheckBox();
            this.testUserChangePasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.testDeleteUserCheckBox = new System.Windows.Forms.CheckBox();
            this.configUserChangePasswordButton = new System.Windows.Forms.Button();
            this.configSearchUserButton = new System.Windows.Forms.Button();
            this.configModifyUserDescriptionButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeConfigFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineLabel = new System.Windows.Forms.Label();
            this.readTestsPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.readTestIconLabel = new System.Windows.Forms.Label();
            this.readTestsLabel = new System.Windows.Forms.Label();
            this.writeTestsPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.writeTestsIconLabel = new System.Windows.Forms.Label();
            this.WriteTestsLabel = new System.Windows.Forms.Label();
            this.readWriteTestsPanel = new System.Windows.Forms.Panel();
            this.setUserButton = new System.Windows.Forms.Button();
            this.currentUserLabel = new System.Windows.Forms.Label();
            this.TestUserLabel = new System.Windows.Forms.Label();
            this.testAdminConnectCheckBox = new System.Windows.Forms.CheckBox();
            this.testInitLibraryCheckBox = new System.Windows.Forms.CheckBox();
            this.testInitLibraryNoAdminCheckBox = new System.Windows.Forms.CheckBox();
            this.testInitLibraryNoAdminLabel = new System.Windows.Forms.Label();
            this.stateInitLibraryNoAdminLabel = new System.Windows.Forms.Label();
            this.testStandardInitLibraryNoAdminCheckBox = new System.Windows.Forms.CheckBox();
            this.testStandardInitLibraryNoAdminLabel = new System.Windows.Forms.Label();
            this.stateStandardInitLibraryNoAdminLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.readTestsPanel.SuspendLayout();
            this.writeTestsPanel.SuspendLayout();
            this.readWriteTestsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // runButton
            // 
            this.runButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runButton.ForeColor = System.Drawing.Color.DodgerBlue;
            this.runButton.Location = new System.Drawing.Point(26, 675);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(149, 48);
            this.runButton.TabIndex = 4;
            this.runButton.Text = "Run Tests";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // testsProgressBar
            // 
            this.testsProgressBar.Location = new System.Drawing.Point(181, 683);
            this.testsProgressBar.Name = "testsProgressBar";
            this.testsProgressBar.Size = new System.Drawing.Size(420, 31);
            this.testsProgressBar.TabIndex = 5;
            // 
            // testsLabel
            // 
            this.testsLabel.AutoSize = true;
            this.testsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testsLabel.Location = new System.Drawing.Point(25, 34);
            this.testsLabel.Name = "testsLabel";
            this.testsLabel.Size = new System.Drawing.Size(135, 25);
            this.testsLabel.TabIndex = 0;
            this.testsLabel.Text = "Basic Tests";
            this.testsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // testInitLibraryLabel
            // 
            this.testInitLibraryLabel.AutoSize = true;
            this.testInitLibraryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testInitLibraryLabel.Location = new System.Drawing.Point(27, 133);
            this.testInitLibraryLabel.Name = "testInitLibraryLabel";
            this.testInitLibraryLabel.Size = new System.Drawing.Size(142, 18);
            this.testInitLibraryLabel.TabIndex = 2;
            this.testInitLibraryLabel.Text = "Complete Init Library";
            this.testInitLibraryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // testAdminConnectLabel
            // 
            this.testAdminConnectLabel.AutoSize = true;
            this.testAdminConnectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testAdminConnectLabel.Location = new System.Drawing.Point(27, 156);
            this.testAdminConnectLabel.Name = "testAdminConnectLabel";
            this.testAdminConnectLabel.Size = new System.Drawing.Size(155, 18);
            this.testAdminConnectLabel.TabIndex = 3;
            this.testAdminConnectLabel.Text = "Administrator Connect";
            this.testAdminConnectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // testCreateUserLabel
            // 
            this.testCreateUserLabel.AutoSize = true;
            this.testCreateUserLabel.Enabled = false;
            this.testCreateUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testCreateUserLabel.Location = new System.Drawing.Point(43, 59);
            this.testCreateUserLabel.Name = "testCreateUserLabel";
            this.testCreateUserLabel.Size = new System.Drawing.Size(88, 18);
            this.testCreateUserLabel.TabIndex = 4;
            this.testCreateUserLabel.Text = "Create User";
            this.testCreateUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // testModifyUserDescriptionLabel
            // 
            this.testModifyUserDescriptionLabel.AutoSize = true;
            this.testModifyUserDescriptionLabel.Enabled = false;
            this.testModifyUserDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testModifyUserDescriptionLabel.Location = new System.Drawing.Point(43, 100);
            this.testModifyUserDescriptionLabel.Name = "testModifyUserDescriptionLabel";
            this.testModifyUserDescriptionLabel.Size = new System.Drawing.Size(167, 18);
            this.testModifyUserDescriptionLabel.TabIndex = 5;
            this.testModifyUserDescriptionLabel.Text = "Modify User Description";
            this.testModifyUserDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // testSearchUserLabel
            // 
            this.testSearchUserLabel.AutoSize = true;
            this.testSearchUserLabel.Enabled = false;
            this.testSearchUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testSearchUserLabel.Location = new System.Drawing.Point(29, 133);
            this.testSearchUserLabel.Name = "testSearchUserLabel";
            this.testSearchUserLabel.Size = new System.Drawing.Size(99, 18);
            this.testSearchUserLabel.TabIndex = 6;
            this.testSearchUserLabel.Text = "Search Users";
            this.testSearchUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // testConnectUserLabel
            // 
            this.testConnectUserLabel.AutoSize = true;
            this.testConnectUserLabel.Enabled = false;
            this.testConnectUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testConnectUserLabel.Location = new System.Drawing.Point(29, 61);
            this.testConnectUserLabel.Name = "testConnectUserLabel";
            this.testConnectUserLabel.Size = new System.Drawing.Size(100, 18);
            this.testConnectUserLabel.TabIndex = 7;
            this.testConnectUserLabel.Text = "Connect User";
            this.testConnectUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // testSearchUserAndConnectLabel
            // 
            this.testSearchUserAndConnectLabel.AutoSize = true;
            this.testSearchUserAndConnectLabel.Enabled = false;
            this.testSearchUserAndConnectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testSearchUserAndConnectLabel.Location = new System.Drawing.Point(29, 97);
            this.testSearchUserAndConnectLabel.Name = "testSearchUserAndConnectLabel";
            this.testSearchUserAndConnectLabel.Size = new System.Drawing.Size(179, 18);
            this.testSearchUserAndConnectLabel.TabIndex = 8;
            this.testSearchUserAndConnectLabel.Text = "Search User and Connect";
            this.testSearchUserAndConnectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // testUserChangePasswordLabel
            // 
            this.testUserChangePasswordLabel.AutoSize = true;
            this.testUserChangePasswordLabel.Enabled = false;
            this.testUserChangePasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testUserChangePasswordLabel.Location = new System.Drawing.Point(43, 141);
            this.testUserChangePasswordLabel.Name = "testUserChangePasswordLabel";
            this.testUserChangePasswordLabel.Size = new System.Drawing.Size(166, 18);
            this.testUserChangePasswordLabel.TabIndex = 9;
            this.testUserChangePasswordLabel.Text = "Change User Password";
            this.testUserChangePasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // testDeleteUserLabel
            // 
            this.testDeleteUserLabel.AutoSize = true;
            this.testDeleteUserLabel.Enabled = false;
            this.testDeleteUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testDeleteUserLabel.Location = new System.Drawing.Point(43, 180);
            this.testDeleteUserLabel.Name = "testDeleteUserLabel";
            this.testDeleteUserLabel.Size = new System.Drawing.Size(86, 18);
            this.testDeleteUserLabel.TabIndex = 10;
            this.testDeleteUserLabel.Text = "Delete User";
            this.testDeleteUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stateInitLibraryLabel
            // 
            this.stateInitLibraryLabel.AutoSize = true;
            this.stateInitLibraryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateInitLibraryLabel.Location = new System.Drawing.Point(295, 135);
            this.stateInitLibraryLabel.Name = "stateInitLibraryLabel";
            this.stateInitLibraryLabel.Size = new System.Drawing.Size(64, 15);
            this.stateInitLibraryLabel.TabIndex = 12;
            this.stateInitLibraryLabel.Text = "Undefined";
            this.stateInitLibraryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateAdminConnectLabel
            // 
            this.stateAdminConnectLabel.AutoSize = true;
            this.stateAdminConnectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateAdminConnectLabel.Location = new System.Drawing.Point(295, 159);
            this.stateAdminConnectLabel.Name = "stateAdminConnectLabel";
            this.stateAdminConnectLabel.Size = new System.Drawing.Size(64, 15);
            this.stateAdminConnectLabel.TabIndex = 13;
            this.stateAdminConnectLabel.Text = "Undefined";
            this.stateAdminConnectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateCreateUserLabel
            // 
            this.stateCreateUserLabel.AutoSize = true;
            this.stateCreateUserLabel.Enabled = false;
            this.stateCreateUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateCreateUserLabel.Location = new System.Drawing.Point(252, 61);
            this.stateCreateUserLabel.Name = "stateCreateUserLabel";
            this.stateCreateUserLabel.Size = new System.Drawing.Size(64, 15);
            this.stateCreateUserLabel.TabIndex = 14;
            this.stateCreateUserLabel.Text = "Undefined";
            this.stateCreateUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateModifyUserDescriptionLabel
            // 
            this.stateModifyUserDescriptionLabel.AutoSize = true;
            this.stateModifyUserDescriptionLabel.Enabled = false;
            this.stateModifyUserDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateModifyUserDescriptionLabel.Location = new System.Drawing.Point(252, 102);
            this.stateModifyUserDescriptionLabel.Name = "stateModifyUserDescriptionLabel";
            this.stateModifyUserDescriptionLabel.Size = new System.Drawing.Size(64, 15);
            this.stateModifyUserDescriptionLabel.TabIndex = 15;
            this.stateModifyUserDescriptionLabel.Text = "Undefined";
            this.stateModifyUserDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateSearchUsersLabel
            // 
            this.stateSearchUsersLabel.AutoSize = true;
            this.stateSearchUsersLabel.Enabled = false;
            this.stateSearchUsersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateSearchUsersLabel.Location = new System.Drawing.Point(252, 135);
            this.stateSearchUsersLabel.Name = "stateSearchUsersLabel";
            this.stateSearchUsersLabel.Size = new System.Drawing.Size(64, 15);
            this.stateSearchUsersLabel.TabIndex = 16;
            this.stateSearchUsersLabel.Text = "Undefined";
            this.stateSearchUsersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateConnectUserLabel
            // 
            this.stateConnectUserLabel.AutoSize = true;
            this.stateConnectUserLabel.Enabled = false;
            this.stateConnectUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateConnectUserLabel.Location = new System.Drawing.Point(252, 63);
            this.stateConnectUserLabel.Name = "stateConnectUserLabel";
            this.stateConnectUserLabel.Size = new System.Drawing.Size(64, 15);
            this.stateConnectUserLabel.TabIndex = 17;
            this.stateConnectUserLabel.Text = "Undefined";
            this.stateConnectUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateSearchUserAndConnectLabel
            // 
            this.stateSearchUserAndConnectLabel.AutoSize = true;
            this.stateSearchUserAndConnectLabel.Enabled = false;
            this.stateSearchUserAndConnectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateSearchUserAndConnectLabel.Location = new System.Drawing.Point(252, 99);
            this.stateSearchUserAndConnectLabel.Name = "stateSearchUserAndConnectLabel";
            this.stateSearchUserAndConnectLabel.Size = new System.Drawing.Size(64, 15);
            this.stateSearchUserAndConnectLabel.TabIndex = 18;
            this.stateSearchUserAndConnectLabel.Text = "Undefined";
            this.stateSearchUserAndConnectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateUserChangePasswordLabel
            // 
            this.stateUserChangePasswordLabel.AutoSize = true;
            this.stateUserChangePasswordLabel.Enabled = false;
            this.stateUserChangePasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateUserChangePasswordLabel.Location = new System.Drawing.Point(252, 143);
            this.stateUserChangePasswordLabel.Name = "stateUserChangePasswordLabel";
            this.stateUserChangePasswordLabel.Size = new System.Drawing.Size(64, 15);
            this.stateUserChangePasswordLabel.TabIndex = 19;
            this.stateUserChangePasswordLabel.Text = "Undefined";
            this.stateUserChangePasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateDeleteUserLabel
            // 
            this.stateDeleteUserLabel.AutoSize = true;
            this.stateDeleteUserLabel.Enabled = false;
            this.stateDeleteUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateDeleteUserLabel.Location = new System.Drawing.Point(252, 182);
            this.stateDeleteUserLabel.Name = "stateDeleteUserLabel";
            this.stateDeleteUserLabel.Size = new System.Drawing.Size(64, 15);
            this.stateDeleteUserLabel.TabIndex = 20;
            this.stateDeleteUserLabel.Text = "Undefined";
            this.stateDeleteUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // testCreateUserCheckBox
            // 
            this.testCreateUserCheckBox.AutoSize = true;
            this.testCreateUserCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testCreateUserCheckBox.Location = new System.Drawing.Point(8, 63);
            this.testCreateUserCheckBox.Name = "testCreateUserCheckBox";
            this.testCreateUserCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testCreateUserCheckBox.TabIndex = 21;
            this.testCreateUserCheckBox.UseVisualStyleBackColor = true;
            this.testCreateUserCheckBox.CheckedChanged += new System.EventHandler(this.testCreateUserCheckBox_CheckedChanged);
            // 
            // testModifyUserDescriptionCheckBox
            // 
            this.testModifyUserDescriptionCheckBox.AutoSize = true;
            this.testModifyUserDescriptionCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testModifyUserDescriptionCheckBox.Location = new System.Drawing.Point(8, 104);
            this.testModifyUserDescriptionCheckBox.Name = "testModifyUserDescriptionCheckBox";
            this.testModifyUserDescriptionCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testModifyUserDescriptionCheckBox.TabIndex = 22;
            this.testModifyUserDescriptionCheckBox.UseVisualStyleBackColor = true;
            this.testModifyUserDescriptionCheckBox.CheckedChanged += new System.EventHandler(this.testModifyUserDescriptionCheckBox_CheckedChanged);
            // 
            // testSearchUsersCheckBox
            // 
            this.testSearchUsersCheckBox.AutoSize = true;
            this.testSearchUsersCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testSearchUsersCheckBox.Location = new System.Drawing.Point(8, 137);
            this.testSearchUsersCheckBox.Name = "testSearchUsersCheckBox";
            this.testSearchUsersCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testSearchUsersCheckBox.TabIndex = 23;
            this.testSearchUsersCheckBox.UseVisualStyleBackColor = true;
            this.testSearchUsersCheckBox.CheckedChanged += new System.EventHandler(this.testSearchUserCheckBox_CheckedChanged);
            // 
            // testConnectUserCheckBox
            // 
            this.testConnectUserCheckBox.AutoSize = true;
            this.testConnectUserCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testConnectUserCheckBox.Location = new System.Drawing.Point(8, 65);
            this.testConnectUserCheckBox.Name = "testConnectUserCheckBox";
            this.testConnectUserCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testConnectUserCheckBox.TabIndex = 24;
            this.testConnectUserCheckBox.UseVisualStyleBackColor = true;
            this.testConnectUserCheckBox.CheckedChanged += new System.EventHandler(this.testConnectUserCheckBox_CheckedChanged);
            // 
            // testSearchUserAndConnectCheckBox
            // 
            this.testSearchUserAndConnectCheckBox.AutoSize = true;
            this.testSearchUserAndConnectCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testSearchUserAndConnectCheckBox.Location = new System.Drawing.Point(8, 101);
            this.testSearchUserAndConnectCheckBox.Name = "testSearchUserAndConnectCheckBox";
            this.testSearchUserAndConnectCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testSearchUserAndConnectCheckBox.TabIndex = 25;
            this.testSearchUserAndConnectCheckBox.UseVisualStyleBackColor = true;
            this.testSearchUserAndConnectCheckBox.CheckedChanged += new System.EventHandler(this.testSearchUserAndConnectCheckBox_CheckedChanged);
            // 
            // testUserChangePasswordCheckBox
            // 
            this.testUserChangePasswordCheckBox.AutoSize = true;
            this.testUserChangePasswordCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testUserChangePasswordCheckBox.Location = new System.Drawing.Point(8, 145);
            this.testUserChangePasswordCheckBox.Name = "testUserChangePasswordCheckBox";
            this.testUserChangePasswordCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testUserChangePasswordCheckBox.TabIndex = 26;
            this.testUserChangePasswordCheckBox.UseVisualStyleBackColor = true;
            this.testUserChangePasswordCheckBox.CheckedChanged += new System.EventHandler(this.testUserChangePasswordCheckBox_CheckedChanged);
            // 
            // testDeleteUserCheckBox
            // 
            this.testDeleteUserCheckBox.AutoSize = true;
            this.testDeleteUserCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testDeleteUserCheckBox.Location = new System.Drawing.Point(8, 184);
            this.testDeleteUserCheckBox.Name = "testDeleteUserCheckBox";
            this.testDeleteUserCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testDeleteUserCheckBox.TabIndex = 27;
            this.testDeleteUserCheckBox.UseVisualStyleBackColor = true;
            this.testDeleteUserCheckBox.CheckedChanged += new System.EventHandler(this.testDeleteUserCheckBox_CheckedChanged);
            // 
            // configUserChangePasswordButton
            // 
            this.configUserChangePasswordButton.Location = new System.Drawing.Point(352, 137);
            this.configUserChangePasswordButton.Name = "configUserChangePasswordButton";
            this.configUserChangePasswordButton.Size = new System.Drawing.Size(176, 29);
            this.configUserChangePasswordButton.TabIndex = 28;
            this.configUserChangePasswordButton.Text = "Set New Password";
            this.configUserChangePasswordButton.UseVisualStyleBackColor = true;
            this.configUserChangePasswordButton.Click += new System.EventHandler(this.configUserChangePasswordButton_Click);
            // 
            // configSearchUserButton
            // 
            this.configSearchUserButton.Location = new System.Drawing.Point(352, 129);
            this.configSearchUserButton.Name = "configSearchUserButton";
            this.configSearchUserButton.Size = new System.Drawing.Size(176, 29);
            this.configSearchUserButton.TabIndex = 29;
            this.configSearchUserButton.Text = "Set Users to Search";
            this.configSearchUserButton.UseVisualStyleBackColor = true;
            this.configSearchUserButton.Click += new System.EventHandler(this.configSearchUserButton_Click);
            // 
            // configModifyUserDescriptionButton
            // 
            this.configModifyUserDescriptionButton.Location = new System.Drawing.Point(352, 96);
            this.configModifyUserDescriptionButton.Name = "configModifyUserDescriptionButton";
            this.configModifyUserDescriptionButton.Size = new System.Drawing.Size(176, 29);
            this.configModifyUserDescriptionButton.TabIndex = 30;
            this.configModifyUserDescriptionButton.Text = "Set New Description";
            this.configModifyUserDescriptionButton.UseVisualStyleBackColor = true;
            this.configModifyUserDescriptionButton.Click += new System.EventHandler(this.configModifyUserDescriptionButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(649, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeConfigFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // lineLabel
            // 
            this.lineLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lineLabel.Location = new System.Drawing.Point(27, 70);
            this.lineLabel.Name = "lineLabel";
            this.lineLabel.Size = new System.Drawing.Size(574, 2);
            this.lineLabel.TabIndex = 14;
            this.lineLabel.Text = "label1";
            // 
            // readTestsPanel
            // 
            this.readTestsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.readTestsPanel.Controls.Add(this.label2);
            this.readTestsPanel.Controls.Add(this.readTestIconLabel);
            this.readTestsPanel.Controls.Add(this.readTestsLabel);
            this.readTestsPanel.Controls.Add(this.testConnectUserCheckBox);
            this.readTestsPanel.Controls.Add(this.testSearchUserAndConnectCheckBox);
            this.readTestsPanel.Controls.Add(this.testSearchUsersCheckBox);
            this.readTestsPanel.Controls.Add(this.stateConnectUserLabel);
            this.readTestsPanel.Controls.Add(this.configSearchUserButton);
            this.readTestsPanel.Controls.Add(this.stateSearchUserAndConnectLabel);
            this.readTestsPanel.Controls.Add(this.stateSearchUsersLabel);
            this.readTestsPanel.Controls.Add(this.testConnectUserLabel);
            this.readTestsPanel.Controls.Add(this.testSearchUserAndConnectLabel);
            this.readTestsPanel.Controls.Add(this.testSearchUserLabel);
            this.readTestsPanel.Location = new System.Drawing.Point(14, 49);
            this.readTestsPanel.Name = "readTestsPanel";
            this.readTestsPanel.Size = new System.Drawing.Size(545, 180);
            this.readTestsPanel.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(8, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(500, 2);
            this.label2.TabIndex = 34;
            this.label2.Text = "label1";
            // 
            // readTestIconLabel
            // 
            this.readTestIconLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.readTestIconLabel.Location = new System.Drawing.Point(510, 2);
            this.readTestIconLabel.Name = "readTestIconLabel";
            this.readTestIconLabel.Size = new System.Drawing.Size(31, 31);
            this.readTestIconLabel.TabIndex = 34;
            this.readTestIconLabel.Text = "-";
            this.readTestIconLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.readTestIconLabel.Click += new System.EventHandler(this.readTestsLabel_Click);
            // 
            // readTestsLabel
            // 
            this.readTestsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.readTestsLabel.Location = new System.Drawing.Point(3, 3);
            this.readTestsLabel.Name = "readTestsLabel";
            this.readTestsLabel.Size = new System.Drawing.Size(501, 31);
            this.readTestsLabel.TabIndex = 33;
            this.readTestsLabel.Text = "Read Tests";
            this.readTestsLabel.Click += new System.EventHandler(this.readTestsLabel_Click);
            // 
            // writeTestsPanel
            // 
            this.writeTestsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.writeTestsPanel.Controls.Add(this.label1);
            this.writeTestsPanel.Controls.Add(this.writeTestsIconLabel);
            this.writeTestsPanel.Controls.Add(this.WriteTestsLabel);
            this.writeTestsPanel.Controls.Add(this.testCreateUserCheckBox);
            this.writeTestsPanel.Controls.Add(this.testModifyUserDescriptionCheckBox);
            this.writeTestsPanel.Controls.Add(this.stateDeleteUserLabel);
            this.writeTestsPanel.Controls.Add(this.configModifyUserDescriptionButton);
            this.writeTestsPanel.Controls.Add(this.testDeleteUserLabel);
            this.writeTestsPanel.Controls.Add(this.stateCreateUserLabel);
            this.writeTestsPanel.Controls.Add(this.testUserChangePasswordLabel);
            this.writeTestsPanel.Controls.Add(this.testUserChangePasswordCheckBox);
            this.writeTestsPanel.Controls.Add(this.stateUserChangePasswordLabel);
            this.writeTestsPanel.Controls.Add(this.stateModifyUserDescriptionLabel);
            this.writeTestsPanel.Controls.Add(this.testDeleteUserCheckBox);
            this.writeTestsPanel.Controls.Add(this.testCreateUserLabel);
            this.writeTestsPanel.Controls.Add(this.configUserChangePasswordButton);
            this.writeTestsPanel.Controls.Add(this.testModifyUserDescriptionLabel);
            this.writeTestsPanel.Location = new System.Drawing.Point(14, 240);
            this.writeTestsPanel.Name = "writeTestsPanel";
            this.writeTestsPanel.Size = new System.Drawing.Size(545, 220);
            this.writeTestsPanel.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(10, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(500, 2);
            this.label1.TabIndex = 34;
            this.label1.Text = "label1";
            // 
            // writeTestsIconLabel
            // 
            this.writeTestsIconLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.writeTestsIconLabel.Location = new System.Drawing.Point(510, 2);
            this.writeTestsIconLabel.Name = "writeTestsIconLabel";
            this.writeTestsIconLabel.Size = new System.Drawing.Size(31, 31);
            this.writeTestsIconLabel.TabIndex = 32;
            this.writeTestsIconLabel.Text = " -";
            this.writeTestsIconLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.writeTestsIconLabel.Click += new System.EventHandler(this.WriteTestsLabel_Click);
            // 
            // WriteTestsLabel
            // 
            this.WriteTestsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WriteTestsLabel.Location = new System.Drawing.Point(3, 4);
            this.WriteTestsLabel.Name = "WriteTestsLabel";
            this.WriteTestsLabel.Size = new System.Drawing.Size(507, 29);
            this.WriteTestsLabel.TabIndex = 31;
            this.WriteTestsLabel.Text = "Write Tests   ";
            this.WriteTestsLabel.Click += new System.EventHandler(this.WriteTestsLabel_Click);
            // 
            // readWriteTestsPanel
            // 
            this.readWriteTestsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.readWriteTestsPanel.Controls.Add(this.setUserButton);
            this.readWriteTestsPanel.Controls.Add(this.currentUserLabel);
            this.readWriteTestsPanel.Controls.Add(this.TestUserLabel);
            this.readWriteTestsPanel.Controls.Add(this.readTestsPanel);
            this.readWriteTestsPanel.Controls.Add(this.writeTestsPanel);
            this.readWriteTestsPanel.Location = new System.Drawing.Point(30, 195);
            this.readWriteTestsPanel.Name = "readWriteTestsPanel";
            this.readWriteTestsPanel.Size = new System.Drawing.Size(580, 474);
            this.readWriteTestsPanel.TabIndex = 33;
            // 
            // setUserButton
            // 
            this.setUserButton.Location = new System.Drawing.Point(448, 11);
            this.setUserButton.Name = "setUserButton";
            this.setUserButton.Size = new System.Drawing.Size(111, 23);
            this.setUserButton.TabIndex = 35;
            this.setUserButton.Text = "Set User";
            this.setUserButton.UseVisualStyleBackColor = true;
            this.setUserButton.Click += new System.EventHandler(this.setUserButton_Click);
            // 
            // currentUserLabel
            // 
            this.currentUserLabel.AutoSize = true;
            this.currentUserLabel.Location = new System.Drawing.Point(80, 16);
            this.currentUserLabel.Name = "currentUserLabel";
            this.currentUserLabel.Size = new System.Drawing.Size(66, 13);
            this.currentUserLabel.TabIndex = 34;
            this.currentUserLabel.Text = "Current User";
            // 
            // TestUserLabel
            // 
            this.TestUserLabel.AutoSize = true;
            this.TestUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestUserLabel.Location = new System.Drawing.Point(14, 14);
            this.TestUserLabel.Name = "TestUserLabel";
            this.TestUserLabel.Size = new System.Drawing.Size(49, 16);
            this.TestUserLabel.TabIndex = 33;
            this.TestUserLabel.Text = "User: ";
            // 
            // testAdminConnectCheckBox
            // 
            this.testAdminConnectCheckBox.AutoSize = true;
            this.testAdminConnectCheckBox.Checked = true;
            this.testAdminConnectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.testAdminConnectCheckBox.Location = new System.Drawing.Point(11, 163);
            this.testAdminConnectCheckBox.Name = "testAdminConnectCheckBox";
            this.testAdminConnectCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testAdminConnectCheckBox.TabIndex = 35;
            this.testAdminConnectCheckBox.UseVisualStyleBackColor = true;
            this.testAdminConnectCheckBox.Visible = false;
            // 
            // testInitLibraryCheckBox
            // 
            this.testInitLibraryCheckBox.AutoSize = true;
            this.testInitLibraryCheckBox.Checked = true;
            this.testInitLibraryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.testInitLibraryCheckBox.Location = new System.Drawing.Point(11, 137);
            this.testInitLibraryCheckBox.Name = "testInitLibraryCheckBox";
            this.testInitLibraryCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testInitLibraryCheckBox.TabIndex = 36;
            this.testInitLibraryCheckBox.UseVisualStyleBackColor = true;
            this.testInitLibraryCheckBox.Visible = false;
            // 
            // testInitLibraryNoAdminCheckBox
            // 
            this.testInitLibraryNoAdminCheckBox.AutoSize = true;
            this.testInitLibraryNoAdminCheckBox.Checked = true;
            this.testInitLibraryNoAdminCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.testInitLibraryNoAdminCheckBox.Location = new System.Drawing.Point(11, 113);
            this.testInitLibraryNoAdminCheckBox.Name = "testInitLibraryNoAdminCheckBox";
            this.testInitLibraryNoAdminCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testInitLibraryNoAdminCheckBox.TabIndex = 39;
            this.testInitLibraryNoAdminCheckBox.UseVisualStyleBackColor = true;
            this.testInitLibraryNoAdminCheckBox.Visible = false;
            // 
            // testInitLibraryNoAdminLabel
            // 
            this.testInitLibraryNoAdminLabel.AutoSize = true;
            this.testInitLibraryNoAdminLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testInitLibraryNoAdminLabel.Location = new System.Drawing.Point(27, 110);
            this.testInitLibraryNoAdminLabel.Name = "testInitLibraryNoAdminLabel";
            this.testInitLibraryNoAdminLabel.Size = new System.Drawing.Size(211, 18);
            this.testInitLibraryNoAdminLabel.TabIndex = 37;
            this.testInitLibraryNoAdminLabel.Text = "Complete Init Library No Admin";
            this.testInitLibraryNoAdminLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stateInitLibraryNoAdminLabel
            // 
            this.stateInitLibraryNoAdminLabel.AutoSize = true;
            this.stateInitLibraryNoAdminLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateInitLibraryNoAdminLabel.Location = new System.Drawing.Point(295, 113);
            this.stateInitLibraryNoAdminLabel.Name = "stateInitLibraryNoAdminLabel";
            this.stateInitLibraryNoAdminLabel.Size = new System.Drawing.Size(64, 15);
            this.stateInitLibraryNoAdminLabel.TabIndex = 38;
            this.stateInitLibraryNoAdminLabel.Text = "Undefined";
            this.stateInitLibraryNoAdminLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // testStandardInitLibraryNoAdminCheckBox
            // 
            this.testStandardInitLibraryNoAdminCheckBox.AutoSize = true;
            this.testStandardInitLibraryNoAdminCheckBox.Checked = true;
            this.testStandardInitLibraryNoAdminCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.testStandardInitLibraryNoAdminCheckBox.Location = new System.Drawing.Point(11, 92);
            this.testStandardInitLibraryNoAdminCheckBox.Name = "testStandardInitLibraryNoAdminCheckBox";
            this.testStandardInitLibraryNoAdminCheckBox.Size = new System.Drawing.Size(15, 14);
            this.testStandardInitLibraryNoAdminCheckBox.TabIndex = 42;
            this.testStandardInitLibraryNoAdminCheckBox.UseVisualStyleBackColor = true;
            this.testStandardInitLibraryNoAdminCheckBox.Visible = false;
            // 
            // testStandardInitLibraryNoAdminLabel
            // 
            this.testStandardInitLibraryNoAdminLabel.AutoSize = true;
            this.testStandardInitLibraryNoAdminLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testStandardInitLibraryNoAdminLabel.Location = new System.Drawing.Point(27, 88);
            this.testStandardInitLibraryNoAdminLabel.Name = "testStandardInitLibraryNoAdminLabel";
            this.testStandardInitLibraryNoAdminLabel.Size = new System.Drawing.Size(203, 18);
            this.testStandardInitLibraryNoAdminLabel.TabIndex = 40;
            this.testStandardInitLibraryNoAdminLabel.Text = "Stardard Init Library No Admin";
            this.testStandardInitLibraryNoAdminLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stateStandardInitLibraryNoAdminLabel
            // 
            this.stateStandardInitLibraryNoAdminLabel.AutoSize = true;
            this.stateStandardInitLibraryNoAdminLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateStandardInitLibraryNoAdminLabel.Location = new System.Drawing.Point(295, 90);
            this.stateStandardInitLibraryNoAdminLabel.Name = "stateStandardInitLibraryNoAdminLabel";
            this.stateStandardInitLibraryNoAdminLabel.Size = new System.Drawing.Size(64, 15);
            this.stateStandardInitLibraryNoAdminLabel.TabIndex = 41;
            this.stateStandardInitLibraryNoAdminLabel.Text = "Undefined";
            this.stateStandardInitLibraryNoAdminLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 737);
            this.Controls.Add(this.testStandardInitLibraryNoAdminCheckBox);
            this.Controls.Add(this.testStandardInitLibraryNoAdminLabel);
            this.Controls.Add(this.stateStandardInitLibraryNoAdminLabel);
            this.Controls.Add(this.testInitLibraryNoAdminCheckBox);
            this.Controls.Add(this.testInitLibraryNoAdminLabel);
            this.Controls.Add(this.stateInitLibraryNoAdminLabel);
            this.Controls.Add(this.testAdminConnectCheckBox);
            this.Controls.Add(this.testInitLibraryCheckBox);
            this.Controls.Add(this.readWriteTestsPanel);
            this.Controls.Add(this.lineLabel);
            this.Controls.Add(this.testsLabel);
            this.Controls.Add(this.testAdminConnectLabel);
            this.Controls.Add(this.testInitLibraryLabel);
            this.Controls.Add(this.testsProgressBar);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.stateAdminConnectLabel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.stateInitLibraryLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "TestForm";
            this.Text = "GUI_LDAPUnitTest";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.readTestsPanel.ResumeLayout(false);
            this.readTestsPanel.PerformLayout();
            this.writeTestsPanel.ResumeLayout(false);
            this.writeTestsPanel.PerformLayout();
            this.readWriteTestsPanel.ResumeLayout(false);
            this.readWriteTestsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.ProgressBar testsProgressBar;
        private System.Windows.Forms.Label testsLabel;
        private System.Windows.Forms.Label testInitLibraryLabel;
        private System.Windows.Forms.Label testAdminConnectLabel;
        private System.Windows.Forms.Label testCreateUserLabel;
        private System.Windows.Forms.Label testModifyUserDescriptionLabel;
        private System.Windows.Forms.Label testSearchUserLabel;
        private System.Windows.Forms.Label testConnectUserLabel;
        private System.Windows.Forms.Label testSearchUserAndConnectLabel;
        private System.Windows.Forms.Label testUserChangePasswordLabel;
        private System.Windows.Forms.Label testDeleteUserLabel;
        private System.Windows.Forms.Label stateInitLibraryLabel;
        private System.Windows.Forms.Label stateAdminConnectLabel;
        private System.Windows.Forms.Label stateCreateUserLabel;
        private System.Windows.Forms.Label stateModifyUserDescriptionLabel;
        private System.Windows.Forms.Label stateSearchUsersLabel;
        private System.Windows.Forms.Label stateConnectUserLabel;
        private System.Windows.Forms.Label stateSearchUserAndConnectLabel;
        private System.Windows.Forms.Label stateUserChangePasswordLabel;
        private System.Windows.Forms.Label stateDeleteUserLabel;
        private System.Windows.Forms.CheckBox testCreateUserCheckBox;
        private System.Windows.Forms.CheckBox testModifyUserDescriptionCheckBox;
        private System.Windows.Forms.CheckBox testSearchUsersCheckBox;
        private System.Windows.Forms.CheckBox testConnectUserCheckBox;
        private System.Windows.Forms.CheckBox testSearchUserAndConnectCheckBox;
        private System.Windows.Forms.CheckBox testUserChangePasswordCheckBox;
        private System.Windows.Forms.CheckBox testDeleteUserCheckBox;
        private System.Windows.Forms.Button configUserChangePasswordButton;
        private System.Windows.Forms.Button configSearchUserButton;
        private System.Windows.Forms.Button configModifyUserDescriptionButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeConfigFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label lineLabel;
        private System.Windows.Forms.Panel readTestsPanel;
        private System.Windows.Forms.Panel writeTestsPanel;
        private System.Windows.Forms.Label WriteTestsLabel;
        private System.Windows.Forms.Panel readWriteTestsPanel;
        private System.Windows.Forms.Label readTestIconLabel;
        private System.Windows.Forms.Label readTestsLabel;
        private System.Windows.Forms.Label writeTestsIconLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TestUserLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button setUserButton;
        private System.Windows.Forms.Label currentUserLabel;
        private System.Windows.Forms.CheckBox testAdminConnectCheckBox;
        private System.Windows.Forms.CheckBox testInitLibraryCheckBox;
        private System.Windows.Forms.CheckBox testInitLibraryNoAdminCheckBox;
        private System.Windows.Forms.Label testInitLibraryNoAdminLabel;
        private System.Windows.Forms.Label stateInitLibraryNoAdminLabel;
        private System.Windows.Forms.CheckBox testStandardInitLibraryNoAdminCheckBox;
        private System.Windows.Forms.Label testStandardInitLibraryNoAdminLabel;
        private System.Windows.Forms.Label stateStandardInitLibraryNoAdminLabel;
    }
}

