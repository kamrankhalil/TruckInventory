using System;
namespace TruckInventory.Validation
{
    public class RequiredRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is string str)
            {
                return str != string.Empty;
            }

            if(value is DateTime dateTime)
            {
                return dateTime != default(DateTime);
            }

            return true;
        }
    }
}
