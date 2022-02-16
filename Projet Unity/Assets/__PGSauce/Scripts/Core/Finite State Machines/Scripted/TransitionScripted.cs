using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.Scripted
{
    public class TransitionScripted<T> : ITransitionBase<T> where T : MonoStateMachineBase<T>
    {
        public delegate bool Decision();

        private readonly Decision _decision;

        public TransitionScripted(StateScripted<T> to, Decision decision, bool allowLoop)
        {
            To = to;
            _decision = decision;
            AllowLoop = allowLoop;
        }

        public bool Decide(T controller)
        {
            return _decision();
        }

        public StateBase<T> To { get; set; }
        public bool AllowLoop { get; set; }
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
