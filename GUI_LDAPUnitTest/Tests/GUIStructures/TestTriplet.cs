using System.Windows.Forms;


namespace GUI_LDAPUnitTest.Tests.GUIStructures
{
    public struct TestTriplet
    {
        private CheckBox _c;
        private Label _l;
        private BusinessLogic.TestType _t;

        public TestTriplet(CheckBox c, BusinessLogic.TestType t, Label l)
        {
            _c = c;
            _t = t;
            _l = l;
        }

        public CheckBox TestCheckbox
        {
            get { return _c; }
            set { _c = value; }
        }

        public BusinessLogic.TestType TestType
        {
            get { return _t; }
            set { _t = value; }
        }

        public Label TestLabel
        {
            get { return _l; }
            set { _l = value; }
        }
    }
}