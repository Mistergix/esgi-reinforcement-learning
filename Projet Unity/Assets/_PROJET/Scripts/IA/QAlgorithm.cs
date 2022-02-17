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
        #endregion
        #region Private Fields
        private Float01 _epsilonGreedyRate;
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
            Run();
        }

        public void Start()
        {
        }
        
        public void Update()
        {
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

        protected abstract void ResetAgent();
        protected abstract QAction<TAgent, TState> GetBestAction();

        #endregion
        #region Private Methods

        private void Run()
        {
            CustomInit();
            Agent = CreateAgent();
            StartCoroutine(Execute());
        }

        private IEnumerator Execute()
        {
            CustomBeforeExecute();

            for (int i = 0; i < MaxEpochs; i++)
            {
                CurrentEpoch = i + 1;
                PGDebug.Message($"------------------NEW EPOCH {CurrentEpoch}---------------------").Log();
                ResetForNewEpoch();
                ResetAgent();
                while (ContinueToRunAlgorithmForThisEpoch(Agent.CurrentState))
                {
                    var action = ChooseAction();
                    UpdateEpsilonGreedyRate();
                    Agent.TakeAction(action);
                    CustomUpdateAfterAgentDoesAction(action);
                    CustomUpdate();
                    PGDebug.Message($"---------------------").Log();
                    yield return new WaitForSeconds(waitTimeTrain);
                }

                yield return new WaitForSeconds(waitTimeTrain * 5);
            }

            CustomAfterTrain();
            
            PGDebug.Message($"------------------- RUN ALGORITHM -------------------").Log();
            ResetForNewEpoch();
            ResetAgent();
            while (ContinueToRunAlgorithmForThisEpoch(Agent.CurrentState))
            {
                var action = GetBestAction();
                Agent.TakeAction(action);
                PGDebug.Message($"---------------------").Log();
                yield return new WaitForSeconds(waitTimeRun);
            }
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
