namespace HospitalManagement.CustomExceptions
{
    public class CannotUploadImageException : Exception
    {
        public string msg = "";
        public CannotUploadImageException()
        {
            msg = "Cannot upload prescription image!";
        }

        public override string Message => msg;
    }
}
