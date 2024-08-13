using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementTests.RepositoryTests
{
    public class UserLoginDetailsRepositoryTests
    {
        IRepository<int, UserLoginDetails> repository;
        HospitalManagementContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HospitalManagementContext(optionsBuilder.Options);
            repository = new UserLoginDetailsRepository(context);
            //UserLoginDetails user = new UserLoginDetails()
            //{
            //    UserId = 1,
            //    Password = new byte[] { },
            //    PasswordHashKey = new byte[] { },
            //    Status = "Active"
            //};
            //await repository.Add(user);

        }


        [Test]
        public async Task AddUserSuccessTest()
        {
            //Arrange 
            UserLoginDetails user = new UserLoginDetails()
            {
                UserId = 2,
                Password = new byte[] { },
                PasswordHashKey = new byte[] { },
                Status = "Active"
            };
            //Action
            var result = await repository.Add(user);
            //Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task DeleteUserSuccessTest()
        {
            //Action
            var result = repository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteUserFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteUserExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }

        [Test]
        public async Task GetUserSuccessTest()
        {            
            //Action
            var result = repository.Get(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }

        [Test]
        public async Task GetUserExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllUserSuccessTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllUserFailTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateUserSuccessTest()
        {
            UserLoginDetails user = await repository.Get(1);
            user.Status = "Disabled";
            var result = await repository.Update(user);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateUserExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }
    }
}
