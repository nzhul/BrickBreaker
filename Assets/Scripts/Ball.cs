using UnityEngine;
using System.Collections;
using System;

public class Ball : MonoBehaviour
{
	public bool isFireball;
	private CircleCollider2D circleCollider;

	public float scale = .7f;
	public ParticleSystem fireBallEffectCore;
	public SpriteRenderer sr;

	public event Action OnFireBallEnable;
	public event Action OnFireBallDisable;

	private void Start()
	{
		this.circleCollider = GetComponent<CircleCollider2D>();
		this.ToggleFireBall();
	}

	private void ToggleFireBall()
	{
		if (this.isFireball)
		{
			this.isFireball = false;
			this.sr.enabled = true;
			fireBallEffectCore.gameObject.SetActive(false);

			if (this.OnFireBallDisable != null)
			{
				this.OnFireBallDisable();
			}
		}
		else
		{
			this.isFireball = true;
			this.sr.enabled = false;
			fireBallEffectCore.gameObject.SetActive(true);

			if (this.OnFireBallEnable != null)
			{
				this.OnFireBallEnable();
			}
		}
	}
}