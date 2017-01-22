using UnityEngine;
using System.Collections;
using System;

public class FireBall : Collectable
{
	protected override void ApplyEffect()
	{
		Ball[] allBalls = FindObjectsOfType<Ball>();

		foreach (Ball ball in allBalls)
		{
			ball.ToggleFireBall();
		}
	}
}
