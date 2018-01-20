using System;
using UnityEngine;

public class Shrink : Collectable
{
	public float NewWidth = 80;

	protected override void ApplyEffect()
	{
		Paddle thePaddle = FindObjectOfType<Paddle>();
		if (thePaddle != null && !thePaddle.PaddleIsTransforming)
		{
			thePaddle.StartWidthAnimation(NewWidth);
		}
	}
}