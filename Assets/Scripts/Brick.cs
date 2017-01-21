using UnityEngine;
using System.Collections;
using System;

public class Brick : MonoBehaviour {

	private SpriteRenderer sr;
	private LevelManager levelManagerl;
	private BoxCollider2D boxCollider;
	private Ball theBall;

	public int BrickIndex;
	public int HitPoints = 1;
	public ParticleSystem DestroyEffect;

	// Events
	public event Action<int> OnBrickDestruction;

	private void Awake()
	{
		this.boxCollider = GetComponent<BoxCollider2D>();
		this.theBall = FindObjectOfType<Ball>();
		this.theBall.OnFireBallEnable += TheBall_OnFireBallEnable;
		this.theBall.OnFireBallDisable += TheBall_OnFireBallDisable;
	}

	// Use this for initialization
	void Start () {
		this.sr = this.GetComponent<SpriteRenderer>();
		this.levelManagerl = FindObjectOfType<LevelManager>();
	}

	private void TheBall_OnFireBallDisable()
	{
		this.boxCollider.isTrigger = false;
	}

	private void TheBall_OnFireBallEnable()
	{
		this.boxCollider.isTrigger = true;
	}

	// Update is called once per frame
	void Update () {
	
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Ball ball = collision.gameObject.GetComponent<Ball>();
		this.BallCollisionLogic(ball);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Ball ball = collision.gameObject.GetComponent<Ball>();
		this.BallCollisionLogic(ball);
	}


	private void BallCollisionLogic(Ball theBall)
	{
		
		this.HitPoints--;

		if (this.HitPoints == 0 || theBall.isFireball)
		{
			if (this.OnBrickDestruction != null)
			{
				this.OnBrickDestruction(this.BrickIndex);
			}

			// Spawn Particle effect and destroy it after 4 seconds
			GameObject newEffect = Instantiate(DestroyEffect.gameObject, this.gameObject.transform.position, Quaternion.identity) as GameObject;
			ParticleSystem ps = newEffect.GetComponent<ParticleSystem>();
			ps.startColor = this.sr.color;
			Destroy(newEffect, DestroyEffect.startLifetime);
			Destroy(this.gameObject);
		}
		else
		{
			this.sr.sprite = this.levelManagerl.Sprites[this.HitPoints - 1];
		}
	}

}
