using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.WithSo
{
    public abstract class SoDecision<T> : SoDecisionBase where T : MonoStateMachineBase<T>
    {
        public abstract bool Decide(T controller);
    }
}
