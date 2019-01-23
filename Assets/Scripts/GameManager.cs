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

        private void Start()
        {
            Ball.OnBallDeath += OnBallDeath;
            Brick.OnBrickDestruction += OnBrickDestruction;
        }

        private void OnBrickDestruction(Brick obj)
        {
            if (LevelManager.Instance.RemainingBricks.Count <= 0)
            {
                BallsManager.Instance.ResetBalls();
                GameManager.Instance.IsGameStarted = false;
                LevelManager.Instance.LoadNextLevel();
            }
        }

        private void OnBallDeath(Ball obj)
        {
            if (BallsManager.Instance.Balls.Count <= 0)
            {
                BallsManager.Instance.ResetBalls();
                GameManager.Instance.IsGameStarted = false;
                LevelManager.Instance.LoadLevel(LevelManager.Instance.CurrentLevel);
            }
        }

        public bool IsGameStarted { get; set; }

        private void OnDisable()
        {
            Ball.OnBallDeath -= OnBallDeath;
        }
    }
}
