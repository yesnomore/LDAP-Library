using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace GUI_LDAPUnitTest
{
	public partial class SetUpForm : Form
	{
        
		OpenFileDialog ofd_appConfigPath;

		public SetUpForm()
		{
			InitializeComponent();
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection LDAPLibrarySection = (ConfigurationSection)configFile.GetSection("LDAPLibrary");
			ofd_appConfigPath = new OpenFileDialog();
			ofd_appConfigPath.Filter = "App Config|*.config";
            appConfigPathTextBox.Text = LDAPLibrarySection.SectionInformation.ConfigSource;
		}

		private void appConfigPathButton_Click(object sender, EventArgs e)
		{
			if (ofd_appConfigPath.ShowDialog() == DialogResult.OK) 
				appConfigPathTextBox.Text = ofd_appConfigPath.FileName;
		}

		private void finishSetUpButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

			if (!string.IsNullOrEmpty(appConfigPathTextBox.Text))
			{
				string path = appConfigPathTextBox.Text;
				try
				{
					//Check  if is valid Path
					if (IsValidPath(path))
					{
						SetConfigFileAtRuntime(path);
						this.DialogResult = DialogResult.OK;
						this.Close();
					}
				}
				catch 
				{
					MessageBox.Show("Cannot load the Config - Path or File Structure Invalid", "Error Config Loading",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		
		/// <summary>
		/// Test if is a valid path. 
		/// Stolen from stackoverflow.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private bool IsValidPath(string path)
		{
			try
			{
				path = path.Replace(@"\\", ":"); // to cancel out c:\\\\test.text
				string temp = Path.GetPathRoot(path); //For cases like: \text.txt
				if (temp.StartsWith(@"\"))
					return false;
				string pt = Path.GetFullPath(path);
			}
			catch //(Exception NotSupportedException) // catch specific exception here or not if you want
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Sets the config file at runtime.
		/// http://www.nitrix-reloaded.com/2010/08/31/using-external-configuration-files-in-net-applications-c/
		/// </summary>
		/// <param name="configFilePath"></param>
		public void SetConfigFileAtRuntime(string configFilePath)
		{
            //Remove previously key in LDAPLibraryConfig
        //    removeActualKeys(Config.LDAPLibrary);

			// Specify config settings at runtime.
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationSection section = (ConfigurationSection)config.GetSection("LDAPLibrary");

            FileStream fs = new FileStream(configFilePath,FileMode.Open);
            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "tempConfig.config", FileMode.Create);
            CopyTo(fs, fs1);
            //fs.CopyTo(fs1);
            fs.Flush();
            fs1.Flush();
            fs.Close();
            fs1.Close();

            section.SectionInformation.ConfigSource = "tempConfig.config";
			//This doesn't actually going to overwrite you Exe App.Config file.
			//Just refreshing the content in the memory.
			config.Save(ConfigurationSaveMode.Modified);

			//Refreshing Config Section
			ConfigurationManager.RefreshSection("LDAPLibrary");
		}

        private void removeActualKeys(System.Collections.Specialized.NameValueCollection sectionKeysCollection)
        {
            foreach (string s in sectionKeysCollection.Keys) 
                sectionKeysCollection.Remove(s);
        }

        // Only useful before .NET 4
        private void CopyTo(Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024]; // Fairly arbitrary size
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
    
	}
}
