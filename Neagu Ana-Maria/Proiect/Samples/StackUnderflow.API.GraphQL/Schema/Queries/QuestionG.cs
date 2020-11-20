using GraphQL.Types;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Queries
{
    public class QuestionG : AutoRegisteringObjectGraphType<Post> // todo: seperate question from answer
    {
        public QuestionG()
        {
            FieldAsync<ListGraphType<QuestionG>>("childPosts", resolve: ResolveChildPosts);
            FieldAsync<ListGraphType<VoteG>>("votes", resolve: ResolveVotes);
            FieldAsync<IntGraphType>("voteScore", resolve: ResolveVoteScore);
        }

        private async Task<object> ResolveVotes(IResolveFieldContext<Post> arg)
        {
            return arg.Source.Vote;
        }

        private async Task<object> ResolveVoteScore(IResolveFieldContext<Post> arg)
        {
            return arg.Source.Vote.Sum(v=>v.VoteTypeId==1 ? 1 : -1);
        }

        private async Task<object> ResolveChildPosts(IResolveFieldContext<Post> arg)
        {
            return arg.Source.InversePostNavigation;
        }
    }
}
