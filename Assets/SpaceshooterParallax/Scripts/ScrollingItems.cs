using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingItems : MonoBehaviour
{

    public float speed; // speed to move obstacles
    public Camera backgroundCamera;

    Vector2 max;
    Vector2 min;
    // Start is called before the first frame update
    void Start()
    {
        max = backgroundCamera.ViewportToWorldPoint(new Vector2(0, 0)); //bottom left screen point
        min = backgroundCamera.ViewportToWorldPoint(new Vector2(1, 1)); //top right screen point


        max.y = max.y - GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        min.y = min.y + GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

    }

    void Update()
    {          
        Vector3 position = transform.localPosition;
        position = new Vector3(position.x, position.y + speed * Time.deltaTime, position.z);
        transform.localPosition = position;

        if (transform.localPosition.y < max.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, min.y, transform.localPosition.z);
            //Random.RandomRange(min.x, max.x)
        }
    }
}
