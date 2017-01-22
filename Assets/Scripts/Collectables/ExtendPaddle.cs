using UnityEngine;
using System.Collections;
using System;

public class ExtendPaddle : Collectable
{
	protected override void ApplyEffect()
	{
		Debug.Log("Extending Paddle");
	}
}
