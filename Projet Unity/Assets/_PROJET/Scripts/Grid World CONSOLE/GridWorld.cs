using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Games.IaEsgi.Ia;
using PGSauce.Unity;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class GridWorld : QAlgorithm<QAgentGridWorldConsole>
    {
        #region Public And Serialized Fields
        [SerializeField] private GridWorldConsoleData level;
        private Dictionary<Coords,QState> _statesDictionary;

        #endregion
        #region Private Fields
        #endregion
        #region Properties
        #endregion
        #region Public Methods
        #endregion
        #region Private Methods
        #endregion

        protected override bool ContinueToRunAlgorithm()
        {
            return Agent.CurrentState.Equals(_statesDictionary[level.end]);
        }

        protected override void InitializeAlgorithm()
        {
            var actionUp = new QActionGridWorldConsole(agent => agent.GoUp());
            var actionDown = new QActionGridWorldConsole(agent => agent.GoDown());
            var actionLeft = new QActionGridWorldConsole(agent => agent.GoLeft());
            var actionRight = new QActionGridWorldConsole(agent => agent.GoRight());
            var actions = new List<QAction<QAgentGridWorldConsole>> {actionDown, actionUp, actionRight, actionLeft};
            _statesDictionary = CreateStates();
            Agent = new QAgentGridWorldConsole(this, _statesDictionary.Values.ToList(), actions, _statesDictionary[level.start]);
        }

        private Dictionary<Coords, QState> CreateStates()
        {
            var states = new Dictionary<Coords, QState>();
            for (var i = 0; i < level.width; i++)
            {
                for (var j = 0; j < level.height; j++)
                {
                    var coords = new Coords(i, j);
                    var state = new QStateGridWorldConsole(coords);
                    states.Add(coords, state);
                }
            }

            return states;
        }

        public QState GoUp()
        {
            throw new System.NotImplementedException();
        }

        public QState GoDown()
        {
            throw new System.NotImplementedException();
        }

        public QState GoLeft()
        {
            throw new System.NotImplementedException();
        }

        public QState GoRight()
        {
            throw new System.NotImplementedException();
        }
    }
}
