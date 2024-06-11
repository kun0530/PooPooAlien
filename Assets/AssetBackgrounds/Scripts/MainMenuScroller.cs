using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScroller : MonoBehaviour
{

    private int length;
    public float speed;


    public void Start()
    {

    }
    void Update()
    {
        // transform.position += Vector3.down * Time.deltaTime * speed;

        // if (transform.position.y < - 19) {
        //     this.transform.position = new Vector3(this.transform.position.x, 19f, this.transform.position.z);

        // }

        transform.localPosition += Vector3.down * Time.deltaTime * speed;

        if (transform.localPosition.y < - 19) {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 19f, this.transform.localPosition.z);

        }
    }

}
