using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using LDAPLibrary.Interfarces;

/*
 *
 * USED WHEN THERE ISN'T and admin, fake values
 *
 */

namespace LDAPLibrary
{
    /// <summary>
    /// Fake user.
    /// Used in the NoAdmin Mode to simulate his presence.
    /// </summary>
    class FakeLdapUser : ILdapUser
    {
        public List<string> GetUserAttribute(string attributeName)
        {
            return new List<string>{""};
        }

        public string[] GetUserAttributeKeys()
        {
            return new string[0];
        }

        public Dictionary<string, List<string>> GetUserAttributes()
        {
            return new Dictionary<string, List<string>>(){{"userPassword",new List<string>{""}}};
        }

        public string GetUserCn()
        {
            return "";
        }

        public string GetUserSn()
        {
            return "";
        }

        public string GetUserDn()
        {
            return "";
        }

        public void OverwriteUserAttribute(string attributeName, List<string> attributeValues)
        {
        }

        public void OverwriteUserAttribute(string attributeName, string attributeValue)
        {
        }

        public void CreateUserAttribute(string attributeName, List<string> attributeValues)
        {
        }

        public void CreateUserAttribute(string attributeName, string attributeValue)
        {
        }

        public void InsertUserAttribute(string attributeName, string attributeValue)
        {
        }

        public void DeleteUserAttribute(string attributeName, string attributeValue)
        {
        }

        public Action GetUserOperation(DirectoryAttributeOperation operationType, string attributeName, string attributeValue)
        {
            return () => { };
        }
    }
}
