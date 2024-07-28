using Hangfire;

namespace HospitalManagement.Jobs
{
    public class ScheduledTask
    {

        public static async Task<string> GenerateMeetingLink()
        {
            return "";
        }

        public static async Task SendEmail(string name, DateTime date, TimeOnly slot, int noOfDays)
        {
            var meetingLink = GenerateMeetingLink();
            var subject = "Your Meeting Link for online consultation";
            var message = $"Hy {name}. Your meet link for online consultation is right here! Join the meet at the right time without any delay!" +
                $"Meeting link: {meetingLink}";
            await SendMeetLink(subject, message, noOfDays);
        }
        public static async Task SendMeetLink(string subject, string message, int days)
        {
            //BackgroundJob.Schedule(
            //    () => ,
            //    TimeSpan.FromDays(days));
        }
    }
}
