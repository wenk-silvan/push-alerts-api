using System;
using System.Collections.Generic;
using NUnit.Framework;
using PushAlertsApi.Models;
using PushAlertsApi.Services;
using System.Linq;
using MockQueryable.NSubstitute;

namespace PushAlertsApi.Tests
{
    public class UsersServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetUserByUuid_Normal()
        {
            // Arrange
            var alice = new User
            {
                Email = "alice@company.com",
                Id = 1,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };
            var bob = new User
            {
                Email = "bob@company.com",
                Id = 2,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };
            var charlie = new User
            {
                Email = "charlie@company.com",
                Id = 3,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };

            List<User> expected = new() { alice, bob, charlie };

            // Act
            var usersService = new UsersService(expected.AsQueryable().BuildMockDbSet());
            var actual = usersService.GetUserByUuid(alice.Uuid.ToString());

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(alice.Id, actual.Id);
        }

        [Test]
        public void GetUserByUuid_NotFound()
        {
            // Arrange
            var alice = new User
            {
                Email = "alice@company.com",
                Id = 1,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };
            var bob = new User
            {
                Email = "bob@company.com",
                Id = 2,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };
            List<User> expected = new() { bob };

            // Act & Assert
            var usersService = new UsersService(expected.AsQueryable().BuildMockDbSet());
            Assert.Throws<InvalidOperationException>(() => usersService.GetUserByUuid(alice.Uuid.ToString()));
        }

        [Test]
        public void GetUserByEmail_Normal()
        {
            // Arrange
            var alice = new User
            {
                Email = "alice@company.com",
                Id = 1,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };
            var bob = new User
            {
                Email = "bob@company.com",
                Id = 2,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };
            var charlie = new User
            {
                Email = "charlie@company.com",
                Id = 3,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };

            List<User> expected = new() { alice, bob, charlie };

            // Act
            var usersService = new UsersService(expected.AsQueryable().BuildMockDbSet());
            var actual = usersService.GetUserByEmail(alice.Email);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(alice.Id, actual.Id);
        }

        [Test]
        public void GetUserByEmail_NotFound()
        {
            // Arrange
            var alice = new User
            {
                Email = "alice@company.com",
                Id = 1,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };
            var bob = new User
            {
                Email = "bob@company.com",
                Id = 2,
                PasswordHash = RandomByteArray(8),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };
            List<User> expected = new() { bob };

            // Act
            var usersService = new UsersService(expected.AsQueryable().BuildMockDbSet());
            var actual = usersService.GetUserByEmail(alice.Email);

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void CreatePasswordHash()
        {
            // Arrange
            List<User> expected = new() { new User() };
            var password = "MyFancyPassword123*_%";

            // Act
            var usersService = new UsersService(expected.AsQueryable().BuildMockDbSet());
            usersService.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            // Assert
            Assert.IsNotNull(passwordHash);
            Assert.IsNotNull(passwordSalt);
        }

        [Test]
        public void CreateToken()
        {
            // Arrange
            var alice = new User
            {
                Email = "alice@company.com",
                Id = 1,
                PasswordHash = RandomByteArray(128),
                PasswordSalt = RandomByteArray(8),
                Projects = null,
                Uuid = Guid.NewGuid(),
            };
            List<User> expected = new() { alice };
            var jwtKey = "fas1-34d3-fa6v-2c3k";

            // Act
            var usersService = new UsersService(expected.AsQueryable().BuildMockDbSet());
            var token = usersService.CreateToken(alice, jwtKey, DateTime.Now.AddDays(1).ToUniversalTime());

            // Assert
            Assert.IsNotNull(token);
        }

        [Test]
        public void VerifyPasswordHash()
        {
            // Arrange
            List<User> expected = new() { new User() };
            var password = "MyFancyPassword123*_%";
            var passwordHash = RandomByteArray(8);
            var passwordSalt = RandomByteArray(8);

            // Act
            var usersService = new UsersService(expected.AsQueryable().BuildMockDbSet());
            var actual = usersService.VerifyPasswordHash(password, passwordHash, passwordSalt);

            // Assert
            Assert.IsFalse(actual);
        }


        private byte[] RandomByteArray(int size)
        {
            var bytes = new byte[size];
            new Random().NextBytes(bytes);
            return bytes;
        }
    }
}