using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityAPI.Models.Enums
{
    public class AuthorizationRole
    {
        public enum Roles
        {
            Admin,
            Editor,
            User
        }

        public const string default_userName = "user";
        public const string default_email = "user@user.com";
        public const string default_password = "password123";
        public const Roles default_role = Roles.User;
    }
}
