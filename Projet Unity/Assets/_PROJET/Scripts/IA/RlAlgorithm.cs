using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class RlAlgorithm<TGame, TAgent, TState> : AlgorithmBase where TAgent : QAgentBase where TState : QState where TGame : MonoRlBase
    {
        #region Public And Serialized Fields
        [SerializeField, Range(0,1)] private float learningRate;
        [SerializeField, Range(0,1)] private float discountRate;
        [SerializeField, Range(0,1)] private float epsilonGreedyInitialRate;
        [SerializeField, UnityEngine.Min(0)] private float waitTimeTrain = 1 / 60f;
        [SerializeField, Min(0)] private float waitTimeRun = 0.5f;
        [SerializeField] private bool showTraining;
        #endregion
        #region Private Fields
        private Float01 _epsilonGreedyRate;
        #endregion
        #region Properties

        public List<QAction<TAgent, TState>> Actions => Agent.Actions;

        public List<TState> States => Agent.States;

        public QAgent<TAgent, TState> Agent { get; private set; }

        public float LearningRate => learningRate;

        public float DiscountRate => discountRate;

        public Float01 EpsilonGreedyRate => _epsilonGreedyRate;
        public bool IsBusy => IsRunning || IsTraining;

        public TGame Game { get; private set; }

        #endregion
        #region Public Methods
        public void Init(TGame monoRl)
        {
            PGDebug.Message($"Game is {monoRl}").Log();
            Game = monoRl;
            Agent = CreateAgent();
            AlgoCustomInit();
        }

        protected abstract void AlgoCustomInit();
        protected abstract void ResetAlgorithmForNewEpoch();

        protected abstract QAgent<TAgent, TState> CreateAgent();

        protected abstract bool ContinueToRunAlgorithmForThisEpoch();

        protected abstract void CustomAfterTrain();

        protected abstract void CustomBeforeTrain();

        protected QAction<TAgent, TState> GetBestAction() => GetBestAction(Agent.CurrentState);
        public abstract QAction<TAgent, TState> GetBestAction(TState state);
        

        protected virtual List<QAction<TAgent, TState>> GetAvailableActionsFromState(TState state)
        {
            return Actions;
        }
        
        protected T GetMaxByProperty<T>(IEnumerable<T> list, Func<T, float> evaluator)
        {
            return list.Aggregate(((a1, a2) => evaluator(a1) > evaluator(a2) ? a1 : a2));
        }
        
        public IEnumerator Train()
        {
            _epsilonGreedyRate = epsilonGreedyInitialRate;
            IsTraining = true;
            CustomBeforeTrain();

            for (var i = 0; i < MaxEpochs; i++)
            {
                CurrentEpoch = i + 1;
                PGDebug.Message($"------------------NEW EPOCH {CurrentEpoch}---------------------").Log();
                ResetForNewEpoch();
                while (ContinueToRunAlgorithmForThisEpoch())
                {
                    CustomTrainEpoch();
                    UpdateEpsilonGreedyRate();
                    PGDebug.Message($"---------------------").Log();
                    if (showTraining)
                    {
                        yield return new WaitForSeconds(waitTimeTrain);
                    }
                }

                if (showTraining)
                {
                    yield return new WaitForSeconds(waitTimeTrain * 5);
                }
            }
            
            IsTraining = false;
            CustomAfterTrain();
        }

        protected abstract void ResetAgent();

        protected abstract void CustomTrainEpoch();

        public IEnumerator RunTrainedAgent()
        {
            PGDebug.Message($"------------------- RUN ALGORITHM -------------------").Log();
            IsRunning = true;
            ResetForNewEpoch();
            while (ContinueToRunAlgorithmForThisEpoch())
            {
                yield return new WaitForSeconds(waitTimeRun);
                var action = GetBestAction();
                Agent.TakeAction(action);
                PGDebug.Message($"---------------------").Log();
            }

            IsRunning = false;
        }

        private void ResetForNewEpoch()
        {
            Game.ResetGameForNewEpoch();
            ResetAlgorithmForNewEpoch();
            ResetAgent();
        }

        #endregion
        #region Private Methods

        private void UpdateEpsilonGreedyRate()
        {
            PGDebug.Message("DO epsilon greed decay").LogTodo();
        }
        
        #endregion
    }
}
