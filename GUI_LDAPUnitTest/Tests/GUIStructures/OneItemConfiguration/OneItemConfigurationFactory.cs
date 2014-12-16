using System;
using GUI_LDAPUnitTest.Tests.BusinessLogic;

namespace GUI_LDAPUnitTest.Tests.GUIStructures.OneItemConfiguration
{
    public static class OneItemConfigurationFactory
    {
        public static IOneItemConfiguration GetOneItemConfiguration(OneItemConfigurationState state,
            TestUserRepository testUserRepository)
        {
            switch (state)
            {
                case OneItemConfigurationState.NewDescription:
                    return new OneItemConfigurationDescripiton(testUserRepository);
                case OneItemConfigurationState.NewPassword:
                    return new OneItemConfigurationPassword(testUserRepository);
                case OneItemConfigurationState.UserToSearch:
                    return new OneItemConfigurationSearchUsers(testUserRepository);
                default: throw new Exception("OneItemConfiguration Value Unespected");
            }
        }
    }
}