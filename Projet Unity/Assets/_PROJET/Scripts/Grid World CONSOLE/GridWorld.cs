using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class GridWorld : QLearning<QAgentGridWorldConsole, QStateGridWorldConsole>
    {
        #region Public And Serialized Fields
        [SerializeField] private GridWorldConsoleData level;

        [SerializeField]
        private Camera cam;

        public int groundTile;
        public GameObject groundPrefabs;

        public int playerTile;
        public GameObject playerPrefabs;

        public int bombsTile;
        public GameObject bombsPrefabs;

        public int energyTile;
        public GameObject energyPrefabs;

        public int wallTile;
        public GameObject wallPrefabs;

        public int endTile;
        public GameObject endPrefabs;

        public GameObject noDirPrefabs;
        public GameObject upDirPrefabs;
        public GameObject rightDirPrefabs;
        public GameObject leftDirPrefabs;
        public GameObject downDirPrefabs;

        public float tileSize;

        #endregion
        #region Private Fields
        private int _rows, _cols;
        private int[,] _levelData;
        private Vector2 _middleOffset = new Vector2();
        private Dictionary<Coords, QStateGridWorldConsole> _statesDictionary;
        private HashSet<Coords> _bombs;
        private HashSet<Coords> _energiesCoords;

        private GameObject player;

        private GameObject[,] dirOnTile;
        private HashSet<GameObject> bombs;
        private HashSet<GameObject> energies;


        #endregion
        #region Properties
        #endregion
        #region Public Methods
        protected override bool ContinueToRunAlgorithmForThisEpoch(QStateGridWorldConsole state)
        {
            return !state.Equals(_statesDictionary[level.end]);
        }

        protected override void CustomInit()
        {
            ParseLevel();
            CreateLevel();
        }

        protected override void ResetForNewEpoch()
        {
            _energiesCoords = new HashSet<Coords>(level.energy);
            _bombs = new HashSet<Coords>(level.bombs);
        }

        protected override void ResetAgentForTraining()
        {
            Agent.CurrentState = _statesDictionary[level.start];
        }

        protected override void ResetTrainedAgent()
        {
            var randomCoords = new Coords(Random.Range(0, level.width), Random.Range(0, level.height));
            Agent.CurrentState = _statesDictionary[randomCoords];
        }

        protected override QAgent<QAgentGridWorldConsole, QStateGridWorldConsole> CreateAgent()
        {
            var actionUp = new QActionGridWorldConsole(agent => agent.GoUp(), "Go up");
            var actionDown = new QActionGridWorldConsole(agent => agent.GoDown(), "Go Down");
            var actionLeft = new QActionGridWorldConsole(agent => agent.GoLeft(), "Go Left");
            var actionRight = new QActionGridWorldConsole(agent => agent.GoRight(), "Go Right");
            var actions = new List<QAction<QAgentGridWorldConsole, QStateGridWorldConsole>> { actionDown, actionUp, actionRight, actionLeft };
            _statesDictionary = CreateStates();
            var agent = new QAgentGridWorldConsole(this, new List<QStateGridWorldConsole>(_statesDictionary.Values.ToList()), actions, _statesDictionary[level.start]);
            return agent;
        }

        protected override void CustomUpdate()
        {
            foreach (var state in States)
            {
                var Action = GetBestAction(state);
                Destroy(dirOnTile[state.Coords.y + 1, state.Coords.x + 1]);

                GameObject chosenPrefabs;
                switch (Action.Name)
                {
                    case "Go up":
                        chosenPrefabs = upDirPrefabs;
                        break;

                    case "Go Down":
                        chosenPrefabs = downDirPrefabs;
                        break;

                    case "Go Left":
                        chosenPrefabs = leftDirPrefabs;
                        break;

                    case "Go Right":
                        chosenPrefabs = rightDirPrefabs;
                        break;
                    default:
                        chosenPrefabs = noDirPrefabs;
                        break;
                }

                dirOnTile[state.Coords.y + 1, state.Coords.x + 1] = Instantiate(chosenPrefabs, new Vector3(0, 0, -0.03f), chosenPrefabs.transform.rotation);
                dirOnTile[state.Coords.y + 1, state.Coords.x + 1].transform.position = GetScreenPointFromLevelIndices(state.Coords.y + 1, state.Coords.x + 1, -0.04f);
                dirOnTile[state.Coords.y + 1, state.Coords.x + 1].transform.localScale *= tileSize;
                dirOnTile[state.Coords.y + 1, state.Coords.x + 1].name = "2dir" + (state.Coords.y).ToString() + "_" + (state.Coords.x).ToString();

            }
            

            foreach (GameObject gameObject in energies)
            {
                Destroy(gameObject);    
            }

            GameObject energy;

            foreach (Coords coords in _energiesCoords)
            {
                energy = Instantiate(energyPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                energy.transform.localScale *= tileSize;
                energy.transform.position = GetScreenPointFromLevelIndices(coords.y + 1, coords.x + 1, -0.02f);
                energies.Add(energy);
            }

            foreach (GameObject gameObject in bombs)
            {
                Destroy(gameObject);
            }

            GameObject bomb;

            foreach (Coords coords in _bombs)
            {
                bomb = Instantiate(bombsPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                bomb.transform.localScale *= tileSize;
                bomb.transform.position = GetScreenPointFromLevelIndices(coords.y + 1, coords.x + 1, -0.02f);
                bombs.Add(bomb);
            }

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
                player.transform.position = GetScreenPointFromLevelIndices(nextCoords.y + 1, nextCoords.x + 1, -0.02f);
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
                return level.bombValue * factor;
            }

            if (_energiesCoords.Contains(coords))
            {
                PGDebug.Message($"{coords} is energy").Log();
                _energiesCoords.Remove(coords);
                return level.energyValue;
            }

            if (level.end.Equals(coords))
            {
                PGDebug.Message($"{coords} is end").Log();
                return level.endValue;
            }

            PGDebug.Message($"{coords} is blank").Log();
            return level.blankValue * factor;
        }
        #endregion
        #region Private Methods

        private void ParseLevel()
        {
            cam.orthographicSize = level.height + 2;
            _rows = level.height + 2;
            _cols = level.width + 2;
            _levelData = new int[_rows, _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (i == 0 || j == 0 || i == _rows - 1 || j == _cols - 1)
                        _levelData[i, j] = wallTile;
                    else
                        _levelData[i, j] = groundTile;
                }
            }

            foreach (Coords coords in level.bombs)
            {
                _levelData[coords.y + 1, coords.x + 1] = bombsTile;
            }

            foreach (Coords coords in level.energy)
            {
                _levelData[coords.y + 1, coords.x + 1] = energyTile;
            }

            _levelData[level.start.y + 1, level.start.x + 1] = playerTile;
            _levelData[level.end.y + 1, level.end.x + 1] = endTile;

        }

        private void CreateLevel()
        {
            _middleOffset.x = _cols * tileSize * 0.5f - tileSize * 0.5f;
            _middleOffset.y = _rows * tileSize * 0.5f - tileSize * 0.5f;
            GameObject tile;
            GameObject energy;
            GameObject bomb;
            GameObject end;

            dirOnTile = new GameObject[_rows, _cols];
            bombs = new HashSet<GameObject>();
            energies = new HashSet<GameObject>();

            int destinationCount = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    int val = _levelData[i, j];

                    if (val != wallTile)
                    {
                        tile = Instantiate(groundPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                        dirOnTile[i, j] = Instantiate(noDirPrefabs, new Vector3(0, 0, -0.03f), Quaternion.identity);

                        tile.name = "tile" + (i - 1).ToString() + "_" + (j - 1).ToString();
                        tile.transform.localScale *= tileSize;
                        tile.transform.position = GetScreenPointFromLevelIndices(i, j, 0);

                        dirOnTile[i, j].transform.position = GetScreenPointFromLevelIndices(i, j, -0.03f);
                        dirOnTile[i, j].transform.localScale *= tileSize;
                        dirOnTile[i, j].name = "1dir" + (i - 1).ToString() + "_" + (j - 1).ToString();
                        if (val == endTile)
                        {
                            end = Instantiate(endPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                            end.transform.localScale *= tileSize;
                            end.transform.position = GetScreenPointFromLevelIndices(i, j, -0.1f);
                        }
                        else
                        {

                            if (val == playerTile)
                            {
                                player = Instantiate(playerPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                player.transform.localScale *= tileSize;
                                player.transform.position = GetScreenPointFromLevelIndices(i, j, -0.02f);
                            }
                            else if (val == bombsTile)
                            {
                                bomb = Instantiate(bombsPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                bomb.transform.localScale *= tileSize;
                                bomb.transform.position = GetScreenPointFromLevelIndices(i, j, -0.02f);
                                bombs.Add(bomb);
                            }

                            else if (val == energyTile)
                            {
                                energy = Instantiate(energyPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                energy.transform.localScale *= tileSize;
                                energy.transform.position = GetScreenPointFromLevelIndices(i, j, -0.02f);
                                energies.Add(energy);
                            }
                        }
                    }
                    else
                    {
                        tile = Instantiate(wallPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                        tile.transform.localScale *= tileSize;
                        tile.transform.position = GetScreenPointFromLevelIndices(i, j, 0);
                    }
                }
            }
        }

        private Vector3 GetScreenPointFromLevelIndices(int row, int col, float z)
        {
            //converting indices to position values, col determines x & row determine y
            return new Vector3((col) * tileSize - _middleOffset.x, (level.height - row) * -tileSize + _middleOffset.y, z);
        }
        #endregion

    }

}
