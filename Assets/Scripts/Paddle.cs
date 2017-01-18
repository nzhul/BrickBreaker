using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{
	private Camera mainCamera;
	private float paddleInitialY;
	private Ball ball;
	private Rigidbody2D ballRb;
	private RectTransform rectTransform;
	private BoxCollider2D boxCol;
	private bool IsGameStarted;

	private static float defaultPaddleWidth = 200;
	private static float defaultLeftClamp = 150;
	private static float defaultRightClamp = 400;

	public float paddleWidth = defaultPaddleWidth;
	public float paddleHeight = 27;
	public float initialBallSpeed = 250;

	// Use this for initialization
	void Start()
	{
		mainCamera = FindObjectOfType<Camera>();
		paddleInitialY = this.transform.position.y;
		ball = FindObjectOfType<Ball>();
		ballRb = ball.GetComponent<Rigidbody2D>();
		rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(paddleWidth, paddleHeight);
		boxCol = GetComponent<BoxCollider2D>();
		boxCol.size = new Vector2(paddleWidth, paddleHeight);
	}

	// Update is called once per frame
	void Update()
	{
		PaddleMovement();

		if (!IsGameStarted)
		{
			ball.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .27f, 0);
			if (Input.GetMouseButtonDown(0))
			{
				this.ballRb.isKinematic = false;
				this.ballRb.AddForce(new Vector2(0, initialBallSpeed)); // TODO: Calculate the initial force based on user mouse position
				IsGameStarted = true;
			}
		}
	}

	private void PaddleMovement()
	{
		float paddleWidthShift = defaultPaddleWidth - paddleWidth;
		float leftClamp = defaultLeftClamp - (paddleWidthShift / 2);
		float rightClamp = defaultRightClamp + (paddleWidthShift / 2);
		float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
		float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
		this.transform.position = new Vector3(mousePositionWorldX, paddleInitialY, 0);
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		// When the ball is near the walls and collide with the paddle.
		// Add force to the oposite side of the wall
		// so it can get out of the wall loop

		if (coll.gameObject.tag == "Ball")
		{
			Vector2 hitPoint = coll.contacts[0].point;
			Vector2 paddleCenter = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

			ballRb.velocity = Vector2.zero;

			float difference = paddleCenter.x - hitPoint.x;

			// Find a way to add stronger force when you have to push to ball back
			// consider removing rigidbody and use manual movement.
			if (hitPoint.x < paddleCenter.x)
			{
				// hit is to the left side
				this.ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), initialBallSpeed)); // difference * 143 = ~100 force at maximum
			}
			else
			{
				// hit is to the right side
				this.ballRb.AddForce(new Vector2(Mathf.Abs(difference * 200), initialBallSpeed)); // difference * 143 = ~100 force at maximum
			}
		}
	}
}
