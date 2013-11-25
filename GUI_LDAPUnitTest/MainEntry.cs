using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LDAPLibrary;

namespace GUI_LDAPUnitTest
{
	static class MainEntry
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			TestForm testUserForm = new TestForm();
			Application.Run(testUserForm);
			
		}
	}
}
