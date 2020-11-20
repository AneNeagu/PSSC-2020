using GraphQL.Types;
using StackUnderflow.Domain.Schema.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Commands
{
    public class GetQuestionCmdG : AutoRegisteringInputObjectGraphType<GetQuestionCmd>
    {
    }
}
