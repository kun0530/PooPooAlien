using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    private Vector3 rotation = new Vector3(0, 1, 0);
    public float rotationSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * rotationSpeed);
    }
}
