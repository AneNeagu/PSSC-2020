using System;

namespace Access.Primitives.Extensions.Cloning
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IncludeInShallowCopyAttribute : Attribute
    {
    }
}