using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public enum GameState
{
    Running,
    Pause,
    GameOver,
    GameClear
}

public class GameManager : MonoBehaviour
{
    public GameState gameState { get; set; }
    public DevelopPlayerData testPlayerData;
    public KillPointData killPointData;

    public MonsterSpawner monsterSpawner;
    public ItemSpawner itemSpawner;

    public PlayerMovement playerMovement;

    public float CurrentGameTimer { get; set; }
    private float nextGameTime;
    private float gameTimeLimit;
    public bool IsTimerStop { get; set;}

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
        set
        {
            if (value < 0)
                return;

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
        set
        {
            if (value < 0)
                return;

            currentKillPoint = value;
            killPointSlider.value = currentKillPoint;
        }
    }

    public TextMeshProUGUI textScore;
    private string formatScore = "{0}";
    public GameObject gameClearUi;

    public TextMeshProUGUI textStartTimer;

    public TextMeshProUGUI goldText;
    private string goldFormat = "{0}K";
    private float earnedGold;
    public float EarnedGold {
        get { return earnedGold; }
        set {
            if (value < 0)
                return;

            earnedGold = value;
            goldText.text = string.Format(goldFormat, (int)earnedGold);
        }
    }

    public int playerHitCount;

    private void Start()
    {
        StageId = Variables.stageId;
        SectionId = 1;

        var stageData = DataTableManager.Get<StageTable>(DataTableIds.Stage).Get(StageId);

        gameState = GameState.Running;

        gameTimeLimit = stageData.StageTimerset;
        CurrentGameTimer = gameTimeLimit;
        nextGameTime = CurrentGameTimer - 1f;
        IsTimerStop = false;
        uiManager.SetGameTimer(CurrentGameTimer);

        CurrentScore = 0;
        CurrentKillPoint = 0;
        targetKillPoint = stageData.StageKillp;
        EarnedGold = 0f;

        nextKillPoint = targetKillPoint * killPointData.killPointBoundaries[0];
        killPointSlider.minValue = 0;
        killPointSlider.maxValue = targetKillPoint;

        playerHitCount = 0;

        textStartTimer.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (gameState != GameState.Running)
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
    }

    private void CalculateGameTime()
    {
        if (IsTimerStop)
            return;

        CurrentGameTimer -= Time.deltaTime;

        if (CurrentGameTimer <= nextGameTime)
        {
            uiManager.SetGameTimer(nextGameTime);
            nextGameTime--;
        }

        if (nextGameTime < 0)
        {
            // time out!
        }
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene(SceneIds.Title);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameState = GameState.Running;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameClear()
    {
        gameState = GameState.GameClear;
        gameClearUi.SetActive(true);
        Time.timeScale = 0f;

        playerMovement.enabled = false;
    }
}
