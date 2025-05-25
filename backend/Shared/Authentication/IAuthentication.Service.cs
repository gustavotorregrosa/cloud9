

using backend.Domains.Users;

namespace backend.Shared.Authentication;

public interface IAuthenticationService
{
    Task<UserDTOout> Authenticate(string email, string password);
    Task<UserDTOout> Register(string name, string email, string password);
    Task<UserDTOout> RefreshToken(string token);
    Task<UserDTOout> ValidateToken(string token, bool refreshToken = false);
    Task<UserDTOout> ValidateRefreshToken(string token);
}