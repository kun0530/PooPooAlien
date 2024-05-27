using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class UiTitleSetting : MonoBehaviour
{
    public TitleUiManager uiManager;

    private Languages selectedLanguage;
    public TextMeshProUGUI languageText;

    public void OnEnable()
    {
        selectedLanguage = Variables.SaveData.CurrentLang;
        languageText.text = selectedLanguage.ToString();
    }

    public void ChangeLanguage(bool isNext)
    {
        selectedLanguage += isNext ? 1 : -1;
        if (selectedLanguage <= Languages.None)
            selectedLanguage = Languages.Count - 1;
        if (selectedLanguage >= Languages.Count)
            selectedLanguage = Languages.None + 1;

        languageText.text = selectedLanguage.ToString();
    }

    public void ExitSettingPanel()
    {
        if (selectedLanguage != Variables.SaveData.CurrentLang)
        {
            Variables.SaveData.CurrentLang = selectedLanguage;
            SaveLoadSystem.Save(Variables.SaveData);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            uiManager.ChangeUiState(UiStates.StageSelect);
        }
    }
}
