using System;
using System.Text;
using Access.Primitives.Orleans;
using Orleans;
using StackUnderflow.Domain.Core.Contexts;

namespace StackUnderflow.Backend.Abstractions.Backoffice
{
    public interface IBackofficeProjection : IGrainWithGuidCompoundKey, IQueryableState<BackofficeReadContext>
    {
    }
}
