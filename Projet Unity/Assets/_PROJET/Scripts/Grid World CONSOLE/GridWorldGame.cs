using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.Games.IaEsgi.Ia;
using UnityEngine;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class GridWorldGame : MonoRl<GridWorldGame, QAgentGridWorldConsole, QStateGridWorldConsole>
    {
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
        
        public float tileSize = 2;
        
        public GameObject noDirPrefabs;
        public GameObject upDirPrefabs;
        public GameObject rightDirPrefabs;
        public GameObject leftDirPrefabs;
        public GameObject downDirPrefabs;
        
        
        private int _rows, _cols;
        private int[,] _levelData;
        private Vector2 _middleOffset = new Vector2();
        private GameObject[,] _dirOnTile;
        private HashSet<GameObject> _bombs;
        private HashSet<GameObject> _energies;
        private GameObject _player;
        private HashSet<Coords> _bombsCoords;
        private HashSet<Coords> _energiesCoords;

        public GridWorldConsoleData Level => level;

        public HashSet<Coords> EnergiesCoords
        {
            get => _energiesCoords;
            set => _energiesCoords = value;
        }

        public HashSet<Coords> BombsCoords
        {
            get => _bombsCoords;
            set => _bombsCoords = value;
        }

        public GameObject Player => _player;

        protected override void GameCustomUpdate()
        {
            foreach (var state in Algorithm.States)
            {
                var action = Algorithm.GetBestAction(state);
                Destroy(_dirOnTile[state.Coords.y + 1, state.Coords.x + 1]);

                GameObject chosenPrefabs;
                switch (action.Name)
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

                _dirOnTile[state.Coords.y + 1, state.Coords.x + 1] = Instantiate(chosenPrefabs, new Vector3(0, 0, -0.03f), chosenPrefabs.transform.rotation);
                _dirOnTile[state.Coords.y + 1, state.Coords.x + 1].transform.position = GetScreenPointFromLevelIndices(state.Coords.y + 1, state.Coords.x + 1, -0.04f);
                _dirOnTile[state.Coords.y + 1, state.Coords.x + 1].transform.localScale *= tileSize;
                _dirOnTile[state.Coords.y + 1, state.Coords.x + 1].name = "2dir" + (state.Coords.y) + "_" + (state.Coords.x);

            }
            

            foreach (var go in _energies)
            {
                Destroy(go);    
            }

            foreach (var coords in EnergiesCoords)
            {
                var energy = Instantiate(energyPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                energy.transform.localScale *= tileSize;
                energy.transform.position = GetScreenPointFromLevelIndices(coords.y + 1, coords.x + 1, -0.02f);
                _energies.Add(energy);
            }

            foreach (var go in _bombs)
            {
                Destroy(go);
            }

            foreach (var coords in BombsCoords)
            {
                var bomb = Instantiate(bombsPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                bomb.transform.localScale *= tileSize;
                bomb.transform.position = GetScreenPointFromLevelIndices(coords.y + 1, coords.x + 1, -0.02f);
                _bombs.Add(bomb);
            }
        }

        public override void ResetGameForNewEpoch()
        {
            EnergiesCoords = new HashSet<Coords>(Level.energy);
            BombsCoords = new HashSet<Coords>(Level.bombs);
        }

        protected override void GameCustomInit()
        {
            EnergiesCoords = new HashSet<Coords>(Level.energy);
            BombsCoords = new HashSet<Coords>(Level.bombs);
            ParseLevel();
            CreateLevel();
        }
        
        private void ParseLevel()
        {
            cam.orthographicSize = Level.height + 2;
            _rows = Level.height + 2;
            _cols = Level.width + 2;
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

            foreach (Coords coords in Level.bombs)
            {
                _levelData[coords.y + 1, coords.x + 1] = bombsTile;
            }

            foreach (Coords coords in Level.energy)
            {
                _levelData[coords.y + 1, coords.x + 1] = energyTile;
            }

            _levelData[Level.start.y + 1, Level.start.x + 1] = playerTile;
            _levelData[Level.end.y + 1, Level.end.x + 1] = endTile;

        }

        private void CreateLevel()
        {
            _middleOffset.x = _cols * tileSize * 0.5f - tileSize * 0.5f;
            _middleOffset.y = _rows * tileSize * 0.5f - tileSize * 0.5f;
            GameObject tile;
            GameObject energy;
            GameObject bomb;
            GameObject end;

            _dirOnTile = new GameObject[_rows, _cols];
            _bombs = new HashSet<GameObject>();
            _energies = new HashSet<GameObject>();

            int destinationCount = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    int val = _levelData[i, j];

                    if (val != wallTile)
                    {
                        tile = Instantiate(groundPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                        _dirOnTile[i, j] = Instantiate(noDirPrefabs, new Vector3(0, 0, -0.03f), Quaternion.identity);

                        tile.name = "tile" + (i - 1).ToString() + "_" + (j - 1).ToString();
                        tile.transform.localScale *= tileSize;
                        tile.transform.position = GetScreenPointFromLevelIndices(i, j, 0);

                        _dirOnTile[i, j].transform.position = GetScreenPointFromLevelIndices(i, j, -0.03f);
                        _dirOnTile[i, j].transform.localScale *= tileSize;
                        _dirOnTile[i, j].name = "1dir" + (i - 1).ToString() + "_" + (j - 1).ToString();
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
                                _player = Instantiate(playerPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                Player.transform.localScale *= tileSize;
                                Player.transform.position = GetScreenPointFromLevelIndices(i, j, -0.02f);
                            }
                            else if (val == bombsTile)
                            {
                                bomb = Instantiate(bombsPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                bomb.transform.localScale *= tileSize;
                                bomb.transform.position = GetScreenPointFromLevelIndices(i, j, -0.02f);
                                _bombs.Add(bomb);
                            }

                            else if (val == energyTile)
                            {
                                energy = Instantiate(energyPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                energy.transform.localScale *= tileSize;
                                energy.transform.position = GetScreenPointFromLevelIndices(i, j, -0.02f);
                                _energies.Add(energy);
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

        public Vector3 GetScreenPointFromLevelIndices(int row, int col, float z)
        {
            //converting indices to position values, col determines x & row determine y
            return new Vector3((col) * tileSize - _middleOffset.x, (Level.height - row) * -tileSize + _middleOffset.y, z);
        }

        public float GetTileValue(Coords coords)
        {
            var factor = coords.Equals(Agent.OldState.Coords) ? 2 : 1;
            if (BombsCoords.Contains(coords))
            {
                PGDebug.Message($"{coords} is bomb").Log();
                BombsCoords.Remove(coords);
                return Level.bombValue * factor;
            }

            if (EnergiesCoords.Contains(coords))
            {
                PGDebug.Message($"{coords} is energy").Log();
                EnergiesCoords.Remove(coords);
                return Level.energyValue;
            }

            if (Level.end.Equals(coords))
            {
                PGDebug.Message($"{coords} is end").Log();
                return Level.endValue;
            }

            PGDebug.Message($"{coords} is blank").Log();
            return Level.blankValue * factor;
        }
    }
}