using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : Item
{
    protected override void ApplyItemEffect(Collider player)
    {
        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            return;

        playerHealth.RestoreHealth(data.Value);
    }
}
