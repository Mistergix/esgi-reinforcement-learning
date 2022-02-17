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
    public abstract class QAlgorithm<TAgent, TState> : PGMonoBehaviour where TAgent : QAgentBase where TState : QState
    {
        #region Public And Serialized Fields
        [SerializeField, Range(0,1)] private float learningRate;
        [SerializeField, Range(0,1)] private float discountRate;
        [SerializeField, Range(0,1)] private float epsilonGreedyInitialRate;
        [SerializeField, Min(1)] private int maxEpochs = 10;
        #endregion
        #region Private Fields
        private Float01 _epsilonGreedyRate;
        private QTable<TAgent, TState> _qTable;

        #endregion
        #region Properties

        public List<QAction<TAgent, TState>> Actions => Agent.Actions;

        public List<TState> States => Agent.States;

        protected QAgent<TAgent, TState> Agent { get; private set; }

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

        #endregion
        #region Private Methods
        
        private void Run()
        {
            Agent = CreateAgent();
            StartCoroutine(Execute());
        }

        protected abstract QAgent<TAgent, TState> CreateAgent();

        private IEnumerator Execute()
        {
            _qTable = new QTable<TAgent, TState>(Actions, States);

            for (int i = 0; i < maxEpochs; i++)
            {
                PGDebug.Message($"------------------NEW EPOCH {i + 1}---------------------").Log();
                ResetForNewEpoch();
                ResetAgent();
                while (ContinueToRunAlgorithmForThisEpoch(Agent.CurrentState))
                {
                    var action = ChooseAction();
                    UpdateEpsilonGreedyRate();
                    Agent.TakeAction(action);
                    UpdateQTable(action);
                    PGDebug.Message($"---------------------").Log();
                    yield return new WaitForEndOfFrame();
                }
            }
            
            _qTable.Print();
            
            PGDebug.Message($"------------------- RUN ALGORITHM -------------------").Log();
            ResetForNewEpoch();
            ResetAgent();
            while (ContinueToRunAlgorithmForThisEpoch(Agent.CurrentState))
            {
                var action = GetBestAction();
                Agent.TakeAction(action);
                PGDebug.Message($"---------------------").Log();
                yield return new WaitForEndOfFrame();
            }
        }

        protected abstract void ResetAgent();

        private void UpdateEpsilonGreedyRate()
        {
            PGDebug.Message("DO epsilon greed decay").LogTodo();
        }
        
        private void UpdateQTable(QAction<TAgent, TState> qAction)
        {
            var old = _qTable.EvaluateAction(qAction, Agent.OldState);
            var reward = Agent.GetCurrentReward();
            var max = GetMaxQ(Agent.CurrentState);
            var newValue = (1 - learningRate) * old + learningRate * reward + discountRate * max;
            _qTable.UpdateValue(Agent.OldState, qAction, newValue);
        }

        private float GetMaxQ(TState state)
        {
            return Actions.Max(a => _qTable.EvaluateAction(a, state));
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

        private QAction<TAgent, TState> GetBestAction()
        {
            return GetMaxByProperty(Actions, action => _qTable.EvaluateAction(action, Agent.CurrentState));
        }

        private T GetMaxByProperty<T>(IEnumerable<T> list, Func<T, float> evaluator)
        {
            return list.Aggregate(((a1, a2) => evaluator(a1) > evaluator(a2) ? a1 : a2));
        }
        #endregion
    }
}
