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

        weaponAttack = Variables.CalculateSaveStat(PlayerStat.FocusAttack);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Fire()
    {
        for (int i = 0; i < playerShooter.WeaponLevel * 2; i++)
        {
            var newBullet = playerShooter.CreateBullet();
            newBullet.transform.localScale = new Vector3(weaponScale, weaponScale, weaponScale);
            newBullet.transform.position = firePositions[i].position;
            newBullet.transform.LookAt(fireDirections[i].position);
            newBullet.atk = playerShooter.FinalAttack + weaponAttack + weaponPhaseAttack;
            newBullet.speed = weaponSpeed;
        }
    }

    public override void ApplyTestData()
    {
        if (!playerShooter.testPlayerData.isTesting)
            return;

        weaponAttack = playerShooter.testPlayerData.focusAttack;
        weaponSpeed = playerShooter.testPlayerData.focusSpeed;
        weaponScale = playerShooter.testPlayerData.focusScale;
        weaponInterval = playerShooter.testPlayerData.focusInterval;
    }
}
