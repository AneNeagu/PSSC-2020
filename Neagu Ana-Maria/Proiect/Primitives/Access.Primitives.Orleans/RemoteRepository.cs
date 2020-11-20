using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Dynamic;
using Newtonsoft.Json;
using Remote.Linq;
using Remote.Linq.Expressions;

namespace Access.Primitives.Orleans
{
    public class RemoteRepository<TGrain, TState> where TGrain : IQueryableState<TState>
    {
        private static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings().ConfigureRemoteLinq();
        private readonly Func<Expression, Task<IEnumerable<DynamicObject>>> dataProvider;
        private readonly TGrain _grain;

        public RemoteRepository(TGrain grain)
        {
            _grain = grain;
            dataProvider = async exp =>
            {
                var query = JsonConvert.SerializeObject(exp, SerializerSettings);
                var result = await _grain.ExecuteQueryAsync(query);
                return result;
            };
        }

        public TGrain Grain => _grain;

        public IQueryable<TState> Root => RemoteQueryable.Factory.CreateQueryable<TState>(dataProvider);
    }
}
