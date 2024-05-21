using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerUp : Item
{
    protected override void ApplyItemEffect(Collider player)
    {
        var playerShooter = player.GetComponent<PlayerShooter>();
        if (playerShooter == null)
            return;

        playerShooter.PowerUpCount++;
    }
}
