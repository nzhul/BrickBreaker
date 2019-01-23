using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class BallsManager : MonoBehaviour
    {
        #region Singleton
        private static BallsManager _instance;

        public static BallsManager Instance => _instance;

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

        [SerializeField]
        private Ball ballPrefab;

        private Ball initialBall;

        private Rigidbody2D initialBallRb;

        public float initialBallSpeed = 250;

        public List<Ball> Balls { get; set; }

        private void Start()
        {
            InitBall();
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameStarted)
            {
                Vector3 paddlePosition = new Vector3(Paddle.Instance.gameObject.transform.position.x, Paddle.Instance.gameObject.transform.position.y + .27f, 0);
                initialBall.transform.position = paddlePosition;
                if (Input.GetMouseButtonDown(0))
                {
                    initialBallRb.isKinematic = false;
                    initialBallRb.AddForce(new Vector2(0, initialBallSpeed));
                    GameManager.Instance.IsGameStarted = true;
                }
            }
        }

        public void ResetBalls()
        {
            foreach (var ball in this.Balls.ToList())
            {
                Destroy(ball.gameObject);
            }

            InitBall();
        }

        private void InitBall()
        {
            Vector3 startingPosition = new Vector3(Paddle.Instance.gameObject.transform.position.x, Paddle.Instance.gameObject.transform.position.y + .27f, 0);
            initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
            initialBallRb = initialBall.GetComponent<Rigidbody2D>();

            this.Balls = new List<Ball>
            {
                initialBall
            };
        }

        public void SpawnBalls(Vector3 position, int count, bool isFireball)
        {
            for (int i = 0; i < count; i++)
            {
                Ball spawnedBall = Instantiate(ballPrefab, position, Quaternion.identity) as Ball;
                if (isFireball)
                {
                    spawnedBall.StartFireBall();
                }

                Rigidbody2D spawnedBallRb = spawnedBall.GetComponent<Rigidbody2D>();
                spawnedBallRb.isKinematic = false;
                spawnedBallRb.AddForce(new Vector2(0, initialBallSpeed)); // TODO: add random force at random direction!!! But prefere upside
                this.Balls.Add(spawnedBall);
            }
        }
    }
}
