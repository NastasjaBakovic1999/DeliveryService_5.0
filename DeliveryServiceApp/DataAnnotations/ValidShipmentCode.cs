using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DeliveryServiceApp.DataAnnotations
{
    public class ValidShipmentCode : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value == null) return false;

            if (value != null)
            {
                if (!Regex.IsMatch(value.ToString(), "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[A-Za-z0-9]{11}$"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
