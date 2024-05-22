using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBooster : Item
{
    protected override void ApplyItemEffect(Collider player)
    {
        var playerBooster = player.GetComponent<PlayerBooster>();
        if (playerBooster == null)
            return;

        playerBooster.BoosterOn();
    }
}
