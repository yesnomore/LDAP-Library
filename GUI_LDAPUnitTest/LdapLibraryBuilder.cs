using System;
using System.DirectoryServices.Protocols;
using LDAPLibrary;
using LDAPLibrary.Interfarces;

namespace GUI_LDAPUnitTest
{
    public static class LdapLibraryBuilder
    {
        public static ILdapManager SetupLdapLibrary()
        {
            var authType = (AuthType)Enum.Parse(typeof(AuthType),
                Config.LDAPLibrary["LDAPAuthType"]);

            if (string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
                return new LdapManager(null,
                    Config.LDAPLibrary["LDAPServer"],
                    Config.LDAPLibrary["LDAPSearchBaseDN"],
                    authType
                    );

            var adminUser = new LdapUser(Config.LDAPLibrary["LDAPAdminUserDN"],
                Config.LDAPLibrary["LDAPAdminUserCN"],
                Config.LDAPLibrary["LDAPAdminUserSN"],
                null);

            adminUser.CreateUserAttribute("userPassword", Config.LDAPLibrary["LDAPAdminUserPassword"]);

            return new LdapManager(adminUser,
                Config.LDAPLibrary["LDAPServer"],
                Config.LDAPLibrary["LDAPSearchBaseDN"],
                authType
                );
        }
    }
}
