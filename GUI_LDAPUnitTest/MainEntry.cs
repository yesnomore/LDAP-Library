using System;
using System.Windows.Forms;

namespace GUI_LDAPUnitTest
{
    internal static class MainEntry
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var testUserForm = new TestForm();
            Application.Run(testUserForm);
        }
    }
}