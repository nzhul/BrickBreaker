using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    public int HitPoints = 1;
    public ParticleSystem DestroyEffect;

    // Events
    public static event Action<Brick> OnBrickDestruction;

    private void Awake()
    {
        this.boxCollider = GetComponent<BoxCollider2D>();
        Ball.OnFireBallEnable += Ball_OnFireBallEnable;
        Ball.OnFireBallDisable += Ball_OnFireBallDisable;
    }

    // Use this for initialization
    private void Start()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
    }

    private void OnBrickDestroy()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float debuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alreadySpawned = false;

        if (buffSpawnChance <= CollectablesManager.Instance.BuffChance)
        {
            alreadySpawned = true;
            Collectable newBuff = this.SpawnCollectable(true);
        }

        if (debuffSpawnChance <= CollectablesManager.Instance.DebuffChance && !alreadySpawned)
        {
            Collectable newDebuff = this.SpawnCollectable(false);
        }
    }

    private Collectable SpawnCollectable(bool isBuff)
    {
        List<Collectable> collection;

        if (isBuff)
        {
            collection = CollectablesManager.Instance.AvailableBuffs;
        }
        else
        {
            collection = CollectablesManager.Instance.AvailableDebuffs;
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
        bool instantKill = false;

        if (collision.collider.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            instantKill = ball.isFireball;
        }

        if (collision.collider.tag == "Ball" || collision.collider.tag == "Projectile")
        {
            this.TakeDamage(instantKill);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool instantKill = false;

        if (collision.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            instantKill = ball.isFireball;
        }

        if (collision.tag == "Ball" || collision.tag == "Projectile")
        {
            this.TakeDamage(instantKill);
        }
    }


    private void TakeDamage(bool instantKill) // This method name was BallCollisionLogic
    {

        this.HitPoints--;

        if (this.HitPoints <= 0 || instantKill)
        {
            BricksManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            OnBrickDestroy();
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            this.sr.sprite = BricksManager.Instance.Sprites[this.HitPoints - 1];
        }
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
        this.HitPoints = hitpoints;
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    private void OnDisable()
    {
        Ball.OnFireBallEnable -= Ball_OnFireBallEnable;
        Ball.OnFireBallDisable -= Ball_OnFireBallDisable;
    }
}