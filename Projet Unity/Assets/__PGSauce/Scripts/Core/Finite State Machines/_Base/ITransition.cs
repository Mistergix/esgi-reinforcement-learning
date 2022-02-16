namespace PGSauce.Core.FSM.Base
{
    public interface ITransition
    {
        AbstractState GetTargetState();
        IDecision GetDecision();
        
    }
}