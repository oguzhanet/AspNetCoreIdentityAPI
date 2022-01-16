using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityAPI.Models
{
    public class AuthenticationModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Role { get; set; }
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
