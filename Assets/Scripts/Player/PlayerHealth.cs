using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public TempPlayerData playerData; // 테스트
    public TextMeshProUGUI textPlayerHealth; // 테스트

    public float invincibleduration = 1f;
    private float invincibleTimer;
    private bool isInvincible;

    private void Start()
    {
        startHealth = playerData.playerHealth;
        currentHealth = startHealth;
        textPlayerHealth.text = $"HP: {currentHealth}"; // 테스트

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

        base.OnDamage(damage);
        textPlayerHealth.text = $"HP: {currentHealth}"; // 테스트
        isInvincible = true;

        Logger.Log("Damage!");
    }

    protected override void OnDie()
    {
        base.OnDie();
        gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void RestoreHealth(float hp)
    {
        currentHealth += hp;
        textPlayerHealth.text = $"HP: {currentHealth}"; // 테스트
    }
}
