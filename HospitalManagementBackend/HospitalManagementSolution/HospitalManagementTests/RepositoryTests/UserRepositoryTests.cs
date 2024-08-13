using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HospitalManagementTests.RepositoryTests
{
    public class UserRepositoryTests
    {
        IRepository<int, User> repository;
        HospitalManagementContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HospitalManagementContext(optionsBuilder.Options);
            repository = new UserRepository(context);
            //User user = new User()
            //{
            //    Name = "Pavi",
            //    DateOfBirth = DateTime.Now,
            //    Age = 22,
            //    Gender = "Female",
            //    EmailId = "pavi@gmail.com",
            //    ContactNo = "+980909890989",
            //    Address = "No 5, NKS nagarm demo"
            //};
            //await repository.Add(user);
        }


        [Test]
        public async Task AddUserSuccessTest()
        {
            //Arrange 
            User user = new User()
            {
                Name = "PaviSSS",
                DateOfBirth = DateTime.Now,
                Age = 22,
                Gender = "Female",
                EmailId = "pavi@gmail.com",
                ContactNo = "+989890980989",
                Address = "No 5, NKS nagarm demo"
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
            Assert.AreEqual("UserDetail Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteUserExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("UserDetail Not available!", exception.Message);
        }

        [Test]
        public async Task GetUserSuccessTest()
        {
            //Arrange
            User user = new User()
            {
                Name = "Pavieeee",
                DateOfBirth = DateTime.Now,
                Age = 22,
                Gender = "Female",
                EmailId = "pavi@gmail.com",
                ContactNo = "+9809090993",
                Address = "No 5, NKS nagarm demo"
            };
            await repository.Add(user);
            //Action
            var result = repository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("UserDetail Not available!", exception.Message);
        }

        [Test]
        public async Task GetUserExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("UserDetail Not available!", exception.Message);

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
            User user = await repository.Get(1);
            user.Age = 25;
            var result = await repository.Update(user);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateUserExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("UserDetail Not available!", exception.Message);
        }
    }
}
