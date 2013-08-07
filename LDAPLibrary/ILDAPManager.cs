using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;

namespace LDAPLibrary
{
    public interface ILDAPManager
    {
        /// <summary>
        /// Create a new LDAP User
        /// </summary>
        /// <param name="newUser"> The LDAPUser object that contain all the details of the new user to create</param>
        /// <returns>Boolean that comunicate the result of creation</returns>
        bool createUser(LDAPUser newUser);

        /// <summary>
        /// delete the specified  LDAPUser
        /// </summary>
        /// <param name="user">LDAPUser to delete</param>
        /// <returns>the result of operation</returns>
        bool deleteUser(LDAPUser user);

        /// <summary>
        /// Modify an LDAPUser Attribute
        /// </summary>
        /// <param name="operationType">Choose the operation to do, it's an enum</param>
        /// <param name="user">The User to Modify the attribute</param>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="attributeValue">Value of the attribute</param>
        /// <returns></returns>
        bool modifyUserAttribute(DirectoryAttributeOperation operationType, LDAPUser user, string attributeName, string attributeValue);

        /// <summary>
        /// Change the user Password
        /// </summary>
        /// <param name="user">LDAPUser to change the password</param>
        /// <returns></returns>
        bool changeUserPassword(LDAPUser user, string newPwd);

        /// <summary>
        /// Search Users in the LDAP system
        /// </summary>
        /// <param name="baseDN">Starting DN of the search</param>
        /// <param name="otherReturnedAttributes">Addictional attributes added to the results LDAPUsers objects</param>
        /// <param name="searchedUsers">Credential for the search</param>
        /// <param name="searchResult">LDAPUsers object returned in the search</param>
        /// <returns>Boolean that comunicate the result of search</returns>
        bool searchUsers(string baseDN, List<string> otherReturnedAttributes, string [] searchedUsers, out List<LDAPUser> searchResult);

        /// <summary>
        /// Return the Error Message of an occurred LDAP Exception
        /// </summary>
        /// <returns></returns>
        string getLDAPMessage();

        /// <summary>
        /// Instance the Ldap connection
        /// </summary>
        /// <returns></returns>
        bool connect();
        bool connect(NetworkCredential credential);

        /// <summary>
        /// Method that wrap SearchUsers and Connect in one operation
        /// </summary>
        /// <param name="baseDN">The nodo where the search starts</param>
        /// <param name="user">The Username to search and Connect</param>
        /// <param name="password">The passwords of the User</param>
        /// <returns>the operation result</returns>
        bool searchUserAndConnect(string baseDN, string user, string password);
    }
}
