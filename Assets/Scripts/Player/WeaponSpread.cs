using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpread : Weapon
{
    public Transform[] firePositions;
    public Transform[] fireDirections;

    protected override void Fire()
    {
        for (int i = 0; i < playerShooter.WeaponLevel * 2; i++)
        {
            var newBullet = playerShooter.CreateBullet();
            newBullet.transform.position = firePositions[i].position;
            newBullet.transform.LookAt(fireDirections[i].position);
            newBullet.SetAtk(playerShooter.FinalAttack + weaponAttack);
        }
    }
}
