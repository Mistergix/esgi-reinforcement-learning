using System.Collections.Generic;
using UnityEngine;

namespace PGSauce.Core.FSM.Base
{
    public abstract class AbstractMonoStateMachine : MonoBehaviour, IStateMachine
    {
        public abstract AbstractState GetCurrentState();
        public abstract List<IAnyTransition> GetAny();
        public abstract AbstractState GetInitialState();
    }
}