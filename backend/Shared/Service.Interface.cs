namespace backend.Shared
{
    public interface IService<I, O>
    {
        Task<IEnumerable<O>> GetAllAsync();
        Task<O> GetByIdAsync(Guid id);
        Task<O> AddAsync(I dto);
        Task<O> UpdateAsync(I dto, Guid id);
        Task<O> DeleteAsync(Guid id);
    }
}