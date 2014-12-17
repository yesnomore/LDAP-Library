using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GUI_LDAPUnitTest.Tests.GUIStructures
{
    internal class TestTripletRepository
    {
        private readonly List<TestTriplet> _testTripletList;

        public TestTripletRepository()
        {
            _testTripletList = new List<TestTriplet>();
        }

        public List<TestTriplet> TestCheckedTripletList
        {
            get
            {
                _testTripletList.Where(t => !t.TestCheckbox.Checked)
                    .ToList()
                    .ForEach(t => t.TestLabel.Text = Constants.TestLableSkipped);
                return _testTripletList.Where(t => t.TestCheckbox.Checked).ToList();
            }
        }

        public void AddTestTriplet(TestTriplet testTriplet)
        {
            _testTripletList.Add(testTriplet);
        }

        /// <summary>
        ///     SetUp Class Statement: list of all state label
        ///     Set the text for all state Label in the form
        /// </summary>
        public void SetAllStateLabelText(string text)
        {
            foreach (TestTriplet t in TestCheckedTripletList.Where(t => !string.IsNullOrEmpty(text)))
            {
                t.TestLabel.Text = text;
                t.TestLabel.ForeColor = Color.Empty;
            }
        }
    }
}