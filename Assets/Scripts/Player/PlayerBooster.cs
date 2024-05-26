using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


public class PlayerBooster : MonoBehaviour
{
    private GameManager gameManager;

    // 부스터 지속 시간, 부스터 스피드(스케일), 부스터 충돌 범위
    public float boosterDuration = 5f;
    private float nextBoosterTimer;

    private float boosterSpeed;
    private float boosterColliderScale;
    public GameObject boosterCollider;

    public ParticleSystem boosterEffect;

    private bool isBoosting;
    public bool IsBoosting {
        get { return isBoosting; }
        private set
        {
            if (isBoosting == value)
                return;

            isBoosting = value;
            if (isBoosting)
                boosterEffect.Play();
            else
                boosterEffect.Stop();
        }
    }

    private void Awake()
    {
        IsBoosting = false;

        boosterSpeed = Variables.CalculateCurrentSaveStat(PlayerStat.BoosterSpeed);
        boosterColliderScale = Variables.CalculateCurrentSaveStat(PlayerStat.BoosterSize);
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ApplyTestData();

        var colliderScale = boosterCollider.transform.localScale;
        boosterCollider.transform.localScale = new Vector3(boosterColliderScale, colliderScale.y, colliderScale.z);
        boosterCollider.SetActive(false);
    }

    private void Update()
    {
        if (!IsBoosting || gameManager.gameState != GameState.Running || Time.timeScale == 0f)
            return;

        nextBoosterTimer += Time.deltaTime / Time.timeScale;

        if (nextBoosterTimer >= boosterDuration)
        {
            gameManager.IsTimerStop = false;
            Time.timeScale = 1f;
            boosterCollider.SetActive(false);
            IsBoosting = false;
        }
    }

    public void BoosterOn()
    {
        if (IsBoosting)
            return;

        nextBoosterTimer = 0f;
        Time.timeScale = boosterSpeed;
        gameManager.IsTimerStop = true;
        boosterCollider.SetActive(true);
        IsBoosting = true;
    }

    [Conditional("DEVELOP_TEST")]
    public void ApplyTestData()
    {
        boosterDuration = gameManager.testPlayerData.boosterDuration;
        boosterSpeed = gameManager.testPlayerData.boosterSpeed;
        boosterColliderScale = gameManager.testPlayerData.boosterSize;
    }
}
