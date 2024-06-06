using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpread : Weapon
{
    public Transform firePosition;
    public List<Transform> fireDirectionsLevel1;
    public List<Transform> fireDirectionsLevel2;
    public List<Transform> fireDirectionsLevel3;
    private Dictionary<int, List<Transform>> fireDirections = new Dictionary<int, List<Transform>>();

    protected override void Awake()
    {
        base.Awake();

        weaponType = WeaponType.Spread;
        weaponAttack = Variables.CalculateCurrentSaveStat(PlayerStat.SpreadAttack);
    }

    protected override void Start()
    {
        base.Start();

        fireDirections.Add(1, fireDirectionsLevel1);
        fireDirections.Add(2, fireDirectionsLevel2);
        fireDirections.Add(3, fireDirectionsLevel3);
    }

    protected override void Fire()
    {
        var fireDirs = fireDirections[playerShooter.WeaponPhase];

        foreach (var dir in fireDirs)
        {
            var newBullet = playerShooter.CreateBullet();
            newBullet.transform.localScale = new Vector3(weaponPhaseData.Scale, weaponPhaseData.Scale, weaponPhaseData.Scale);
            newBullet.transform.position = firePosition.transform.position;
            newBullet.transform.LookAt(dir.position);
            newBullet.atk = playerShooter.FinalAttack + weaponAttack + weaponPhaseData.Damage;
            newBullet.speed = weaponPhaseData.Speed;
        }
    }

    public override void ApplyTestData()
    {
        if (!playerShooter.testPlayerData.isTesting)
            return;

        weaponAttack = playerShooter.testPlayerData.spreadAttack;
        weaponPhaseData.Speed = playerShooter.testPlayerData.spreadSpeed;
        weaponPhaseData.Scale = playerShooter.testPlayerData.spreadScale;
        weaponPhaseData.Interval = playerShooter.testPlayerData.spreadInterval;

        weaponPhaseData.Damage = 0f;
    }
}
