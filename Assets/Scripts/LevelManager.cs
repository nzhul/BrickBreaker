using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    private static LevelManager _instance;

    public static LevelManager Instance => _instance;
    #endregion

    private float maxColumns;
    private float maxRows;
    private int maxColCount = 12;
    private int maxRowCount = 17;
    private float initialBrickSpawnPositionX = -1.96f;
    private float initialBrickSpawnPositionY = 3.325f;
    private float shiftAmmount = 0.365f;
    private List<int[,]> LevelsData;
    public List<Brick> RemainingBricks { get; set; }
    public int InitialBricksCount { get; set; }
    private Ball theBall;

    // Settings
    public int CurrentLevel = 0;
    public Color32[] BrickColors;
    public Sprite[] Sprites;
    public Brick brickPrefab;

    private GameObject bricksContainer;

    public event Action OnLevelLoaded;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        bricksContainer = new GameObject("BricksContainer");
        Screen.SetResolution(540, 960, false);
        this.RemainingBricks = new List<Brick>();
        this.LevelsData = LoadLevelsData();
        this.GenerateBricks();
        this.OnLevelLoaded?.Invoke();
    }

    public void LoadNextLevel()
    {
        this.CurrentLevel++;

        if (this.CurrentLevel >= this.LevelsData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
        }
    }

    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.RemainingBricks = new List<Brick>();
        this.GenerateBricks();
        this.OnLevelLoaded?.Invoke();
    }

    private void ClearRemainingBricks()
    {
        foreach (Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    private void GenerateBricks()
    {
        int[,] currentLevelData = this.LevelsData[CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0f;
        int brickNumber = 1;

        for (int row = 0; row < this.maxRowCount; row++)
        {
            for (int col = 0; col < this.maxColCount; col++)
            {
                int brickType = currentLevelData[row, col];

                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.transform.SetParent(bricksContainer.transform);
                    SpriteRenderer sr = newBrick.GetComponent<SpriteRenderer>();
                    sr.sprite = this.Sprites[brickType - 1];
                    sr.color = this.BrickColors[brickType];
                    newBrick.HitPoints = brickType;
                    newBrick.BrickIndex = brickNumber;
                    //newBrick.OnBrickDestruction += NewBrick_OnBrickDestruction;

                    this.RemainingBricks.Add(newBrick);
                    brickNumber++;
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmmount;
                if (col + 1 == this.maxColCount)
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmmount;
        }

        this.InitialBricksCount = this.RemainingBricks.Count;
    }

    //private void OnBrickDestruction(Brick brick)
    //{
    //    Brick brickToRemove = this.RemainingBricks.Where(b => b.BrickIndex == brick.BrickIndex).FirstOrDefault();
    //    this.RemainingBricks.Remove(brickToRemove);
    //}

    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRowCount, maxColCount];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                // end of current level
                // add the matrix to the list and continue the loop
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRowCount, maxColCount];
            }
        }

        return levelsData;
    }
}
