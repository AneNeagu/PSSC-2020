using GraphQL.Types;
using StackUnderflow.Domain.Schema.Backoffice.InviteUserOp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Commands
{
    public class InviteUserCmdG : AutoRegisteringInputObjectGraphType<InviteUserCmd>
    {
    }
}
