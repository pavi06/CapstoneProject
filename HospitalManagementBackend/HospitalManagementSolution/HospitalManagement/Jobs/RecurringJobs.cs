using Hangfire;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;

namespace HospitalManagement.Jobs
{
    public class RecurringJobs
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;

        public RecurringJobs(IRepository<int, Appointment> appointmentRepo)
        {
            _appointmentRepository = appointmentRepo;
        }

        public async Task UpdateAppointmentStatus()
        {
            var appointments = _appointmentRepository.Get().Result.Where(a=>a.AppointmentDate < DateTime.Now.Date);
            appointments.Select(async a => {
                a.AppointmentStatus = "Completed";
                await _appointmentRepository.Update(a);
            });
        }

        public void StartUpdating()
        {
            RecurringJob.AddOrUpdate(
                "appointmentUpdatejob",
                () => UpdateAppointmentStatus(),
                Cron.MinuteInterval(30));
        }
    }
}
