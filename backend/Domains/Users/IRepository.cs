using backend.Shared;

namespace backend.Domains.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmail(string email);
    }
}