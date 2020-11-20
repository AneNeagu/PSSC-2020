using Access.Primitives.Orleans;
using Orleans;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Core.Contexts.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Backend.Abstractions.FrontOffice
{
    public interface IUserProjectionGrain : IGrainWithStringKey, IQueryableState<UserReadContext>
    {
    }
}
