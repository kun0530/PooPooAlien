using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UiTitleSetting : MonoBehaviour
{
    public TitleUiManager uiManager;

    private Languages selectedLanguage;
    public TextMeshProUGUI languageText;

    // Audio
    public Slider backgroundMusicSlider;
    public Slider effectSoundSlider;
    public AudioMixer masterMixer;
    private AudioManager audioMgr;
    public AudioManager audioManager{
        get
        {
            if (audioMgr == null)
                audioMgr = AudioManager.Instance;
            if (audioMgr.masterMixer == null)
                audioMgr.masterMixer = masterMixer;
            return audioMgr;
        }
    }

    public float MusicVolume
    {
        get { return audioManager.MusicVolume; }
        set { audioManager.MusicVolume = value; }
    }

    public float EffectsVolume
    {
        get { return audioManager.EffectsVolume; }
        set { audioManager.EffectsVolume = value; }
    }

    private void OnEnable()
    {
        selectedLanguage = Variables.SaveData.CurrentLang;
        languageText.text = selectedLanguage.ToString();

        backgroundMusicSlider.value = MusicVolume;
        effectSoundSlider.value = EffectsVolume;
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
            Variables.isStartGame = true;
            uiManager.ChangeUiState(UiStates.StageSelect);
        }
    }
}
