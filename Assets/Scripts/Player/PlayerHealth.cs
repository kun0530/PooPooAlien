using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public TempPlayerData playerData; // 테스트
    public GameManager gameManager;

    public float invincibleduration = 1f;
    private float invincibleTimer;
    private bool isInvincible;
    private float CurrentHealth {
        get { return currentHealth; }
        set {
            currentHealth = value;
            gameManager.uiManager.SetPlayerHealth((int)currentHealth);
        }
    }

    private void Start()
    {
        startHealth = playerData.playerHealth;
        CurrentHealth = startHealth;

        invincibleTimer = 0f;
        isInvincible = false;
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
        if (isInvincible)
            return;

        CurrentHealth -= damage;

        if (!isDead && CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDie();
        }
        isInvincible = true;
    }

    protected override void OnDie()
    {
        base.OnDie();
        gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void RestoreHealth(float hp)
    {
        CurrentHealth += hp;
    }
}
