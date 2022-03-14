using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using UnityEngine;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class PolicyIteration<TGame, TAgent, TState> : RlAlgorithm<TGame, TAgent, TState> where TAgent : QAgentBase where TState : QState where TGame : MonoRlBase
    {
        [SerializeField] private float theta;
        
        private Dictionary<TState, float> _values;
        private Dictionary<TState, QPolicy<TAgent, TState>> _policies;
        private bool _policyStable;

        protected sealed override void CustomBeforeTrain()
        {
            Init();
        }

        private void Init()
        {
            _values = new Dictionary<TState, float>();
            _policies = new Dictionary<TState, QPolicy<TAgent, TState>>();
            foreach (var state in States)
            {
                _values.Add(state, 0f);
                var policy = GetRandomPolicy(state);
                _policies.Add(state, policy);
            }

            _policyStable = false;
        }

        protected override void AlgoCustomInit()
        {
            Init();
        }

        protected override void CustomAfterTrain()
        {
            
        }
        
        public override QAction<TAgent, TState> GetBestAction(TState state)
        {
            return _policies[state].Action;
        }

        protected override bool ContinueToRunAlgorithmForThisEpoch()
        {
            return !_policyStable;
        }

        protected sealed override void CustomTrainEpoch()
        {
            PolicyEvaluation();
            PolicyImprovement();
        }

        private void PolicyImprovement()
        {
            _policyStable = true;
            foreach (var state in States)
            {
                var temp = _policies[state];
                _policies[state] = GetBestPolicy(state);
                if (!temp.Equals(_policies[state]))
                {
                    _policyStable = false;
                }
            }
        }

        private void PolicyEvaluation()
        {
            var delta = 0f;
            do
            {
                delta = 0f;
                foreach (var state in States)
                {
                    var temp = _values[state];
                    _values[state] = GetValue(state);
                    delta = Mathf.Max(delta, Mathf.Abs(temp - _values[state]));
                }
            } while (delta < theta);
        }
        
        private QPolicy<TAgent, TState> GetBestPolicy(TState state)
        {
            var allPolicies = _policies.Values.ToList();
            var best = GetMaxByProperty(allPolicies, policy =>
            {
                var value = GetValue(state, policy);
                return value;
            });

            return best;
        }

        private float GetValue(TState state, QPolicy<TAgent, TState> policy)
        {
            var v = 0f;
            foreach (var qState in States)
            {
                var proba = GetProbability(state, qState, policy);
                var reward = GetReward(state, qState, policy);
                var discountedValue = DiscountRate * _values[qState];
                v += proba * (reward + discountedValue);
            }

            return v;
        }

        private float GetProbability(TState from, TState to, QPolicy<TAgent, TState> policy)
        {
            PGDebug.Message($"Do proba maybe ?").LogTodo();
            return 1f;
        }

        private float GetReward(TState from, TState to, QPolicy<TAgent, TState> policy)
        {
            return Agent.GetCurrentReward(from, to, policy.Action);
        }

        private float GetValue(TState state)
        {
            return GetValue(state, _policies[state]);
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