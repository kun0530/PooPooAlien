using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamageActive : MonoBehaviour
{
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.GetComponent<Monster>().isDamageAble = true;
        }
    }
}
