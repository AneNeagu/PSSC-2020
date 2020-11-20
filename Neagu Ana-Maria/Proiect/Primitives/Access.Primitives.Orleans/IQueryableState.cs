using System.Collections.Generic;
using System.Threading.Tasks;
using Aqua.Dynamic;
using Orleans.Concurrency;

namespace Access.Primitives.Orleans
{
    public interface IQueryableState<TState>
    {
        [AlwaysInterleave]
        Task<IEnumerable<DynamicObject>> ExecuteQueryAsync(string query);
    }
}