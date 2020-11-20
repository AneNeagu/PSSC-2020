using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infrastructure.ValidationAttributes
{
    public class NumeralStringAttribute : ValidationAttribute
    {

        public NumeralStringAttribute() : base("Field [{0}] should be convertible to int") { }
        public override bool IsValid(object value)
        {
            return int.TryParse(value.ToString(), out int result);
        }
    }
}
