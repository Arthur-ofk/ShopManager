using BLL.Services;
using BLL.Shared;
using DAL.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace UserService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _userService;
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;

        public UsersController(UsersService userService, UserManager<User> userManager , JwtService jwtService)
        {
            _userService = userService;
            _userManager = userManager;
            _jwtService = jwtService;
        }

       
        [Authorize(AuthenticationSchemes ="Bearer",Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetUsers([FromQuery] string searchTerm, [FromQuery] string sortBy = "UserName", [FromQuery] bool ascending = true, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = await _userService.GetAllUsersAsync(searchTerm, sortBy, ascending, page, pageSize);
            return Ok(users);
           
        }
        [Authorize(AuthenticationSchemes = "Bearer",Roles = "ad,Admin")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserDTO>> GetUser(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CreateUserDTO>> CreateUser(CreateUserDTO userDto, string roleName)
        {
            var result = await _userService.AddUserAsync(userDto, roleName);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPut("{userName}")]
        public async Task<ActionResult> UpdateUser(string userName, UpdateUserDTO userDto)
        {
           

            await _userService.UpdateUserAsync(userDto, userName);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }
    }
}
