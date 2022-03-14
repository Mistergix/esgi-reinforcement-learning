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

        public int CrateCount => level.Caisses.Count;
        
        public override void ResetGameForNewEpoch()
        {
            Init();
        }

        protected override void GameCustomInit()
        {
            Init();
        }

        public void Init()
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
                Destroy(_dirOnTile[state.PlayerCoords.y + 1, state.PlayerCoords.x + 1]);

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

                _dirOnTile[state.PlayerCoords.y + 1, state.PlayerCoords.x + 1] = Instantiate(chosenPrefabs, new Vector3(0, 0, -0.03f), chosenPrefabs.transform.rotation);
                _dirOnTile[state.PlayerCoords.y + 1, state.PlayerCoords.x + 1].transform.position = GetScreenPointFromLevelIndices(state.PlayerCoords.y + 1, state.PlayerCoords.x + 1, -0.04f);
                _dirOnTile[state.PlayerCoords.y + 1, state.PlayerCoords.x + 1].transform.localScale *= tileSize;
                _dirOnTile[state.PlayerCoords.y + 1, state.PlayerCoords.x + 1].name = "2dir" + (state.PlayerCoords.y) + "_" + (state.PlayerCoords.x);

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
            var factor = coords.Equals(Agent.OldState.PlayerCoords) ? 2 : 1;
            

            PGDebug.Message($"{coords} is blank").Log();
            return Level.blankValue * factor;
        }
        
        private void TryMoveHero(int direction)
        {
            Vector2 heroPos;
            Vector2 oldHeroPos;
            Vector2 nextPos;
            occupants.TryGetValue(_player, out oldHeroPos);
            heroPos = GetNextPositionAlong(oldHeroPos, direction);//find the next array position in given direction

            if (IsValidPosition(heroPos))
            {//check if it is a valid position & falls inside the level array
                if (!IsOccuppied(heroPos))
                {//check if it is occuppied by a ball
                 //move hero
                    RemoveOccuppant(oldHeroPos);//reset old level data at old position
                    _player.transform.position = GetScreenPointFromLevelIndices((int)heroPos.x, (int)heroPos.y, -0.2f);
                    occupants[_player] = heroPos;
                    if (levelData[(int)heroPos.x, (int)heroPos.y] == groundTile)
                    {//moving onto a ground tile
                        levelData[(int)heroPos.x, (int)heroPos.y] = playerTile;
                    }
                    else if (levelData[(int)heroPos.x, (int)heroPos.y] == objectifTile)
                    {//moving onto a destination tile
                        levelData[(int)heroPos.x, (int)heroPos.y] = playerOnDestinationTile;
                    }
                }
                else
                {
                    //we have a ball next to hero, check if it is empty on the other side of the ball
                    nextPos = GetNextPositionAlong(heroPos, direction);
                    if (IsValidPosition(nextPos))
                    {
                        if (!IsOccuppied(nextPos))
                        {//we found empty neighbor, so we need to move both ball & hero
                            GameObject ball = GetOccupantAtPosition(heroPos);//find the ball at this position
                            if (ball == null) Debug.Log("no ball");
                            RemoveOccuppant(heroPos);//ball should be moved first before moving the hero
                            ball.transform.position = GetScreenPointFromLevelIndices((int)nextPos.x, (int)nextPos.y, - 0.2f);
                            occupants[ball] = nextPos;
                            if (levelData[(int)nextPos.x, (int)nextPos.y] == groundTile)
                            {
                                levelData[(int)nextPos.x, (int)nextPos.y] = caisseTile;
                            }
                            else if (levelData[(int)nextPos.x, (int)nextPos.y] == objectifTile)
                            {
                                levelData[(int)nextPos.x, (int)nextPos.y] = caisseOnDestinationTile;
                            }
                            RemoveOccuppant(oldHeroPos);//now move hero
                            _player.transform.position = GetScreenPointFromLevelIndices((int)heroPos.x, (int)heroPos.y, -0.2f);
                            occupants[_player] = heroPos;
                            if (levelData[(int)heroPos.x, (int)heroPos.y] == groundTile)
                            {
                                levelData[(int)heroPos.x, (int)heroPos.y] = playerTile;
                            }
                            else if (levelData[(int)heroPos.x, (int)heroPos.y] == objectifTile)
                            {
                                levelData[(int)heroPos.x, (int)heroPos.y] = playerOnDestinationTile;
                            }
                        }
                    }
                }
                CheckCompletion();//check if all balls have reached destinations
            }
        }
        
        private GameObject GetOccupantAtPosition(Vector2 heroPos)
        {//loop through the occupants to find the ball at given position
            GameObject ball;
            foreach (KeyValuePair<GameObject, Vector2> pair in occupants)
            {
                if (pair.Value == heroPos)
                {
                    ball = pair.Key;
                    return ball;
                }
            }
            return null;
        }
        
        private void RemoveOccuppant(Vector2 objPos)
        {
            if (levelData[(int)objPos.x, (int)objPos.y] == playerTile || levelData[(int)objPos.x, (int)objPos.y] == caisseTile)
            {
                levelData[(int)objPos.x, (int)objPos.y] = groundTile;//ball moving from ground tile
            }
            else if (levelData[(int)objPos.x, (int)objPos.y] == playerOnDestinationTile)
            {
                levelData[(int)objPos.x, (int)objPos.y] = objectifTile;//hero moving from destination tile
            }
            else if (levelData[(int)objPos.x, (int)objPos.y] == caisseOnDestinationTile)
            {
                levelData[(int)objPos.x, (int)objPos.y] = objectifTile;//ball moving from destination tile
            }
        }
        
        private bool IsOccuppied(Vector2 objPos)
        {//check if there is a ball at given array position
            return (levelData[(int)objPos.x, (int)objPos.y] == caisseTile || levelData[(int)objPos.x, (int)objPos.y] == caisseOnDestinationTile);
        }
        
        public bool IsValidPosition(Vector2 objPos)
        {//check if the given indices fall within the array dimensions
            if (objPos.x > -1 && objPos.x < _rows && objPos.y > -1 && objPos.y < _cols)
            {
                return levelData[(int)objPos.x, (int)objPos.y] != wallTile;
            }
            else return false;
        }
        
        private void CheckCompletion()
        {
            int ballsOnDestination = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (levelData[i, j] == caisseOnDestinationTile)
                    {
                        ballsOnDestination++;
                    }
                }
            }
            if (ballsOnDestination == caisseCount)
            {
                Debug.Log("level complete");
                gameOver = true;
            }
        }
        
        private Vector2 GetNextPositionAlong(Vector2 objPos, int direction)
        {
            switch (direction)
            {
                case 0:
                    objPos.x -= 1;//up
                    break;
                case 1:
                    objPos.y += 1;//right
                    break;
                case 2:
                    objPos.x += 1;//down
                    break;
                case 3:
                    objPos.y -= 1;//left
                    break;
            }
            return objPos;
        }
    }
}
