using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private int currentScore;
    public int CurrentScore {
        get { return currentScore; }
        private set
        {
            currentScore = value;
            textScore.text = string.Format(formatScore, currentScore);
        }
    }
    private int currentKillpoint;
    public int CurrentKillPoint {
        get { return currentKillpoint; }
        private set
        {
            currentKillpoint = value;
            textKillPoint.text = string.Format(formatKillPoint, currentKillpoint);
        }
    }

    private void Start()
    {
        StageId = Variables.stageId;
        SectionId = 1;

        CurrentScore = 0;
        CurrentKillPoint = 0;
    }

    public TextMeshProUGUI textScore;
    private string formatScore = "Score: {0}";
    public TextMeshProUGUI textKillPoint;
    private string formatKillPoint = "KillPoint: {0}";

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
    }

    public void AddKillPoint(int point)
    {
        CurrentKillPoint += point;
    }
}
