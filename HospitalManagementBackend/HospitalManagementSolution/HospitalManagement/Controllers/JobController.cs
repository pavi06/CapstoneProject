using HospitalManagement.CustomExceptions;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Jobs;
using HospitalManagement.Interfaces;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;
        private readonly ILogger<RecurringJobs> _logger;
        public JobController(IRepository<int, Appointment> appointmentRepo, ILogger<RecurringJobs> logger)
        {
            _appointmentRepository = appointmentRepo;
            _logger = logger;
        }

        [HttpPost]
        [Route("updateAppointmentJob")]
        public ActionResult ActivateRecurringJobs()
        {

            RecurringJobs job = new RecurringJobs(_appointmentRepository, _logger);
            job.StartUpdating();
            return Ok();
        }
    }
}
