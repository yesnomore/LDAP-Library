using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace LDAPLibrary
{
    public class LDAPUserManipulator
    {
        private readonly LdapConnection ldapConnection;
        private readonly string defaultUserSn;
        private readonly string defaultUserCn;
        private string LDAPUserManipulationMessage;

        /// <summary>
        /// Constructor of the LDAP User Manipulator
        /// </summary>
        /// <param name="ldapConnection">Connection of an admin User</param>
        /// <param name="defaultUserSn">Default SN for a new User</param>
        public LDAPUserManipulator(LdapConnection ldapConnection, string defaultUserCn, string defaultUserSn)
        {
            this.ldapConnection = ldapConnection;
            this.defaultUserCn = defaultUserCn;
            this.defaultUserSn = defaultUserSn;
        }

        /// <summary>
        /// For give the class operations messages
        /// </summary>
        /// <returns>Operation Success/Error Messages</returns>
        public string getLDAPUserManipulationMessage()
        {
            return LDAPUserManipulationMessage;
        }

        /// <summary>
        /// Create a new LDAPuser
        /// </summary>
        /// <param name="newUser">User to create</param>
        /// <param name="LDAPCurrentState">State of LDAP</param>
        /// <returns> Success or Failed</returns>
        public bool createUser(LDAPUser newUser, out LDAPState LDAPCurrentState, string objectClass)
        {
            try
            {

                AddRequest addReq = new AddRequest() { DistinguishedName = newUser.getUserDn() };
                addReq.Attributes.Add(new DirectoryAttribute("objectClass", objectClass));
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
                ldapConnection.SendRequest(addReq);
            }
            catch (Exception e)
            {
                LDAPCurrentState = LDAPState.LDAPCreateUserError;
                LDAPUserManipulationMessage = e.Message;
                return false;
            }
            LDAPCurrentState = LDAPState.LDAPUserManipulatorSuccess;
            LDAPUserManipulationMessage = "Create User Operation Success";
            return true;
        }

        /// <summary>
        /// Delete an LDAPUser
        /// </summary>
        /// <param name="user">User to delete</param>
        /// <param name="LDAPCurrentState">State of LDAP</param>
        /// <returns>Success or Failed</returns>
        public bool deleteUser(LDAPUser user, out LDAPState LDAPCurrentState)
        {
            try
            {

                //Create the delete request
                DeleteRequest deleteReq = new DeleteRequest(user.getUserDn());

                //Perform the deletion
                ldapConnection.SendRequest(deleteReq);
            }
            catch (Exception e)
            {
                LDAPCurrentState = LDAPState.LDAPDeleteUserError;
                LDAPUserManipulationMessage = e.Message;
                return false;
            }

            LDAPUserManipulationMessage = "Delete User Operation Success";
            LDAPCurrentState = LDAPState.LDAPUserManipulatorSuccess;
            return true;

        }

        /// <summary>
        /// Modify an LDAPUser Attribute
        /// </summary>
        /// <param name="operationType">Operation to execute on the attribute</param>
        /// <param name="user">LDAPUser's Attribute</param>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="attributeValue">Attribute Value</param>
        /// <param name="LDAPCurrentState">State of LDAP</param>
        /// <returns>Success or Failed</returns>
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


            //Update LDAPUser Structure.
            string[] tempArray;

            switch (operationType)
            {
                case DirectoryAttributeOperation.Add:
                    user.insertUserAttribute(attributeName, attributeValue);
                    break;
                case DirectoryAttributeOperation.Delete:
                    user.deleteUserAttribute(attributeName, attributeValue);
                    break;
                case DirectoryAttributeOperation.Replace:
                    user.setUserAttribute(attributeName, attributeValue);
                    break;
            }


            LDAPUserManipulationMessage = "Modify User Attribute Operation Success";
            LDAPCurrentState = LDAPState.LDAPUserManipulatorSuccess;
            return true;

        }

        /// <summary>
        /// Change an LDAPUser's Password
        /// </summary>
        /// <param name="user">LDAPUser to change the password</param>
        /// <param name="newPwd">New Passowrd</param>
        /// <param name="LDAPCurrentState">State of LDAP</param>
        /// <returns>Success or Failed</returns>
        public bool changeUserPassword(LDAPUser user, string newPwd, out LDAPState LDAPCurrentState)
        {
            try
            {

                DirectoryAttributeModification modifyUserPassword = new DirectoryAttributeModification()
                {
                    Operation = DirectoryAttributeOperation.Replace,
                    Name = "userPassword"
                };
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

            user.setUserAttribute("userPassword", newPwd);

            LDAPUserManipulationMessage = "Change Password Operation Success";
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
        public bool searchUsers(string baseDN, string userObjectClass, string MatchFieldUsername, List<string> otherReturnedAttributes, string[] searchedUsers, out List<LDAPUser> searchResult, out LDAPState LDAPCurrentState)
        {

            searchResult = new List<LDAPUser>();

            if (otherReturnedAttributes == null)
                otherReturnedAttributes = new List<string>();

            //Add required search return attributes to the list

            if (!otherReturnedAttributes.Contains("cn")) otherReturnedAttributes.Add("cn");
            if (!otherReturnedAttributes.Contains("sn")) otherReturnedAttributes.Add("sn");

            try
            {
                //Foreach all the credential,for everyone do the search and add user results to the out parameter
                foreach (string users in searchedUsers)
                {
                    //Create the filter for the search
                    string LDAPSearchFilter = String.Format("(&(objectClass={0})({1}={2}))", userObjectClass, MatchFieldUsername, users);

                    //Componing search request
                    SearchRequest search = new SearchRequest(baseDN, LDAPSearchFilter, SearchScope.Subtree, otherReturnedAttributes.ToArray());

                    //Perforing the search
                    SearchResponse searchReturn = (SearchResponse)ldapConnection.SendRequest(search);

                    //For all the searchReturn we create a new LDAPUser obj and add that to the return searchResult
                    foreach (SearchResultEntry userReturn in searchReturn.Entries)
                    {
                        //Required attributes inizialization
                        string tempUserDN = userReturn.DistinguishedName;
                        string tempUserCN = defaultUserCn;
                        string tempUserSN = defaultUserSn;
                        Dictionary<string, List<string>> tempUserOtherAttributes = new Dictionary<string, List<string>>();

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

                                List<string> stringValues = new List<string>(Array.ConvertAll<object, string>(values, Convert.ToString));
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
