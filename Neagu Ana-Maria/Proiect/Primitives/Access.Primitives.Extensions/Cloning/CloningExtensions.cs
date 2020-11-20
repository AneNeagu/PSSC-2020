using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Access.Primitives.Extensions.Expressions;
using Aqua.Dynamic;

namespace Access.Primitives.Extensions.Cloning
{
    public static class CloningExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            return new DynamicObject(obj).CreateObject<T>();
        }

        public static T ShallowClone<T>(this T obj, params Expression<Func<T, object>>[] includedProperties)
        {
            includedProperties = includedProperties ?? new Expression<Func<T, object>>[0];
            var dynamicObject = new DynamicObject(obj, 
                new ShallowCopyDynamicObjectMapper(includedProperties.Select(p => obj.GetPropertyInfo(p)).ToArray()));
            return dynamicObject.CreateObject<T>();
        }
    }
}
