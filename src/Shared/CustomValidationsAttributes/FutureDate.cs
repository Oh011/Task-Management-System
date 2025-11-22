using System.ComponentModel.DataAnnotations;

namespace Shared.CustomValidationsAttributes
{
    public class FutureDate : ValidationAttribute
    {

        public override bool IsValid(object? value)
        {


            if (value == null) return true;


            if (value is DateTime dateValue)
            {

                return dateValue > DateTime.UtcNow.AddSeconds(-50);
            }


            return false;


        }

        // name is the display name of the property that is being validated.


        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a future date.";
        }

    }
}
