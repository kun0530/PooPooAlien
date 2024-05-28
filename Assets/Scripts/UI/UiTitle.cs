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
        if (TouchManager.Instance.Tap)
        {
            uiManager.ChangeUiState(UiStates.StageSelect);
        }
    }
}
