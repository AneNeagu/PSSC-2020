using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EarlyPay.Primitives.ValidationAttributes
{
    public class OptionValidatorAttribute :  ValidationAttribute
    {
        private readonly Type _validatorAttribute;
        private ValidationAttribute _instance;

        public OptionValidatorAttribute(Type validatorAttribute, params object[] args)
        {
            _instance = Activator.CreateInstance(validatorAttribute, args) as ValidationAttribute;
            _validatorAttribute = validatorAttribute;
        }
        public override bool IsValid(object value)
        {
            var optional = (IOptional)value;
            var isValid = optional.MatchUntyped(o => _instance.IsValid(o), () => false);
            return isValid;
        }
    }
}
