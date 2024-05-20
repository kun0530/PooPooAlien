using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected PlayerShooter playerShooter;

    protected float weaponAttack;
    protected float weaponSpeed;
    protected float weaponInterval;

    protected float nextFireTime;

    protected virtual void Awake()
    {
        playerShooter = GetComponentInParent<PlayerShooter>();
        nextFireTime = 0f;
    }

    // Start는 테스트용
    private void Start()
    {
        weaponAttack = 1f;
        weaponSpeed = 1f;
        weaponInterval = 10f;
    }

    protected virtual void Update()
    {
        if (nextFireTime < Time.time)
        {
            Fire();
            nextFireTime = Time.time + weaponInterval;
        }
    }

    protected abstract void Fire();
}
