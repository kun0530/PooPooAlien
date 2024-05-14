using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    public ItemSpawner itemSpawner;

    public int StageId { get; set; }
    private int sectionId;
    public int SectionId {
        get { return sectionId; }
        set {
            sectionId = value;
            monsterSpawner.ChangeMonsterSpawnGroup(StageId, sectionId);
        }
    }

    public int CurrentScore { get; private set; }
    public int CurrentKillPoint { get; private set; }

    private void Start()
    {
        StageId = Variables.stageId;
        SectionId = 1;
    }

    private void Update()
    {
        // 테스트 코드
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SectionId++;
            if (SectionId > 3)
            {
                SectionId = 1;
            }
        }
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
        // UI 갱신
    }

    public void AddKillPoint(int point)
    {
        CurrentKillPoint += point;
        // UI 갱신
    }
}
