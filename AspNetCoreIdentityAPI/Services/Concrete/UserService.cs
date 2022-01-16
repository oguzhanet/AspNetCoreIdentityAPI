using AspNetCoreIdentityAPI.Models;
using AspNetCoreIdentityAPI.Models.Enums;
using AspNetCoreIdentityAPI.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityAPI.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jWT;

        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jWT)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jWT = jWT.Value;
        }

        public async Task<string> AddRoleAsync(AddRoleModel addRoleModel)
        {
            var user = await _userManager.FindByEmailAsync(addRoleModel.Email);

            if (user == null) { return $"No Accounts Registered with {addRoleModel.Email}"; }

            if (await _userManager.CheckPasswordAsync(user, addRoleModel.Password))
            {
                var roleExists = Enum.GetNames(typeof(AuthorizationRole.Roles)).Any(x => x.ToLower() == addRoleModel.Role.ToLower());

                if (roleExists)
                {
                    var validRole = Enum.GetValues(typeof(AuthorizationRole.Roles)).Cast<AuthorizationRole.Roles>()
                        .Where(x => x.ToString().ToLower() == addRoleModel.Role.ToLower()).FirstOrDefault();

                    await _userManager.AddToRoleAsync(user, validRole.ToString());
                    return $"Added {addRoleModel.Role} to user {addRoleModel.Email}.";
                }

                return $"Role {addRoleModel.Role} not found.";
            }

            return $"Incorrect Credentials for user {user.Email}."; 
        }

        public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel tokenRequestModel)
        {
            AuthenticationModel authenticationModel;

            var user = await _userManager.FindByEmailAsync(tokenRequestModel.Email);
            if (user == null) { return new AuthenticationModel { IsAuthenticated = false, Message = $"No Accounts Registered with {tokenRequestModel.Email}." }; }

            if (await _userManager.CheckPasswordAsync(user, tokenRequestModel.Password))
            {
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);

                authenticationModel = new AuthenticationModel
                {
                    IsAuthenticated = true,
                    Message = jwtSecurityToken.ToString(),
                    UserName = user.UserName,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Email = user.Email
                };

                var rolesLİst = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Role = rolesLİst.ToList();
                return authenticationModel;
            }
            else
            {
                authenticationModel = new AuthenticationModel() { IsAuthenticated = false, Message = $"Incorrect Credentials for user {user.Email}." };
            }

            return authenticationModel;
        }

        public async Task<string> RegisterAsync(RegisterModel registerModel)
        {
            var user = new AppUser
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName
            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(registerModel.Email);

            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, registerModel.Password);
                }
                return $"User Registered {user.UserName}";
            }
            else { return $"Email {user.Email} is already registered"; }
        }

        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var userClaim = await _userManager.GetClaimsAsync(user);
            var role = await _userManager.GetRolesAsync(user);
            var roleClaim = new List<Claim>();

            for (int i = 0; i < role.Count; i++)
            {
                roleClaim.Add(new Claim("role", role[i]));
            }

            var claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)
            }
            .Union(userClaim)
            .Union(roleClaim);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWT.Key));
            var singInCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(issuer: _jWT.Issuer, audience: _jWT.Audience, claims: claim, expires: DateTime.UtcNow.AddMinutes(_jWT.DurationInMinutes), signingCredentials: singInCredential);
            return jwtSecurityToken;
        }
    }
}
