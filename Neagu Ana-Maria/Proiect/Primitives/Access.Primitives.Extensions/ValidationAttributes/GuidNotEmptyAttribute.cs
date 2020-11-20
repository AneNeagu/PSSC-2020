using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EarlyPay.Primitives.ValidationAttributes
{
    public class GuidNotEmptyAttribute : ValidationAttribute
    {
        public GuidNotEmptyAttribute() : base("This field [{0}] should not be Guid.Empty") { }


        public override bool IsValid(object value)
        {
            return !((Guid)value).Equals(Guid.Empty);
        }
    }

    public class StringGuidNotEmptyAttribute : ValidationAttribute
    {
        public StringGuidNotEmptyAttribute() : base("This field should not be an empty or invalid Guid") { }

        public override bool IsValid(object value)
        {
            if (!(value is string))
                return false;
            var parsed = Guid.TryParse(value.ToString(), out Guid guid);
            return parsed && !guid.Equals(Guid.Empty);
        }
        
    }
}
