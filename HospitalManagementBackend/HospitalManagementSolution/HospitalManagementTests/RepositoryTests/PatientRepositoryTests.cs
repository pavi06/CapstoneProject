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
    class PatientRepositoryTests
    {
        IRepository<int, Patient> repository;
        HospitalManagementContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HospitalManagementContext(optionsBuilder.Options);
            repository = new PatientRepository(context);
            Patient patient = new Patient()
            {
                PatientId = 1
            };
            await repository.Add(patient);
        }


        [Test]
        public async Task AddPatientSuccessTest()
        {
            //Arrange 
            Patient patient = new Patient()
            {
                PatientId = 2
            };
            //Action
            var result = await repository.Add(patient);
            //Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task DeletePatientSuccessTest()
        {
            //Action
            var result = repository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeletePatientFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(1));
            //Assert
            Assert.AreEqual("Patient Not available!", exception.Message);
        }

        [Test]
        public async Task DeletePatientExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Patient Not available!", exception.Message);
        }

        [Test]
        public async Task GetPatientSuccessTest()
        {
            //Arrange
            Patient patient = new Patient()
            {
                PatientId = 2
            };
            await repository.Add(patient);
            //Action
            var result = repository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetPatientFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Patient Not available!", exception.Message);
        }

        [Test]
        public async Task GetPatientExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Patient Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllPatientSuccessTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllPatientFailTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }
    }
}
