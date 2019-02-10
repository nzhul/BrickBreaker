using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isFireball;
    public float fireballDuration;

    public float scale = .7f;
    public ParticleSystem fireBallEffectCore;
    private SpriteRenderer sr;

    public static event Action<Ball> OnFireBallEnable;
    public static event Action<Ball> OnFireBallDisable;
    public static event Action<Ball> OnBallDeath;

    private void Awake()
    {
        this.sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void StartFireBall()
    {
        if (!this.isFireball)
        {
            this.isFireball = true;
            this.sr.enabled = false;
            fireBallEffectCore.gameObject.SetActive(true);
            StartCoroutine(StopFireBallAfterTime(this.fireballDuration));

            OnFireBallEnable?.Invoke(this);
        }
    }

    public void StopFireball()
    {
        if (this.isFireball)
        {
            this.isFireball = false;
            this.sr.enabled = true;
            fireBallEffectCore.gameObject.SetActive(false);

            OnFireBallDisable?.Invoke(this);
        }
    }

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    private IEnumerator StopFireBallAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        StopFireball();
    }
}