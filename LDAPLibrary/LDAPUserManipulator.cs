﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace LDAPLibrary
{
    public class LDAPUserManipulator
    {
        private readonly LdapConnection _ldapConnection;
        private readonly string _defaultUserSn;
        private readonly string _defaultUserCn;
        private string _ldapUserManipulationMessage;

        /// <summary>
        /// Constructor of the LDAP User Manipulator
        /// </summary>
        /// <param name="ldapConnection">Connection of an admin User</param>
        /// <param name="defaultUserCn"></param>
        /// <param name="defaultUserSn">Default SN for a new User</param>
        public LDAPUserManipulator(LdapConnection ldapConnection, string defaultUserCn, string defaultUserSn)
        {
            _ldapConnection = ldapConnection;
            _defaultUserCn = defaultUserCn;
            _defaultUserSn = defaultUserSn;
        }

        /// <summary>
        /// For give the class operations messages
        /// </summary>
        /// <returns>Operation Success/Error Messages</returns>
        public string GetLdapUserManipulationMessage()
        {
            return _ldapUserManipulationMessage;
        }

        /// <summary>
        /// Create a new LDAPUser
        /// </summary>
        /// <param name="newUser">User to create</param>
        /// <param name="ldapCurrentState">State of LDAP</param>
        /// <param name="objectClass"></param>
        /// <returns> Success or Failed</returns>
        public bool CreateUser(LDAPUser newUser, out LDAPState ldapCurrentState, string objectClass)
        {
            try
            {

                AddRequest addReq = new AddRequest { DistinguishedName = newUser.GetUserDn() };
                addReq.Attributes.Add(new DirectoryAttribute("objectClass", objectClass));
                addReq.Attributes.Add(new DirectoryAttribute("cn", newUser.GetUserCn()));
                addReq.Attributes.Add(new DirectoryAttribute("sn", newUser.GetUserSn()));

                foreach (string attributeName in newUser.GetUserAttributeKeys())
                {
                    foreach (string attributeValue in newUser.GetUserAttribute(attributeName))
                    {
                        addReq.Attributes.Add(new DirectoryAttribute(attributeName, attributeValue));
                    }
                }
                // cast the returned DirectoryResponse as an AddResponse object
                _ldapConnection.SendRequest(addReq);
            }
            catch (Exception e)
            {
                ldapCurrentState = LDAPState.LdapCreateUserError;
                _ldapUserManipulationMessage = e.Message;
                return false;
            }
            ldapCurrentState = LDAPState.LdapUserManipulatorSuccess;
            _ldapUserManipulationMessage = "Create User Operation Success";
            return true;
        }

        /// <summary>
        /// Delete an LDAPUser
        /// </summary>
        /// <param name="user">User to delete</param>
        /// <param name="ldapCurrentState">State of LDAP</param>
        /// <returns>Success or Failed</returns>
        public bool DeleteUser(LDAPUser user, out LDAPState ldapCurrentState)
        {
            try
            {

                //Create the delete request
                DeleteRequest deleteReq = new DeleteRequest(user.GetUserDn());

                //Perform the deletion
                _ldapConnection.SendRequest(deleteReq);
            }
            catch (Exception e)
            {
                ldapCurrentState = LDAPState.LdapDeleteUserError;
                _ldapUserManipulationMessage = e.Message;
                return false;
            }

            _ldapUserManipulationMessage = "Delete User Operation Success";
            ldapCurrentState = LDAPState.LdapUserManipulatorSuccess;
            return true;

        }

        /// <summary>
        /// Modify an LDAPUser Attribute
        /// </summary>
        /// <param name="operationType">Operation to execute on the attribute</param>
        /// <param name="user">LDAPUser's Attribute</param>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="attributeValue">Attribute Value</param>
        /// <param name="ldapCurrentState">State of LDAP</param>
        /// <returns>Success or Failed</returns>
        public bool ModifyUserAttribute(DirectoryAttributeOperation operationType, LDAPUser user, string attributeName, string attributeValue, out LDAPState ldapCurrentState)
        {
            try
            {
                //Prepare the modify request
                ModifyRequest modRequest = new ModifyRequest(user.GetUserDn(), operationType, attributeName, attributeValue);

                //Perform the modify
                _ldapConnection.SendRequest(modRequest);
            }
            catch (Exception e)
            {
                ldapCurrentState = LDAPState.LdapModifyUserAttributeError;
                _ldapUserManipulationMessage = e.Message;
                return false;
            }

            switch (operationType)
            {
                case DirectoryAttributeOperation.Add:
                    user.InsertUserAttribute(attributeName, attributeValue);
                    break;
                case DirectoryAttributeOperation.Delete:
                    user.DeleteUserAttribute(attributeName, attributeValue);
                    break;
                case DirectoryAttributeOperation.Replace:
                    user.SetUserAttribute(attributeName, attributeValue);
                    break;
            }


            _ldapUserManipulationMessage = "Modify User Attribute Operation Success";
            ldapCurrentState = LDAPState.LdapUserManipulatorSuccess;
            return true;

        }

        /// <summary>
        /// Change an LDAPUser's Password
        /// </summary>
        /// <param name="user">LDAPUser to change the password</param>
        /// <param name="newPwd">New Passowrd</param>
        /// <param name="ldapCurrentState">State of LDAP</param>
        /// <returns>Success or Failed</returns>
        public bool ChangeUserPassword(LDAPUser user, string newPwd, out LDAPState ldapCurrentState)
        {
            try
            {

                DirectoryAttributeModification modifyUserPassword = new DirectoryAttributeModification
                {
                    Operation = DirectoryAttributeOperation.Replace,
                    Name = "userPassword"
                };
                modifyUserPassword.Add(newPwd);

                ModifyRequest modifyRequest = new ModifyRequest(user.GetUserDn(), modifyUserPassword);

                _ldapConnection.SendRequest(modifyRequest);
            }
            catch (Exception e)
            {
                ldapCurrentState = LDAPState.LdapChangeUserPasswordError;
                _ldapUserManipulationMessage = e.Message;
                return false;
            }

            user.SetUserAttribute("userPassword", newPwd);

            _ldapUserManipulationMessage = "Change Password Operation Success";
            ldapCurrentState = LDAPState.LdapUserManipulatorSuccess;
            return true;

        }

        /// <summary>
        /// Search Users in the LDAP system
        /// </summary>
        /// <param name="baseDn">Starting DN of the search</param>
        /// <param name="matchFieldUsername"></param>
        /// <param name="otherReturnedAttributes">Addictional attributes added to the results LDAPUsers objects</param>
        /// <param name="searchedUsers">Credential for the search</param>
        /// <param name="searchResult">LDAPUsers object returned in the search</param>
        /// <param name="ldapCurrentState">Return the state of the LDAP to parent class</param>
        /// <param name="userObjectClass"></param>
        /// <returns>Boolean that comunicate the result of search</returns>
        public bool SearchUsers(string baseDn, string userObjectClass, string matchFieldUsername, List<string> otherReturnedAttributes, string[] searchedUsers, out List<LDAPUser> searchResult, out LDAPState ldapCurrentState)
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
                    string ldapSearchFilter = String.Format("(&(objectClass={0})({1}={2}))", userObjectClass, matchFieldUsername, users);

                    //Componing search request
                    SearchRequest search = new SearchRequest(baseDn, ldapSearchFilter, SearchScope.Subtree, otherReturnedAttributes.ToArray());

                    //Perforing the search
                    SearchResponse searchReturn = (SearchResponse)_ldapConnection.SendRequest(search);

                    //For all the searchReturn we create a new LDAPUser obj and add that to the return searchResult
                    foreach (SearchResultEntry userReturn in searchReturn.Entries)
                    {
                        //Required attributes inizialization
                        string tempUserDn = userReturn.DistinguishedName;
                        string tempUserCn = _defaultUserCn;
                        string tempUserSn = _defaultUserSn;
                        Dictionary<string, List<string>> tempUserOtherAttributes = new Dictionary<string, List<string>>();

                        //Cycle attributes
                        foreach (DirectoryAttribute userReturnAttribute in userReturn.Attributes.Values)
                        {

                            //if is CN or SN, set right String else add attribute to dictionary
                            if (userReturnAttribute.Name.Equals("cn") || userReturnAttribute.Name.Equals("CN"))
                            {

                                object[] values = userReturnAttribute.GetValues(Type.GetType("System.String"));
                                tempUserCn = (string)values[0];
                            }
                            else if (userReturnAttribute.Name.Equals("sn") || userReturnAttribute.Name.Equals("SN"))
                            {

                                object[] values = userReturnAttribute.GetValues(Type.GetType("System.String"));
                                tempUserSn = (string)values[0];
                            }
                            else
                            {

                                object[] values = userReturnAttribute.GetValues(Type.GetType("System.String"));

                                List<string> stringValues = new List<string>(Array.ConvertAll(values, Convert.ToString));
                                tempUserOtherAttributes.Add(userReturnAttribute.Name, stringValues);
                            }
                        }
                        //Create new tempUser and add to the searchResult
                        LDAPUser tempUser = new LDAPUser(tempUserDn, tempUserCn, tempUserSn, tempUserOtherAttributes);
                        searchResult.Add(tempUser);

                    }
                }
            }
            catch (Exception e)
            {
                ldapCurrentState = LDAPState.LdapSearchUserError;
                _ldapUserManipulationMessage = e.Message;
                return false;
            }

            if (searchResult.Count == 0)
            {
                ldapCurrentState = LDAPState.LdapSearchUserError;
                _ldapUserManipulationMessage = "Search Operation with NO RESULTS";
                return false;
            }

            ldapCurrentState = LDAPState.LdapUserManipulatorSuccess;
            _ldapUserManipulationMessage = "Search Operation Success";
            return true;
        }

    }
}
