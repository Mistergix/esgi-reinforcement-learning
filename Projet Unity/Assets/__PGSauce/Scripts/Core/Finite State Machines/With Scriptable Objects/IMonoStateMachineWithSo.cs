using System.Collections.Generic;
using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.WithSo
{
    public interface IMonoStateMachineWithSo : IStateMachine
    {
        SoStateBase InitialState();
        List<AnyTransitionBase> GetAny();
        SoStateBase GetCurrentState();
    }
}