using System.Collections.Generic;

namespace PGSauce.Core.FSM.Base
{
    public interface IAnyTransition
    {
        public List<AbstractState> ExcludedStates();
        AbstractState GetTargetState();
        IDecision GetDecision();
        bool GetReverseValue();
    }
}