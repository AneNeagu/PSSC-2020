using GraphQL.Types;
using StackUnderflow.Domain.Schema.Voting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Commands
{
    public class VoteCmdG : AutoRegisteringInputObjectGraphType<VoteCmd>
    {
    }
}
