using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    private float startHealth = 100f;
    public float currentHealth { get; private set; }
    public bool isDead { get; private set; }
    // public event Action onDeath; 사망시 발동할 이벤트

    // RestoreHealth와 ApplyUpdateHealth는 필요하면 추가

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = startHealth;
    }

    public virtual void OnDamage(float damage)
    {
        currentHealth -= damage;

        if (!isDead && currentHealth <= 0)
        {
            currentHealth = 0;
            OnDie();
        }
    }

    protected virtual void OnDie()
    {
        isDead = true;
    }
}