using Core.Entities;
using Core.Interface;

namespace Application;
public class UserService
{
    private IUserRepository _userRepository;
    private IGenericRepository<User> _userGenericRepository;

    public UserService(IUserRepository userRepository, IGenericRepository<User> userGenericRepository)
    {
        _userRepository = userRepository;
        _userGenericRepository = userGenericRepository;
    }

    #region USER_CRUD_OPERATIONS

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userGenericRepository.GetAll() ;
    }

    public async Task<User> GetById(int id)
    {
        return await _userGenericRepository.GetById(id);
    }

    public async Task Insert(User Entity)
    {
        await _userGenericRepository.Insert(Entity);
    }

    public async Task Update(User Entity)
    {
        await _userGenericRepository.Update(Entity);
    }

    public async Task Delete(int id)
    {
        await _userGenericRepository.Delete(id);
    }

#endregion

    public async Task<bool> Login(User user)
    {
        return await _userRepository.Login(user);
    }

    public async Task<bool> Signup(User user)
    {
        return await _userRepository.Signup(user);
    }

    public async Task<bool> IdExists(User user)
    {
        return await _userRepository.IdExists(user);
    }

    public async Task<bool> PersonalInfo(User user)
    {
        return await _userRepository.PersonalInfo(user);
    }

    public async Task<User> GetUserByUserName(string? un)
    {
        return await _userRepository.GetUserByUserName(un);
    }

    public async Task<User?> GetUserByEmail(string? email)
    {
        return await _userRepository.GetUserByEmail(email);
    }

    public async Task<User> GetUserByEmailOrUsername(string? Query)
    {
        return await _userRepository.GetUserByEmailOrUsername(Query);
    }
}
