using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace PGSauce.Core.FSM.Base
{
    /// <summary>
    /// The MonoBehaviour Implementing the FSM
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoStateMachineBase<T> : AbstractMonoStateMachine where T : MonoStateMachineBase<T>
    {
        [SerializeField] private bool showDebug;
        
        protected abstract StateBase<T> InitialState { get; }
        protected abstract List<AnyTransitionBase<T>> AnyTransitions { get; }
        public bool ShowDebug => showDebug;
        [ShowInInspector, LabelText("Current State")]
        public string CurrentStateName => Fsm != null ? Fsm.CurrentState.Name() : "";
        public StateMachineBase<T> Fsm { get; private set; }

        protected void Awake()
        {
            InitFsm();
            Fsm = new StateMachineBase<T>(this as T, InitialState, AnyTransitions);
            CustomInit();
        }
        
        protected void Update()
        {
            BeforeFsmUpdate();
            Fsm.LogicUpdate();
            Fsm.CheckTransitions();
            AfterFsmUpdate();
        }
        
        protected void FixedUpdate()
        {
            Fsm.PhysicsUpdate();
        }
        
        public override AbstractState GetCurrentState()
        {
            return Fsm?.CurrentState;
        }

        public override List<IAnyTransition> GetAny()
        {
            return AnyTransitions == null ? new List<IAnyTransition>() : AnyTransitions.Cast<IAnyTransition>().ToList();
        }

        public override AbstractState GetInitialState()
        {
            return InitialState;
        }

        protected virtual void CustomInit()
        {
        }

        protected virtual void InitFsm()
        {
        }
        
        protected virtual void BeforeFsmUpdate()
        {
        }

        protected virtual void AfterFsmUpdate()
        {
        }

        protected void OnDrawGizmos()
        {
            if(!showDebug) {return;}
#if UNITY_EDITOR
            Handles.Label(transform.position + Vector3.up * 3, CurrentStateName);  
#endif
            
        }
    }
}