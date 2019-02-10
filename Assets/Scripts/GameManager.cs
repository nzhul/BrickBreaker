using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;

        public static GameManager Instance => _instance;

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
        #endregion

        public GameObject gameOverScreen;

        public GameObject victoryScreen;

        public int AvailibleLives = 3;

        public int Lives { get; set; }

        public bool IsGameStarted { get; set; }

        public event Action<int> OnLiveLost;

        private void Start()
        {
            this.Lives = this.AvailibleLives;
            Ball.OnBallDeath += OnBallDeath;
            Brick.OnBrickDestruction += OnBrickDestruction;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ShowVictoryScreen()
        {
            victoryScreen.SetActive(true);
        }

        private void OnBrickDestruction(Brick obj)
        {
            if (BricksManager.Instance.RemainingBricks.Count <= 0)
            {
                BallsManager.Instance.ResetBalls();
                GameManager.Instance.IsGameStarted = false;
                BricksManager.Instance.LoadNextLevel();
            }
        }

        private void OnBallDeath(Ball obj)
        {
            if (BallsManager.Instance.Balls.Count <= 0)
            {
                this.Lives--;

                if (this.Lives < 1)
                {
                    gameOverScreen.SetActive(true);
                }
                else
                {
                    OnLiveLost?.Invoke(this.Lives);
                    BallsManager.Instance.ResetBalls();
                    GameManager.Instance.IsGameStarted = false;
                    BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
                }
            }
        }

        private void OnDisable()
        {
            Ball.OnBallDeath -= OnBallDeath;
        }
    }
}
