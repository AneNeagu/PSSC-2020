using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;

namespace StackUnderflow.API.GraphQL.Schema.Commands
{
    public class CreateTenantCmdG : AutoRegisteringInputObjectGraphType<CreateTenantCmd>
    {
    }
}
