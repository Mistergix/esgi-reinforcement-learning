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
            cam.orthographicSize = level.height;
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
            rows = level.height;
            cols = level.width;
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
                levelData[coords.x, coords.y] = wallTile;
            }

            foreach (Coords coords in level.Caisses)
            {
                levelData[coords.x, coords.y] = caisseTile;
            }

            foreach (Coords coords in level.Objectifs)
            {
                levelData[coords.x, coords.y] = objectifTile;
            }

            levelData[level.start.x, level.start.y] = playerTile;
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

        void CreateLevel()
        {
            //calculate the offset to align whole level to scene middle
            middleOffset.x = cols * tileSize * 0.5f - tileSize * 0.5f;
            middleOffset.y = rows * tileSize * 0.5f - tileSize * 0.5f;
            GameObject tile;
            SpriteRenderer sr;
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
                        //tile = new GameObject("tile" + i.ToString() + "_" + j.ToString());//create new tile
                        tile = Instantiate(groundPrefabs , new Vector3(0, 0, 0), Quaternion.identity);
                        tile.name = "tile" + i.ToString() + "_" + j.ToString();
                        tile.transform.localScale *= tileSize;//set tile size
                        //sr = tile.AddComponent<SpriteRenderer>();//add a sprite renderer
                        //sr.sprite = tileSprite;//assign tile sprite
                        tile.transform.position = GetScreenPointFromLevelIndices(i, j, 0);//place in scene based on level indices
                        if (val == objectifTile)
                        {//if it is a destination tile, give different color
                            objectif = Instantiate(objectifPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                            objectif.transform.localScale *= tileSize;
                            objectif.transform.position = GetScreenPointFromLevelIndices(i, j, -0.1f);
                            //sr.color = objectifColor;
                            destinationCount++;//count destinations
                        }
                        else
                        {
                            if (val == playerTile)
                            {//the hero tile
                                //player = new GameObject("hero");
                                player= Instantiate(playerPrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                player.transform.localScale *= tileSize;
                                //sr = player.AddComponent<SpriteRenderer>();
                                //sr.sprite = heroSprite;
                                //sr.sortingOrder = 1;//hero needs to be over the ground tile
                                //sr.color = Color.red;
                                player.transform.position = GetScreenPointFromLevelIndices(i, j, -0.2f);
                                occupants.Add(player, new Vector2(i, j));//store the level indices of hero in dict
                            }
                            else if (val == caisseTile)
                            {//ball tile
                                caisseCount++;//increment number of balls in level
                                //ball = new GameObject("ball" + ballCount.ToString());
                                caisse = Instantiate(caissePrefabs, new Vector3(0, 0, -0.01f), Quaternion.identity);
                                caisse.transform.localScale *= tileSize;
                                //sr = caisse.AddComponent<SpriteRenderer>();
                                //sr.sprite = ballSprite;
                                //sr.sortingOrder = 1;//ball needs to be over the ground tile
                                //sr.color = Color.black;
                                caisse.transform.position = GetScreenPointFromLevelIndices(i, j, -0.2f);
                                occupants.Add(caisse, new Vector2(i, j));//store the level indices of ball in dict
                            }
                        }
                    }
                    else
                    {
                        //tile = new GameObject("tile" + j.ToString() + "_" + i.ToString());//create new tile
                        tile = Instantiate(wallPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                        tile.transform.localScale *= tileSize;//set tile size
                        //sr = tile.AddComponent<SpriteRenderer>();//add a sprite renderer
                        //sr.sprite = tileSprite;//assign tile sprite
                        tile.transform.position = GetScreenPointFromLevelIndices(i, j, 0);
                        //sr.color = WallColor;
                    }
                }
            }
            if (caisseCount > destinationCount) Debug.LogError("there are more balls than destinations");
        }

        Vector3 GetScreenPointFromLevelIndices(int row, int col, float z)
        {
            //converting indices to position values, col determines x & row determine y
            return new Vector3(col * tileSize - middleOffset.x, row * -tileSize + middleOffset.y, z);
        }

        private void TryMoveHero(int direction)
        {
            Vector2 heroPos;
            Vector2 oldHeroPos;
            Vector2 nextPos;
            occupants.TryGetValue(player, out oldHeroPos);
            heroPos = GetNextPositionAlong(oldHeroPos, direction);//find the next array position in given direction

            if (IsValidPosition(heroPos))
            {//check if it is a valid position & falls inside the level array
                if (!IsOccuppied(heroPos))
                {//check if it is occuppied by a ball
                 //move hero
                    RemoveOccuppant(oldHeroPos);//reset old level data at old position
                    player.transform.position = GetScreenPointFromLevelIndices((int)heroPos.x, (int)heroPos.y, -0.2f);
                    occupants[player] = heroPos;
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
                CheckCompletion();//check if all balls have reached destinations
            }
        }

        private bool IsOccuppied(Vector2 objPos)
        {//check if there is a ball at given array position
            return (levelData[(int)objPos.x, (int)objPos.y] == caisseTile || levelData[(int)objPos.x, (int)objPos.y] == caisseOnDestinationTile);
        }

        private bool IsValidPosition(Vector2 objPos)
        {//check if the given indices fall within the array dimensions
            if (objPos.x > -1 && objPos.x < rows && objPos.y > -1 && objPos.y < cols)
            {
                return levelData[(int)objPos.x, (int)objPos.y] != wallTile;
            }
            else return false;
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
