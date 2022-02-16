using System.Collections.Generic;
using PGSauce.Core.FSM.WithSo;

namespace PGSauce.Core.FSM.Base
{
    public interface IStateMachine
    {
        AbstractState GetCurrentState();
        List<IAnyTransition> GetAny();
        AbstractState GetInitialState();
    }
}