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
    public class RoomRepositoryTests
    {
        HospitalManagementContext context;
        IRepository<int, Room> repository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HospitalManagementContext(optionsBuilder.Options);
            repository = new RoomRepository(context);
            Room room = new Room()
            {
                RoomId = 1,
                WardTypeId = 1,
                IsAllotted = false,
                CostsPerDay = 1500
            };
            await repository.Add(room);

        }


        [Test]
        public async Task AddRoomSuccessTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            Room room = new Room()
            {
                RoomId = 2,
                WardTypeId = 1,
                IsAllotted = false,
                CostsPerDay = 1500
            };
            var result = await repository.Add(room);
            Assert.That(result.RoomId, Is.EqualTo(2));
        }

        [Test]
        public async Task DeleteRoomSuccessTest()
        {
            Room room = new Room()
            {
                RoomId = 3,
                WardTypeId = 1,
                IsAllotted = false,
                CostsPerDay = 1500
            };
            var res = await repository.Add(room);
            //Action
            var result = await repository.Delete(res.RoomId);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRoomFailTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteRoomExceptionTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task GetRoomSuccessTest()
        {
            //Action
            var result = await repository.Get(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRoomFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(1));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task GetRoomExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllRoomSuccessTest()
        {
            Room room = new Room()
            {
                RoomId = 3,
                WardTypeId = 1,
                IsAllotted = false,
                CostsPerDay = 1500
            };
            await repository.Add(room);
            //Action
            var result = await repository.Get();
            //Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllRoomFailTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateRoomSuccessTest()
        {
            var room = repository.Get(1).Result;
            room.IsAllotted = true;
            //Action
            var result = repository.Update(room).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRoomExceptionTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }
    }
}
