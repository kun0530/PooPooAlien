using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Diagnostics;

public class PlayerHealth : LivingEntity
{
    public DevelopPlayerData testPlayerData;
    public GameManager gameManager;

    private PlayerBooster playerBooster;

    private float invincibleduration = 1f;
    private float invincibleTimer;
    private bool isInvincible;
    public float MaxHealth { get; private set; }
    public float StartHealth {
        get { return startHealth; }
        private set {
            if (MaxHealth < value)
            {
                value = MaxHealth;
            }
            startHealth = value;
        }
    }
    private float CurrentHealth {
        get { return currentHealth; }
        set {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            gameManager.uiManager.SetPlayerHealth((int)currentHealth);
        }
    }

    public ParticleSystem hitEffect;
    public ParticleSystem deathEffect;

    private void Awake()
    {
        playerBooster = GetComponent<PlayerBooster>();
    }

    private void Start()
    {
        MaxHealth = Variables.CalculateCurrentSaveStat(PlayerStat.MaxHP);
        StartHealth = Variables.CalculateCurrentSaveStat(PlayerStat.StartHP);
        CurrentHealth = startHealth;

        invincibleTimer = 0f;
        isInvincible = false;

        ApplyTestData();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (invincibleTimer >= invincibleduration)
            {
                isInvincible = false;
                invincibleTimer = 0f;
            }
            else
            {
                invincibleTimer += Time.deltaTime;
            }
        }
    }

    public override void OnDamage(float damage)
    {
        if (isInvincible || playerBooster.IsBoosting)
            return;

        hitEffect.Play();

        CurrentHealth -= damage;
        gameManager.playerHitCount++;

        if (!isDead && CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDie();
        }
        isInvincible = true;
    }

    public override void OnDie()
    {
        if (isDead)
            return;

        deathEffect.Play();

        base.OnDie();
        gameObject.SetActive(false);
        gameManager.ChangeGameState(GameState.GameOver);
    }

    public void RestoreHealth(float hp)
    {
        CurrentHealth += hp;
    }

    [Conditional("DEVELOP_TEST")]
    public void ApplyTestData()
    {
        if (!gameManager.testPlayerData.isTesting)
            return;
            
        MaxHealth = testPlayerData.maxHp;
        StartHealth = testPlayerData.startHp;
        CurrentHealth = startHealth;
        
        invincibleduration = testPlayerData.invincibleDuration;
    }
}
