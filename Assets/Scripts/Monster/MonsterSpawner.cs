using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Diagnostics;

public class MonsterSpawner : MonoBehaviour
{
    public Monster[] monsterPrefabs;
    private Dictionary<MonsterType, IObjectPool<Monster>> poolEnemies = new Dictionary<MonsterType, IObjectPool<Monster>>();
    // private IObjectPool<Enemy> poolEnemy;
    private List<List<int>> monsterSpawnGroups;
    private int currentMosterSpawnGroupIndex = 0;
    private MonsterTable monsterTable;
    public Transform[] spawnPositions;

    private GameManager gameManager;

    private float nextCreateTime;
    private float spawnInterval = 2.3f;

    private void Start()
    {
        // 시작 몬스터 스폰 처리
        // nextCreateTime = Time.time + interval;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        for (int i = 0; i < (int)MonsterType.Count; i++)
        {
            int index = i;
            var monsterType = (MonsterType)index;

            IObjectPool<Monster> poolEnemy = new ObjectPool<Monster>(
                () => {
                    var monster = Instantiate(monsterPrefabs[index]);
                    monster.monseterType = monsterType;
                    monster.pool = poolEnemies[monsterType];
                    return monster;
                },
                OnTakeFromPool,
                OnReturnToPool,
                OnDestroyPoolObject,
                true, 10, 100
            );

            poolEnemies.Add(monsterType, poolEnemy);
        }

        monsterTable = DataTableManager.Get<MonsterTable>(DataTableIds.Monster);

        ApplyTestData();
    }

    private void Update()
    {
        if (nextCreateTime < Time.time)
        {
            if (currentMosterSpawnGroupIndex >= monsterSpawnGroups.Count)
            {
                currentMosterSpawnGroupIndex = 0;
            }
            var monsterSpawnGroup = monsterSpawnGroups[currentMosterSpawnGroupIndex++];

            for (int i = 0; i < 5; i++)
            {
                var monsterData = monsterTable.Get(monsterSpawnGroup[i]);
                CreateEnemy(monsterData, spawnPositions[i].position);
            }

            nextCreateTime = Time.time + spawnInterval;
        }
    }

    private void CreateEnemy(MonsterData data, Vector3 pos)
    {
        if (data.Hp <= 0)
        {
            DropItem(data, pos);
            return;
        }

        var newEnemy = poolEnemies[(MonsterType)data.Type].Get();
        newEnemy.Data = data;
        newEnemy.transform.position = pos;
    }

    private void DropItem(MonsterData data, Vector3 pos)
    {
        gameManager.AddScore(data.Score);
        gameManager.AddKillPoint(data.KillPoint);
        gameManager.itemSpawner.DropItem(data.ItemDropId, pos);
    }

    // private Enemy CreatePooledItem()
    // {
    //     var enemy = Instantiate(enemyPrefab);
    //     enemy.pool = poolEnemy;
    //     return enemy;
    // }

    private void OnTakeFromPool(Monster enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Monster enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Monster enemy)
    {
        Destroy(enemy);
    }

    public bool ChangeMonsterSpawnGroup((int, int) key)
    {
        monsterSpawnGroups = DataTableManager.Get<MonsterSpawnTable>(DataTableIds.MonsterSpawn).Get(key);
        if (monsterSpawnGroups == null)
            return false;
        return true;
    }

    public bool ChangeMonsterSpawnGroup(int stageId, int sectionId)
    {
        return ChangeMonsterSpawnGroup((stageId, sectionId));
    }

    [Conditional("DEVELOP_TEST")]
    public void ApplyTestData()
    {
        if (!gameManager.testPlayerData.isTesting)
            return;

        spawnInterval = gameManager.testPlayerData.monsterSpawnInterval;
    }
}
