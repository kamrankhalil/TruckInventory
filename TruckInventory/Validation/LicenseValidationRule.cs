using System;
using System.Text.RegularExpressions;

namespace TruckInventory.Validation
{
    public class LicenseValidationRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            //Regex to match exactly 3 letters and 3 numbers as mentioned in requirements/
            string pattern = @"^[a-zA-Z]{3}\d{3}$";

            if (!(value is string))
            {
                return false;
            }

            var str = value as string;

            Match match = Regex.Match(str, pattern, RegexOptions.IgnoreCase);

            return match.Success;
        }
    }
}
