using System;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using PGSauce.Games.IaEsgi.GridWorldConsole;
using Random = UnityEngine.Random;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QLearning<TGame, TAgent, TState> : RlAlgorithm<TGame, TAgent, TState> where TAgent : QAgentBase where TState : QState where TGame : MonoRlBase
    {
        #region Private Fields
        private QTable<TAgent, TState> _qTable;
        #endregion

        protected sealed override void CustomTrainEpoch()
        {
            var action = ChooseAction();
            Agent.TakeAction(action);
            UpdateQTable(action);
        }

        protected sealed override bool ContinueToRunAlgorithmForThisEpoch()
        {
            return !Agent.CurrentState.Equals(GoalState);
        }

        protected abstract TState GoalState { get; }

        protected sealed override void CustomBeforeTrain()
        {
            _qTable = new QTable<TAgent, TState>(Actions, States);
        }

        protected override void AlgoCustomInit()
        {
            _qTable = new QTable<TAgent, TState>(Actions, States);
        }

        protected sealed override void CustomAfterTrain()
        {
            _qTable.Print();
        }

        public override QAction<TAgent, TState> GetBestAction(TState state)
        {
            return GetMaxByProperty(Actions, action => _qTable.EvaluateAction(action, state));
        }

        private QAction<TAgent, TState> ChooseAction()
        {
            var value = Random.value;
            if (value <= EpsilonGreedyRate)
            {
                PGDebug.Message($"EXPLORATION").Log();
                return Actions.GetRandomValue();
            }
            
            PGDebug.Message($"EXPLOITATION").Log();
            return GetBestAction();
        }


        private void UpdateQTable(QAction<TAgent, TState> qAction)
        {
            var old = _qTable.EvaluateAction(qAction, Agent.OldState);
            var reward = Agent.GetCurrentReward(qAction);
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