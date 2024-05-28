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
        //bottom left screen point
        max = backgroundCamera.ViewportToWorldPoint(new Vector2(0, 0));
        //top right screen point 
        min = backgroundCamera.ViewportToWorldPoint(new Vector2(1, 1));


        max.y = max.y - GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        min.y = min.y + GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

    }

    // Update is called once per frame
    void Update()
    {

        //get the position of the item
        Vector3 position = transform.position;              
        //change in the position of the item 
        position = new Vector3(position.x,position.y + speed * Time.deltaTime, transform.position.z);
        //update the position
        transform.position = position;


        //if items go beyond the screen reset them here
        if (transform.position.y < max.y) {
            transform.position = new Vector2(transform.position.x,
                min.y );
            //Random.RandomRange(min.x, max.x)
        }
    }
}
