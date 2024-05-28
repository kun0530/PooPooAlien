using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPause : MonoBehaviour
{
    public TextMeshProUGUI pauseText;
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI vfxText;
    public Button backToMainButton;
    public Button restartButton;
    public Button continueButton;

    private void Start()
    {
        var stringTable = DataTableManager.GetStringTable();
        pauseText.text = stringTable.Get(StringTableIds.Pause_Title_Text);

        bgmText.text = stringTable.Get(StringTableIds.Setting_BGM_Text);
        vfxText.text = stringTable.Get(StringTableIds.Setting_VFX_Text);

        var backToMainText = backToMainButton.GetComponentInChildren<TextMeshProUGUI>();
        if (backToMainText != null)
            backToMainText.text = stringTable.Get(StringTableIds.Pause_BackToMain_Text);
        var restartText = restartButton.GetComponentInChildren<TextMeshProUGUI>();
        if (restartText != null)
            restartText.text = stringTable.Get(StringTableIds.Pause_Restart_Text);
        var continueText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
        if (continueText != null)
            continueText.text = stringTable.Get(StringTableIds.Pause_Continue_Text);
    }
}
