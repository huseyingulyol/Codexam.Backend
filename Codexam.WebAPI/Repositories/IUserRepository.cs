using Codexam.WebAPI.Entities;

namespace Codexam.WebAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByCredentialsAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int userId);
        Task AddUserAsync(User user);
        //Task UpdateUser(User user);
        //Task DeleteUser(int userId);
    }

}