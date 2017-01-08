using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{
	private Camera mainCamera;
	private float paddleInitialY;
	private Ball ball;
	private RectTransform rectTransform;
	private BoxCollider2D boxCol;
	private bool IsGameStarted;

	private static float defaultPaddleWidth = 200;
	private static float defaultLeftClamp = 150;
	private static float defaultRightClamp = 400;

	public float paddleWidth = defaultPaddleWidth;
	public float paddleHeight = 27;

	// Use this for initialization
	void Start()
	{
		mainCamera = FindObjectOfType<Camera>();
		paddleInitialY = this.transform.position.y;
		ball = FindObjectOfType<Ball>();
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
				Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
				ballRb.isKinematic = false;
				ballRb.AddForce(new Vector2(2,5), ForceMode2D.Impulse); // TODO: Calculate the initial force based on user mouse position
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
}
