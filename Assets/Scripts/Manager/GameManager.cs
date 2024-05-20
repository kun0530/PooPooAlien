using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


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

    public PlayerMovement playerMovement;

    private float currentGameTimer;
    private float nextGameTime;
    private readonly float gameTimeLimit = 600f;

    public StageUiManager uiManager;

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

    public Slider killPointSlider;
    public float targetKillPoint { get; private set; }
    public float nextKillPoint { get; private set; }
    private float currentKillPoint;
    public float CurrentKillPoint {
        get { return currentKillPoint; }
        private set
        {
            currentKillPoint = value;
            killPointSlider.value = currentKillPoint;
        }
    }

    public TextMeshProUGUI textScore;
    private string formatScore = "{0}";
    public TextMeshProUGUI textGameClear;

    private void Start()
    {
        gameStatus = GameStatus.Running;
        textGameClear.enabled = false;

        currentGameTimer = gameTimeLimit;
        nextGameTime = currentGameTimer - 1f;
        uiManager.SetGameTimer(currentGameTimer);

        StageId = Variables.stageId;
        SectionId = 1;

        CurrentScore = 0;
        CurrentKillPoint = 0;
        targetKillPoint = 100; // 테스트

        nextKillPoint = targetKillPoint * killPointData.killPointBoundaries[0];
        killPointSlider.minValue = 0;
        killPointSlider.maxValue = targetKillPoint;
    }

    private void Update()
    {
        if (gameStatus != GameStatus.Running)
            return;

        CalculateGameTime();

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
                GameClear();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameClear();
        }
    }

    private void CalculateGameTime()
    {
        currentGameTimer -= Time.deltaTime;

        if (currentGameTimer <= nextGameTime)
        {
            uiManager.SetGameTimer(nextGameTime);
            nextGameTime--;
        }

        if (nextGameTime < 0)
        {
            // time out!
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

    public void GoToTitle()
    {
        SceneManager.LoadScene(SceneIds.Title);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameStatus = GameStatus.Running;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameClear()
    {
        playerMovement.enabled = false;
        gameStatus = GameStatus.GameClear;
        textGameClear.enabled = true;
        Time.timeScale = 0;
    }
}
