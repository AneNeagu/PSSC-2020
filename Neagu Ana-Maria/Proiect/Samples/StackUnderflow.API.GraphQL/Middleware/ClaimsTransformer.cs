using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Middleware
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // todo - replace hard coded orgid with lookup from workspace
            principal.AddIdentity(new ClaimsIdentity(new[] { new Claim("organisationId", "77701C43-74E1-4C4C-8A32-B1460DE27441") }));
            return Task.FromResult(principal);
        }
    }
}
