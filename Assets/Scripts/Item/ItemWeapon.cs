using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : Item
{
    protected override void ApplyItemEffect(Collider player)
    {
        var playerShooter = player.GetComponent<PlayerShooter>();
        if (playerShooter == null)
            return;

        playerShooter.ChangeOrUpgradeWeapon((WeaponType)data.Value);
    }
}
