using GraphQL.Types;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Queries
{
    public class PostG : AutoRegisteringObjectGraphType<Post>
    {
        public PostG()
        {
            Field<ListGraphType<PostG>>("answers", resolve: ResolveAnswers);
            Field<ListGraphType<PostG>>("comments", resolve: ResolveComments);
        }

        private object ResolveComments(IResolveFieldContext<Post> arg)
        {
            int id = arg.Source.PostId;
            return arg.Source.InversePostNavigation.Where(a => a.ParentPostId == id && a.PostTypeId == 2);
        }

        private object ResolveAnswers(IResolveFieldContext<Post> arg)
        {
            int id = arg.Source.PostId;
            return arg.Source.InversePostNavigation.Where(a => a.ParentPostId == id && a.PostTypeId == 1);
        }
    }
}
