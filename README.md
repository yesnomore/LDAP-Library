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

    LDAPServer: URL of the LDAP server with port (address:port - 127.0.0.1:389)
    LDAPAuthType: see this page for the correct value, tipically "Basic" works with OpenLDAP and "Negotiate" with AD
    LDAPAdminUserDN: the DN of the user admin
    LDAPAdminUserCN: the CN of the user admin
    LDAPAdminUserSN: the SN of the user admin
    LDAPAdminUserPassword: the password of the user admin
    LDAPUserObjectClass: the attribute ObjectClass used to indentify an user
    LDAPMatchFieldUsername: field used in search filter to know what is the LDAP attribute to match with username
    LDAPSearchBaseDN: base node where admin user can operate
    enableLDAPLibraryLog[Boolean Type]: enable the write of a log file
    LDAPLibraryLogPath: path of a log file (KEEP ATTENTION AT THE final '/' in the path)
    secureSocketLayerFlag[Boolean Type]: specify if establish the connection through SecureSocketLayer
    transportSocketLayerFlag[Boolean Type]: specify if establish the connection through TransportSocketLayer
    ClientCertificationFlag[Boolean Type]: specify if establish the connection through ClientCertification
    clientCertificatePath: path of a certificate file ( insert the name in the value )
