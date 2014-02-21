using System;
using System.Collections.Generic;
using System.Linq;

namespace LDAPLibrary
{
    [Serializable]
    public class LDAPUser
    {
        private readonly string cn;
        private readonly string sn;
        private readonly string dn;

        private readonly Dictionary<string, List<string>> otherAttributes;

        public LDAPUser(string userDN, string userCN, string userSN, Dictionary<string, List<string>> otherAttribute)
        {
            if (LDAPManager.checkLibraryParameters(new string[] { userDN, userSN, userCN }))
            {
                sn = userSN;
                dn = userDN;
                cn = userCN;
                if (otherAttribute != null)
                {
                    otherAttributes = new Dictionary<string, List<string>>(otherAttribute);
                }
                else
                {
                    otherAttributes = new Dictionary<string, List<string>>();
                }
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
        public List<string> getUserAttribute(string attributeName)
        {
            if (otherAttributes.ContainsKey(attributeName))
            {
                return otherAttributes[attributeName];
            }
            else
            {
                throw new ArgumentException("L'attributo cercato non è presente nel dizionario degli attributi dell'utente", attributeName);
            }
        }

        /// <summary>
        /// Get all the Attribute Names of an LDAPUser
        /// </summary>
        /// <returns>All the Attribute Names</returns>
        public string[] getUserAttributeKeys()
        {
            return otherAttributes.Keys.ToArray();
        }

        public Dictionary<string, List<string>> getUserAttributes()
        {
            return otherAttributes;
        }

        /// <summary>
        /// Get User CN
        /// </summary>
        /// <returns>User CN</returns>
        public string getUserCn()
        {
            return cn;
        }

        /// <summary>
        /// Get User SN
        /// </summary>
        /// <returns>User SN</returns>
        public string getUserSn()
        {
            return sn;
        }

        /// <summary>
        /// Get User DN
        /// </summary>
        /// <returns>User DN</returns>
        public string getUserDn()
        {
            return dn;
        }

        /// <summary>
        /// Set the user attribute values list with a new one
        /// ERASE THE OLD ONE!!
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValues"></param>
        public void setUserAttributes(string attributeName, List<string> attributeValues)
        {
            if (otherAttributes.ContainsKey(attributeName))
                otherAttributes[attributeName] = attributeValues;
            else
                otherAttributes.Add(attributeName, attributeValues);
        }
        /// <summary>
        /// Set the user attribute values list with a new one that contain only the parameter value 
        /// ERASE THE OLD ONE!!
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public void setUserAttribute(string attributeName, string attributeValue)
        {
            if (otherAttributes.ContainsKey(attributeName))
                otherAttributes[attributeName] = new List<string>() { attributeValue };
            else
                otherAttributes.Add(attributeName, new List<string>() { attributeValue });
        }

        /// <summary>
        /// Add a new value to the existing user attribute list
        /// THOW EXCEPTION if attribute name isn't found
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public void insertUserAttribute(string attributeName, string attributeValue)
        {
            if (otherAttributes.ContainsKey(attributeName))
            {
                if (string.IsNullOrEmpty(attributeValue))
                    otherAttributes[attributeName].Add(attributeValue);
            }
            else
                throw new ArgumentException("L'attributo cercato non è presente nel dizionario degli attributi dell'utente", attributeName);
        }

        /// <summary>
        /// Delete an User attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public void deleteUserAttribute(string attributeName, string attributeValue)
        {
            List<string> tempUserAttriutes = getUserAttribute(attributeName);
            if (tempUserAttriutes.Remove(attributeValue))
                setUserAttributes(attributeName, tempUserAttriutes);
            else
                throw new ArgumentException(string.Format("Impossibile rimuovere il valore dagli attributi dell'utente: {0}", attributeValue));
        }

    }
}
