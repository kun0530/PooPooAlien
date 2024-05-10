using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerShooter : MonoBehaviour
{
    public Bullet bulletPrefab;
    private IObjectPool<Bullet> poolBullet;
    public Transform firePosition;

    private float nextCreateTime;
    private float interval = 0.1f;

    

    private void Start()
    {
        nextCreateTime = Time.time + interval;

        poolBullet = new ObjectPool<Bullet>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            true, 10, 100
        );

        for (int i = 0; i < 10; i++)
        {
            CreatePooledItem().gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (nextCreateTime < Time.time)
        {
            CreateBullet();

            nextCreateTime = Time.time + interval;
        }
    }

    private void CreateBullet()
    {
        var newBullet = poolBullet.Get();
        newBullet.transform.position = firePosition.position;
    }

    private Bullet CreatePooledItem()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.pool = poolBullet;
        return bullet;
    }

    private void OnTakeFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Bullet bullet)
    {
        Destroy(bullet);
    }
}