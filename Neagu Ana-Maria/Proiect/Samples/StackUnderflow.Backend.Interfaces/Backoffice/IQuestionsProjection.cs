using Access.Primitives.Orleans;
using Orleans;
using StackUnderflow.Domain.Core.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Backend.Abstractions
{
    public interface IQuestionsProjection : IGrainWithStringKey, IQueryableState<QuestionsReadContext>
    {
    }
}
