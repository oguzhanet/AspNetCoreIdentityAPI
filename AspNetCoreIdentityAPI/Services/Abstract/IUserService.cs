using AspNetCoreIdentityAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityAPI.Services.Abstract
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterModel registerModel);
        Task<AuthenticationModel> GetTokenAsync(TokenRequestModel tokenRequestModel);
        Task<string> AddRoleAsync(AddRoleModel addRoleModel);
    }
}
