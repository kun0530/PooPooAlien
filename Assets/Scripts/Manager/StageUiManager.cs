using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageUiManager : MonoBehaviour
{
    public TextMeshProUGUI textGameTimer;

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    public void SetGameTimer(float time)
    {
        textGameTimer.text = $"{time}";
    }
}
