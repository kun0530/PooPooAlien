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
        if (isDead)
        {
            if (deathEffect.isPlaying)
                return;
            else
            {
                gameObject.SetActive(false);
                gameManager.ChangeGameState(GameState.GameOver);
            }
        }

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
        if (isInvincible || playerBooster.IsBoosting || isDead)
            return;

        hitEffect.Play();
        audioPlayer.PlayOneShot(hitSound);

        CurrentHealth -= damage;
        gameManager.playerHitCount++;

        isInvincible = true;
        nextBlinkTime = Time.time + blinkInterval;

        if (!isDead && CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDie();
        }
    }

    public override void OnDie()
    {
        if (isDead)
            return;

        deathEffect.Play();
        audioPlayer.PlayOneShot(deathSound);

        isInvincible = false;

        playerRender.SetActive(false);
        var playerMovement = gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = false;
        var playerShooter = gameObject.GetComponent<PlayerShooter>();
        if (playerShooter != null)
            playerShooter.CurrentWeaponType = WeaponType.None;

        base.OnDie();
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
