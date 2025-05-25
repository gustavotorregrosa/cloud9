using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Domains.Users;
using Microsoft.IdentityModel.Tokens;

namespace backend.Shared.Authentication;

public class AuthenticateService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthenticateService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<UserDTOout> Authenticate(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return await GenerateJWT(user);
    }

   
    public async Task<UserDTOout> Register(string name, string email, string password)
    {
        var existingUser = await _userRepository.GetByEmail(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = HashPassword(password)
        };

        await _userRepository.AddAsync(user);

        var token = GenerateToken(user);
        var refreshToken = GenerateRefreshToken(user);
        return new UserDTOout
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            AccessToken = token,
            RefreshToken = refreshToken,
            Active = user.Active
        };
    }

    public async Task<UserDTOout> RefreshToken(string token)
    {
        UserDTOout _userDTO = await ValidateToken(token, true);
        User user = await _userRepository.GetByEmail(_userDTO.Email);

        return await GenerateJWT(user);
    }

  
    public Task<UserDTOout> ValidateToken(string token, bool refreshToken = false)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = refreshToken ? Encoding.ASCII.GetBytes(_configuration["Jwt:RefreshKey"]) : Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;

            var userId = jwtToken.Claims.First(x => x.Type.Equals("nameid")).Value;
            var userName = jwtToken.Claims.First(x => x.Type.Equals("unique_name")).Value;
            var userEmail = jwtToken.Claims.First(x => x.Type.Equals("email")).Value;

            var user = new UserDTOout
            {
                Id = Guid.Parse(userId),
                Name = userName,
                Email = userEmail,
            };
            
            return Task.FromResult(user);
        }
        catch
        {
            throw new UnauthorizedAccessException("Invalid token.");
        }
       
    }

    public Task<UserDTOout> ValidateRefreshToken(string refreshToken)
    {
        return ValidateToken(refreshToken, true);
    }

    private string GenerateToken(User user, string? key = null, int? expiration = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        string _token_key = key ?? _configuration["Jwt:Key"];
        var _key = Encoding.ASCII.GetBytes(_token_key);
        // int _expiration = expiration ?? 15; 
        int _expiration = 2;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_expiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken(User user)
    {
        string _key = _configuration["Jwt:RefreshKey"];
        int _minutes = 60 * 24;

        return GenerateToken(user, _key, _minutes);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private async Task<UserDTOout> GenerateJWT(User user){
        var token = GenerateToken(user);
        var refreshToken = GenerateRefreshToken(user);
        return new UserDTOout
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            AccessToken = token,
            RefreshToken = refreshToken,
            Active = user.Active
        };
    }

}