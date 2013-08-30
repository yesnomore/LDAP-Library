using System;
using System.Collections.Generic;
using System.Linq;

namespace LDAPLibrary
{
    public enum LDAPState
    {
        LDAPConnectionSuccess,
        LDAPUserManipulatorSuccess,
        LDAPConnectionError,
        LDAPCreateUserError,
        LDAPDeleteUserError,
        LDAPModifyUserAttributeError,
        LDAPChangeUserPasswordError,
        LDAPSearchUserError,
        LDAPGenericError
    }
}
