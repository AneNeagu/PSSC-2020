using GraphQL.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Middleware
{
    public class GraphQLSettings
    {
        public PathString Path { get; set; } = "";
        public Func<HttpContext, object> BuildUserContext { get; set; }
        public List<IValidationRule> ValidationRules { get; }

        public GraphQLSettings()
        {
            ValidationRules = new List<IValidationRule>();
        }
    }
}
