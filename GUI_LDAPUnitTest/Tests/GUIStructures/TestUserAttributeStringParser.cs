using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI_LDAPUnitTest.Tests.GUIStructures
{
    public static class TestUserAttributeStringParser
    {
        public static Dictionary<string,List<string>> ParseTestUserAttributes(string st)
        {
             var attributes = st.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            attributes = attributes.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            return attributes.Select(attribute => attribute.Split(new[] {'='})).ToDictionary(temp => temp[0], temp => new List<string> {temp[1]});
        }
    }
}
