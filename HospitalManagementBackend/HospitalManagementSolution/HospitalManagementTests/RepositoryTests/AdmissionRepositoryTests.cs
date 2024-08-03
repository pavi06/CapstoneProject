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
    internal class AdmissionRepositoryTests
    {
        IRepository<int, Admission> repository;
        HospitalManagementContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HospitalManagementContext(optionsBuilder.Options);
            repository = new AdmissionRepository(context);
            Admission ad = new Admission()
            {
                PatientId = 1,
                DoctorId = 1,
                Description = "Bad Health"
            };
            await repository.Add(ad);
        }


        [Test]
        public async Task AddAdmissionSuccessTest()
        {
            //Arrange 
            Admission ad = new Admission()
            {
                PatientId = 2,
                DoctorId = 1,
                Description = "Bad Health"
            };
            //Action
            var result = await repository.Add(ad);
            //Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task DeleteAdmissionSuccessTest()
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
            Assert.AreEqual("Admission Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteAdmissionExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Admission Not available!", exception.Message);
        }

        [Test]
        public async Task GetAdmissionSuccessTest()
        {
            //Arrange
            Admission ad = new Admission()
            {
                PatientId = 3,
                DoctorId = 1,
                Description = "Bad Health"
            };
            await repository.Add(ad);
            //Action
            var result = repository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAdmissionFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Admission Not available!", exception.Message);
        }

        [Test]
        public async Task GetAdmissionExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Admission Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllAdmissionSuccessTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllAdmissionFailTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateAdmissionSuccessTest()
        {
            Admission app = await repository.Get(1);
            app.DoctorId = 2;
            var result = await repository.Update(app);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateAdmissionExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Admission Not available!", exception.Message);
        }
    }
}
