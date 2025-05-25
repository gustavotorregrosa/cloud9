using Microsoft.AspNetCore.Mvc;

namespace backend.Domains.Users
{
    //[ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                IEnumerable<UserDTOout> _users = await _userService.GetAllAsync();
                return Ok(_users);
            }
            catch (Exception ex) 
            { 
                return StatusCode(500, new { Message = "An error occurred while retrieving users." });
            }
   
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var _user = await _userService.GetByIdAsync(id);
            if (_user == null)
            {
                return NotFound(new { Message = $"User with ID: {id} not found." });
            }
            return Ok(_user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDtoIn user)
        {
            var _user = await _userService.AddAsync(user);
            if (_user == null)
            {
                return BadRequest(new { Message = "Failed to create user." });
            }

            return Ok(_user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDtoIn user)
        {   
            var _user = await _userService.UpdateAsync(user, id);
            return Ok(new { Message = $"Update user with ID: {id}", User = user });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok(new { Message = $"Delete user with ID: {id}" });
        }
    }
}