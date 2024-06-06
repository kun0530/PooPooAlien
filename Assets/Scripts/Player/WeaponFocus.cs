using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WeaponFocus : Weapon
{
    public List<Transform> firePositions;
    public List<Transform> fireDirections;

    protected override void Awake()
    {
        base.Awake();

        weaponType = WeaponType.Focus;
        weaponAttack = Variables.CalculateCurrentSaveStat(PlayerStat.FocusAttack);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Fire()
    {
        for (int i = 0; i < playerShooter.WeaponPhase * 2; i++)
        {
            var newBullet = playerShooter.CreateBullet();
            newBullet.transform.localScale = new Vector3(weaponPhaseData.Scale, weaponPhaseData.Scale, weaponPhaseData.Scale);
            newBullet.transform.position = firePositions[i].position;
            newBullet.transform.LookAt(fireDirections[i].position);
            newBullet.atk = (playerShooter.FinalAttack + weaponAttack + weaponPhaseData.Damage) / (playerShooter.WeaponPhase * 2);
            newBullet.speed = weaponPhaseData.Speed;
        }
    }

    public override void ApplyTestData()
    {
        if (!playerShooter.testPlayerData.isTesting)
            return;

        weaponAttack = playerShooter.testPlayerData.focusAttack;
        weaponPhaseData.Speed = playerShooter.testPlayerData.focusSpeed;
        weaponPhaseData.Scale = playerShooter.testPlayerData.focusScale;
        weaponPhaseData.Interval = playerShooter.testPlayerData.focusInterval;

        weaponPhaseData.Damage = 0f;
    }
}
