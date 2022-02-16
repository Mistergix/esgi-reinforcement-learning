using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.WithSo
{
    public abstract class SoAction<T> : SoActionBase where T : MonoStateMachineBase<T>
    {
        public abstract void Act(T controller);
    }
}
