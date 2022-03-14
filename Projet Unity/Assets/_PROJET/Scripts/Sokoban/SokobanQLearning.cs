using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.Sokoban
{
    [CreateAssetMenu(menuName = Strings.SokobanAlgos + "Q Learning")]
    public class SokobanQLearning : QLearning<SokobanGame, QAgentSokoban, QStateSokoban>, ISokobanAi
    {
        #region Public And Serialized Fields
        #endregion
        #region Private Fields
        private Dictionary<Coords, QStateSokoban> _statesDictionary;
        #endregion
        #region Properties
        #endregion
        #region Unity Functions
        protected override QStateSokoban GoalState => throw new System.NotImplementedException();

        public SokobanData Level => Game.Level;
        protected override void ResetAlgorithmForNewEpoch()
        {
            throw new System.NotImplementedException();
        }

        protected override QAgent<QAgentSokoban, QStateSokoban> CreateAgent()
        {
            var actionUp = new QActionSokoban(agent => agent.GoUp(), "Go up");
            var actionDown = new QActionSokoban(agent => agent.GoDown(), "Go Down");
            var actionLeft = new QActionSokoban(agent => agent.GoLeft(), "Go Left");
            var actionRight = new QActionSokoban(agent => agent.GoRight(), "Go Right");
            var actions = new List<QAction<QAgentSokoban, QStateSokoban>> { actionDown, actionUp, actionRight, actionLeft };
            _statesDictionary = CreateStates();
            var agent = new QAgentSokoban(this, new List<QStateSokoban>(_statesDictionary.Values.ToList()), actions, _statesDictionary[Level.start]);
            return agent;
        }

        private Dictionary<Coords, QStateSokoban> CreateStates()
        {
            var states = new Dictionary<Coords, QStateSokoban>();
            for (var i = 0; i < Level.width; i++)
            {
                for (var j = 0; j < Level.height; j++)
                {
                    var coords = new Coords(i, j);
                    var state = new QStateSokoban(coords);
                    states.Add(coords, state);
                }
            }

            return states;
        }

        protected override void ResetAgent()
        {
            
        }

        public QStateSokoban GoUp()
        {
            return Move(new Coords(0, 1));
        }

        public QStateSokoban GoDown()
        {
            return Move(new Coords(0, -1));
        }

        public QStateSokoban GoLeft()
        {
            return Move(new Coords(-1, 0));
        }

        public QStateSokoban GoRight()
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

        private QStateSokoban Move(Coords offset)
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
        #region Public Methods
        #endregion
        #region Private Methods
        #endregion
    }
}
