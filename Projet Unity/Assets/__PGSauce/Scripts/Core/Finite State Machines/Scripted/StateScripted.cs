using System;
using System.Collections.Generic;
using PGSauce.Core.FSM.Base;
using PGSauce.Core.Utilities;

namespace PGSauce.Core.FSM.Scripted
{
    public abstract class StateScripted<T> : StateBase<T> where T : MonoStateMachineBase<T>
    {
        protected StateScripted()
        {
            Transitions = new List<ITransitionBase<T>>();
        }
        
        public override string Name()
        {
            var name = GetType().ToString();
            
            return name.RemoveNamespace();
        }

        public override bool IsNullState => false;
    }
}
