using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StackUnderflow.Bots.Scenarios
{
    public class ScenarioEventHandler : Tuple<Type, Func<object, Task>, Func<object, bool>>
    {
        public ScenarioEventHandler(Type type, Func<object, Task> action, Func<object, bool> shouldExecute) : base(type, action, shouldExecute)
        {
        }
    }
}
