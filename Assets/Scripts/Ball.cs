using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	private Rigidbody2D rb;

	// Use this for initialization
	void Start()
	{
		rb = this.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
