using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tests.User.Api.Controllers;
using Tests.User.Api.Models;
using Tests.User.Api.Services;

namespace Tests.User.Api.Test
{
    public class UserControllerTests : IDisposable
    {
        private readonly UserController _controller;
        private readonly DatabaseContext _context;

        public UserControllerTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DatabaseContext(options);
            _controller = new UserController(new UserService(_context));

            SeedTestDatabase();
        }

        private void SeedTestDatabase()
        {
            _context.Users.Add(new UserModel
            {
                FirstName = "Test",
                LastName = "User",
                Age = 20
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task Should_Return_User_When_Valid_Id_Passed()
        {
            //Arrange
            var userInDb = _context.Users.First();

            //Act
            var result = await _controller.GetAsync(userInDb.Id);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var userReturned = okResult.Value as UserDetailItem;
            Assert.NotNull(userReturned);
            Assert.Equal(userInDb.Id, userReturned.Id);
            Assert.Equal(userInDb.FirstName, userReturned.FirstName);
            Assert.Equal(userInDb.LastName, userReturned.LastName);
            Assert.Equal(userInDb.Age, userReturned.Age);
        }

        [Fact]
        public async Task Should_Return_Valid_When_User_Created()
        {
            // Arrange
            var createUserReq = new CreateUserReq
            {
                FirstName = "New",
                LastName = "User",
                Age = 25
            };

            //Act
            var result = await _controller.CreateAsync(createUserReq);
            var okResult = result as OkResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            // Verify the user was created
            var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == createUserReq.FirstName && u.LastName == createUserReq.LastName);
            Assert.NotNull(createdUser);
        }

        [Fact]
        public async Task Should_Return_Valid_When_User_Updated()
        {
            // Arrange
            var updateUserReq = new UpdateUserReq
            {
                Id = 1,
                FirstName = "Updated",
                LastName = "User",
                Age = 21
            };

            //Act
            IActionResult result = await _controller.UpdateAsync(updateUserReq);
            var okResult = result as OkResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            // Verify the user was updated
            var updatedUser = await _context.Users.FindAsync(1);
            Assert.NotNull(updatedUser);
            Assert.Equal("Updated", updatedUser.FirstName);
            Assert.Equal(21, updatedUser.Age);
        }

        [Fact]
        public async Task Should_Return_Valid_When_User_Removed()
        {
            //Arrange
            var userInDb = _context.Users.First();

            //Act
            var result = await _controller.DeleteAsync(userInDb.Id);
            var okResult = result as OkResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            // Verify the user was removed
            var deletedUser = await _context.Users.FindAsync(userInDb.Id);
            Assert.Null(deletedUser);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}