namespace HospitalManagement.CustomExceptions
{
    public class InvalidInputException : Exception
    {
        public string msg = "";
        public InvalidInputException()
        {
            msg = "Invalid input!";
        }
        public override string Message => msg;
    }
}
