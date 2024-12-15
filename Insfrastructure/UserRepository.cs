using Core.Entities;
using Core.Interface;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GadgetGourmentDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public async Task<bool> Login(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM [user] WHERE (username = @UserName OR email = @Email) AND password = @Password";
                int result = await connection.ExecuteScalarAsync<int>(query, new { user.UserName, user.Email, user.Password });
                return result > 0;
            }
        }

        public async Task<bool> Signup(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO [user] (username, email, password) VALUES (@UserName, @Email, @Password)";
                int rowsAffected = await connection.ExecuteAsync(query, new { user.UserName, user.Email, user.Password });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> IdExists(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM [user] WHERE username = @UserName OR email = @Email";
                int result = await connection.ExecuteScalarAsync<int>(query, new { user.UserName, user.Email });
                return result > 0;
            }
        }

        public async Task<bool> PersonalInfo(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [user] 
                                 SET name = @Name, address = @Address, phone = @Phone, gender = @Gender, dateofbirth = @DateOfBirth 
                                 WHERE username = @UserName";
                int rowsAffected = await connection.ExecuteAsync(query, new
                {
                    user.UserName,
                    user.Name,
                    user.Address,
                    user.Phone,
                    user.Gender,
                    user.DateOfBirth
                });
                return rowsAffected > 0;
            }
        }

        public async Task<User?> GetUserByUserName(string? userName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [user] WHERE username = @UserName";
                return await connection.QuerySingleOrDefaultAsync<User>(query, new { UserName = userName });
            }
        }

        public async Task<User?> GetUserByEmail(string? email)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [user] WHERE email = @Email";
                return await connection.QuerySingleOrDefaultAsync<User>(query, new { Email = email });
            }
        }

        public async Task<User?> GetUserByEmailOrUsername(string? queryValue)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [user] WHERE email = @QueryValue OR username = @QueryValue";
                return await connection.QuerySingleOrDefaultAsync<User>(query, new { QueryValue = queryValue });
            }
        }
    }
}
