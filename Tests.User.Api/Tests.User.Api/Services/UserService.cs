using Microsoft.EntityFrameworkCore;
using Tests.User.Api.Exceptions;
using Tests.User.Api.Models;

namespace Tests.User.Api.Services
{
    public class UserService
    {
        private readonly DatabaseContext _dbContext;


        public UserService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDetailItem?> GetUserAsync(int id)
        {
            var user = await GetUserFromDbAsync(id);

            return new UserDetailItem()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age
            };
        }

        public async Task CreateUserAsync(CreateUserReq userArgs)
        {
            var user = new UserModel
            {
                FirstName = userArgs.FirstName,
                LastName = userArgs.LastName,
                Age = userArgs.Age
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UpdateUserReq userArgs)
        {
            var user = await GetUserFromDbAsync(userArgs.Id);

            user.FirstName = userArgs.FirstName;
            user.LastName = userArgs.LastName;
            user.Age = userArgs.Age;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserFromDbAsync(id);

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<UserModel> GetUserFromDbAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new UserNotFoundException($"User with ID {id} not found.");
        }
    }
}

