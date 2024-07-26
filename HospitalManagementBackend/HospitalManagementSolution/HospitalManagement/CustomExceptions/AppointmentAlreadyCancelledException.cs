namespace HospitalManagement.CustomExceptions
{
    public class AppointmentAlreadyCancelledException : Exception
    {
        public string msg = "";
        public AppointmentAlreadyCancelledException()
        {
            msg = "Appointment is already cancelled!";
        }
        public override string Message => msg;
    }
}
