using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace backend.Domains.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>().ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Set<User>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user by ID.", ex);
            }
        }

        public async Task<User> AddAsync(User user)
        {
            try
            {
                var _user = await _context.Set<User>().AddAsync(user);
                await _context.SaveChangesAsync();

                return _user.Entity;
            }          
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user.", ex);
            }
        }

        public async Task<User> UpdateAsync(User user)
        {
            try
            {
                var _user = _context.Set<User>().Update(user);
                await _context.SaveChangesAsync();
                return _user.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        public async Task<User> DeleteAsync(Guid id){
            try
            {
                var user = await GetByIdAsync(id);

                if(user == null)
                {
                    throw new Exception("User not found.");
                }
  
                _context.Set<User>().Remove(user);
                await _context.SaveChangesAsync();
                return user;
                
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user.", ex);
            }
                
        }

        public async Task<User?> GetByEmail(string email)
        {
            try
            {
                return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user by email.", ex);
            }
        }
    }


}