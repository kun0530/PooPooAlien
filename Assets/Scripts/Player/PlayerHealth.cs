using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public TempPlayerData playerData; // 테스트
    public TextMeshProUGUI textPlayerHealth; // 테스트

    private void Start()
    {
        startHealth = playerData.playerHealth;
        currentHealth = startHealth;
        textPlayerHealth.text = $"HP: {currentHealth}"; // 테스트
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        textPlayerHealth.text = $"HP: {currentHealth}"; // 테스트
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
