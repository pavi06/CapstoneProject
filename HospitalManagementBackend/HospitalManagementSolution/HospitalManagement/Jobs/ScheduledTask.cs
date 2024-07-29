using Hangfire;

namespace HospitalManagement.Jobs
{
    public class ScheduledTask
    {

        public static async Task<string> GenerateMeetingLink()
        {
            return "@123demo/-meeting/-link@123";
        }

        public static async Task<string> SendEmail(string name, DateTime date, TimeOnly slot, int noOfDays)
        {
            var meetingLink = GenerateMeetingLink();
            var subject = "Your Meeting Link for online consultation";
            var message = $"Hy {name}. Your meet link for online consultation is right here! Join the meet at the right time without any delay!" +
                $"Meeting link: {meetingLink}";
            //await SendMeetLink(subject, message, noOfDays);
            return message;
        }
    }
}
