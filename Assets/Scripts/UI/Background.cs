using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public List<GameObject> backgrounds;

    private void Start()
    {
        if (backgrounds != null && backgrounds.Count > 0)
        {
            var index = Mathf.Clamp(Variables.stageId, 0, backgrounds.Count);
            Instantiate(backgrounds[index], gameObject.transform);
        }
    }
}
