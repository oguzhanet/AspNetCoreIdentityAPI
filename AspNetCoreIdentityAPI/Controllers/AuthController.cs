using AspNetCoreIdentityAPI.Models;
using AspNetCoreIdentityAPI.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterModel registerModel)
        {
            var result = await _userService.RegisterAsync(registerModel);
            return Ok(result);
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync(AddRoleModel addRoleModel)
        {
            var result = await _userService.AddRoleAsync(addRoleModel);
            return Ok(result);
        }

        [HttpPost("gettoken")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestModel tokenRequestModel)
        {
            var result = await _userService.GetTokenAsync(tokenRequestModel);
            return Ok(result);
        }
    }
}
