using System.Windows.Forms;

namespace GUI_LDAPUnitTest
{
    public struct TestTriplet
    {
        private CheckBox _c;
        private Tests _t;
        private Label _l;

        public CheckBox TestCheckbox
        {
            get { return _c; }
            set { _c = value; }
        }
        public Tests TestType
        {
            get { return _t; }
            set { _t = value; }
        }
        public Label TestLabel
        {
            get { return _l; }
            set { _l = value; }
        }

        public TestTriplet(CheckBox c, Tests t, Label l)
        {
            _c = c;
            _t = t;
            _l = l;
        }
    }
}