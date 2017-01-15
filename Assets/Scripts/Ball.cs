using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	Rigidbody2D rb;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// When the ball is near the walls and collide with the paddle.
		// Add force to the oposite side of the wall
		// so it can get out of the wall loop
	}
}
