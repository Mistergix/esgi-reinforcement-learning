using System.Collections;
using System.Collections.Generic;

namespace PGSauce.Core.FSM.Base
{
    public abstract class AbstractState
    {
        public abstract bool IsNullState { get; }
        public abstract string Name();

        public abstract List<ITransition> GetTransitions();

        public abstract void CleanTemporaryTransitions();

        public abstract List<ITransition> GetTransitionsIncludingTemporary();

        public abstract void AddTemporaryTransition(IAnyTransition any);
    }
}