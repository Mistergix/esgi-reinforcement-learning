using System;
using System.Collections.Generic;
using System.Linq;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QLearning<TAgent, TState> : QAlgorithm<TAgent, TState> where TAgent : QAgentBase where TState : QState
    {
        #region Private Fields
        private QTable<TAgent, TState> _qTable;
        #endregion

        protected sealed override void CustomBeforeExecute()
        {
            _qTable = new QTable<TAgent, TState>(Actions, States);
        }

        protected sealed override void CustomAfterTrain()
        {
            _qTable.Print();
        }

        protected sealed override void CustomUpdateAfterAgentDoesAction(QAction<TAgent, TState> action)
        {
            UpdateQTable(action);
        }

        protected override QAction<TAgent, TState> GetBestAction()
        {
            return GetBestAction(Agent.CurrentState);
        }

        protected QAction<TAgent, TState> GetBestAction(TState state)
        {
            return GetMaxByProperty(Actions, action => _qTable.EvaluateAction(action, state));
        }

        private T GetMaxByProperty<T>(IEnumerable<T> list, Func<T, float> evaluator)
        {
            return list.Aggregate(((a1, a2) => evaluator(a1) > evaluator(a2) ? a1 : a2));
        }
        

        private void UpdateQTable(QAction<TAgent, TState> qAction)
        {
            var old = _qTable.EvaluateAction(qAction, Agent.OldState);
            var reward = Agent.GetCurrentReward();
            var max = GetMaxQ(Agent.CurrentState);
            var newValue = (1 - LearningRate) * old + LearningRate * reward + DiscountRate * max;
            _qTable.UpdateValue(Agent.OldState, qAction, newValue);
        }
        
        private float GetMaxQ(TState state)
        {
            return Actions.Max(a => _qTable.EvaluateAction(a, state));
        }
    }
}