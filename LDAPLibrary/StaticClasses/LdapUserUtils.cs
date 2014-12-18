using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary.StaticClasses
{
    internal static class LdapUserUtils
    {
        private const string DefaultUserSn = "Default Surname";
        private const string DefaultUserCn = "Default CommonName";

        /// <summary>
        ///     This method get the search respose and return a list of users
        ///     NOT TESTED: Because it's difficult to mockup the SearchResponse Obj
        /// </summary>
        /// <param name="searchResponse"></param>
        /// <returns></returns>
        public static List<ILdapUser> ConvertToLdapUsers(SearchResponse searchResponse)
        {
            var searchResult = new List<ILdapUser>();

            if (searchResponse == null) return searchResult;

            searchResult.AddRange(from SearchResultEntry userReturn in searchResponse.Entries select ConvertToLdapUser(userReturn));

            return searchResult;
        }

        public static ILdapUser ConvertToLdapUser(SearchResultEntry searchResultEntry)
        {
            //Required attributes inizialization
            var tempUserCn = DefaultUserCn;
            var tempUserSn = DefaultUserSn;
            var tempUserOtherAttributes = new Dictionary<string, List<string>>();

            //Cycle attributes
            if (searchResultEntry.Attributes.Values != null)
                foreach (DirectoryAttribute userReturnAttribute in searchResultEntry.Attributes.Values)
                {
                    //if is CN or SN, set right String else add attribute to dictionary
                    switch (userReturnAttribute.Name.ToUpper())
                    {
                        case "CN":
                            tempUserCn =
                                (string)userReturnAttribute.GetValues(Type.GetType("System.String"))[0];
                            break;
                        case "SN":
                            tempUserSn =
                                (string)userReturnAttribute.GetValues(Type.GetType("System.String"))[0];
                            break;
                        default:
                            tempUserOtherAttributes.Add(
                                userReturnAttribute.Name,
                                new List<string>(Array.ConvertAll(
                                    userReturnAttribute.GetValues(Type.GetType("System.String")), Convert.ToString)));
                            break;
                    }
                }
            return new LdapUser(searchResultEntry.DistinguishedName, tempUserCn, tempUserSn,tempUserOtherAttributes);
        }
    }
}