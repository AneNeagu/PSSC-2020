using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Middleware
{
    public class GraphQLUserContext
    {
        public Guid UserId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
