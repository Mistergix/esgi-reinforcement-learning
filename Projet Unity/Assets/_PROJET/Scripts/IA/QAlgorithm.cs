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
    public abstract class QAlgorithm : PGMonoBehaviour
    {
        #region Public And Serialized Fields
        [SerializeField] private float learningRate;
        [SerializeField] private float discountRate;
        [SerializeField, Range(0,1)] private float epsilonGreedyInitialRate;
        #endregion
        #region Private Fields
        private Float01 _epsilonGreedyRate;
        private List<QState> _states;
        private List<QAction> _actions;
        private QTable _qTable;
        private QAgent _agent;

        #endregion
        #region Properties
        #endregion
        #region Unity Functions
        public void Awake()
        {
            _epsilonGreedyRate = epsilonGreedyInitialRate;
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
        public void Execute(List<QAction> actions, List<QState> states, QAgent agent)
        {
            _agent = agent;
            _actions = actions;
            _states = states;
            _qTable = new QTable(actions, states);
            while (ContinueToRunAlgorithm())
            {
                var action = ChooseAction();
                UpdateEpsilonGreedyRate();
                _agent.TakeAction(action);
                UpdateQTable();
            }
        }

        protected abstract bool ContinueToRunAlgorithm();

        #endregion
        #region Private Methods
        
        private void UpdateEpsilonGreedyRate()
        {
            PGDebug.Message("DO epsilon greed decay").LogTodo();
        }
        
        private void UpdateQTable()
        {
            throw new NotImplementedException();
        }

        private QAction ChooseAction()
        {
            var value = Random.value;
            if (value <= _epsilonGreedyRate)
            {
                return _actions.GetRandomValue();
            }

            return GetMaxByProperty(_actions, action => _qTable.EvaluateAction(action, _agent.CurrentState));
        }

        private T GetMaxByProperty<T>(IEnumerable<T> list, Func<T, float> evaluator)
        {
            return list.Aggregate(((a1, a2) => evaluator(a1) > evaluator(a2) ? a1 : a2));
        }
        #endregion
    }
}
