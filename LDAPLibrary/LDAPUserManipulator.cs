using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using LDAPLibrary.Interfarces;
using LDAPLibrary.StaticClasses;

namespace LDAPLibrary
{
    public class LdapUserManipulator : ILdapConnectionObserver, ILdapUserManipulator
    {
        private readonly ILogger _logger;
        private readonly ILdapConfigRepository _configRepository;
        private LdapConnection _ldapConnection;

        public LdapUserManipulator(ILogger logger, ILdapConfigRepository configRepository)
        {
            _logger = logger;
            _configRepository = configRepository;
        }

        public void SetLdapConnection(LdapConnection ldapConnection)
        {
            _ldapConnection = ldapConnection;
        }

        /// <summary>
        ///     Create a new LDAPUser
        /// </summary>
        /// <param name="newUser">User to create</param>
        /// <param name="objectClass"></param>
        /// <returns> Success or Failed</returns>
        public LdapState CreateUser(ILdapUser newUser)
        {
            try
            {
                _ldapConnection.SendRequest(LdapRequestBuilder.GetAddRequest(newUser, _configRepository.GetUserObjectClass()));
            }
            catch (Exception e)
            {
                _logger.Write(_logger.BuildLogMessage(e.Message, LdapState.LdapCreateUserError));
                return LdapState.LdapCreateUserError;
            }
            _logger.Write(_logger.BuildLogMessage("Create User Operation Success", LdapState.LdapCreateUserError));
            return LdapState.LdapUserManipulatorSuccess;
        }

        /// <summary>
        ///     Delete an LDAPUser
        /// </summary>
        /// <param name="user">User to delete</param>
        /// <returns>Success or Failed</returns>
        public LdapState DeleteUser(ILdapUser user)
        {
            try
            {
                _ldapConnection.SendRequest(LdapRequestBuilder.GetDeleteRequest(user));
            }
            catch (Exception e)
            {
                _logger.Write(_logger.BuildLogMessage(e.Message, LdapState.LdapDeleteUserError));
                return LdapState.LdapDeleteUserError;
            }
            _logger.Write(_logger.BuildLogMessage("Delete User Operation Success", LdapState.LdapUserManipulatorSuccess));
            return LdapState.LdapUserManipulatorSuccess;
        }

        /// <summary>
        ///     Modify an LDAPUser Attribute
        /// </summary>
        /// <param name="operationType">Operation to execute on the attribute</param>
        /// <param name="user">LDAPUser's Attribute</param>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="attributeValue">Attribute Value</param>
        /// <returns>Success or Failed</returns>
        public LdapState ModifyUserAttribute(DirectoryAttributeOperation operationType, ILdapUser user,
            string attributeName,
            string attributeValue)
        {
            try
            {
                _ldapConnection.SendRequest(LdapRequestBuilder.GetModifyRequest(user, operationType, attributeName,
                    attributeValue));
            }
            catch (Exception e)
            {
                _logger.Write(_logger.BuildLogMessage(e.Message, LdapState.LdapModifyUserAttributeError));
                return LdapState.LdapModifyUserAttributeError;
            }

            user.GetUserOperation(operationType, attributeName, attributeValue)();

            _logger.Write(_logger.BuildLogMessage("Modify User Attribute Operation Success",
                LdapState.LdapUserManipulatorSuccess));
            return LdapState.LdapUserManipulatorSuccess;
        }

        /// <summary>
        ///     Change an LDAPUser's Password
        /// </summary>
        /// <param name="user">LDAPUser to change the password</param>
        /// <param name="newPwd">New Passowrd</param>
        /// <returns>Success or Failed</returns>
        public LdapState ChangeUserPassword(ILdapUser user, string newPwd)
        {
            try
            {
                _ldapConnection.SendRequest(LdapRequestBuilder.GetModifyPasswordRequest(user, newPwd));
            }
            catch (Exception e)
            {
                _logger.Write(_logger.BuildLogMessage(e.Message, LdapState.LdapChangeUserPasswordError));
                return LdapState.LdapChangeUserPasswordError;
            }

            user.OverwriteUserAttribute("userPassword", newPwd);

            _logger.Write(_logger.BuildLogMessage("Change Password Operation Success",
                LdapState.LdapUserManipulatorSuccess));
            return LdapState.LdapUserManipulatorSuccess;
        }

        /// <summary>
        ///     Search Users in the LDAP system
        /// </summary>
        /// <param name="otherReturnedAttributes">Addictional attributes added to the results LDAPUsers objects</param>
        /// <param name="searchedUsers">Credential for the search</param>
        /// <param name="searchResult">LDAPUsers object returned in the search</param>
        /// <returns>Boolean that comunicate the result of search</returns>
        public LdapState SearchUsers(List<string> otherReturnedAttributes, string[] searchedUsers, out List<ILdapUser> searchResult)
        {
            searchResult = new List<ILdapUser>();
            try
            {
                otherReturnedAttributes = (new List<string> { "cn", "sn" }).Union((otherReturnedAttributes ?? new List<string>())).ToList();
                //Foreach all the credential,for everyone do the search and add user results to the out parameter
                searchResult = searchedUsers.Select(
                    users => 
                        (SearchResponse) _ldapConnection.SendRequest(
                        LdapRequestBuilder.GetSearchUserRequest(_configRepository.GetSearchBaseDn(), LdapFilterBuilder.GetSearchFilter(_configRepository.GetUserObjectClass(), _configRepository.GetMatchFieldName(), users), otherReturnedAttributes)
                        )).Aggregate(searchResult, (current, searchReturn) => current.Concat(LdapUserUtils.ConvertToLdapUsers(searchReturn)).ToList());
            }
            catch (Exception e)
            {
                _logger.Write(_logger.BuildLogMessage(e.Message, LdapState.LdapSearchUserError));
                return LdapState.LdapSearchUserError;
            }

            if (searchResult.Count == 0)
            {
                _logger.Write(_logger.BuildLogMessage("Search Operation with NO RESULTS", LdapState.LdapSearchUserError));
                return LdapState.LdapSearchUserError;
            }
            _logger.Write(_logger.BuildLogMessage("Search Operation Success", LdapState.LdapUserManipulatorSuccess));
            return LdapState.LdapUserManipulatorSuccess;
        }
    }
}