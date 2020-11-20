using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remote.Linq;

public static class RemoteRepositoryExtensions
{
    public static Task<List<T>> EvalAsync<T>(this IQueryable<T> source) => AsyncEnumerableExtensions.ToListAsync(source);
}
