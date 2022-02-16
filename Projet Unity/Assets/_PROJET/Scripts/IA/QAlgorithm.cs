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
    public abstract class QAlgorithm<TAgent> : PGMonoBehaviour where TAgent : QAgentBase
    {
        #region Public And Serialized Fields
        [SerializeField] private float learningRate;
        [SerializeField] private float discountRate;
        [SerializeField, Range(0,1)] private float epsilonGreedyInitialRate;
        #endregion
        #region Private Fields
        private Float01 _epsilonGreedyRate;
        private List<QState> _states;
        private List<QAction<TAgent>> _actions;
        private QTable<TAgent> _qTable;
        private QAgent<TAgent> _agent;

        #endregion
        #region Properties

        public List<QAction<TAgent>> Actions => Agent.Actions;

        public List<QState> States => Agent.States;

        protected QAgent<TAgent> Agent
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
            _qTable = new QTable<TAgent>(Actions, States);
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
            throw new NotImplementedException();
        }

        private QAction<TAgent> ChooseAction()
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
