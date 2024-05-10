using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    private IObjectPool<Enemy> poolEnemy;
    public Transform[] spawnPositions;

    private float nextCreateTime;
    private float interval = 5f;

    private void Start()
    {
        // nextCreateTime = Time.time + interval;

        poolEnemy = new ObjectPool<Enemy>(
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
            foreach (var spawnPosition in spawnPositions)
            {
                CreateEnemy(spawnPosition.position);
            }

            nextCreateTime = Time.time + interval;
        }
    }

    private void CreateEnemy(Vector3 pos)
    {
        var newBullet = poolEnemy.Get();
        newBullet.transform.position = pos;
    }

    private Enemy CreatePooledItem()
    {
        var enemy = Instantiate(enemyPrefab);
        enemy.pool = poolEnemy;
        return enemy;
    }

    private void OnTakeFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Enemy enemy)
    {
        Destroy(enemy);
    }
}
