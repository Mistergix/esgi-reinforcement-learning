using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.WithSo
{
    public class TransitionWithSo<T> : ITransitionBase<T> where T : MonoStateMachineBase<T>
    {
        private readonly SoDecision<T> _decision;
        private readonly bool _reverseValue;
        
        public TransitionWithSo(StateBase<T> to, SoDecision<T> decision, bool reverseValue, bool allowLoop)
        {
            To = to;
            _decision = decision;
            _reverseValue = reverseValue;
            AllowLoop = allowLoop;
        }

        public bool Decide(T controller)
        {
            var ok = _decision.Decide(controller);
            if (ReverseValue)
            {
                ok = !ok;
            }
            return ok;
        }

        public StateBase<T> To { get; set; }
        public bool AllowLoop { get; set; }

        public bool ReverseValue => _reverseValue;

        public AbstractState GetTargetState()
        {
            return To;
        }

        public IDecision GetDecision()
        {
            return new DecisionInterface();
        }
    }
}