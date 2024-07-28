using Hangfire;

namespace HospitalManagement.Jobs
{
    public class RecurringJobs
    {
        public static void StartUpdating()
        {
            RecurringJob.AddOrUpdate(
                "myrecurringjob",
                () => Console.WriteLine("Recurring!"),
                Cron.Minutely);
        }
    }
}
