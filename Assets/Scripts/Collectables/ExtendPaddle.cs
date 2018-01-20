using UnityEngine;
using System.Collections;
using System;

public class ExtendPaddle : Collectable
{
	public float NewWidth = 200;

	protected override void ApplyEffect()
	{
		Paddle thePaddle = FindObjectOfType<Paddle>();
		if (thePaddle != null && !thePaddle.PaddleIsTransforming)
		{
			thePaddle.StartWidthAnimation(NewWidth);
		}
	}
}
