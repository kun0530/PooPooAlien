using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTitle : MonoBehaviour
{
    public TitleUiManager uiManager;

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            uiManager.Status = UiStatus.StageSelect;
        }
    }
}
