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
using static System.Reflection.Metadata.BlobBuilder;

namespace HospitalManagementTests.RepositoryTests
{
    internal class DoctorRepositoryTests
    {
        IRepository<int, Doctor> repository;
        HospitalManagementContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HospitalManagementContext(optionsBuilder.Options);
            repository = new DoctorRepository(context);
            Doctor doctor = new Doctor()
            {
                DoctorId = 1,
                DoctorType = "Inhouse",
                Specialization = "Cardiology",
                Experience = 5,
                Qualification = "MBBS",
                LanguagesKnown = new List<string>() { "Tamil", "Enlish" },
                ShiftStartTime = new TimeOnly(10, 0, 0),
                ShiftEndTime = new TimeOnly(12, 0, 0),
                Slots = new List<TimeOnly>() { new TimeOnly(10, 30, 0), new TimeOnly(11, 0, 0), new TimeOnly(11, 30, 0) },
                AvailableDays = new List<string>() { "Monday", "Tuesday"}
            };
            await repository.Add(doctor);
        }


        [Test]
        public async Task AddDoctorSuccessTest()
        {
            //Arrange 
            Doctor doctor = new Doctor()
            {
                DoctorId = 2,
                DoctorType = "Inhouse",
                Specialization = "Cardiology",
                Experience = 5,
                Qualification = "MBBS",
                LanguagesKnown = new List<string>() { "Tamil", "Enlish" },
                ShiftStartTime = new TimeOnly(10, 0, 0),
                ShiftEndTime = new TimeOnly(12, 0, 0),
                Slots = new List<TimeOnly>() { new TimeOnly(10, 30, 0), new TimeOnly(11, 0, 0), new TimeOnly(11, 30, 0) },
                AvailableDays = new List<string>() { "Monday", "Tuesday" }
            };
            //Action
            var result = await repository.Add(doctor);
            //Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task DeleteDoctorSuccessTest()
        {
            //Action
            var result = repository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteDoctorFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Doctor Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteDoctorExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Doctor Not available!", exception.Message);
        }

        [Test]
        public async Task GetDoctorSuccessTest()
        {
            //Arrange
            Doctor doctor = new Doctor()
            {
                DoctorId = 3,
                DoctorType = "Inhouse",
                Specialization = "Cardiology",
                Experience = 5,
                Qualification = "MBBS",
                LanguagesKnown = new List<string>() { "Tamil", "Enlish" },
                ShiftStartTime = new TimeOnly(10, 0, 0),
                ShiftEndTime = new TimeOnly(12, 0, 0),
                Slots = new List<TimeOnly>() { new TimeOnly(10, 30, 0), new TimeOnly(11, 0, 0), new TimeOnly(11, 30, 0) },
                AvailableDays = new List<string>() { "Monday", "Tuesday" }
            };
            await repository.Add(doctor);
            //Action
            var result = repository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetDoctorFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Doctor Not available!", exception.Message);
        }

        [Test]
        public async Task GetDoctorExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Doctor Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllDoctorSuccessTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllDoctorFailTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateDoctorSuccessTest()
        {
            Doctor doc = await repository.Get(1);
            doc.Qualification = "MBBS,";
            var result = await repository.Update(doc);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateDoctorExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Doctor Not available!", exception.Message);
        }
    }
}
