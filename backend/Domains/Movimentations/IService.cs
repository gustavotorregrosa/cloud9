using backend.Shared;

namespace backend.Domains.Movimentations
{
    public interface IMovimentationService : IService<MovimentationDtoIn, MovimentationDtoOut>
    {
        Task<IEnumerable<MovimentationDtoOut>> GetByProductIdAsync(Guid productId);

        Task<IEnumerable<StockPositionDTO>> GetStockPositionOverTime(Guid productId);

    }
}
