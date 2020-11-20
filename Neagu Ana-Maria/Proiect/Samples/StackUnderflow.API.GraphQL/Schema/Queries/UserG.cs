using GraphQL.Types;
using StackUnderflow.EF.Models;

namespace StackUnderflow.API.GraphQL.Schema.Queries
{
    public class UserG : AutoRegisteringObjectGraphType<User>
    {
    }
}