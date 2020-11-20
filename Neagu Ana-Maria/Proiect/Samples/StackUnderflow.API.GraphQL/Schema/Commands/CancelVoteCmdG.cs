using GraphQL.Types;
using StackUnderflow.Domain.Schema.Voting.CancelVoteOp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Commands
{
    public class CancelVoteCmdG : AutoRegisteringInputObjectGraphType<CancelVoteCmd>
    {
    }
}
