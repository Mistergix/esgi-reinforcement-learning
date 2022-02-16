namespace PGSauce.Core.FSM.WithSo
{
    public interface TransitionBase
    {
        public SoDecisionBase GetDecision();
        public SoStateBase GetTargetState();
        public bool ReverseValue();
    }
}
