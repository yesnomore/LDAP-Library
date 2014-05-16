using System;
using System.Windows.Forms;

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

            var testUserForm = new TestForm();
            Application.Run(testUserForm);

        }
    }
}
