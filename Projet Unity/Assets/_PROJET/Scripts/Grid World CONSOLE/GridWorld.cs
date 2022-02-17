using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using PGSauce.Games.IaEsgi.Ia;
using PGSauce.Unity;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class GridWorld : QLearning<QAgentGridWorldConsole, QStateGridWorldConsole>
    {
        #region Public And Serialized Fields
        [SerializeField] private GridWorldConsoleData level;
        private Dictionary<Coords,QStateGridWorldConsole> _statesDictionary;
        private HashSet<Coords> _bombs;
        private HashSet<Coords> _energies;

        #endregion
        #region Private Fields
        #endregion
        #region Properties
        #endregion
        #region Public Methods
        #endregion
        #region Private Methods
        #endregion

        protected override bool ContinueToRunAlgorithmForThisEpoch(QStateGridWorldConsole state)
        {
            return !state.Equals(_statesDictionary[level.end]);
        }

        protected override void ResetForNewEpoch()
        {
            _energies = new HashSet<Coords>(level.energy);
            _bombs = new HashSet<Coords>(level.bombs);
        }

        protected override void ResetAgent()
        {
            Agent.CurrentState = _statesDictionary[level.start];
        }

        protected override QAgent<QAgentGridWorldConsole, QStateGridWorldConsole> CreateAgent()
        {
            var actionUp = new QActionGridWorldConsole(agent => agent.GoUp(), "Go up");
            var actionDown = new QActionGridWorldConsole(agent => agent.GoDown(), "Go Down");
            var actionLeft = new QActionGridWorldConsole(agent => agent.GoLeft(), "Go Left");
            var actionRight = new QActionGridWorldConsole(agent => agent.GoRight(), "Go Right");
            var actions = new List<QAction<QAgentGridWorldConsole, QStateGridWorldConsole>> {actionDown, actionUp, actionRight, actionLeft};
            _statesDictionary = CreateStates();
            var agent = new QAgentGridWorldConsole(this, new List<QStateGridWorldConsole>(_statesDictionary.Values.ToList()), actions, _statesDictionary[level.start]);
            return agent;
        }

        private Dictionary<Coords, QStateGridWorldConsole> CreateStates()
        {
            var states = new Dictionary<Coords, QStateGridWorldConsole>();
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

        public QStateGridWorldConsole GoUp()
        {
            return Move(new Coords(0, 1));
        }

        public QStateGridWorldConsole GoDown()
        {
            return Move(new Coords(0, -1));
        }

        public QStateGridWorldConsole GoLeft()
        {
            return Move(new Coords(-1, 0));
        }

        public QStateGridWorldConsole GoRight()
        {
            return Move(new Coords(1, 0));
        }
        
        private bool IsInBounds(Coords nextCoords)
        {
            return nextCoords.x.IsBetween(0, level.width - 1) && nextCoords.y.IsBetween(0, level.height - 1);
        }
        
        private QStateGridWorldConsole Move(Coords offset)
        {
            var currenState = Agent.CurrentState;
            var nextCoords = currenState.Coords + offset;
            if (IsInBounds(nextCoords))
            {
                return _statesDictionary[nextCoords];
            }

            return Agent.CurrentState;
        }

        public float GetTileValue(Coords coords)
        {
            var factor = coords.Equals(Agent.OldState.Coords) ? 2 : 1;
            if (_bombs.Contains(coords))
            {
                PGDebug.Message($"{coords} is bomb").Log();
                _bombs.Remove(coords);
                return -100f * factor;
            }

            if (_energies.Contains(coords))
            {
                PGDebug.Message($"{coords} is energy").Log();
                _energies.Remove(coords);
                return 5f;
            }

            if (level.end.Equals(coords))
            {
                PGDebug.Message($"{coords} is end").Log();
                return 1000f;
            }

            PGDebug.Message($"{coords} is blank").Log();
            return -1f * factor;
        }
    }
}
