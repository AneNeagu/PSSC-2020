using System;
using System.Threading.Tasks;
using GraphQL.Types;
using StackUnderflow.API.GraphQL.Schema.Queries;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Core.Contexts.Questions;

namespace StackUnderflow.API.GraphQL.Schema
{
    public class StackUnderflowQuery : ObjectGraphType
    {
        public StackUnderflowQuery()
        {
            FieldAsync<BackofficeProjectionG>("backoffice", resolve: ResolveBackoffice);
            FieldAsync<QuestionsProjectionG>("questions", resolve: ResolveQuestionsList);
            FieldAsync<QuestionProjectionG>("question", resolve: ResolveQuestion);
        }

        private async Task<object> ResolveQuestion(IResolveFieldContext<object> arg)
        {
            return new QuestionReadContext(null);
        }

        private async Task<object> ResolveBackoffice(IResolveFieldContext<object> arg)
        {
            return new BackofficeReadContext(null, null);
        }

        private async Task<object> ResolveQuestionsList(IResolveFieldContext<object> arg)
        {
            return new UserReadContext(null);
        }
    }
}