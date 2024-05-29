using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceManager : MonoBehaviour
{
    public TitleUiManager uiManager;

    private EnhanceTable enhanceTable;

    public TextMeshProUGUI titleText;
    public UiNumberIncrease goldText;
    
    public Button exitButton;

    public ScrollRect scrollRect;
    public GameObject content;
    public EnhanceStatPanel enhanceStatPanelPrefab;
    private Dictionary<PlayerStat, EnhanceStatPanel> enhanceStats = new Dictionary<PlayerStat, EnhanceStatPanel>();

    public GameObject enhanceInfo;
    public TextMeshProUGUI selectStatText;
    public Image selectedStatIcon; 
    public TextMeshProUGUI selectedStatNameText;
    public TextMeshProUGUI selectedStatLevelText;
    private string levelFormat = "{0} / {1}";
    public TextMeshProUGUI selectedStatDescText;
    public TextMeshProUGUI selectedStatIncreaseText;
    private string increaseFormat = "{0} ▶ {1}";
    public TextMeshProUGUI selectedStatRequiredGoldText;
    public Button enhanceButton;

    private EnhanceData selectedEnhanceData;
    public EnhanceData SelectedEnhanceData{
        get{ return selectedEnhanceData; }
        set{
            selectedEnhanceData = value;
            UpdateEnhanceInfo();
        }
    }

    private void Awake()
    {
        enhanceTable = DataTableManager.Get<EnhanceTable>(DataTableIds.Enhance);
        for (int i = 0; i < (int)PlayerStat.Count; i++)
        {
            var data = enhanceTable.Get((PlayerStat)i);
            if (data == null)
                continue;

            var panel = Instantiate(enhanceStatPanelPrefab);
            panel.transform.SetParent(content.transform, false);
            panel.Data = data;

            var button = panel.GetComponent<Button>();
            button.onClick.AddListener( () => {
                SelectedEnhanceData = panel.Data;
            });

            enhanceStats.Add((PlayerStat)i, panel);
        }
    }

    private void OnEnable()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        SelectedEnhanceData = null;
    }

    private void Start()
    {
        var stringTable = DataTableManager.GetStringTable();
        titleText.text = stringTable.Get(StringTableIds.Enhance_Title_Text);
        selectStatText.text = stringTable.Get(StringTableIds.Enhance_Choice_Text);

        var buttonImage = exitButton.GetComponent<Image>();
        if (buttonImage != null)
            buttonImage.alphaHitTestMinimumThreshold = 0.1f;

        exitButton.onClick.AddListener(() => {
            uiManager.ChangeUiState(UiStates.StageSelect);
        });

        goldText.CurrentNum = Variables.SaveData.Gold;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.ChangeUiState(UiStates.StageSelect);
        }
    }

    public void EnhanceStat()
    {
        if (SelectedEnhanceData == null)
            return;
        
        int currentLevel = Variables.SaveData.EnhanceStatData[(PlayerStat)SelectedEnhanceData.Stat];
        float requiredGold = SelectedEnhanceData.RequiredGold + SelectedEnhanceData.RequiredGoldIncrease * (currentLevel - 1);

        Logger.Log($"현재 레벨: {currentLevel}");
        Logger.Log($"필요 금액: {Utils.NumberToString(requiredGold)}");

        if (currentLevel >= SelectedEnhanceData.MaxLevel)
        {
            Logger.Log("최대 레벨");
            return;
        }

        if (Variables.SaveData.Gold < requiredGold)
        {
            Logger.Log("돈 없음");
            return;
        }

        Variables.SaveData.Gold -= requiredGold;
        currentLevel = ++Variables.SaveData.EnhanceStatData[(PlayerStat)SelectedEnhanceData.Stat];
        Logger.Log($"강화: {currentLevel}");
        Logger.Log($"남은 금액: {Utils.NumberToString(Variables.SaveData.Gold)}");
        goldText.TargetNum = Variables.SaveData.Gold;

        foreach (var enhanceStat in enhanceStats)
        {
            if (enhanceStat.Value != null)
                enhanceStat.Value.Data = enhanceStat.Value.Data;
        }

        UpdateEnhanceInfo();

        SaveLoadSystem.Save(Variables.SaveData);
    }

    private void UpdateEnhanceInfo()
    {
        if (SelectedEnhanceData == null)
        {
            selectStatText.gameObject.SetActive(true);
            enhanceInfo.SetActive(false);
            return;
        }

        selectStatText.gameObject.SetActive(false);
        enhanceInfo.SetActive(true);

        var currentLevel = Variables.SaveData.EnhanceStatData[(PlayerStat)SelectedEnhanceData.Stat];
        var maxLevel = SelectedEnhanceData.MaxLevel;
        var currentStat = Variables.CalculateCurrentSaveStat((PlayerStat)SelectedEnhanceData.Stat);
        var nextStat = Variables.CalculateStat((PlayerStat)SelectedEnhanceData.Stat, currentLevel + 1);
        float requiredGold = SelectedEnhanceData.RequiredGold + SelectedEnhanceData.RequiredGoldIncrease * (currentLevel - 1);

        selectedStatIcon.sprite = SelectedEnhanceData.GetIcon();
        selectedStatNameText.text = SelectedEnhanceData.GetName();
        selectedStatLevelText.text = string.Format(levelFormat, currentLevel, maxLevel);
        selectedStatDescText.text = SelectedEnhanceData.GetDesc();
        if (currentLevel >= SelectedEnhanceData.MaxLevel)
        {
            selectedStatIncreaseText.text = $"{currentStat}";
            selectedStatRequiredGoldText.text = DataTableManager.GetStringTable().Get(StringTableIds.Enhance_Complete_Text);
            enhanceButton.interactable = false;
        }
        else
        {
            selectedStatIncreaseText.text = string.Format(increaseFormat, currentStat, nextStat);
            selectedStatRequiredGoldText.text = string.Format("{0} Gold", requiredGold);
            enhanceButton.interactable = Variables.SaveData.Gold >= requiredGold;
        }
    }
}
