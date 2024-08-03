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
    class AppointmentRepositoryTests
    {
        IRepository<int, Appointment> repository;
        HospitalManagementContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HospitalManagementContext(optionsBuilder.Options);
            repository = new AppointmentRepository(context);
            Appointment app = new Appointment()
            {
                AppointmentId = 1,
                AppointmentDate = DateTime.Now,
                Slot = new TimeOnly(10, 0, 0),
                DoctorId = 1,
                Speciality = "Dermatology",
                PatientId = 1,
                Description = "demo",
                AppointmentStatus = "confirmed",
                AppointmentType = "general Checkup",
                AppointmentMode = "Offline"
            };
            await repository.Add(app);
        }


        [Test]
        public async Task AddAppointmentSuccessTest()
        {
            //Arrange 
            Appointment app = new Appointment()
            {
                AppointmentId = 2,
                AppointmentDate = DateTime.Now,
                Slot = new TimeOnly(10, 0, 0),
                DoctorId = 1,
                Speciality = "Dermatology",
                PatientId = 1,
                Description = "demo",
                AppointmentStatus = "confirmed",
                AppointmentType = "general Checkup",
                AppointmentMode = "Offline"
            };
            //Action
            var result = await repository.Add(app);
            //Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task DeleteAppointmentSuccessTest()
        {
            //Action
            var result = repository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteAppointmentFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Appointment Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteAppointmentExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Appointment Not available!", exception.Message);
        }

        [Test]
        public async Task GetAppointmentSuccessTest()
        {
            //Arrange
            Appointment app = new Appointment()
            {
                AppointmentId = 2,
                AppointmentDate = DateTime.Now,
                Slot = new TimeOnly(10, 0, 0),
                DoctorId = 1,
                Speciality = "Dermatology",
                PatientId = 1,
                Description = "demo",
                AppointmentStatus = "confirmed",
                AppointmentType = "general Checkup",
                AppointmentMode = "Offline"
            };
            await repository.Add(app);
            //Action
            var result = repository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAppointmentFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Appointment Not available!", exception.Message);
        }

        [Test]
        public async Task GetAppointmentExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Appointment Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllAppointmentSuccessTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllAppointmentFailTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateAppointmentSuccessTest()
        {
            Appointment app = await repository.Get(1);
            app.AppointmentStatus = "completed";
            var result = await repository.Update(app);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateAppointmentExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Appointment Not available!", exception.Message);
        }
    }
}
