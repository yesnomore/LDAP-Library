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

those parameters are a localhost installation of [openLDAP for windows](http://www.userbooster.de/en/download/openldap-for-windows.aspx)
Here a [guide](http://www.userbooster.de/en/support/feature-articles/openldap-for-windows-installation.aspx) to the installation.
I suggest to install with the MDB (Memory Mapped Database) option.

The *ReadOnlyUser* already exist in the LDAP localhost test enviroment, the *WriteUser* not. Either will be instantiate in the tests.

Create a LDAP User
-------------

The LDAP User object is an entity used inside the library to manage the users and their attributes.
It's used in the whole library as input and output parameters.
Here's a simple snippet showing how to create one of this entities.

```cs
private static readonly LdapUser AdminUser = new LdapUser(LdapAdminUserDn,
    LdapAdminUserCn,
    LdapAdminUserSn,
    new Dictionary<string, List<string>> {{"userPassword", new List<string> {LdapAdminUserPassword}}});
```

As you can see the DN, CN and SN attribute are required for the creation. The last parameter specify the key-value pairs of attributes for the specific user.
Attention: with this creation the ldap system is not modified, no operation will be perform. To apply some change use the operations below.

Init of the Library
-------------
Here's how to init the library with all the parameters and with another simplest constructor with only the needed parameters. The optional parameters in this case will be inizialized with the default values.
```cs
ILdapManager _ldapManagerObj = new LdapManager(AdminUser,AdminMode,
			                LdapServer,
			                LdapSearchBaseDn,
			                LdapAuthType,
			                SecureSocketLayerFlag,
			                TransportSocketLayerFlag,
			                ClientCertificationFlag,
			                ClientCertificatePath,
			                EnableLdapLibraryLog,
			                LdapLibraryLogPath,
			                LdapUserObjectClass,
			                LdapMatchFieldUsername
			                );

ILdapManager _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType, EnableLdapLibraryLog, LdapLibraryLogPath
                );			                
```

Connect of the Administator User
-------------
In order to use the library with CRUD operations, first of all, it's required to connect the admin user with this call:

```cs
_ldapManagerObj.Connect()
```

with the *Anonymous* and *NoAdmin* mode this operation is not required, but probably the library will be unable to perform the write operations on the user. It depends from the policy of the LDAP system to query.

READ OPERATIONS (can be done in any library mode)
=============

For every read operation there's a test also with write permission.
Check the [Unit Test Project code](https://github.com/Apex-net/LDAP-Library/tree/documentation/LDAPLibraryUnitTest) for details.

Direct User Connect
-------------

Here's how to test directly the connection of an user.
It can be usefull in case you want to check the connection of a specified user after a large search in the LDAP system or in case of a *NoAdmin* mode, where the ldap system respond directly to the connect method and you don't need to provide a dn attribute for instance.

```cs 
bool result = _ldapManagerObj.Connect(new NetworkCredential(
                ReadOnlyUserDn, ReadOnlyUserPwd,
                ""),
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag);

Assert.IsTrue(result);
```

Search User and Connect
-------------
Here's how to search an user and try directly to connect it.
This method is a syntactic sugar, it can be done executing first the search operation and after the connect one.

```cs
bool result = _ldapManagerObj.SearchUserAndConnect(ReadOnlyUserCn, ReadOnlyUserPwd);

Assert.IsTrue(result);
```

Search Users 
-------------

In this snippet there's showed how to search a set of users and also how to specify a particular set of returning attributes of the users result.
The search operation happens using *LDAPSearchBaseDN* as a base root node and the *LDAPMatchFieldUsername* after the result of the search, in order to know what is the parameter to match the username, and try the connect operation.

```cs
string[] userIdToSearch =
{
    ReadOnlyUserCn
};
string[] fakeuserIdToSearch =
{
    WriteUserCn
};
var userAttributeToReturnBySearch = new List<string>
{
    "description"
};

List<ILdapUser> returnUsers;

bool result = _ldapManagerObj.SearchUsers(userAttributeToReturnBySearch, fakeuserIdToSearch, out returnUsers);

Assert.IsFalse(result);
Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP SEARCH USER ERROR: ");

result = _ldapManagerObj.SearchUsers(null, userIdToSearch, out returnUsers);

Assert.IsTrue(result);
Assert.AreEqual(returnUsers.Count, userIdToSearch.Length);
Assert.AreEqual(returnUsers[0].GetUserCn(), ReadOnlyUserCn);
Assert.IsTrue(returnUsers[0].GetUserAttributes().Count == 0);

result = _ldapManagerObj.SearchUsers(userAttributeToReturnBySearch, userIdToSearch, out returnUsers);

Assert.IsTrue(result);
Assert.AreEqual(returnUsers.Count, userIdToSearch.Length);
Assert.AreEqual(returnUsers[0].GetUserCn(), ReadOnlyUserCn);
```



CUD OPERATIONS (require the administrator user and write permissions )
=============

Create an User
-------------

Here's a snippet to create a new user:

```cs
//Init the DLL and connect the admin
TestAdminConnect();

//Create existing user
bool result = _ldapManagerObj.CreateUser(existingUser);

//Assert the correct operations
Assert.IsFalse(result);
Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP CREATE USER ERROR: ");

//Create user
result = _ldapManagerObj.CreateUser(tempUser);

//Assert the correct operations
Assert.IsTrue(result);
Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
    "LDAP USER MANIPULATION SUCCESS: ");
```

As you can see, in the first call to the *CreateUser* method it return false because the user already exist in the system, but in the second call the result is true for a new user. Another thing to mention here is the call at the *GetLdapMessage* method that return the last executed operation output message.

Delete an User
-------------

In the same way for the create user here you can see two call of the *DeleteUser* and the relative out message.

```cs
//Delete user
result = _ldapManagerObj.DeleteUser(testLdapUser);

//Assert the correct operations
Assert.IsTrue(result);
Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
    "LDAP USER MANIPULATION SUCCESS: ");

//Delete user again with error
result = _ldapManagerObj.DeleteUser(testLdapUser);

//Assert the correct operations
Assert.IsFalse(result);
Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP DELETE USER ERROR: ");
```

Modify User Attribute
-------------

In this snippet is used the *ModifyUserAttribute*. 
The first few lines is showed the creation of the testuser object, so you can see what attributes it has.
In the first call of the modify method is showed a wrong call that return a false value. This is because the attribute involved is a fake one and don't exist in the user's attributes set ("ciccio").
The second one is a successfull call that change the value of description attribute.
As always is also showed the output message of the library.

```cs

var testLdapUser = new LdapUser(WriteUserDn, WriteUserCn, "test",
    new Dictionary<string, List<string>> {{"description", new List<string> {"test"}}});
bool result = _ldapManagerObj.CreateUser(testLdapUser);

Assert.IsTrue(result);

List<ILdapUser> returnUsers;
const string userAttributeValue = "description Modified";

result = _ldapManagerObj.ModifyUserAttribute(DirectoryAttributeOperation.Delete, testLdapUser, "ciccio",
    userAttributeValue);

Assert.IsFalse(result);
Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
    "LDAP MODIFY USER ATTRIBUTE ERROR: ");

result = _ldapManagerObj.ModifyUserAttribute(DirectoryAttributeOperation.Replace, testLdapUser,
    "description", userAttributeValue);

Assert.IsTrue(result);
Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
    "LDAP USER MANIPULATION SUCCESS: ");
```
Change User Password
-------------
In this case there's only a success call of the Change User Password.
Later is test the connection with the old and new credentials using the *Connect(NetworkCredential)* method, you can check out this part in the unit test project. 

```cs
var testUser = new LdapUser(WriteUserDn, WriteUserCn, "test",
new Dictionary<string, List<string>> {{"userPassword", new List<string> {WriteUserPwd}}});
//Create the user
bool result = _ldapManagerObj.CreateUser(testUser);

Assert.IsTrue(result);

//Perform change of password
result = _ldapManagerObj.ChangeUserPassword(testUser, newPassword);
Assert.IsTrue(result);
Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
"LDAP USER MANIPULATION SUCCESS: ");
```
Extended Documentation and Code Coverage References
-------------
If you want to explore every part of the library, you can check [the online documentation](http://apex-net.github.io/LDAP-Library/LDAPLibraryDocumentation/) and the [CodeCoverage] (http://apex-net.github.io/LDAP-Library/LDAPLibraryCodeCoverage/)
