using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


public class PlayerBooster : MonoBehaviour
{
    private GameManager gameManager;

    // 부스터 지속 시간, 부스터 스피드(스케일), 부스터 충돌 범위
    private float boosterDuration = 5f;
    private float nextBoosterTimer;
    private float boosterSpeed = 2f;
    private float boosterColliderRange;

    private void Awake()
    {
        // boosterDuration = Variables.CalculateSaveStat(PlayerStat.BoosterDuration);
        // boosterSpeed = Variables.CalculateSaveStat(PlayerStat.BoosterSpeed);
        // boosterColliderRange = gameManager.testPlayerData.boosterSize;
    }

    private void OnEnable()
    {
        // 타이머 시작
        nextBoosterTimer = 0f;
        Time.timeScale = boosterSpeed;
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ApplyTestData();
        this.enabled = false;
    }

    private void Update()
    {
        if (gameManager.gameStatus == GameStatus.Pause) // 게임 매니저에서 상태 확인
            return;

        nextBoosterTimer += Time.deltaTime / Time.timeScale;

        if (nextBoosterTimer >= boosterDuration)
        {
            Time.timeScale = 1f;
            this.enabled = false;
        }
    }

    [Conditional("DEVELOP_TEST")]
    public void ApplyTestData()
    {
        boosterDuration = gameManager.testPlayerData.boosterDuration;
        boosterSpeed = gameManager.testPlayerData.boosterSpeed;
        boosterColliderRange = gameManager.testPlayerData.boosterSize;
    }
}
