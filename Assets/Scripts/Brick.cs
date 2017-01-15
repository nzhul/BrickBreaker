using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public int HitPoints = 1;
	public ParticleSystem DestroyEffect;
	private SpriteRenderer sr;
	private Ball theBall;

	// Use this for initialization
	void Start () {
		this.sr = this.GetComponent<SpriteRenderer>();
		this.theBall = FindObjectOfType<Ball>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		this.HitPoints--;
		if (this.HitPoints == 0)
		{
			// Spawn Particle effect and destroy it after 4 seconds
			GameObject newEffect = Instantiate(DestroyEffect.gameObject, this.gameObject.transform.position, Quaternion.identity) as GameObject;
			ParticleSystem ps = newEffect.GetComponent<ParticleSystem>();
			ps.startColor = this.sr.color;
			Destroy(newEffect, DestroyEffect.startLifetime);
			Destroy(this.gameObject);
		}
	}

}
