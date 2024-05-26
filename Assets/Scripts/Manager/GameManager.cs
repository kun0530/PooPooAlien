using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Analytics;


public enum GameState
{
    None = -1,
    Running,
    Pause,
    GameOver,
    GameClear,
    Count
}

public class GameManager : MonoBehaviour
{
    public GameState gameState { get; private set; } = GameState.None;
    public DevelopPlayerData testPlayerData;
    public KillPointData killPointData;

    public MonsterSpawner monsterSpawner;
    public ItemSpawner itemSpawner;

    private float prevTimeScale;
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

    public UiNumberIncrease goldText;
    private float earnedGold;
    public float EarnedGold {
        get { return earnedGold; }
        set {
            if (value < 0)
                return;

            earnedGold = value;
            goldText.TargetNum = earnedGold;
        }
    }

    public int playerHitCount;

    private void Start()
    {
        StageId = Variables.stageId;
        SectionId = 1;
        var stageData = DataTableManager.Get<StageTable>(DataTableIds.Stage).Get(StageId);

        prevTimeScale = 1f;
        ChangeGameState(GameState.Running);

        gameTimeLimit = stageData.StageTimerset;
        CurrentGameTimer = gameTimeLimit;
        nextGameTime = CurrentGameTimer - 1f;
        IsTimerStop = false;
        uiManager.SetGameTimer(CurrentGameTimer);

        CurrentScore = 0;
        CurrentKillPoint = 0;
        targetKillPoint = stageData.StageKillp;
        earnedGold = 0f;
        goldText.CurrentNum = earnedGold;

        nextKillPoint = targetKillPoint * killPointData.killPointBoundaries[0];
        killPointSlider.minValue = 0;
        killPointSlider.maxValue = targetKillPoint;

        playerHitCount = 0;
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
                ChangeGameState(GameState.GameClear);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeGameState(GameState.Pause);
        }
    }

    private void CalculateGameTime()
    {
        if (IsTimerStop)
            return;

        CurrentGameTimer -= Time.deltaTime;

        if (CurrentGameTimer <= nextGameTime)
        {
            uiManager.SetGameTimer(CurrentGameTimer);
            nextGameTime--;
        }

        if (CurrentGameTimer <= 0f)
        {
            uiManager.SetGameTimer(0);
            ChangeGameState(GameState.GameOver);
        }
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneIds.Title);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [VisibleEnum(typeof(GameState))]
    public void ChangeGameState(int state)
    {
        if (gameState == (GameState)state)
            return;
        
        switch (state)
        {
            case (int)GameState.Running:
                {
                    uiManager.CurrentActivePanel = null;
                    uiManager.stagePanel.SetActive(true);

                    Time.timeScale = prevTimeScale;
                    textStartTimer.gameObject.SetActive(true);

                    if (playerMovement != null)
                        playerMovement.enabled = true;
                    break;
                }
            case (int)GameState.GameOver:
            case (int)GameState.GameClear:
            case (int)GameState.Pause:
                {
                    if (gameState != GameState.Running)
                        return;

                    uiManager.CurrentActivePanel = uiManager.panels[(GameState)state];
                    uiManager.stagePanel.SetActive(false);

                    prevTimeScale = Time.timeScale;
                    Time.timeScale = 0f;

                    if (playerMovement != null)
                        playerMovement.enabled = false;

                    if (state == (int)GameState.GameOver)
                    {
                        Variables.SaveData.Gold += earnedGold;
                        SaveLoadSystem.Save(Variables.SaveData);
                    }

                    break;
                }
            default:
                return;
        }
        gameState = (GameState)state;
    }
    public void ChangeGameState(GameState state)
    {
        ChangeGameState((int)state);
    }
}
