using backend.Shared;

namespace backend.Domains.Movimentations
{
    public class MovimentationService : IMovimentationService
    {
        private readonly IMovimentationRepository _repository;

        public MovimentationService(IMovimentationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MovimentationDtoOut>> GetByProductIdAsync(Guid productId)
        {
            return (await _repository.GetByProductIdAsync(productId)).Select(MapToDto);
        }

        public async Task<IEnumerable<StockPositionDTO>> GetStockPositionOverTime(Guid productId)
        {
            var movementations = await _repository.GetByProductIdAsync(productId);
            List<StockPositionDTO> stockPositions = new List<StockPositionDTO>();
            foreach (var movimentation in movementations)
            {
                StockPositionDTO _movimentation = new StockPositionDTO
                {
                    AtDate = movimentation.CreatedAt.Date,
                    Value = movimentation.Amount
                };

                stockPositions.Add(_movimentation);

            }

            return stockPositions.OrderBy(x => x.AtDate);

        }

        public async Task<IEnumerable<MovimentationDtoOut>> GetAllAsync()
        {
            return (await _repository.GetAllAsync()).Select(MapToDto);
        }

        public async Task<MovimentationDtoOut> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Movimentation not found.");
            return MapToDto(entity);
        }

        public async Task<MovimentationDtoOut> AddAsync(MovimentationDtoIn dto)
        {
            var entity = MapToEntity(dto);
            var added = await _repository.AddAsync(entity);
            return MapToDto(added);
        }

        public async Task<MovimentationDtoOut> UpdateAsync(MovimentationDtoIn dto, Guid id)
        {
            var entity = MapToEntity(dto);
            entity.Id = id;
            var updated = await _repository.UpdateAsync(entity);
            return MapToDto(updated);
        }

        public async Task<MovimentationDtoOut> DeleteAsync(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (deleted == null)
                throw new Exception("Movimentation not found.");
            return MapToDto(deleted);
        }

        private MovimentationDtoOut MapToDto(Movimentation entity)
        {
            return new MovimentationDtoOut
            {
                Id = entity.Id,
                Amount = entity.Amount,
                ProductId = entity.ProductId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        private Movimentation MapToEntity(MovimentationDtoIn dto)
        {
            return new Movimentation
            {
                Id = dto.Id ?? Guid.Empty,
                Amount = dto.Amount,
                ProductId = dto.ProductId
            };
        }
    }
}
