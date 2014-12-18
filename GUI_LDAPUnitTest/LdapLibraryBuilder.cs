using System;
using System.DirectoryServices.Protocols;
using LDAPLibrary;
using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;

namespace GUI_LDAPUnitTest
{
    public static class LdapLibraryBuilder
    {
        public static ILdapManager SetupLdapLibrary()
        {
            var authType = (AuthType) Enum.Parse(typeof (AuthType),
                Config.LdapLibrary["LDAPAuthType"]);

            if (string.IsNullOrEmpty(Config.LdapLibrary["LDAPAdminUserDN"]))
                return new LdapManager(null,
                    Config.LdapLibrary["LDAPServer"],
                    Config.LdapLibrary["LDAPSearchBaseDN"],
                    authType,
                    (LoggerType) Enum.Parse(typeof (LoggerType), Config.LdapLibrary["enableLDAPLibraryLog"]),
                    Config.LdapLibrary["LDAPLibraryLogPath"]
                    );

            var adminUser = new LdapUser(Config.LdapLibrary["LDAPAdminUserDN"],
                Config.LdapLibrary["LDAPAdminUserCN"],
                Config.LdapLibrary["LDAPAdminUserSN"],
                null);

            adminUser.CreateUserAttribute("userPassword", Config.LdapLibrary["LDAPAdminUserPassword"]);

            return new LdapManager(adminUser,
                Config.LdapLibrary["LDAPServer"],
                Config.LdapLibrary["LDAPSearchBaseDN"],
                authType,
                (LoggerType) Enum.Parse(typeof (LoggerType), Config.LdapLibrary["enableLDAPLibraryLog"]),
                Config.LdapLibrary["LDAPLibraryLogPath"]
                );
        }
    }
}