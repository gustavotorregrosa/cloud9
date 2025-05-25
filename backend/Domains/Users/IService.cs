using backend.Domains;
using backend.Shared;

namespace backend.Domains.Users
{
    public interface IUserService : IService<UserDtoIn, UserDTOout>
    {
        Task<UserDTOout?> GetByEmail(string email);

    }
}
