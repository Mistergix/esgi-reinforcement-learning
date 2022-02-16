using System;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.WithSo
{
    [Serializable]
    public class AnyTransition<T> : Transition<T>, AnyTransitionBase where T : MonoStateMachineBase<T>
    {
        public List<SoState<T>> excludedStates;
        public List<SoStateBase> ExcludedStates()
        {
            var list = excludedStates.Select(state => state as SoStateBase).ToList();
            return list;
        }
    }
}