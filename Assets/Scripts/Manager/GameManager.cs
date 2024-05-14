using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameStatus
{
    Running,
    Pause,
    GameOver,
    GameClear
}

public class GameManager : MonoBehaviour
{
    public GameStatus gameStatus { get; set; }
    public KillPointData killPointData;

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

    public float targetKillPoint { get; private set; }
    public float nextKillPoint { get; private set; }
    private float currentKillPoint;
    public float CurrentKillPoint {
        get { return currentKillPoint; }
        private set
        {
            currentKillPoint = value;
            textKillPoint.text = string.Format(formatKillPoint, currentKillPoint);
        }
    }

    public TextMeshProUGUI textScore;
    private string formatScore = "Score: {0}";
    public TextMeshProUGUI textKillPoint;
    private string formatKillPoint = "KillPoint: {0}";
    public TextMeshProUGUI textGameClear;

    private void Start()
    {
        gameStatus = GameStatus.Running;
        textGameClear.enabled = false;

        StageId = Variables.stageId;
        SectionId = 1;

        CurrentScore = 0;
        CurrentKillPoint = 0;
        targetKillPoint = 100; // 테스트

        nextKillPoint = targetKillPoint * killPointData.killPointBoundaries[0];
    }

    private void Update()
    {
        if (gameStatus != GameStatus.Running)
            return;

        if (currentKillPoint >= nextKillPoint)
        {
            if (SectionId < killPointData.killPointBoundaries.Count)
            {
                nextKillPoint = targetKillPoint * killPointData.killPointBoundaries[SectionId];
                SectionId++;
            }
            else if (SectionId == killPointData.killPointBoundaries.Count)
            {
                nextKillPoint = targetKillPoint;
                SectionId++;
            }
            else
            {
                gameStatus = GameStatus.GameClear;
                textGameClear.enabled = true;
                Time.timeScale = 0;
            }
        }

        // 테스트 코드
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AddKillPoint(10);
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            AddKillPoint(1);
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
