using Core.Entities;

namespace Core.Interface
{
    // pure abstract class
    public interface IUserRepository
    { 
        Task<bool> Login(User user);
        Task<bool> Signup(User user);
        Task<bool> PersonalInfo(User user);
        Task<User> GetUserByUserName(string? un);

        // Utility Functions
        public Task<bool> IdExists(User user);
        public Task<User?> GetUserByEmail(string? email);
        public Task<User> GetUserByEmailOrUsername(string? Query);

	}
}
