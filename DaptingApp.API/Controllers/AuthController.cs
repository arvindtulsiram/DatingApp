using System.Threading.Tasks;
using DaptingApp.API.Data;
using DaptingApp.API.Dtos;
using DaptingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DaptingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repos;
        public AuthController(IAuthRepository repos)
        {
            _repos = repos;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
            if (await _repos.UserExists(userForRegisterDto.UserName))
            {
                return BadRequest("User already exists.");
            }
            var userToCreate = new User
            {
                UserName = userForRegisterDto.UserName
            };

            var createdUser = await _repos.Register(userToCreate, userForRegisterDto.Password);
            return StatusCode(201);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(UserForRegisterDto userForRegisterDto)
        {
            if (!await _repos.UserExists(userForRegisterDto.UserName))
                return BadRequest("Invalid login credentials. User does not exist.");
            
            if (await _repos.Login(userForRegisterDto.UserName, userForRegisterDto.Password) == null)
                return BadRequest("Invalid login credentials.");

            return StatusCode(200);

        }
    }
}