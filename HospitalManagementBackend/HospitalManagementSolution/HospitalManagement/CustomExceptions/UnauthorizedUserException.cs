namespace HospitalManagement.CustomExceptions
{
    public class UnauthorizedUserException : Exception
    {
        public string msg = "";
        public UnauthorizedUserException()
        {
            msg = "Invalid user!";
        }
        public override string Message => msg;
    }
}
