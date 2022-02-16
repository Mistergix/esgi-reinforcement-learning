using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.WithSo
{
    [System.Serializable]
    public class Transition<T> : TransitionBase where T : MonoStateMachineBase<T>
    {
        public SoDecision<T> decision;
        public SoState<T> to;
        public bool reverseValue;
        public bool allowLoop;

        public SoDecisionBase GetDecision()
        {
            return decision;
        }

        public SoStateBase GetTargetState()
        {
            return to;
        }

        public bool ReverseValue()
        {
            return reverseValue;
        }
    }
}
