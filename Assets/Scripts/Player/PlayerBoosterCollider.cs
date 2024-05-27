using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoosterCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            var living = collider.GetComponent<LivingEntity>();
            living.OnDie();
        }
    }
}
