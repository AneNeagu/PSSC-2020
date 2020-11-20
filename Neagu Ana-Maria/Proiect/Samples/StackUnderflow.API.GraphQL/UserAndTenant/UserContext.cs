using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.UserAndTenant
{
    public class UserContext
    {
        public Guid UserId { get; set; }
        public int TenantId { get; set; }
    }
}
