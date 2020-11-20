using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;

namespace Access.Primitives.Extensions.ObjectExtensions
{
    public static class CastExtensions
    {
        public static Option<T> SafeCast<T>(this object source)
        {
            if (source == null)
                return Option<T>.None;
            if (source is T cast)
                return Option<T>.Some(cast);
            return Option<T>.None;
        }
    }
}
