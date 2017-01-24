using UnityEngine;
using System.Collections;
using System;

public class Ball : MonoBehaviour
{
	public bool isFireball;
	public float fireballDuration;

	public float scale = .7f;
	public ParticleSystem fireBallEffectCore;
	public SpriteRenderer sr;

	public event Action OnFireBallEnable;
	public event Action OnFireBallDisable;

	public void StartFireBall()
	{
		if (!this.isFireball)
		{
			this.isFireball = true;
			this.sr.enabled = false;
			fireBallEffectCore.gameObject.SetActive(true);
			StartCoroutine(StopFireBallAfterTime(this.fireballDuration));

			if (this.OnFireBallEnable != null)
			{
				this.OnFireBallEnable();
			}
		}
	}

	public void StopFireball()
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
	}

	private IEnumerator StopFireBallAfterTime(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		StopFireball();
	}
}