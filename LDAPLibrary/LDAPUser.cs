using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Reflection.Emit;
using LDAPLibrary.Interfarces;
using LDAPLibrary.StaticClasses;

namespace LDAPLibrary
{
    /// <summary>
    /// Entity used to rapresent an LDAP User
    /// </summary>
    [Serializable]
    public class LdapUser : ILdapUser
    {
        private readonly string _cn;
        private readonly string _dn;
        private readonly Dictionary<string, List<string>> _otherAttributes;
        private readonly string _sn;
        private const string AttributeUserRemoveError = "Cannot remove the value of user's attribute : {0}";
        private const string AttributeNotFoundError = "The attribute is not in the dictionary of the user's attributes";
        private const string AttributeAlreadyExistError = "The attribute you want to create already exist";

        public LdapUser(string userDn, string userCn, string userSn, Dictionary<string, List<string>> otherAttribute)
        {
            if (!LdapParameterChecker.ParametersIsNullOrEmpty(new[] {userDn, userSn, userCn}))
            {
                _sn = userSn;
                _dn = userDn;
                _cn = userCn;
                _otherAttributes = otherAttribute == null
                    ? new Dictionary<string, List<string>>()
                    : new Dictionary<string, List<string>>(otherAttribute);
            }
            else
            {
                throw new ArgumentException("The first 3 parameters cannot be null or empty");
            }
        }

        #region Getters

        public List<string> GetUserAttribute(string attributeName)
        {
            if (_otherAttributes.ContainsKey(attributeName))
            {
                return _otherAttributes[attributeName];
            }
            throw new ArgumentException(AttributeNotFoundError, attributeName);
        }

        public string[] GetUserAttributeKeys()
        {
            return _otherAttributes.Keys.ToArray();
        }

        public Dictionary<string, List<string>> GetUserAttributes()
        {
            return _otherAttributes;
        }

        public string GetUserCn()
        {
            return _cn;
        }

        public string GetUserSn()
        {
            return _sn;
        }

        public string GetUserDn()
        {
            return _dn;
        }

        #endregion

        #region Operations

        public void OverwriteUserAttribute(string attributeName, List<string> attributeValues)
        {
            if (_otherAttributes.ContainsKey(attributeName))
                _otherAttributes[attributeName] = attributeValues;
            else
                throw new ArgumentException(
                    AttributeNotFoundError,
                    attributeName);
        }

        public void OverwriteUserAttribute(string attributeName, string attributeValue)
        {
            if (_otherAttributes.ContainsKey(attributeName))
                _otherAttributes[attributeName] = new List<string> {attributeValue};
            else
                throw new ArgumentException(
                    AttributeNotFoundError,
                    attributeName);
        }

        public void CreateUserAttribute(string attributeName, List<string> attributeValues)
        {
            if (!_otherAttributes.ContainsKey(attributeName))
                _otherAttributes.Add(attributeName, attributeValues);
            else
                throw new ArgumentException(AttributeAlreadyExistError, attributeName);
        }

        public void CreateUserAttribute(string attributeName, string attributeValue)
        {
            if (!_otherAttributes.ContainsKey(attributeName))
                _otherAttributes.Add(attributeName, new List<string> {attributeValue});
            else
                throw new ArgumentException(AttributeAlreadyExistError, attributeName);
        }

        public void InsertUserAttribute(string attributeName, string attributeValue)
        {
            if (!_otherAttributes.ContainsKey(attributeName))
                throw new ArgumentException(
                    AttributeNotFoundError, attributeName);
            if (!string.IsNullOrEmpty(attributeValue))
                _otherAttributes[attributeName].Add(attributeValue);
        }

        public void DeleteUserAttribute(string attributeName, string attributeValue)
        {
            List<string> tempUserAttriutes = GetUserAttribute(attributeName);
            if (tempUserAttriutes.Remove(attributeValue))
                OverwriteUserAttribute(attributeName, tempUserAttriutes);
            else
                throw new ArgumentException(
                    string.Format(AttributeUserRemoveError, attributeValue));
        }

        //Not tested because i don't know how to test an ACTION
        public Action GetUserOperation(DirectoryAttributeOperation operationType, string attributeName,
            string attributeValue)
        {
            switch (operationType)
            {
                case DirectoryAttributeOperation.Add:
                    return (() => InsertUserAttribute(attributeName, attributeValue));
                case DirectoryAttributeOperation.Delete:
                    return (() => DeleteUserAttribute(attributeName, attributeValue));
                case DirectoryAttributeOperation.Replace:
                    return (() => OverwriteUserAttribute(attributeName, attributeValue));
                default:
                    throw new Exception(
                        "LdapUser GetUserOperation Method: Not valid DirectoryAttributeOperation specified - " +
                        operationType);
            }
        }

        #endregion
    }
}