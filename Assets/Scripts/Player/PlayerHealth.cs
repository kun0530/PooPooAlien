using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
    }

    protected override void OnDie()
    {
        base.OnDie();
    }
}
