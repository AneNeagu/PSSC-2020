using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EarlyPay.Primitives.ValidationAttributes
{
    public class NonDefaultRequiredAttribute : ValidationAttribute
    {
        public NonDefaultRequiredAttribute() : base("The field [{0}] should not have the default value") { }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            return !value.Equals(GetDefault(value.GetType()));
        }

        private object GetDefault(Type t)
        {
            return this.GetType().GetMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(this, null);
        }

        public T GetDefaultGeneric<T>()
        {
            return default(T);
        }
    }
}
