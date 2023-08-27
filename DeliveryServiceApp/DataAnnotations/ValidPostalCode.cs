﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DeliveryServiceApp.DataAnnotations
{
    public class ValidPostalCode : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value == null) return false;

            if (value != null)
            {
                if (!Regex.IsMatch(value.ToString(), "^[0-9]{5,5}$"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
