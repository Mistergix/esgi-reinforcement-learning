using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using PGSauce.Unity;
using Random = UnityEngine.Random;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QAlgorithm<TAgent, TState> : AlgorithmBase where TAgent : QAgentBase where TState : QState
    {
        #region Public And Serialized Fields
        [SerializeField, Range(0,1)] private float learningRate;
        [SerializeField, Range(0,1)] private float discountRate;
        [SerializeField, Range(0,1)] private float epsilonGreedyInitialRate;
        [SerializeField, UnityEngine.Min(0)] private float waitTimeTrain;
        [SerializeField, Min(0)] private float waitTimeRun;
        [SerializeField] private bool showTraining;
        #endregion
        #region Private Fields
        private Float01 _epsilonGreedyRate;
        private bool _isRunning;

        #endregion
        #region Properties

        public List<QAction<TAgent, TState>> Actions => Agent.Actions;

        public List<TState> States => Agent.States;

        protected QAgent<TAgent, TState> Agent { get; private set; }

        public float LearningRate => learningRate;

        public float DiscountRate => discountRate;

        #endregion
        #region Unity Functions
        public void Awake()
        {
            PGDebug.Message($"AWAKE").Log();
            _epsilonGreedyRate = epsilonGreedyInitialRate;
            _isRunning = true;
            Run();
        }

        public void Start()
        {
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryRunTrainedAgent();
            }
        }

        private void TryRunTrainedAgent()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;
            StartCoroutine(RunTrainedAgent());
        }

        public void OnEnable()
        {
        }
        
        public void OnDisable()
        {
        }
        
        public void OnDestroy()
        {
        }
        
        #endregion
        #region Public Methods
        protected abstract bool ContinueToRunAlgorithmForThisEpoch(TState agentCurrentState);
        protected abstract void ResetForNewEpoch();
        
        protected abstract void CustomInit();

        protected abstract QAgent<TAgent, TState> CreateAgent();
        
        protected abstract void CustomUpdateAfterAgentDoesAction(QAction<TAgent, TState> action);

        protected abstract void CustomUpdate();

        protected abstract void CustomAfterTrain();

        protected abstract void CustomBeforeExecute();

        protected abstract void ResetAgentForTraining();
        protected abstract QAction<TAgent, TState> GetBestAction();
        
        protected abstract void ResetTrainedAgent();

        protected virtual List<QAction<TAgent, TState>> GetAvailableActionsFromState(TState state)
        {
            return Actions;
        }

        #endregion
        #region Private Methods

        private void Run()
        {
            CustomInit();
            Agent = CreateAgent();
            StartCoroutine(Train());
        }

        private IEnumerator Train()
        {
            CustomBeforeExecute();

            for (int i = 0; i < MaxEpochs; i++)
            {
                CurrentEpoch = i + 1;
                PGDebug.Message($"------------------NEW EPOCH {CurrentEpoch}---------------------").Log();
                ResetForNewEpoch();
                ResetAgentForTraining();
                while (ContinueToRunAlgorithmForThisEpoch(Agent.CurrentState))
                {
                    var action = ChooseAction();
                    UpdateEpsilonGreedyRate();
                    Agent.TakeAction(action);
                    CustomUpdateAfterAgentDoesAction(action);
                    CustomUpdate();
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

            CustomAfterTrain();
            _isRunning = false;
        }

        private IEnumerator RunTrainedAgent()
        {
            PGDebug.Message($"------------------- RUN ALGORITHM -------------------").Log();
            ResetForNewEpoch();
            ResetTrainedAgent();
            while (ContinueToRunAlgorithmForThisEpoch(Agent.CurrentState))
            {
                var action = GetBestAction();
                Agent.TakeAction(action);
                PGDebug.Message($"---------------------").Log();
                yield return new WaitForSeconds(waitTimeRun);
            }

            _isRunning = false;
        }

        private void UpdateEpsilonGreedyRate()
        {
            PGDebug.Message("DO epsilon greed decay").LogTodo();
        }

        private QAction<TAgent, TState> ChooseAction()
        {
            var value = Random.value;
            if (value <= _epsilonGreedyRate)
            {
                PGDebug.Message($"EXPLORATION").Log();
                return Actions.GetRandomValue();
            }
            
            PGDebug.Message($"EXPLOITATION").Log();
            return GetBestAction();
        }
        #endregion
    }
}
