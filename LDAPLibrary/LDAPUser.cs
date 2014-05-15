using System;
using System.Collections.Generic;
using System.Linq;

namespace LDAPLibrary
{
    [Serializable]
    public class LDAPUser
    {
        private readonly string _cn;
        private readonly string _sn;
        private readonly string _dn;

        private readonly Dictionary<string, List<string>> _otherAttributes;

        public LDAPUser(string userDn, string userCn, string userSn, Dictionary<string, List<string>> otherAttribute)
        {
            if (LDAPManager.CheckLibraryParameters(new[] { userDn, userSn, userCn }))
            {
                _sn = userSn;
                _dn = userDn;
                _cn = userCn;
                _otherAttributes = otherAttribute != null ? new Dictionary<string, List<string>>(otherAttribute) : new Dictionary<string, List<string>>();
            }
            else
            {
                throw new ArgumentException("I valori dei primi 3 argomenti non possono essere null o vuoti");
            }
        }


        /// <summary>
        /// Returns the values for the specified attribute name
        /// THROW EXCEPTION if attribute name isn't found
        /// </summary>
        /// <param name="attributeName">Attribute Name to search of</param>
        /// <returns>Values in string array</returns>
        public List<string> GetUserAttribute(string attributeName)
        {
            if (_otherAttributes.ContainsKey(attributeName))
            {
                return _otherAttributes[attributeName];
            }
            throw new ArgumentException("L'attributo cercato non è presente nel dizionario degli attributi dell'utente", attributeName);
        }

        /// <summary>
        /// Get all the Attribute Names of an LDAPUser
        /// </summary>
        /// <returns>All the Attribute Names</returns>
        public string[] GetUserAttributeKeys()
        {
            return _otherAttributes.Keys.ToArray();
        }

        public Dictionary<string, List<string>> GetUserAttributes()
        {
            return _otherAttributes;
        }

        /// <summary>
        /// Get User CN
        /// </summary>
        /// <returns>User CN</returns>
        public string GetUserCn()
        {
            return _cn;
        }

        /// <summary>
        /// Get User SN
        /// </summary>
        /// <returns>User SN</returns>
        public string GetUserSn()
        {
            return _sn;
        }

        /// <summary>
        /// Get User DN
        /// </summary>
        /// <returns>User DN</returns>
        public string GetUserDn()
        {
            return _dn;
        }

        /// <summary>
        /// Set the user attribute values list with a new one
        /// ERASE THE OLD ONE!!
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValues"></param>
        public void SetUserAttributes(string attributeName, List<string> attributeValues)
        {
            if (_otherAttributes.ContainsKey(attributeName))
                _otherAttributes[attributeName] = attributeValues;
            else
                _otherAttributes.Add(attributeName, attributeValues);
        }
        /// <summary>
        /// Set the user attribute values list with a new one that contain only the parameter value 
        /// ERASE THE OLD ONE!!
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public void SetUserAttribute(string attributeName, string attributeValue)
        {
            if (_otherAttributes.ContainsKey(attributeName))
                _otherAttributes[attributeName] = new List<string> { attributeValue };
            else
                _otherAttributes.Add(attributeName, new List<string> { attributeValue });
        }

        /// <summary>
        /// Add a new value to the existing user attribute list
        /// THOW EXCEPTION if attribute name isn't found
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public void InsertUserAttribute(string attributeName, string attributeValue)
        {
            if (_otherAttributes.ContainsKey(attributeName))
            {
                if (string.IsNullOrEmpty(attributeValue))
                    _otherAttributes[attributeName].Add(attributeValue);
            }
            else
                throw new ArgumentException("L'attributo cercato non è presente nel dizionario degli attributi dell'utente", attributeName);
        }

        /// <summary>
        /// Delete an User attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public void DeleteUserAttribute(string attributeName, string attributeValue)
        {
            List<string> tempUserAttriutes = GetUserAttribute(attributeName);
            if (tempUserAttriutes.Remove(attributeValue))
                SetUserAttributes(attributeName, tempUserAttriutes);
            else
                throw new ArgumentException(string.Format("Impossibile rimuovere il valore dagli attributi dell'utente: {0}", attributeValue));
        }

    }
}
