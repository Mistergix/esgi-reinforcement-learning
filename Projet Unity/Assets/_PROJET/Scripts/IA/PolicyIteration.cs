using System.Collections.Generic;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class PolicyIteration<TAgent, TState> : QAlgorithm<TAgent, TState> where TAgent : QAgentBase where TState : QState
    {
        private Dictionary<TState, float> _v;

        protected sealed override void CustomBeforeExecute()
        {
            _v = new Dictionary<TState, float>();
            foreach (var state in States)
            {
                _v.Add(state, 0f);
            }
        }
    }
}