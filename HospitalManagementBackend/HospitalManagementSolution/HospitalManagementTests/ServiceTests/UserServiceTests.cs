using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models.DTOs.UserDTOs;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Contexts;
using System.Net;
using System.Xml.Linq;

namespace HospitalManagementTests.ServiceTests
{
    internal class UserServiceTests
    {
        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        HospitalManagementContext context;
        IUserService userService;
        IRepository<int, User> userRepo;
        IRepository<int, UserLoginDetails> userLoginRepo;
        ITokenService tokenService;
        ILogger<UserService> logger;
        ILogger<TokenService> logger2;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                         .UseInMemoryDatabase("dummyDB");
            context = new HospitalManagementContext(optionsBuilder.Options);
            userRepo = new UserRepository(context);
            userLoginRepo = new UserLoginDetailsRepository(context);
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which is too long.");
            Mock<IConfigurationSection> configTokenSection = new Mock<IConfigurationSection>();
            configTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(configTokenSection.Object);
            logger = Mock.Of<ILogger<UserService>>();
            logger2 = Mock.Of<ILogger<TokenService>>();
            tokenService = new TokenService(mockConfig.Object);
            userService = new UserService(userLoginRepo, userRepo, tokenService);
            await userService.Register(new UserRegistrationDTO() { Name = "Pavithra", EmailId = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", ContactNo = "9765345656" });

        }

        [Test]
        public async Task UserRegisterPassTest()
        {
            var res = userService.Register(new UserRegistrationDTO() { Name = "Pavithra", EmailId = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", ContactNo = "9765345656" });
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task RegisterExceptionTest()
        {
            var res = Assert.ThrowsAsync<ObjectAlreadyExistsException>(() => userService.Register(new UserRegistrationDTO() { Name = "Pavithra", EmailId = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", ContactNo = "9765345656" }));
            Assert.AreEqual("User Already Exists!", res.Message);
        }


        [Test]
        public async Task UserLoginPassTest()
        {
            var res = userService.Login(new UserLoginDTO() { Email = "pavi@gmail.com", Password = "Pavi123" });
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task UserLoginFailTest()
        {
            IUserService userService = new UserService(userLoginRepo, userRepo, tokenService);
            await userService.Register(new UserRegistrationDTO() { Name = "Pavithra", EmailId = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", ContactNo = "9876565647" });
            var res = Assert.ThrowsAsync<UnauthorizedUserException>(() => userService.Login(new UserLoginDTO() { Email = "pavi@gmail.com", Password = "pavi123" }));
            Assert.That("Invalid username or password", Is.EqualTo(res.Message));
        }

        [Test]
        public async Task UserLoginExceptionTest()
        {
            var res = Assert.ThrowsAsync<ObjectNotAvailableException>(() => userService.Login(new UserLoginDTO() { Email = "sai@gmail.com", Password = "sai123" }));
            Assert.AreEqual("User Not available!", res.Message);
        }

    }
}
