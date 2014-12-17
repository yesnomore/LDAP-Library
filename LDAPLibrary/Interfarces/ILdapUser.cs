using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace LDAPLibrary.Interfarces
{
    public interface ILdapUser
    {
        /// <summary>
        ///     Returns the values for the specified attribute name
        ///     THROW EXCEPTION if attribute name isn't found
        /// </summary>
        /// <param name="attributeName">Attribute Name to search of</param>
        /// <returns>Values in string array</returns>
        List<string> GetUserAttribute(string attributeName);

        /// <summary>
        ///     Get all the Attribute Names of an LDAPUser
        /// </summary>
        /// <returns>All the Attribute Names</returns>
        string[] GetUserAttributeKeys();

        Dictionary<string, List<string>> GetUserAttributes();

        /// <summary>
        ///     Get User CN
        /// </summary>
        /// <returns>User CN</returns>
        string GetUserCn();

        /// <summary>
        ///     Get User SN
        /// </summary>
        /// <returns>User SN</returns>
        string GetUserSn();

        /// <summary>
        ///     Get User DN
        /// </summary>
        /// <returns>User DN</returns>
        string GetUserDn();

        /// <summary>
        ///     Set the user attribute values list with a new one
        ///     ERASE THE OLD ONE!!
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValues"></param>
        void OverwriteUserAttribute(string attributeName, List<string> attributeValues);

        void OverwriteUserAttribute(string attributeName, string attributeValue);

        /// <summary>
        ///     Create a new user attribute with the given attribute values
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValues"></param>
        void CreateUserAttribute(string attributeName, List<string> attributeValues);

        void CreateUserAttribute(string attributeName, string attributeValue);

        /// <summary>
        ///     Add a new value to the existing user attribute list
        ///     THOW EXCEPTION if attribute name isn't found
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        void InsertUserAttribute(string attributeName, string attributeValue);

        /// <summary>
        ///     Delete an User attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        void DeleteUserAttribute(string attributeName, string attributeValue);

        /// <summary>
        ///     Retun an Action method from the above depending on the value of the operation type.
        ///     If the operation type in different from: Add, Replace, Remove it throw an exception
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        Action GetUserOperation(DirectoryAttributeOperation operationType, string attributeName, string attributeValue);
    }
}