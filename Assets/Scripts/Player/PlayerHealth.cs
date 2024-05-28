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

    public GameObject playerRender;
    public float blinkInterval = 0.3f;
    private float nextBlinkTime;

    public ParticleSystem hitEffect;
    public ParticleSystem deathEffect;

    private AudioSource audioPlayer;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private void Awake()
    {
        playerBooster = GetComponent<PlayerBooster>();
        audioPlayer = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerRender.SetActive(true);
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
                playerRender.SetActive(true);
                isInvincible = false;
                invincibleTimer = 0f;
            }
            else
            {
                invincibleTimer += Time.deltaTime;
                if (nextBlinkTime >= Time.time)
                {
                    playerRender.SetActive(!playerRender.activeSelf);
                    nextBlinkTime = Time.time + blinkInterval;
                }
            }
        }
    }

    public override void OnDamage(float damage)
    {
        if (isInvincible || playerBooster.IsBoosting)
            return;

        hitEffect.Play();
        audioPlayer.PlayOneShot(hitSound);

        CurrentHealth -= damage;
        gameManager.playerHitCount++;

        if (!isDead && CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDie();
        }
        isInvincible = true;
        nextBlinkTime = Time.time + blinkInterval;
    }

    public override void OnDie()
    {
        if (isDead)
            return;

        deathEffect.Play();
        audioPlayer.PlayOneShot(deathSound);

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
