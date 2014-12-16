using System.Windows.Forms;

namespace GUI_LDAPUnitTest.Tests.GUIStructures.OneItemConfiguration
{
    public interface IOneItemConfiguration
    {
        void SetConfiguraionLabel(Label label);
        void SetConfiguraionTextBox(TextBox textBox);
        void SaveUserRepositoryConfiguration(TextBox sourceTextBox);
    }
}