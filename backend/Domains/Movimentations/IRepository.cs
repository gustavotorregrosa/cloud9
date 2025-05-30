using backend.Shared;

namespace backend.Domains.Movimentations
{
    public interface IMovimentationRepository : IRepository<Movimentation>
    {
        Task<IEnumerable<Movimentation>> GetByProductIdAsync(Guid productId);
    }
}
