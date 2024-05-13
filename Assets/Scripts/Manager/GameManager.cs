using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MonsterSpawner enemySpawner;

    private (int stageId, int sectionId) monsterSpawnKey;
    public (int stageId, int sectionId) MonsterSpawnKey {
        get { return monsterSpawnKey; }
        set {
            monsterSpawnKey = value;
            enemySpawner.ChangeMonsterSpawnGroup(monsterSpawnKey);
        }
    }

    private void Start()
    {
        // 테스트 코드
        MonsterSpawnKey = (1, 1);
    }

    private void Update()
    {
        // 테스트 코드
        if (Input.GetKeyDown(KeyCode.Return))
        {
            monsterSpawnKey.sectionId++;
            if (monsterSpawnKey.sectionId > 3)
            {
                monsterSpawnKey.sectionId = 1;
            }
            MonsterSpawnKey = (1, monsterSpawnKey.sectionId);
        }
    }
}
