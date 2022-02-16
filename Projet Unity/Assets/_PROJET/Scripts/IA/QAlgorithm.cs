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
        [SerializeField] private float learningRate;
        [SerializeField] private float discountRate;
        [SerializeField, Range(0,1)] private float epsilonGreedyInitialRate;
        #endregion
        #region Private Fields
        private Float01 _epsilonGreedyRate;
        private List<QState> _states;
        private List<QAction<TAgent, TState>> _actions;
        private QTable<TAgent, TState> _qTable;
        private QAgent<TAgent, TState> _agent;

        #endregion
        #region Properties

        public List<QAction<TAgent, TState>> Actions => Agent.Actions;

        public List<TState> States => Agent.States;

        protected QAgent<TAgent, TState> Agent
        {
            get => _agent;
            set => _agent = value;
        }
        #endregion
        #region Unity Functions
        public void Awake()
        {
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
        protected abstract bool ContinueToRunAlgorithm();
        protected abstract void InitializeAlgorithm();

        #endregion
        #region Private Methods
        
        private void Run()
        {
            InitializeAlgorithm();
            Execute();
        }

        private void Execute()
        {
            _qTable = new QTable<TAgent, TState>(Actions, States);
            while (ContinueToRunAlgorithm())
            {
                var action = ChooseAction();
                UpdateEpsilonGreedyRate();
                Agent.TakeAction(action);
                UpdateQTable();
            }
        }
        
        private void UpdateEpsilonGreedyRate()
        {
            PGDebug.Message("DO epsilon greed decay").LogTodo();
        }
        
        private void UpdateQTable()
        {
            PGDebug.Message("DO Update Q Table").LogTodo();
        }

        private QAction<TAgent, TState> ChooseAction()
        {
            var value = Random.value;
            if (value <= _epsilonGreedyRate)
            {
                return Actions.GetRandomValue();
            }

            return GetMaxByProperty(Actions, action => _qTable.EvaluateAction(action, Agent.CurrentState));
        }

        private T GetMaxByProperty<T>(IEnumerable<T> list, Func<T, float> evaluator)
        {
            return list.Aggregate(((a1, a2) => evaluator(a1) > evaluator(a2) ? a1 : a2));
        }
        #endregion
    }
}
