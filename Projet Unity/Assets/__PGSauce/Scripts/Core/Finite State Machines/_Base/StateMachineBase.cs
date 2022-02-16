using System.Collections.Generic;
using PGSauce.Core.PGDebugging;

namespace PGSauce.Core.FSM.Base
{
    /// <summary>
    /// The FSM per se
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StateMachineBase<T> where T : MonoStateMachineBase<T>
    {
        private readonly T _controller;
        private int _currentStateIndex;
        
        private bool ShowDebug => _controller.ShowDebug;
        public StateBase<T> CurrentState
        {
            get;
            private set;
        }
        private List<AnyTransitionBase<T>> Any
        {
            get;
            set;
        }
        public T Controller => _controller;

        public StateMachineBase(T stateControllerBase, StateBase<T> initialState, List<AnyTransitionBase<T>> any)
        {
            _controller = stateControllerBase;
            CurrentState = initialState;
            Any = any;
            Enter();
        }
        
        public void ChangeState(StateBase<T> newState)
        {
            if (newState.IsNullState) {return;}
            Exit();
            CurrentState = newState;
            _currentStateIndex = CurrentState.debugIndex;
            Enter();
        }

        public void CheckTransitions()
        {
            var changed = CurrentState.CheckTransitions(Controller);

            if (!changed && Any != null)
            {
                CurrentState.CheckAnyTransitions(Controller, Any);
            }
        }

        public void LogicUpdate()
        {
            CurrentState.LogicUpdate(Controller);
        }

        public void PhysicsUpdate()
        {
            CurrentState.PhysicsUpdate(Controller);
        }

        private void Exit()
        {
            PGDebug.SetCondition(ShowDebug).Message($"{Controller.name} Exited state {CurrentState}").Log();
            CurrentState.Exit(Controller);
        }
        
        private void Enter()
        {
            PGDebug.SetCondition(ShowDebug).Message($"{Controller.name} Entered state {CurrentState}").Log();
            CurrentState.Enter(Controller);
        }
    }
}