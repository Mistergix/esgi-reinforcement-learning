using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.Utilities;
using PGSauce.Games.IaEsgi.Ia;
using UnityEngine;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    [CreateAssetMenu(menuName = Strings.GridWorldConsoleAlgos + "Policy Iteration")]
    public class GridWorldPolicyIteration : PolicyIteration<GridWorldGame, QAgentGridWorldConsole, QStateGridWorldConsole>, IGridWorldAi
    {
        #region Public And Serialized Fields
        #endregion
        #region Private Fields
        private Dictionary<Coords, QStateGridWorldConsole> _statesDictionary;
        #endregion
        #region Properties
        public GridWorldConsoleData Level => Game.Level;

        #endregion
        #region Public Methods
        protected override void ResetAlgorithmForNewEpoch()
        {
            
        }

        protected override void ResetAgent()
        {
            var randomCoords = new Coords(Random.Range(0, Level.width), Random.Range(0, Level.height));
            Agent.CurrentState = _statesDictionary[randomCoords];
            Move(new Coords(0, 0));
        }

        protected override QAgent<QAgentGridWorldConsole, QStateGridWorldConsole> CreateAgent()
        {
            var actionUp = new QActionGridWorldConsole(agent => agent.GoUp(), "Go up");
            var actionDown = new QActionGridWorldConsole(agent => agent.GoDown(), "Go Down");
            var actionLeft = new QActionGridWorldConsole(agent => agent.GoLeft(), "Go Left");
            var actionRight = new QActionGridWorldConsole(agent => agent.GoRight(), "Go Right");
            var actions = new List<QAction<QAgentGridWorldConsole, QStateGridWorldConsole>> { actionDown, actionUp, actionRight, actionLeft };
            _statesDictionary = CreateStates();
            var agent = new QAgentGridWorldConsole(this, new List<QStateGridWorldConsole>(_statesDictionary.Values.ToList()), actions, _statesDictionary[Level.start]);
            return agent;
        }
        
        private Dictionary<Coords, QStateGridWorldConsole> CreateStates()
        {
            var states = new Dictionary<Coords, QStateGridWorldConsole>();
            for (var i = 0; i < Level.width; i++)
            {
                for (var j = 0; j < Level.height; j++)
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
        
        public float GetTileValue(Coords coords)
        {
            return Game.GetTileValue(coords);
        }

        private bool IsInBounds(Coords nextCoords)
        {
            return nextCoords.x.IsBetween(0, Level.width - 1) && nextCoords.y.IsBetween(0, Level.height - 1);
        }

        private QStateGridWorldConsole Move(Coords offset)
        {
            var currenState = Agent.CurrentState;
            var nextCoords = currenState.Coords + offset;
            if (IsInBounds(nextCoords))
            {
                Game.Player.transform.position = Game.GetScreenPointFromLevelIndices(nextCoords.y + 1, nextCoords.x + 1, -0.02f);
                return _statesDictionary[nextCoords];
            }

            return Agent.CurrentState;
        }
        #endregion
        #region Private Methods

        
        #endregion
    }
}