using System;
using System.Collections.Generic;
using System.Linq;

namespace LDAPLibrary
{
    [Serializable]
    public class LDAPUser
    {
        private string cn;
        private string sn;
        private string dn;

        private Dictionary<string, string[]> otherAttributes;

        public LDAPUser(string userDN, string userCN, string userSN, Dictionary<string, string[]> otherAttribute)
        {
            if ((!userDN.Equals(string.Empty) && userDN != null) &&
                    (!userSN.Equals(string.Empty) && userSN != null) &&
                    (!userCN.Equals(string.Empty) && userCN != null))
            {
                sn = userSN;
                dn = userDN;
                cn = userCN;
                if (otherAttribute != null)
                {
                    otherAttributes = new Dictionary<string, string[]>(otherAttribute);
                }
                else
                {
                    otherAttributes = new Dictionary<string, string[]>();
                }
            }
            else
            {
                throw new ArgumentException("I valori dei primi 3 argomenti non possono essere null o vuoti");
            }
        }


        /// <summary>
        /// Returns the values for the specified attribute name
        /// </summary>
        /// <param name="attributeName">Attribute Name to search of</param>
        /// <returns>Values in string array</returns>
        public string[] getUserAttribute(string attributeName)
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
    }
}
