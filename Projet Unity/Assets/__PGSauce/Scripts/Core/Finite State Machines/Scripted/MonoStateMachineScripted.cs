using System.Collections.Generic;
using PGSauce.Core.FSM.Base;
using PGSauce.Core.Strings;
using UnityEngine;

namespace PGSauce.Core.FSM.Scripted
{
    [HelpURL(DocsPaths.MonoStateMachineScripted)]
    public abstract class MonoStateMachineScripted<T> : MonoStateMachineBase<T>, IMonoStateMachineScripted where T : MonoStateMachineScripted<T>
    {
        protected sealed override void InitFsm()
        {
            base.InitFsm();
            InitializeStates();
            CreateTransitions();
        }

        protected void AddNewTransition(StateScripted<T> from, StateScripted<T> to, TransitionScripted<T>.Decision decision, bool allowLoop)
        {
            var stateTransition = new TransitionScripted<T>(to, decision, allowLoop);
            from.Transitions.Add(stateTransition);
        }

        protected abstract void InitializeStates();

        protected abstract void CreateTransitions();
    }
}
