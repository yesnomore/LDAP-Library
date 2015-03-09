LDAP-Library
============


The library is based on System.DirectoryServices.Protocols methods see [here](http://msdn.microsoft.com/en-us/library/bb332056.aspx) for more information about the middleware.
With this you can manage the LDAP tree/subtree with read and write operations like:

- **CRUD OPERATION** on an user 
- Search for an user under a specified node of the LDAP tree.
- Check the credential of a specified user

Watch the relative interface in the [ILDAPManager.cs](https://github.com/Apex-net/LDAP-Library/blob/master/LDAPLibrary/Interfarces/ILDAPManager.cs) file.

The philosophy used in this library bargains for the presence of an administrator user that have permissions, on a specified node of the LDAP tree where he can operate some of the previous operations. 
There's also some particular configuration of the library where the presence of the Administrator user is not needed.

Input Parameters List
---------------------

To make all things work the library needs a set of input parameters. Check the table below for the details of this parameters

|        Parameter Name        |                                                                         Meaning                                                                         |           Example           | Optional Value                                       | Default Value |
|:----------------------------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------:|:---------------------------:|------------------------------------------------------|---------------|
|        **LDAPServer**        | URL of the LDAP server with port                                                                                                                        |        127.0.0.1:389        | No                                                   |               |
|       **LDAPAuthType**       | see [this](http://msdn.microsoft.com/it-it/library/system.directoryservices.protocols.authtype.aspx) page for the correct value.                        |            Basic            | No                                                   |               |
|       **LDAPAdminMode**      | used to specify if the LDAP Library need to connect with: *Admin*, *NoAdmin* or *Anonymous* mode to the LDAP server in order to perform the operations. |            Admin            | No                                                   |               |
|      **LDAPAdminUserDN**     | Distinguished Name of the Admin User                                                                                                                    | cn=Manager,dc=maxcrc,dc=com | No, if the *LDAPAdminMode* in *Admin*. Yes Otherwise |               |
|      **LDAPAdminUserCN**     | Common Name of the Admin User                                                                                                                           |           Manager           | No, if the *LDAPAdminMode* in *Admin*. Yes Otherwise |               |
|      **LDAPAdminUserSN**     | Surename of the Admin User                                                                                                                              |             Cool            | No, if the *LDAPAdminMode* in *Admin*. Yes Otherwise |               |
|   **LDAPAdminUserPassword**  | Password of the Admin User                                                                                                                              |            secret           | No, if the *LDAPAdminMode* in *Admin*. Yes Otherwise |               |
|    **LDAPUserObjectClass**   | The attribute *ObjectClass* used to indentify an user                                                                                                   |            person           | Requested only in the Full Library Constructor       | person        |
|  **LDAPMatchFieldUsername**  | Field used in search filter to know what is the LDAP attribute to match with username                                                                   |              cn             | Requested only in the Full Library Constructor       | cn            |
|     **LDAPSearchBaseDN**     | Base node where the library can operate (search, connect, create, modify)                                                                               |  ou=People,dc=maxcrc,dc=com | No                                                   |               |
|   **enableLDAPLibraryLog**   | Show where log the library. Possible values: *File*, *EventViewer*, *None*                                                                              |             File            | No                                                   |               |
|    **LDAPLibraryLogPath**    | Location into write the log file if the enableLDAPLibraryLog has value File. (optional value)                                                           |           C:\.....          | No, if theenableLDAPLibraryLog is *File*             |               |
|   **secureSocketLayerFlag**  | Specify if establish the connection through SecureSocketLayer                                                                                           |             true            | Requested only in the Full Library Constructor       | false         |
| **transportSocketLayerFlag** | Specify if establish the connection through TransportSocketLayer                                                                                        |             true            | Requested only in the Full Library Constructor       | false         |
|  **ClientCertificationFlag** | Specify if establish the connection through a specific Certification file                                                                               |             true            | Requested only in the Full Library Constructor       | false         |
|   **clientCertificatePath**  | Path of a certificate file                                                                                                                              |          C:\......          | Requested only in the Full Library Constructor       | ""            |

Code Snippets
=============

Here some example of how to use the library to perform the main operations. You can see this code in action in the unit test project inside the repository.

Parameters used in those snippets:

```cs
#region Users

//READ ONLY USER
private const string ReadOnlyUserCn = "Matteo";
private const string ReadOnlyUserPwd = "1";
private const string ReadOnlyUserDn = "cn=" + ReadOnlyUserCn + ",o=ApexNet,ou=People,dc=maxcrc,dc=com";
//WRITE USER THIS MUST NOT EXIST INITIALLY
private const string WriteUserCn = "Fabio";
private const string WriteUserPwd = "1";
private const string WriteUserDn = "cn=" + WriteUserCn + ",o=ApexNet,ou=People,dc=maxcrc,dc=com";

#endregion

#region Localhost Configuration

private const AuthType LdapAuthType = AuthType.Basic;
private const string LdapServer = "127.0.0.1:389";

private const string LdapAdminUserDn = "cn=Manager,dc=maxcrc,dc=com";
private const string LdapAdminUserCn = "Manager";
private const string LdapAdminUserSn = "test";
private const string LdapAdminUserPassword = "secret";

private const string LdapSearchBaseDn = "o=ApexNet,ou=People,dc=maxcrc,dc=com";
private const string LdapUserObjectClass = "person";
private const string LdapMatchFieldUsername = "cn";
private const LoggerType EnableLdapLibraryLog = LoggerType.File;
private const bool SecureSocketLayerFlag = false;
private const bool TransportSocketLayerFlag = false;
private const bool ClientCertificationFlag = false;
private const string ClientCertificatePath = "null";
private const LDAPAdminMode AdminMode = LDAPAdminMode.Admin;

private static readonly LdapUser AdminUser = new LdapUser(LdapAdminUserDn,
    LdapAdminUserCn,
    LdapAdminUserSn,
    new Dictionary<string, List<string>> {{"userPassword", new List<string> {LdapAdminUserPassword}}});

private static readonly string LdapLibraryLogPath = string.Format("{0}", AppDomain.CurrentDomain.BaseDirectory);

```