using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public  class ValidationHelper
    {
        internal static void ValidateObject<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException($"{typeof(T).Name} object is null");
            }
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid)
            {
                string errorMessages = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                throw new ArgumentException($"{typeof(T).Name} object is not valid: {errorMessages}");
            }
        }
    }
}
