using System;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    private SpriteRenderer sr;
    private LevelManager levelManager;
    private CollectablesManager collectablesManager;
    private BoxCollider2D boxCollider;
    private Ball theBall;

    public int BrickIndex;
    public int HitPoints = 1;
    public ParticleSystem DestroyEffect;

    // Events
    public event Action<int> OnBrickDestruction;

    private void Awake()
    {
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.theBall = FindObjectOfType<Ball>();
        Ball.OnFireBallEnable += Ball_OnFireBallEnable;
        Ball.OnFireBallDisable += Ball_OnFireBallDisable;
        this.OnBrickDestruction += Brick_OnBrickDestruction;
    }

    // Use this for initialization
    private void Start()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
        this.levelManager = FindObjectOfType<LevelManager>();
        this.collectablesManager = FindObjectOfType<CollectablesManager>();
    }

    // Yes, we subscribe on our own event, why not :)
    private void Brick_OnBrickDestruction(int obj)
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float debuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alreadySpawned = false;

        if (buffSpawnChance <= this.collectablesManager.BuffChance)
        {
            alreadySpawned = true;
            Collectable newBuff = this.SpawnCollectable(true);
        }

        if (debuffSpawnChance <= this.collectablesManager.DebuffChance && !alreadySpawned)
        {
            Collectable newDebuff = this.SpawnCollectable(false);
        }
    }

    private Collectable SpawnCollectable(bool isBuff)
    {
        List<Collectable> collection;

        if (isBuff)
        {
            collection = this.collectablesManager.AvailableBuffs;
        }
        else
        {
            collection = this.collectablesManager.AvailableDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab, this.transform.position, Quaternion.identity) as Collectable;

        return newCollectable;
    }

    private void Ball_OnFireBallDisable(Ball obj)
    {
        if (this != null)
        {
            this.boxCollider.isTrigger = false;
        }
    }

    private void Ball_OnFireBallEnable(Ball obj)
    {
        if (this != null)
        {
            this.boxCollider.isTrigger = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        this.BallCollisionLogic(ball);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            this.BallCollisionLogic(ball);
        }
    }


    private void BallCollisionLogic(Ball theBall)
    {

        this.HitPoints--;

        if (this.HitPoints == 0 || theBall.isFireball)
        {
            if (this.OnBrickDestruction != null)
            {
                this.OnBrickDestruction(this.BrickIndex);
            }

            // Spawn Particle effect and destroy it after 4 seconds
            GameObject newEffect = Instantiate(DestroyEffect.gameObject,
                new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z - 0.2f), Quaternion.identity) as GameObject;
            ParticleSystem ps = newEffect.GetComponent<ParticleSystem>();
            ps.startColor = this.sr.color;
            Destroy(newEffect, DestroyEffect.startLifetime);
            Destroy(this.gameObject);
        }
        else
        {
            this.sr.sprite = this.levelManager.Sprites[this.HitPoints - 1];
        }
    }

}
