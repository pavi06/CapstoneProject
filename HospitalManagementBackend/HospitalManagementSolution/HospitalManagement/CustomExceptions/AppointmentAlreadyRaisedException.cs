namespace HospitalManagement.CustomExceptions
{
    public class AppointmentAlreadyRaisedException:Exception
    {
        public string msg = "";
        public AppointmentAlreadyRaisedException()
        {
            msg = "Your appointment already scheduled!";
        }
        public override string Message => msg;
    }
}
