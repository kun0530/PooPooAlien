using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateGold : MonoBehaviour
{
    private string goldFormat = "{0}K";
    private TextMeshProUGUI goldText;

    private void Awake()
    {
        goldText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        goldText.text = string.Format(goldFormat, (int)Variables.SaveData.Gold);
    }
}
