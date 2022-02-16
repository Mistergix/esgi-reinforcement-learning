using UnityEngine;
using PGSauce.Core.FSM.Base;
using Sirenix.OdinInspector;

namespace PGSauce.Core.FSM.WithSo
{
    public abstract class SoState<T> : SoStateBase where T : MonoStateMachineBase<T>
    {
        [SerializeField] [HideIf("IsNullState")] private SoAction<T> enterAction;
        [SerializeField] [HideIf("IsNullState")] private SoAction<T> updateAction;
        [SerializeField] [HideIf("IsNullState")] private SoAction<T> fixedUpdateAction;
        [SerializeField] [HideIf("IsNullState")] private SoAction<T> exitAction;

        [SerializeField] [HideIf("IsNullState")] private Transition<T>[] transitions;

        public Transition<T>[] Transitions => transitions;

        public void OnEnter(T controller) {
            if(enterAction == null) { return; }
            enterAction.Act(controller);
        }
        public void LogicUpdate(T controller) {
            if (updateAction == null) { return; }
            updateAction.Act(controller);
        }
        public void PhysicsUpdate(T controller) {
            if (fixedUpdateAction == null) { return; }
            fixedUpdateAction.Act(controller);
        }
        public void OnExit(T controller) {
            if (exitAction == null) { return; }
            exitAction.Act(controller);
        }
    }
}
