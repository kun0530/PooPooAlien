using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazor : MonoBehaviour
{
    private float atk = 10000f;

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            var living = collider.GetComponent<LivingEntity>();
            living.OnDamage(atk);
        }
    }
}
