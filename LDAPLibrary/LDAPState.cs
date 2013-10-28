using System;
using System.Collections.Generic;
using System.Linq;

namespace LDAPLibrary
{
    public enum LDAPState
    {
		LDAPLibraryInitSuccess,
        LDAPConnectionSuccess,
        LDAPUserManipulatorSuccess,
        LDAPConnectionError,
        LDAPCreateUserError,
        LDAPDeleteUserError,
        LDAPModifyUserAttributeError,
        LDAPChangeUserPasswordError,
        LDAPSearchUserError,
		LDAPLibraryInitError,
        LDAPGenericError
    }
}
