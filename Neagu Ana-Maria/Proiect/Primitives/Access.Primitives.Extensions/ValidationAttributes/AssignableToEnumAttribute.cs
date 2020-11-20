using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EarlyPay.Primitives.ValidationAttributes
{
    public class AssignableToEnumAttribute : ValidationAttribute
    {
        public Type Type { get; }
        public bool AllowNull { get; }

        public AssignableToEnumAttribute(Type type, bool allowNull = false) : base($"Field [{{0}}] can not be assigned to {type.Name}")
        {
            if (!type.IsEnum)
                throw new ArgumentException("Invalid argument. Type must be an enum");

            Type = type;
            AllowNull = allowNull;
        }

        public override bool IsValid(object value)
        {
            if (AllowNull && value == null)
                return true;

            if (value is byte bValue)
                value = Convert.ToInt32(bValue);

            return value != null && Enum.IsDefined(Type, value);
        }
    }
}
