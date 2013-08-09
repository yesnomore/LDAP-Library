using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDAPLibrary
{
    [Serializable]
    public class LDAPUser
    {
        //Required Fields
        private string cn;
        private string sn;
        private string dn;

        private Dictionary<string, string[]> otherAttributes;

        public LDAPUser(string userDN, string userCN, string userSN, Dictionary<string, string[]> otherAttribute)
        {
            //Check parameters
            if ((!userDN.Equals("") && userDN != null) &&
                    (!userSN.Equals("") && userSN != null) &&
                    (!userCN.Equals("") && userCN != null))
            {
                sn = userSN;
                dn = userDN;
                cn = userCN;
                if (otherAttribute != null)
                    otherAttributes = new Dictionary<string, string[]>(otherAttribute);
                else
                    otherAttributes = new Dictionary<string, string[]>();
            }
            else
                throw new ArgumentException("I valori dei primi 3 argomenti non possono essere null o vuoti");

        }

        #region Get Methods

        /// <summary>
        /// Returns the values for the specified attribute name
        /// </summary>
        /// <param name="attributeName">Attribute Name to search of</param>
        /// <returns>Values in string array</returns>
        public string[] getUserAttribute(string attributeName)
        {
            if (otherAttributes.ContainsKey(attributeName))
                return otherAttributes[attributeName];
            else
                throw new ArgumentException("L'attributo cercato non è presente nel dizionario degli attributi dell'utente", attributeName);
        }
        public string[] getUserAttributeKeys() 
        {
            return otherAttributes.Keys.ToArray();
        }

        public string getUserCn() { return cn; }
        public string getUserSn() { return sn; }
        public string getUserDn() { return dn; }

        #endregion
    }
}
