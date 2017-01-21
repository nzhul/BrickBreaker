using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelManager : MonoBehaviour {

	private float maxColumns;
	private float maxRows;
	private int maxColCount = 12;
	private int maxRowCount = 17;
	private float initialBrickSpawnPositionX = -1.96f;
	private float initialBrickSpawnPositionY = 3.325f;
	private float shiftAmmount = 0.365f;
	private List<int[,]> LevelsData;
	public List<Brick> RemainingBricks;
	private Ball theBall;

	// Settings
	public int currentLevel = 0;
	public Color32[] BrickColors;
	public Sprite[] Sprites;
	public Brick brickPrefab;

	private void Awake()
	{
		// Game initialization logic
		Screen.SetResolution(540, 960, false);
		this.RemainingBricks = new List<Brick>();
		this.LevelsData = LoadLevelsData();
		this.GenerateBricks();
	}

	void Start () {


	}

	private void GenerateBricks()
	{
		int[,] currentLevelData = this.LevelsData[currentLevel];
		float currentSpawnX = initialBrickSpawnPositionX;
		float currentSpawnY = initialBrickSpawnPositionY;
		float zShift = 0.0001f;
		int brickNumber = 1;

		for (int row = 0; row < this.maxRowCount; row++)
		{
			for (int col = 0; col < this.maxColCount; col++)
			{
				int brickType = currentLevelData[row, col];

				if (brickType > 0)
				{
					Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
					SpriteRenderer sr = newBrick.GetComponent<SpriteRenderer>();
					sr.sprite = this.Sprites[brickType - 1];
					sr.color = this.BrickColors[brickType];
					newBrick.HitPoints = brickType;
					newBrick.BrickIndex = brickNumber;
					newBrick.OnBrickDestruction += NewBrick_OnBrickDestruction;

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
	}

	private void NewBrick_OnBrickDestruction(int index)
	{
		Brick brickToRemove = this.RemainingBricks.Where(b => b.BrickIndex == index).FirstOrDefault();
		this.RemainingBricks.Remove(brickToRemove);
	}

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
