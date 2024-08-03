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

        public void UpdateAppointmentStatus()
        {
            var appointments = _appointmentRepository.Get().Result.Where(a=>a.AppointmentDate <= DateTime.Now.Date && a.Slot < TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay));
            appointments.Select(async a => {
                a.AppointmentStatus = "Completed";
                var res = _appointmentRepository.Update(a).Result;
                _logger.LogInformation("job trigerred");
                _logger.LogInformation("res -> ", res.AppointmentId);
            });
        }

        public async Task StartUpdating()
        {
            RecurringJob.AddOrUpdate(
                "appointmentUpdatejob",
                () => UpdateAppointmentStatus(), "*/5 * * * *");

            //RecurringJob.AddOrUpdate(
            //    "appointmentUpdatejob",
            //    () => Console.WriteLine("hello"), "* * * * *");
        }
    }
}
