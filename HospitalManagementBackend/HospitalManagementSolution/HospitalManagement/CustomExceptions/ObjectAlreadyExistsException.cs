namespace HospitalManagement.CustomExceptions
{
    public class ObjectAlreadyExistsException:Exception
    {
        public string msg = "";
        public ObjectAlreadyExistsException(string? message)
        {
            msg = $"{message} already available!";
        }

        public override string Message => msg;

    }
}
