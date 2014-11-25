using System;
using System.Collections.Generic;
using System.Linq;
using LDAPLibrary.Interfarces;
using LDAPLibrary.StaticClasses;

namespace LDAPLibrary
{
    [Serializable]
    public class LdapUser : ILdapUser
    {
        private readonly string _cn;
        private readonly string _dn;
        private readonly Dictionary<string, List<string>> _otherAttributes;
        private readonly string _sn;

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
                throw new ArgumentException("I valori dei primi 3 argomenti non possono essere null o vuoti");
            }
        }

        #region Getters

        public List<string> GetUserAttribute(string attributeName)
        {
            if (_otherAttributes.ContainsKey(attributeName))
            {
                return _otherAttributes[attributeName];
            }
            throw new ArgumentException(
                "L'attributo cercato non è presente nel dizionario degli attributi dell'utente", attributeName);
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
                    "L'attributo di cui si vuole sovrascrivere i valori non è presente nel dizionario degli attributi dell'utente",
                    attributeName);
        }

        public void OverwriteUserAttribute(string attributeName, string attributeValue)
        {
            if (_otherAttributes.ContainsKey(attributeName))
                _otherAttributes[attributeName] = new List<string> {attributeValue};
            else
                throw new ArgumentException(
                    "L'attributo di cui si vuole sovrascrivere i valori non è presente nel dizionario degli attributi dell'utente",
                    attributeName);
        }

        public void CreateUserAttribute(string attributeName, List<string> attributeValues)
        {
            if (!_otherAttributes.ContainsKey(attributeName))
                _otherAttributes.Add(attributeName, attributeValues);
            else
                throw new ArgumentException(
                    "L'attributo da creare è gia' presente nel dizionario degli attributi dell'utente", attributeName);
        }

        public void CreateUserAttribute(string attributeName, string attributeValue)
        {
            if (!_otherAttributes.ContainsKey(attributeName))
                _otherAttributes.Add(attributeName, new List<string> {attributeValue});
            else
                throw new ArgumentException(
                    "L'attributo da creare è gia' presente nel dizionario degli attributi dell'utente", attributeName);
        }

        public void InsertUserAttribute(string attributeName, string attributeValue)
        {
            if (!_otherAttributes.ContainsKey(attributeName))
                throw new ArgumentException(
                    "L'attributo cercato non è presente nel dizionario degli attributi dell'utente", attributeName);
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
                    string.Format("Impossibile rimuovere il valore dagli attributi dell'utente: {0}", attributeValue));
        }

        #endregion
    }
}