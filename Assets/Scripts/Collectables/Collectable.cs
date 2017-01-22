using UnityEngine;
using System.Collections;

public abstract class Collectable : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Paddle")
		{
			this.ApplyEffect();
		}

		if (collision.tag == "Paddle" || collision.tag == "DeadCollider")
		{
			// Consider instantiating small effect for collection - only on Paddle collision
			Destroy(this.gameObject);
		}
	}

	protected abstract void ApplyEffect();
}
