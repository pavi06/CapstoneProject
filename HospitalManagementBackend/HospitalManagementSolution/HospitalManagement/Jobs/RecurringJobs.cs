using Hangfire;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;

namespace HospitalManagement.Jobs
{
    public class RecurringJobs
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;
        private readonly ILogger<RecurringJobs> _logger;

        public RecurringJobs(IRepository<int, Appointment> appointmentRepo, ILogger<RecurringJobs> logger)
        {
            _appointmentRepository = appointmentRepo;
            _logger = logger;
        }

        public async Task UpdateAppointmentStatus()
        {
            var appointments = _appointmentRepository.Get().Result.Where(a=>a.AppointmentDate < DateTime.Now.Date);
            appointments.Select(async a => {
                a.AppointmentStatus = "Completed";
                var res = await _appointmentRepository.Update(a);
                _logger.LogInformation("job trigerred");
                _logger.LogInformation("res -> ", res.AppointmentId);
            });
        }

        public async Task StartUpdating()
        {
            //RecurringJob.AddOrUpdate(
            //    "appointmentUpdatejob",
            //    () => UpdateAppointmentStatus(), "* * * * *");

            RecurringJob.AddOrUpdate(
                "appointmentUpdatejob",
                () => Console.WriteLine("hello"), "* * * * *");
        }
    }
}
