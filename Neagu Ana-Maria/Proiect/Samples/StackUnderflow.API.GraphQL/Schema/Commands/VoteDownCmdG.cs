using GraphQL.Types;
using StackUnderflow.Domain.Schema.Voting.VoteDownOp;
using StackUnderflow.Domain.Schema.Voting.VoteUpOp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Commands
{
    public class VoteDownCmdG : AutoRegisteringInputObjectGraphType<VoteDownCmd>
    {
    }
}
