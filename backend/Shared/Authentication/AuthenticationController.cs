using Microsoft.AspNetCore.Mvc;
using backend.Shared.Authentication;
using backend.Domains.Users;

namespace backend.Shared.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // POST: api/Authentication/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _authenticationService.Authenticate(loginDto.Email, loginDto.Password);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        // POST: api/Authentication/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var user = await _authenticationService.Register(registerDto.Name, registerDto.Email, registerDto.Password);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Authentication/RefreshToken
        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {

                var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    throw new UnauthorizedAccessException("Authorization header missing or invalid.");
                }
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                // var refreshTokenDto = new RefreshTokenDto { Token = token };
                var user = await _authenticationService.RefreshToken(token);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        // POST: api/Authentication/ValidateToken
        [HttpPost("ValidateToken")]
        public async Task<IActionResult> ValidateToken([FromBody] TokenDto tokenDto)
        {
            try
            {
                var user = await _authenticationService.ValidateToken(tokenDto.Token);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}