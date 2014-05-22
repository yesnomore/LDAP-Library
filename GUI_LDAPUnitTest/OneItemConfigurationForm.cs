using System;
using System.Windows.Forms;

namespace GUI_LDAPUnitTest
{
	public partial class OneItemConfigurationForm : Form
	{
		private oneItemConfigurationState oneItemConfigurationMode;
		private TestManager testManager;
		public OneItemConfigurationForm(oneItemConfigurationState oneItemConfigurationMode,TestManager tf)
		{
			InitializeComponent();
			this.oneItemConfigurationMode = oneItemConfigurationMode;
			testManager = tf;
			switch (oneItemConfigurationMode)
			{
				case oneItemConfigurationState.newDescription:
					oneItemConfigurationLabel.Text = "Set the New Description";
					oneItemConfigurationTextBox.Text = tf.GetTestUserNewDescription();
					break;
				case oneItemConfigurationState.newPassword:
					oneItemConfigurationLabel.Text = "Set the New Password";
					oneItemConfigurationTextBox.Text = tf.GetTestUserNewPassword();
					break;
				case oneItemConfigurationState.userToSearch:
					oneItemConfigurationLabel.Text = "Set the Users to search (separated by breaklines)";
					oneItemConfigurationTextBox.Multiline = true;
					oneItemConfigurationTextBox.Height = 60;
					oneItemConfigurationTextBox.ScrollBars = ScrollBars.Both;
					string users = null;
					foreach (string user in tf.GetUserToSearch()) users += user + Environment.NewLine;
					oneItemConfigurationTextBox.Text = users;
					break;
			}
		}

		private void oneItemConfigurationButton_Click(object sender, EventArgs e)
		{
			switch (oneItemConfigurationMode)
			{
				case oneItemConfigurationState.newDescription:
					testManager.SetupTestUserNewDescription(oneItemConfigurationTextBox.Text);
					break;
				case oneItemConfigurationState.newPassword:
					testManager.SetupTestUserNewPassword(oneItemConfigurationTextBox.Text);
					break;
				case oneItemConfigurationState.userToSearch:
					string[] users = oneItemConfigurationTextBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
					testManager.SetupUsersToSearch(users);
					break;
			}
			Close();
		}
	}

	public enum oneItemConfigurationState 
	{
		newPassword,
		newDescription,
		userToSearch
	}
}
