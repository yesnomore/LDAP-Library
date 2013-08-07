using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;

namespace LDAPLibrary
{
    public class LDAPUserManipulator
    {
        private LdapConnection ldapConnection;
        private string LDAPUserManipulationMessage;

        public LDAPUserManipulator(LdapConnection ldapConnection)
        {
            this.ldapConnection = ldapConnection;
        }

        public string getLDAPUserManipulationMessage()
        {
            return LDAPUserManipulationMessage;
        }

        //DA IMPLEMENTARE

        public bool createUser(LDAPUser newUser, out LDAPState LDAPCurrentState)
        {
            try
            {

                AddRequest addReq = new AddRequest();
                addReq.DistinguishedName = newUser.getUserDn();
                addReq.Attributes.Add(new DirectoryAttribute("objectClass", new object[] { "person", "top" }));
                addReq.Attributes.Add(new DirectoryAttribute("cn", newUser.getUserCn()));
                addReq.Attributes.Add(new DirectoryAttribute("sn", newUser.getUserSn()));

                foreach (string attributeName in newUser.getUserAttributeKeys())
                {
                    foreach (string attributeValue in newUser.getUserAttribute(attributeName))
                    {
                        addReq.Attributes.Add(new DirectoryAttribute(attributeName, attributeValue));
                    }
                }
                // cast the returned DirectoryResponse as an AddResponse object
                AddResponse addResponse = (AddResponse)ldapConnection.SendRequest(addReq);
            }
            catch (Exception e)
            {
                LDAPCurrentState = LDAPState.LDAPCreateUserError;
                LDAPUserManipulationMessage = e.Message;
                return false;
            }
            LDAPCurrentState = LDAPState.LDAPUserManipulatorSuccess;
            LDAPUserManipulationMessage = "Operation Success";
            return true;
        }


        public bool deleteUser(LDAPUser user, out LDAPState LDAPCurrentState)
        {
            try
            {

                //Create the delete request
                DeleteRequest deleteReq = new DeleteRequest(user.getUserDn());

                //Perform the deletion
                DeleteResponse deleteResponse = (DeleteResponse)ldapConnection.SendRequest(deleteReq);
            }
            catch (Exception e)
            {
                LDAPCurrentState = LDAPState.LDAPDeleteUserError;
                LDAPUserManipulationMessage = e.Message;
                return false;
            }

            LDAPUserManipulationMessage = "Operation Success";
            LDAPCurrentState = LDAPState.LDAPUserManipulatorSuccess;
            return true;

        }
        public bool modifyUserAttribute(DirectoryAttributeOperation operationType, LDAPUser user, string attributeName, string attributeValue, out LDAPState LDAPCurrentState)
        {
            try
            {
                //Prepare the modify request
                ModifyRequest modRequest = new ModifyRequest(user.getUserDn(), operationType, attributeName, attributeValue);

                //Perform the modify
                ldapConnection.SendRequest(modRequest);
            }
            catch (Exception e)
            {
                LDAPCurrentState = LDAPState.LDAPModifyUserAttributeError;
                LDAPUserManipulationMessage = e.Message;
                return false;
            }

            LDAPUserManipulationMessage = "Operation Success";
            LDAPCurrentState = LDAPState.LDAPUserManipulatorSuccess;
            return true;

        }


        public bool changeUserPassword(LDAPUser user, string newPwd, out LDAPState LDAPCurrentState)
        {
            try
            {

                DirectoryAttributeModification modifyUserPassword = new DirectoryAttributeModification();
                modifyUserPassword.Operation = DirectoryAttributeOperation.Replace;
                modifyUserPassword.Name = "userPassword";
                modifyUserPassword.Add(newPwd);

                ModifyRequest modifyRequest = new ModifyRequest(user.getUserDn(), modifyUserPassword);

                ldapConnection.SendRequest(modifyRequest);
            }
            catch (Exception e)
            {
                LDAPCurrentState = LDAPState.LDAPChangeUserPasswordError;
                LDAPUserManipulationMessage = e.Message;
                return false;
            }

            LDAPUserManipulationMessage = "Operation Success";
            LDAPCurrentState = LDAPState.LDAPUserManipulatorSuccess;
            return true;

        }

        /// <summary>
        /// Search Users in the LDAP system
        /// </summary>
        /// <param name="baseDN">Starting DN of the search</param>
        /// <param name="otherReturnedAttributes">Addictional attributes added to the results LDAPUsers objects</param>
        /// <param name="searchedUsers">Credential for the search</param>
        /// <param name="searchResult">LDAPUsers object returned in the search</param>
        /// <param name="LDAPCurrentState">Return the state of the LDAP to parent class</param>
        /// <returns>Boolean that comunicate the result of search</returns>
        public bool searchUsers(string baseDN, List<string> otherReturnedAttributes, string[] searchedUsers, out List<LDAPUser> searchResult, out LDAPState LDAPCurrentState)
        {

            searchResult = new List<LDAPUser>();

            if (otherReturnedAttributes == null)
                otherReturnedAttributes = new List<string>();

            //Add required search return attributes to the list
            otherReturnedAttributes.Add("cn");
            otherReturnedAttributes.Add("sn");
            try
            {
                //Foreach all the credential,for everyone do the search and add user results to the out parameter
                foreach (string users in searchedUsers)
                {
                    //Create the filter for the search
                    string LDAPSearchFilter = "(&(|(objectClass=person)(objectClass=user))(cn=" + users + "))";

                    //Componing search request
                    SearchRequest search = new SearchRequest(baseDN, LDAPSearchFilter, SearchScope.Subtree, otherReturnedAttributes.ToArray());

                    //Perforing the search
                    SearchResponse searchReturn = (SearchResponse)ldapConnection.SendRequest(search);

                    //For all the searchReturn we create a new LDAPUser obj and add that to the return searchResult
                    foreach (SearchResultEntry userReturn in searchReturn.Entries)
                    {
                        //Required attributes inizialization
                        string tempUserDN = userReturn.DistinguishedName;
                        string tempUserCN = null, tempUserSN = null;
                        Dictionary<string, string[]> tempUserOtherAttributes = new Dictionary<string, string[]>();

                        //Cycle attributes
                        foreach (DirectoryAttribute userReturnAttribute in userReturn.Attributes.Values)
                        {

                            //if is CN or SN, set right String else add attribute to dictionary
                            if (userReturnAttribute.Name.Equals("cn") || userReturnAttribute.Name.Equals("CN"))
                            {

                                object[] values = userReturnAttribute.GetValues(Type.GetType("System.String"));
                                tempUserCN = (string)values[0];
                            }
                            else if (userReturnAttribute.Name.Equals("sn") || userReturnAttribute.Name.Equals("SN"))
                            {

                                object[] values = userReturnAttribute.GetValues(Type.GetType("System.String"));
                                tempUserSN = (string)values[0];
                            }
                            else
                            {

                                object[] values = userReturnAttribute.GetValues(Type.GetType("System.String"));
                                string[] stringValues = new string[values.Length];
                                for (int i = 0; i < values.Length; i++)
                                    stringValues[i] = (string)values[i];
                                tempUserOtherAttributes.Add(userReturnAttribute.Name, stringValues);
                            }
                        }
                        //Create new tempUser and add to the searchResult
                        LDAPUser tempUser = new LDAPUser(tempUserDN, tempUserCN, tempUserSN, tempUserOtherAttributes);
                        searchResult.Add(tempUser);
                    }
                }
            }
            catch (Exception e)
            {
                LDAPCurrentState = LDAPState.LDAPSearchUserError;
                LDAPUserManipulationMessage = e.Message;
                return false;
            }

            if (searchResult.Count == 0)
            {
                LDAPCurrentState = LDAPState.LDAPSearchUserError;
                LDAPUserManipulationMessage = "Search Operation with NO RESULTS";
                return false;
            }

            LDAPCurrentState = LDAPState.LDAPUserManipulatorSuccess;
            LDAPUserManipulationMessage = "Search Operation Success";
            return true;
        }
    }
}
