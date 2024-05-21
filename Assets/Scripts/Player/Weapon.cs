using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using System.Diagnostics;

public abstract class Weapon : MonoBehaviour
{
    protected PlayerShooter playerShooter;

    protected float weaponAttack;
    protected float weaponPhaseAttack;
    protected float weaponScale;
    protected float weaponSpeed;
    protected float weaponInterval;

    protected float nextFireTime;

    protected virtual void Awake()
    {
        playerShooter = GetComponentInParent<PlayerShooter>();
        nextFireTime = 0f;
    }

    protected virtual void Start()
    {
        ApplyTestData();
    }

    protected virtual void Update()
    {
        if (nextFireTime < Time.time)
        {
            Fire();
            nextFireTime = Time.time + weaponInterval;
        }
    }

    public void ChangeWeaponPhase(WeaponType type)
    {
        var data = DataTableManager.Get<ProjectileTable>(DataTableIds.Projectile).Get(type, playerShooter.WeaponLevel);
        weaponPhaseAttack = data.Damage;
        weaponScale = data.Scale;
        weaponSpeed = data.Speed;
        weaponInterval = data.Interval;

        ApplyTestData();
    }

    protected abstract void Fire();

    [Conditional("DEVELOP_TEST")]
    public abstract void ApplyTestData();
}
