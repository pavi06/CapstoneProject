using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.CustomValidations
{
    public class CustomDateValidation : RangeAttribute
    {
        public CustomDateValidation()
              : base(typeof(DateTime),
                      DateTime.Now.ToShortDateString(),
                      DateTime.Now.AddYears(1).ToShortDateString())
        {
        }
    }
}
