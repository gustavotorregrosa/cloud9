using backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace backend.Domains.Movimentations
{
    public class MovimentationRepository : IMovimentationRepository
    {
        private readonly ApplicationDbContext _context;

        public MovimentationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movimentation>> GetAllAsync()
        {
            return await _context.Set<Movimentation>().ToListAsync();
        }

        public async Task<Movimentation> GetByIdAsync(Guid id)
        {
            return await _context.Set<Movimentation>().FindAsync(id);
        }

        public async Task<Movimentation> AddAsync(Movimentation movimentation)
        {
            movimentation.CreatedAt = DateTime.UtcNow;
            movimentation.UpdatedAt = DateTime.UtcNow;
            var entity = await _context.Set<Movimentation>().AddAsync(movimentation);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<Movimentation> UpdateAsync(Movimentation movimentation)
        {
            movimentation.UpdatedAt = DateTime.UtcNow;
            var entity = _context.Set<Movimentation>().Update(movimentation);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<Movimentation> DeleteAsync(Guid id)
        {
            var movimentation = await GetByIdAsync(id);
            if (movimentation == null)
                return null;
            _context.Set<Movimentation>().Remove(movimentation);
            await _context.SaveChangesAsync();
            return movimentation;
        }
    }
}
