using GraphQL.Types;
using StackUnderflow.Domain.Schema.Models;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Queries
{
    public class QuestionSummaryG : AutoRegisteringObjectGraphType<QuestionSummary>
    {
    }
}
