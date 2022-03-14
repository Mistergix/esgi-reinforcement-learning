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
            
        }

        protected override QAgent<QAgentSokoban, QStateSokoban> CreateAgent()
        {
            var actionUp = new QActionSokoban(agent => agent.GoUp(), "Go up");
            var actionDown = new QActionSokoban(agent => agent.GoDown(), "Go Down");
            var actionLeft = new QActionSokoban(agent => agent.GoLeft(), "Go Left");
            var actionRight = new QActionSokoban(agent => agent.GoRight(), "Go Right");
            var pushUp = new QActionSokoban(agent => agent.PushUp(), "Push up");
            var pushRight = new QActionSokoban(agent => agent.PushRight(), "Push Right");
            var pushLeft = new QActionSokoban(agent => agent.PushLeft(), "Push Left");
            var pushDown = new QActionSokoban(agent => agent.PushDown(), "Push Down");
            var actions = new List<QAction<QAgentSokoban, QStateSokoban>>
                {actionDown, actionUp, actionRight, actionLeft, pushDown, pushLeft, pushRight, pushUp};
            var _statesDictionary = CreateStates();
            var agent = new QAgentSokoban(this, new List<QStateSokoban>(_statesDictionary.ToList()), actions, _statesDictionary[0]);
            return agent;
        }

        private List<QStateSokoban> CreateStates()
        {
            Game.Init();
            var states = new List<QStateSokoban>();
            var validPositions = new List<Coords>();
            for (var x = 0; x < Level.width; x++)
            {
                for (var y = 0; y < Level.height; y++)
                {
                    if (!Game.IsValidPosition(new Vector2(x, y)))
                    {
                        continue;
                    }

                    validPositions.Add(new Coords(x, y));
                }
            }

            var coordsCountPerState = 1 + Game.CrateCount;

            var p = coordsCountPerState;
            var n = validPositions.Count;

            var indices = Enumerable.Range(0, p).ToList();
            var arrangements = new List<List<int>> {new List<int>(indices)};

            var i = p - 1;
            var k = p;

            while (i != -1)
            {
                PGDebug.Message($"i,k,p {i}, {k}, {p}").Log();
                if (k < n)
                {
                    if (!indices.Take(i).Contains(k))
                    {
                        indices[i] = k;
                        k = 0;
                        var j = i + 1;
                        while (j < p)
                        {
                            if (!indices.Take(i + 1).Contains(k))
                            {
                                indices[j] = k;
                                j++;
                            }

                            k++;
                        }
                        arrangements.Add(new List<int>(indices));
                        i = p - 1;
                        k = indices[i];
                    }

                    k++;
                }
                else
                {
                    i--;
                    k = indices[i] + 1;
                }
            }
            
            PGDebug.Message($"Count = {arrangements.Count}").Log();

            foreach (var arrangement in arrangements)
            {
                PGDebug.Log(arrangement);
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

        public QStateSokoban PushUp()
        {
            throw new System.NotImplementedException();
        }

        public QStateSokoban PushDown()
        {
            throw new System.NotImplementedException();
        }

        public QStateSokoban PushLeft()
        {
            throw new System.NotImplementedException();
        }

        public QStateSokoban PushRight()
        {
            throw new System.NotImplementedException();
        }

        private bool IsInBounds(Coords nextCoords)
        {
            return nextCoords.x.IsBetween(0, Level.width - 1) && nextCoords.y.IsBetween(0, Level.height - 1);
        }

        private QStateSokoban Move(Coords offset)
        {
            var currenState = Agent.CurrentState;
            var nextCoords = currenState.PlayerCoords + offset;
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
