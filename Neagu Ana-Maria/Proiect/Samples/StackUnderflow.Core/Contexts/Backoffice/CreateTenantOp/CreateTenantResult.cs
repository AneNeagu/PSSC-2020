using System;
using System.Collections.Generic;
using System.Text;
using Access.Primitives.Extensions.Cloning;
using Access.Primitives.IO;
using CSharp.Choices;
using LanguageExt;
using StackUnderflow.EF.Models;

namespace StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp
{
    [AsChoice]
    public static partial class CreateTenantResult
    {
        public interface ICreateTenantResult : IDynClonable { }

        public class TenantCreated : ICreateTenantResult
        {
            public Tenant Tenant { get; }
            public User AdminUser { get; }

            public TenantCreated(Tenant tenant, User adminUser)
            {
                Tenant = tenant;
                AdminUser = adminUser;
            }

            public object Clone() => this.ShallowClone();

        }

        public class TenantNotCreated : ICreateTenantResult
        {
            public string Reason { get; private set; }

            ///TODO
            public object Clone() => this.ShallowClone();
        }

        public class InvalidRequest : ICreateTenantResult
        {
            public string Message { get; }

            public InvalidRequest(string message)
            {
                Message = message;
            }

            ///TODO
            public object Clone() => this.ShallowClone();
        }
    }
}
