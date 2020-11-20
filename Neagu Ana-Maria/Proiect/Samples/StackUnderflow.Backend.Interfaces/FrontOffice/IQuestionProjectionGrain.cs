using Access.Primitives.Orleans;
using Orleans;
using StackUnderflow.Domain.Core.Contexts.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Backend.Abstractions.FrontOffice
{
    public interface IQuestionProjectionGrain : IGrainWithStringKey, IQueryableState<QuestionReadContext>
    {
    }
}
