using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Access.Primitives.Extensions.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value, Func<TValue, TValue, TValue> updateCallback)
        {
            if (source.ContainsKey(key))
            {
                var chosenValue = updateCallback(source[key], value);
                source.Remove(key);
                source.Add(key, chosenValue);
            }
            else
            {
                source.Add(key, value);
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> factory)
        {
            if (source.ContainsKey(key))
                return source[key];
            var value = factory();
            source.Add(key, value);
            return source[key];
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> other)
        {
            if (source == null && other == null) return source;
            if (source == null) source = new Dictionary<TKey, TValue>();

            foreach (var kvp in other)
            {
                source.AddOrUpdate(kvp.Key, kvp.Value, (key, oldValue) => kvp.Value);
            }
            return source;
        }

        public static IDictionary<TKey, T> ConvertToDictionary<TKey, T>(this Tuple<TKey, T> tuple)
        {
            return new[] { tuple }.ToDictionary(p => p.Item1, p => p.Item2);
        }
    }
}
