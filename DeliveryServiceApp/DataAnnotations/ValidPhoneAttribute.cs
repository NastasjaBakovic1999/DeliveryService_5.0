using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DeliveryServiceApp.DataAnnotations
{
    public class ValidPhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value == null) return false;

            if(value != null)
            {
                if (!Regex.IsMatch(value.ToString(), "^06[0-9]{7,9}$"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
