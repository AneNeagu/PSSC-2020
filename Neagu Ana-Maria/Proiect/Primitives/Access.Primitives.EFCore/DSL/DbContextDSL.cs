using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Access.Primitives.Extensions.Extensions;
using Access.Primitives.IO;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Access.Primitives.EFCore.DSL
{
    public static class DbContextDSL<T> where T : DbContext
    {
        public static Port<T> CreateDbContext(IServiceProvider serviceProvider)
        {
            return new Inline<T>(() =>
            {
                return serviceProvider.GetService<T>();
            });
        }

        public static Port<RLSItems> CreateRLS() => new Inline<RLSItems>(() => new RLSItems());

        public static Port<RLSItems> SetSessionKey(RLSItems rlsItems, string key, string value) => new Inline<RLSItems>(
            () =>
            {
                rlsItems.AddOrUpdate(key, value, (oldValue, newValue) => newValue);
                return rlsItems;
            });

        public static Port<T> WithRls(T dbContext, RLSItems rlsItems) => new Inline<T>(() =>
        {
            var connection = dbContext.Database.GetDbConnection();
            if (rlsItems != null && !rlsItems.Equals(RLSItems.Empty))
                connection.StateChange += (sender, e) =>
                {
                    switch (e.CurrentState)
                    {
                        case ConnectionState.Open:
                            var dbConnection = (IDbConnection) sender;
                            dbConnection.Execute(rlsItems.ToSql(), rlsItems.ToParameters());
                            break;
                    }
                };
            return dbContext;
        });
    }
}
