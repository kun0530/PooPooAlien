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

    public TextMeshProUGUI settingText;
    public TextMeshProUGUI languageText;
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI effectSoundText;
    public Button titleButton;
    public Button exitGameButton;

    private Languages selectedLanguage;
    public TextMeshProUGUI selectedlanguageText;

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
        selectedlanguageText.text = selectedLanguage.ToString();

        backgroundMusicSlider.value = MusicVolume;
        effectSoundSlider.value = EffectsVolume;
    }

    private void Start()
    {
        var stringTable = DataTableManager.GetStringTable();
        settingText.text = stringTable.Get(StringTableIds.Setting_Title_Text);
        languageText.text = stringTable.Get(StringTableIds.Setting_Lang_Text);
        bgmText.text = stringTable.Get(StringTableIds.Setting_BGM_Text);
        effectSoundText.text = stringTable.Get(StringTableIds.Setting_VFX_Text);
        var titleButtonText = titleButton.GetComponentInChildren<TextMeshProUGUI>();
        if (titleButtonText != null)
            titleButtonText.text = stringTable.Get(StringTableIds.Setting_BackToTitle_Text);
        var exitGameButtonText = exitGameButton.GetComponentInChildren<TextMeshProUGUI>();
        if (exitGameButtonText != null)
            exitGameButtonText.text = stringTable.Get(StringTableIds.Setting_Quit_Text);

        titleButton.onClick.AddListener( () => {
            if (selectedLanguage != Variables.SaveData.CurrentLang)
            {
                Variables.SaveData.CurrentLang = selectedLanguage;
                SaveLoadSystem.Save(Variables.SaveData);
                Variables.isStartGame = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                uiManager.ChangeUiState(UiStates.Title);
            }
        });
        exitGameButton.onClick.AddListener( () => {
            Application.Quit();
        });
    }

    public void ChangeLanguage(bool isNext)
    {
        selectedLanguage += isNext ? 1 : -1;
        if (selectedLanguage <= Languages.None)
            selectedLanguage = Languages.Count - 1;
        if (selectedLanguage >= Languages.Count)
            selectedLanguage = Languages.None + 1;

        selectedlanguageText.text = selectedLanguage.ToString();
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
