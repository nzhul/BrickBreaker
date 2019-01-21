using System.Collections;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton
    private static Paddle _instance;

    public static Paddle Instance => _instance;

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

    private Camera mainCamera;
    private float paddleInitialY;
    private Ball ball;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    [HideInInspector]
    public bool PaddleIsTransforming;

    private static float defaultPaddleWidthInPixels = 200;
    private static float defaultLeftClamp = 150;
    private static float defaultRightClamp = 400;

    public float paddleWidth = 2;
    public float paddleHeight = 0.28f;
    public float initialBallSpeed = 250;
    public float extendShrinkDuration = 10;

    // Use this for initialization
    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        //float paddleWidthShift = defaultPaddleWidth - 188; // 188 is the current width of the paddle in pixels. before was this.recTransform.size.x
        float paddleWidthShift = defaultPaddleWidthInPixels - (94 * this.sr.size.x);
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
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector2 hitPoint = coll.contacts[0].point;
            Vector2 paddleCenter = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;

            // Find a way to add stronger force when you have to push to ball back
            // consider removing rigidbody and use manual movement.
            if (hitPoint.x < paddleCenter.x)
            {
                // hit is to the left side
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), initialBallSpeed)); // difference * 143 = ~100 force at maximum
            }
            else
            {
                // hit is to the right side
                ballRb.AddForce(new Vector2(Mathf.Abs(difference * 200), initialBallSpeed)); // difference * 143 = ~100 force at maximum
            }
        }
    }

    public void StartWidthAnimation(float width)
    {
        StartCoroutine(AnimatePaddleWidth(width));
    }

    // TODO: Fix Code duplication
    public IEnumerator AnimatePaddleWidth(float width)
    {
        this.PaddleIsTransforming = true;
        this.StartCoroutine(ResetPaddleWidthAfterTime(this.extendShrinkDuration));

        if (width > this.sr.size.x)
        {
            float currentWidth = this.sr.size.x;
            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.sr.size.x;
            while (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }

        this.PaddleIsTransforming = false;
    }

    private IEnumerator ResetPaddleWidthAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.StartWidthAnimation(this.paddleWidth);
    }
}
