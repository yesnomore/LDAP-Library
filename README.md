LDAP-Library
============


The library is based on System.DirectoryServices.Protocols methods see http://msdn.microsoft.com/en-us/library/bb332056.aspx for more information
With this you can manage the LDAP tree/subtree with read and write operations like:

Write Operations

    Create an user
    Delete an user
    Modify an user attribute
    Change the user password

Read Operations

    Read an user attribute
    Connect the Admin User ( see below for more information )
    Check user connection through proper credential, TSL,SSL and a certificate
    Search an user and try to connect it 

Watch the relative interface in the ILDAPManager.cs file.

The philosophy used in this library bargains for the presence of an administrator user that have permission on a specified node of the LDAP tree where he can operate some of the previous operations ( it depends on the kind of permission, read or write ). After the successfull connection of that user through the Connect method it can make one or more operation listed above.
So to make all things work the library must be configured properly through a config file like the app.config in the unit test project.

Parameters underlined are required, the others can be an arbitrary value

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
