using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetCube : MonoBehaviour
{
    public float penetAttack { get; set; }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            var living = collider.GetComponent<LivingEntity>();
            if (living != null)
                living.OnDamage(penetAttack);
        }
    }
}
