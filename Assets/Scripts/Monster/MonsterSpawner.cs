using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Diagnostics;

public class MonsterSpawner : MonoBehaviour
{
    public List<Monster> monsterPrefabs;
    private Dictionary<int, IObjectPool<Monster>> poolEnemies = new Dictionary<int, IObjectPool<Monster>>();
    // private IObjectPool<Enemy> poolEnemy;
    public Transform monsterPool;
    private List<List<int>> monsterSpawnGroups;
    private int currentMosterSpawnGroupIndex = 0;
    private MonsterTable monsterTable;
    public Transform[] spawnPositions;

    private GameManager gameManager;

    private float nextCreateTime;
    public float spawnInterval = 0.55f;

    private void Start()
    {
        // 시작 몬스터 스폰 처리
        // nextCreateTime = Time.time + interval;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        for (int i = 0; i < monsterPrefabs.Count; i++)
        {
            int index = i;

            IObjectPool<Monster> poolEnemy = new ObjectPool<Monster>(
                () => {
                    var monster = Instantiate(monsterPrefabs[index], monsterPool);
                    monster.pool = poolEnemies[index];
                    return monster;
                },
                OnTakeFromPool,
                OnReturnToPool,
                OnDestroyPoolObject,
                true, 10, 100
            );

            poolEnemies.Add(index, poolEnemy);
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

        var prefabNum = Mathf.Clamp(data.Type, 0, monsterPrefabs.Count - 1);
        var newEnemy = poolEnemies[prefabNum].Get();
        newEnemy.Data = data;
        newEnemy.transform.position = pos;
    }

    private void DropItem(MonsterData data, Vector3 pos)
    {
        gameManager.CurrentScore += data.Score;
        gameManager.CurrentKillPoint += data.KillPoint;
        gameManager.EarnedGold += data.GetGold;
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
