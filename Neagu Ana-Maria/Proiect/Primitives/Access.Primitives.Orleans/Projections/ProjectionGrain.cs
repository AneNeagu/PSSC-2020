using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Dynamic;
using Newtonsoft.Json;
using Orleans;
using Remote.Linq;
using Remote.Linq.Expressions;

namespace Access.Primitives.Orleans
{
    public abstract class ProjectionGrain<TState> : Grain, IQueryableState<TState>
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings().ConfigureRemoteLinq();

        protected abstract Func<Type, IQueryable> QueryableResourceProvider { get; }

        public Task<IEnumerable<DynamicObject>> ExecuteQueryAsync(string query)
        {
            MethodCallExpression exp = JsonConvert.DeserializeObject<MethodCallExpression>(query, SerializerSettings);
            var data = exp.Execute(QueryableResourceProvider);
            return Task.FromResult(data);
        }
    }
}