using System;
using System.Collections.Generic;
using System.Text;
using Access.Primitives.EFCore.DSL;
using Access.Primitives.IO;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.EF.Models;
using UserDbContextFactory = Access.Primitives.EFCore.DSL.DbContextDSL<StackUnderflow.EF.UserDbContext>; 

namespace StackUnderflow.EF
{
    public class UserDbContext : StackUnderflowContext
    {
        public UserDbContext() { }

        public UserDbContext(DbContextOptions<StackUnderflowContext> options)
            : base(options)
        {
        }

        protected override string DefaultSchema => "user";

        /// <summary>
        /// This represents just a simple function that creats a DbContext
        /// The whole idea is to move as much as possible towards a DSL approach.
        /// We combine all types of operations that we want to allow over DbContext.
        /// For example: For user context we require 'UserId' as a session context variable.
        /// We can extend this factory to adnotate the DbContext with arbitrary config items (specific to UserDbContext)
        /// </summary>
        /// <param name="sp">Service provider</param>
        /// <param name="userId">UserId required for RLS</param>
        /// <returns></returns>
        public static Port<UserDbContext> DbContextFactory(IServiceProvider sp, string userId)
            => from dbContext in UserDbContextFactory.CreateDbContext(sp)
               from rls in UserDbContextFactory.CreateRLS()
               from rls1 in UserDbContextFactory.SetSessionKey(rls, "UserId", userId)
               from dbContext1 in UserDbContextFactory.WithRls(dbContext, rls1)
               select dbContext1;
    }
}
