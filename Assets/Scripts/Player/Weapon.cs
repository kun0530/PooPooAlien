using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using System.Diagnostics;

public abstract class Weapon : MonoBehaviour
{
    protected PlayerShooter playerShooter;

    protected WeaponType weaponType;

    protected float weaponAttack;
    protected ProjectileData weaponPhaseData;

    protected float nextFireTime;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        playerShooter = GetComponentInParent<PlayerShooter>();
        UpdateWeaponPhaseData();

        ApplyTestData();
        nextFireTime = Time.time + weaponPhaseData.Interval;
    }

    protected virtual void Update()
    {
        if (nextFireTime < Time.time)
        {
            Fire();
            nextFireTime = Time.time + weaponPhaseData.Interval;
        }
    }

    public void UpdateWeaponPhaseData()
    {
        weaponPhaseData = DataTableManager.Get<ProjectileTable>(DataTableIds.Projectile).Get(weaponType, playerShooter.WeaponPhase);

        ApplyTestData();
    }

    protected abstract void Fire();

    [Conditional("DEVELOP_TEST")]
    public abstract void ApplyTestData();
}
