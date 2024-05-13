using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum MonsterType
{
    None = -1,
    Normal,
    Physic,
    Speed,
    Reaf,
    Projectile,
    Count
};

public class Monster : LivingEntity
{
    public MonsterType monseterType = MonsterType.None;

    private float speed = 3f;
    private float atk = 50f;
    private Vector3 direction;

    public IObjectPool<Monster> pool;

    public ItemSpawner itemSpawner;

    private void Start()
    {
        direction = -transform.forward;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<LivingEntity>().OnDamage(atk);
        }
        else if (collider.CompareTag("Wall") && pool != null)
        {
            pool.Release(this);
        }
    }

    protected override void OnDie()
    {
        base.OnDie();
        itemSpawner.CreateItem(transform.position);
        if (pool != null)
        {
            pool.Release(this);
        }
    }
}