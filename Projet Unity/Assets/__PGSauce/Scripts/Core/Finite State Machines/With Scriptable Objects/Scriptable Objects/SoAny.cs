using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.FSM.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PGSauce.Core.FSM.WithSo
{
    [InlineEditor]
    public class SoAny<T> : SoAnyBase where T : MonoStateMachineWithSo<T>
    {
        [SerializeField] private AnyTransition<T>[] transitions;
        public List<AnyTransitionBase<T>> GetTransitions(MonoStateMachineWithSo<T> monoStateMachineWithSo)
        {
            return transitions.Select(t =>
                    new AnyTransitionWithSo<T>(monoStateMachineWithSo.GetState(t.to), t.decision,
                        t.reverseValue, t.allowLoop,
                        t.excludedStates.Select(monoStateMachineWithSo.GetState).ToList()))
                .Cast<AnyTransitionBase<T>>().ToList();
        }

        public List<AnyTransitionBase> GetTransitionsInterfaces()
        {
            return transitions.Select(transition => transition as AnyTransitionBase).ToList();
        }
    }
}