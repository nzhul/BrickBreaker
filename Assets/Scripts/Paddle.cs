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
    private readonly Ball ball;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    [HideInInspector]
    public bool PaddleIsTransforming;

    // Shooting
    public bool PaddleIsShooting { get; set; }
    public GameObject leftMuzzle;
    public GameObject rightMuzzle;
    public Projectile bulletPrefab;

    private static readonly float defaultPaddleWidthInPixels = 188;
    private static readonly float defaultLeftClamp = 135;
    private static readonly float defaultRightClamp = 410;

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
        UpdateMuzzlesPosition();
    }

    private void UpdateMuzzlesPosition()
    {
        leftMuzzle.transform.position = new Vector3(this.transform.position.x - (this.sr.size.x / 2) + 0.1f, this.transform.position.y + 0.2f, this.transform.position.z);
        rightMuzzle.transform.position = new Vector3(this.transform.position.x + (this.sr.size.x / 2) - 0.153f, this.transform.position.y + 0.2f, this.transform.position.z);
    }

    private void PaddleMovement()
    {
        //float paddleWidthShift = defaultPaddleWidth - 188; // 188 is the current width of the paddle in pixels. before was this.recTransform.size.x
        float paddleWidthShift = (defaultPaddleWidthInPixels - ((defaultPaddleWidthInPixels / 2) * this.sr.size.x)) / 2;
        float leftClamp = defaultLeftClamp - paddleWidthShift;
        float rightClamp = defaultRightClamp + paddleWidthShift;
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

    public void StartShooting()
    {
        if (!this.PaddleIsShooting)
        {
            this.PaddleIsShooting = true;
            StartCoroutine(StartShootingRoutine());
        }
    }

    public IEnumerator StartShootingRoutine()
    {
        float fireCooldown = .5f;
        float fireCooldownLeft = 0;

        float shootingDuration = 10;
        float shootingDurationLeft = shootingDuration;

        //Debug.Log("START SHOOTING");

        while (shootingDurationLeft >= 0)
        {
            fireCooldownLeft -= Time.deltaTime;
            shootingDurationLeft -= Time.deltaTime;

            if (fireCooldownLeft <= 0)
            {
                this.Shoot();
                fireCooldownLeft = fireCooldown;
                //Debug.Log($"Shoot at {Time.time}");
            }

            yield return null;
        }

        //Debug.Log("STOP SHOOTING");

        this.PaddleIsShooting = false;
        leftMuzzle.SetActive(false);
        rightMuzzle.SetActive(false);
    }

    private void Shoot()
    {
        leftMuzzle.SetActive(false);
        rightMuzzle.SetActive(false);

        leftMuzzle.SetActive(true);
        rightMuzzle.SetActive(true);

        SpawnBullet(leftMuzzle);
        SpawnBullet(rightMuzzle);
    }

    private void SpawnBullet(GameObject muzzle)
    {
        Vector3 spawnPosition = new Vector3(muzzle.transform.position.x, muzzle.transform.position.y + 0.2f, muzzle.transform.position.z);
        Projectile bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(new Vector2(0, 450f));
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
