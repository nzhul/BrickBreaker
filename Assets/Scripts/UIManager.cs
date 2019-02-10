using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public Text Target;
        public Text ScoreText;
        public Text LivesText;

        public int Score { get; set; }

        private void Start()
        {
            Brick.OnBrickDestruction += OnBrickDestruction;
            BricksManager.Instance.OnLevelLoaded += Instance_OnLevelLoaded;
            GameManager.Instance.OnLiveLost += OnLiveLost;
            OnLiveLost(GameManager.Instance.AvailibleLives);
        }

        private void OnLiveLost(int remainingLives)
        {
            LivesText.text = $"LIVES: {remainingLives}";
        }

        private void Instance_OnLevelLoaded()
        {
            UpdateRemainingBricksText();
            UpdateScoreText(0);
        }

        private void OnBrickDestruction(Brick brick)
        {
            UpdateRemainingBricksText();
            UpdateScoreText(10);
        }

        private void UpdateScoreText(int increment)
        {
            this.Score += increment;
            string scoreString = this.Score.ToString().PadLeft(5, '0');
            ScoreText.text = $@"SCORE:
{scoreString}";
        }

        private void UpdateRemainingBricksText()
        {
            Target.text = $@"Target:
{BricksManager.Instance.RemainingBricks.Count} / {BricksManager.Instance.InitialBricksCount}";
        }

        private void OnDisable()
        {
            Brick.OnBrickDestruction -= OnBrickDestruction;
            BricksManager.Instance.OnLevelLoaded -= Instance_OnLevelLoaded;
        }
    }
}
