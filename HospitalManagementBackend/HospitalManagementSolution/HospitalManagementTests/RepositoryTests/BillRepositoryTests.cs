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
    internal class BillRepositoryTests
    {
        IRepository<int, Bill> repository;
        HospitalManagementContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HospitalManagementContext(optionsBuilder.Options);
            repository = new BillRepository(context);
            Bill bill = new Bill()
            {
                BillGeneratedFor = 1,
                PatientId = 1,
                PatientType = "OutPatient",
                Description = "Checkup",
                Amount = 1500
            };
            await repository.Add(bill);
        }


        [Test]
        public async Task AddBillSuccessTest()
        {
            //Arrange 
            Bill bill = new Bill()
            {
                BillGeneratedFor = 2,
                PatientId = 1,
                PatientType = "OutPatient",
                Description = "Checkup",
                Amount = 1500
            };
            //Action
            var result = await repository.Add(bill);
            //Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task DeleteBillSuccessTest()
        {
            //Action
            var result = repository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteAdmissionFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Bill Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteBillExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Bill Not available!", exception.Message);
        }

        [Test]
        public async Task GetBillSuccessTest()
        {
            //Arrange
            Bill bill = new Bill()
            {
                BillGeneratedFor = 1,
                PatientId = 1,
                PatientType = "OutPatient",
                Description = "Checkup",
                Amount = 1500
            };
            await repository.Add(bill);
            //Action
            var result = repository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetBillFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Bill Not available!", exception.Message);
        }

        [Test]
        public async Task GetBillExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Bill Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllBillSuccessTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllBillFailTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateBillSuccessTest()
        {
            Bill app = await repository.Get(1);
            app.Amount = 2000;
            var result = await repository.Update(app);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateBillExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Bill Not available!", exception.Message);
        }
    }
}
