using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aqua.Dynamic;

namespace Access.Primitives.Extensions.Cloning
{
    public class ShallowCopyDynamicObjectMapper : DynamicObjectMapper
    {
        private readonly PropertyInfo[] includedProperties;

        public ShallowCopyDynamicObjectMapper(params PropertyInfo[] includedProperties)
        {
            this.includedProperties = includedProperties;
        }

        protected override IEnumerable<PropertyInfo> GetPropertiesForMapping(Type type)
        {
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0);

            var valueTypes = properties.Where(p => p.PropertyType.IsValueType || p.PropertyType.Equals(typeof(string)));

            foreach (var prop in valueTypes)
            {
                yield return prop;
            }
            foreach (var prop in properties.Where(p => p.GetCustomAttribute<IncludeInShallowCopyAttribute>() != null))
            {
                yield return prop;
            }
            foreach (var prop in includedProperties)
            {
                yield return prop;
            }
        }
    }
}