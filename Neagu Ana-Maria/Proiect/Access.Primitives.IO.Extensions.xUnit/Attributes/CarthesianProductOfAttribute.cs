using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Access.Primitives.IO.Extensions.xUnit
{
    public class CarthesianProductOfAttribute : DataAttribute
    {
        public object[] Input { get; }

        public CarthesianProductOfAttribute(params object[] input)
        {
            Input = input;
        }
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var statics = Input.Where(p => !(p is Type)).ToArray();
            var types =
                Input
                    .OfType<Type>()
                    .Where(t => t.IsEnum)
                    .Select(p => Enum.GetValues(p).Cast<object>().ToList())
                    .ToList();

            var cartesianProduct = CartesianProduct(types);

            foreach (var product in cartesianProduct)
            {
                yield return product.Concat(statics).ToArray();
            }
        }

        static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item })
            );
        }
    }
}
