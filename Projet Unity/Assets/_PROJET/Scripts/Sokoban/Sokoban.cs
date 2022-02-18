using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGSauce.Games.IaEsgi.Sokoban
{

    
    public class Sokoban : MonoBehaviour
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

        public int wallTile;
        public int groundTile;
        public int objectifTile;
        public int playerTile;
        public int caisseTile;
        public int playerOnDestinationTile;
        public int caisseOnDestinationTile;

        public KeyCode[] userInputKeys;

        public float tileSize;

        private int rows, cols;
        int[,] levelData;

        Vector2 middleOffset = new Vector2();

        GameObject player;

        Dictionary<GameObject, Vector2> occupants;

        int caisseCount;

        bool gameOver;

        // Start is called before the first frame update
        void Start()
        {
            gameOver = false;
            caisseCount = 0;
            occupants = new Dictionary<GameObject, Vector2>();
            cam.orthographicSize = level.height + 2;
            ParseLevel();
            CreateLevel();
        }

        // Update is called once per frame
        void Update()
        {
            if (gameOver) return;
            ApplyUserInput();
        }

        void ParseLevel()
        {
            rows = level.height + 2;
            cols = level.width + 2;
            levelData = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if(i == 0 || j == 0 || i == rows - 1 || j == cols - 1)
                        levelData[i, j] = wallTile;
                    else
                        levelData[i, j] = groundTile;
                }
            }

            foreach(Coords coords in level.Murs)
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

        private void ApplyUserInput()
        {
            if (Input.GetKeyDown(userInputKeys[0]))
            {
                TryMoveHero(0);//up
            }
            else if (Input.GetKeyDown(userInputKeys[1]))
            {
                TryMoveHero(1);//right
            }
            else if (Input.GetKeyDown(userInputKeys[2]))
            {
                TryMoveHero(2);//down
            }
            else if (Input.GetKeyDown(userInputKeys[3]))
            {
                TryMoveHero(3);//left
            }
        }

        private void CreateLevel()
        {
           
            middleOffset.x = cols * tileSize * 0.5f - tileSize * 0.5f;
            middleOffset.y = rows * tileSize * 0.5f - tileSize * 0.5f;
            GameObject tile;
            GameObject caisse;
            GameObject objectif;
            int destinationCount = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int val = levelData[i, j];

                    if (val != wallTile)
                    {
                        tile = Instantiate(groundPrefabs , new Vector3(0, 0, 0), Quaternion.identity);
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
                                player= Instantiate(playerPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                player.transform.localScale *= tileSize;
                                player.transform.position = GetScreenPointFromLevelIndices(i, j, -0.2f);
                                occupants.Add(player, new Vector2(i, j));
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

        Vector3 GetScreenPointFromLevelIndices(int row, int col, float z)
        {
            return new Vector3(col * tileSize - middleOffset.x, (level.height - row) * -tileSize + middleOffset.y, z);
        }

        private void TryMoveHero(int direction)
        {
            Vector2 heroPos;
            Vector2 oldHeroPos;
            Vector2 nextPos;
            occupants.TryGetValue(player, out oldHeroPos);
            heroPos = GetNextPositionAlong(oldHeroPos, direction);

            if (IsValidPosition(heroPos))
            {
                if (!IsOccuppied(heroPos))
                {
                    RemoveOccuppant(oldHeroPos);
                    player.transform.position = GetScreenPointFromLevelIndices((int)heroPos.x, (int)heroPos.y, -0.2f);
                    occupants[player] = heroPos;
                    if (levelData[(int)heroPos.x, (int)heroPos.y] == groundTile)
                    {
                        levelData[(int)heroPos.x, (int)heroPos.y] = playerTile;
                    }
                    else if (levelData[(int)heroPos.x, (int)heroPos.y] == objectifTile)
                    {
                        levelData[(int)heroPos.x, (int)heroPos.y] = playerOnDestinationTile;
                    }
                }
                else
                {
                    nextPos = GetNextPositionAlong(heroPos, direction);
                    if (IsValidPosition(nextPos))
                    {
                        if (!IsOccuppied(nextPos))
                        {
                            GameObject ball = GetOccupantAtPosition(heroPos);
                            if (ball == null) Debug.Log("no ball");
                            RemoveOccuppant(heroPos);
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
                            RemoveOccuppant(oldHeroPos);
                            player.transform.position = GetScreenPointFromLevelIndices((int)heroPos.x, (int)heroPos.y, -0.2f);
                            occupants[player] = heroPos;
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
                CheckCompletion();
            }
        }

        private bool IsOccuppied(Vector2 objPos)
        {
            return (levelData[(int)objPos.x, (int)objPos.y] == caisseTile || levelData[(int)objPos.x, (int)objPos.y] == caisseOnDestinationTile);
        }

        private bool IsValidPosition(Vector2 objPos)
        {
            if (objPos.x > -1 && objPos.x < rows && objPos.y > -1 && objPos.y < cols)
            {
                return levelData[(int)objPos.x, (int)objPos.y] != wallTile;
            }
            else return false;
        }

        private GameObject GetOccupantAtPosition(Vector2 heroPos)
        {
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
                levelData[(int)objPos.x, (int)objPos.y] = groundTile;
            }
            else if (levelData[(int)objPos.x, (int)objPos.y] == playerOnDestinationTile)
            {
                levelData[(int)objPos.x, (int)objPos.y] = objectifTile;
            }
            else if (levelData[(int)objPos.x, (int)objPos.y] == caisseOnDestinationTile)
            {
                levelData[(int)objPos.x, (int)objPos.y] = objectifTile;
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

        private void CheckCompletion()
        {
            int ballsOnDestination = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
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

    }


}
