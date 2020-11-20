using GraphQL.Types;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Queries
{
    public class VoteG : AutoRegisteringObjectGraphType<Vote>
    {
    }
}
