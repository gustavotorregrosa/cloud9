namespace backend.Domains.Users;

class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDTOout>> GetAllAsync()
    {
        return (await _userRepository.GetAllAsync()).ToList().Select(user => MapToDtoOut(user));
    }

    public async Task<UserDTOout> GetByIdAsync(Guid id)
    {
        User _user = await _userRepository.GetByIdAsync(id);

        if (_user == null) {
            throw new Exception("User not found.");
        }

        return MapToDtoOut(_user);
    }

    public async Task<UserDTOout> AddAsync(UserDtoIn user)
    {
        try
        {
            User _user = MapToEntity(user);
            var _userEntity = await _userRepository.AddAsync(_user);

            if (_userEntity == null)
            {
                throw new Exception("Failed to add the user.");
            }

            return MapToDtoOut(_userEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding the user: {ex.Message}");
        }
    }

    public async Task<UserDTOout> UpdateAsync(UserDtoIn user, Guid id)
    {
        try
        {
            User _user = MapToEntity(user);
            _user.Id = id;
            var _userEntity = await _userRepository.UpdateAsync(_user);

            if (_userEntity == null)
            {
                throw new Exception("User not found.");
            }

            return MapToDtoOut(_userEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating the user: {ex.Message}");
        }
    }

    public async Task<UserDTOout> DeleteAsync(Guid id)
    {
        try
        {
            var _userEntity = await _userRepository.DeleteAsync(id);
            if (_userEntity == null)
            {
                throw new Exception("User not found.");
            }
            return MapToDtoOut(_userEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the user: {ex.Message}");
        }
    }

    public async Task<UserDTOout> GetByEmail(string email)
    {
        try
        {
            var _userEntity = await _userRepository.GetByEmail(email);
            if (_userEntity == null)
            {
            throw new Exception("User not found.");
            }
            return MapToDtoOut(_userEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the user by email: {ex.Message}");
        }
    }

    private UserDTOout MapToDtoOut(User user)
    {
       UserDTOout userDto = new UserDTOout
       {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Active = user.Active
       };
       
       return userDto;
    }

    private User MapToEntity(UserDtoIn userDto)
    {
        User user = new User
        {
            Id = userDto.Id ?? Guid.Empty,
            Name = userDto.Name,
            Email = userDto.Email,
            
            Active = true
        };
        return user;
    }
}