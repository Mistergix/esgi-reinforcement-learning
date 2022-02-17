using System.Collections.Generic;
using PGSauce.Core.Utilities;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class PolicyIteration<TAgent, TState> : QAlgorithm<TAgent, TState> where TAgent : QAgentBase where TState : QState
    {
        private Dictionary<TState, float> _values;
        private Dictionary<TState, QPolicy<TAgent, TState>> _policies;

        protected sealed override void CustomBeforeExecute()
        {
            _values = new Dictionary<TState, float>();
            _policies = new Dictionary<TState, QPolicy<TAgent, TState>>();
            foreach (var state in States)
            {
                _values.Add(state, 0f);
                var policy = GetRandomPolicy(state);
                _policies.Add(state, policy);
            }
        }

        private QPolicy<TAgent,TState> GetRandomPolicy(TState state)
        {
            var actions = GetAvailableActionsFromState(state);
            var policy = new QPolicy<TAgent, TState>();
            policy.Action = actions.GetRandomValue();
            return policy;
        }
    }
}