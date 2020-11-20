using System;
using System.Threading.Tasks;
using Access.Primitives.IO.Mocking;

namespace Access.Primitives.IO
{
    public interface IInterpreter<S, D> : IInterpreter
    {
        Task<A> Mock<A>(MockContext mockContext, Port<A> ma, S state, D dependencies, Func<Port<A>, Task<A>> interpret);
    }

    public interface IInterpreter
    {
        Task<A> Interpret<A>(Port<A> ma, object state, object dependencies, Func<Port<A>, Task<A>> interpret);
    }
}