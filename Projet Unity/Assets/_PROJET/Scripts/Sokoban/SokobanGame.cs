using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.Sokoban
{
    public class SokobanGame : MonoRl<SokobanGame, QAgentSokoban, QStateSokoban>
    {

        [SerializeField]
        private Camera cam;

        [SerializeField]
        private SokobanData level;

        public Sprite tileSprite;
        public Sprite heroSprite;
        public Sprite ballSprite;

        public Color objectifColor;
        public Color WallColor;

        public GameObject groundPrefabs;
        public GameObject playerPrefabs;
        public GameObject caissePrefabs;
        public GameObject objectifPrefabs;
        public GameObject wallPrefabs;

        public GameObject noDirPrefabs;
        public GameObject upDirPrefabs;
        public GameObject rightDirPrefabs;
        public GameObject leftDirPrefabs;
        public GameObject downDirPrefabs;

        public int wallTile;
        public int groundTile;
        public int objectifTile;
        public int playerTile;
        public int caisseTile;
        public int playerOnDestinationTile;
        public int caisseOnDestinationTile;

        public KeyCode[] userInputKeys;

        public float tileSize;

        private int _rows, _cols;
        int[,] levelData;

        Vector2 middleOffset = new Vector2();
        private GameObject[,] _dirOnTile;

        GameObject _player;

        Dictionary<GameObject, Vector2> occupants;

        int caisseCount;

        bool gameOver;

        public SokobanData Level => level;

        public GameObject Player => _player;
        public override void ResetGameForNewEpoch()
        {
            gameOver = false;
            caisseCount = 0;
            occupants = new Dictionary<GameObject, Vector2>();
            ParseLevel();
            CreateLevel();
        }

        protected override void GameCustomInit()
        {
            gameOver = false;
            caisseCount = 0;
            occupants = new Dictionary<GameObject, Vector2>();
            cam.orthographicSize = level.height + 2;
            ParseLevel();
            CreateLevel();
        }

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

        }

        void ParseLevel()
        {
            _rows = level.height + 2;
            _cols = level.width + 2;
            levelData = new int[_rows, _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (i == 0 || j == 0 || i == _rows - 1 || j == _cols - 1)
                        levelData[i, j] = wallTile;
                    else
                        levelData[i, j] = groundTile;
                }
            }

            foreach (Coords coords in level.Murs)
            {
                levelData[coords.y + 1, coords.x + 1] = wallTile;
            }

            foreach (Coords coords in level.Caisses)
            {
                levelData[coords.y + 1, coords.x + 1] = caisseTile;
            }

            foreach (Coords coords in level.Objectifs)
            {

                levelData[coords.y + 1, coords.x + 1] = objectifTile;
            }

            levelData[level.start.y + 1, level.start.x + 1] = playerTile;
        }

        private void CreateLevel()
        {

            middleOffset.x = _cols * tileSize * 0.5f - tileSize * 0.5f;
            middleOffset.y = _rows * tileSize * 0.5f - tileSize * 0.5f;
            GameObject tile;
            GameObject caisse;
            GameObject objectif;

            _dirOnTile = new GameObject[_rows, _cols];
            int destinationCount = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    int val = levelData[i, j];

                    if (val != wallTile)
                    {
                        tile = Instantiate(groundPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                        tile.name = "tile" + (i - 1).ToString() + "_" + (j - 1).ToString();
                        tile.transform.localScale *= tileSize;
                        tile.transform.position = GetScreenPointFromLevelIndices(i, j, 0);
                        if (val == objectifTile)
                        {
                            objectif = Instantiate(objectifPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                            objectif.transform.localScale *= tileSize;
                            objectif.transform.position = GetScreenPointFromLevelIndices(i, j, -0.1f);
                            destinationCount++;
                        }
                        else
                        {
                            if (val == playerTile)
                            {
                                _player = Instantiate(playerPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                _player.transform.localScale *= tileSize;
                                _player.transform.position = GetScreenPointFromLevelIndices(i, j, -0.2f);
                                occupants.Add(_player, new Vector2(i, j));
                            }
                            else if (val == caisseTile)
                            {
                                caisseCount++;
                                caisse = Instantiate(caissePrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                caisse.transform.localScale *= tileSize;
                                caisse.transform.position = GetScreenPointFromLevelIndices(i, j, -0.2f);
                                occupants.Add(caisse, new Vector2(i, j));
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
            if (caisseCount > destinationCount) Debug.LogError("there are more balls than destinations");
        }

        public Vector3 GetScreenPointFromLevelIndices(int row, int col, float z)
        {
            return new Vector3(col * tileSize - middleOffset.x, (level.height - row) * -tileSize + middleOffset.y, z);
        }

        public float GetTileValue(Coords coords)
        {
            var factor = coords.Equals(Agent.OldState.Coords) ? 2 : 1;
            

            PGDebug.Message($"{coords} is blank").Log();
            return Level.blankValue * factor;
        }
    }
}
