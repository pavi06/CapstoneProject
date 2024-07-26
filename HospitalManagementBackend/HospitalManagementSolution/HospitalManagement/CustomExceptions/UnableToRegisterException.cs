namespace HospitalManagement.CustomExceptions
{
    public class UnableToRegisterException : Exception
    {
        public string msg = "";
        public UnableToRegisterException()
        {
            msg = "Unable to register at this moment!";
        }

        public override string Message => msg;
    }
}
