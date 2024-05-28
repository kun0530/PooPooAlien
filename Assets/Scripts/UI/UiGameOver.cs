using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGameOver : MonoBehaviour
{
    public Button backToMainButton;
    public Button retryButton;

    private void Start()
    {
        var stringTable = DataTableManager.GetStringTable();

        var backToMainText = backToMainButton.GetComponentInChildren<TextMeshProUGUI>();
        if (backToMainText != null)
            backToMainText.text = stringTable.Get(StringTableIds.Gameover_BackToMain_Text);
        var retryText = retryButton.GetComponentInChildren<TextMeshProUGUI>();
        if (retryText != null)
            retryText.text = stringTable.Get(StringTableIds.Gameover_Retry_Text);
    }
}
