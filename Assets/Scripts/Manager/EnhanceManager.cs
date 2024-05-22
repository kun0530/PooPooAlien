using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceManager : MonoBehaviour
{
    public TitleUiManager uiManager;

    private EnhanceTable enhanceTable;

    public TextMeshProUGUI goldText;
    private string goldTextFormat = "{0}K";
    
    public Button exitButton;

    public ScrollRect scrollRect;
    public GameObject content;
    public EnhanceStatPanel enhanceStatPanelPrefab;
    private Dictionary<PlayerStat, EnhanceStatPanel> enhanceStats = new Dictionary<PlayerStat, EnhanceStatPanel>();

    public Image selectedStatIcon; 
    public TextMeshProUGUI selecetedStatNameText;
    public TextMeshProUGUI selecetedStatLevelText;
    private string levelFormat = "{0} / {1}";
    public TextMeshProUGUI selecetedStatDescText;

    private EnhanceData selectedEnhanceData;
    public EnhanceData SelectedEnhanceData{
        get{ return selectedEnhanceData; }
        set{
            if (value == null)
                return;

            selectedEnhanceData = value;
            selectedStatIcon.enabled = true;
            selectedStatIcon.sprite = selectedEnhanceData.GetIcon();

            selecetedStatNameText.enabled = true;
            selecetedStatNameText.text = selectedEnhanceData.Name;
            selecetedStatLevelText.enabled = true;
            selecetedStatLevelText.text = string.Format(levelFormat, Variables.SaveData.EnhanceStatData[(PlayerStat)SelectedEnhanceData.Stat], selectedEnhanceData.MaxLevel);
            selecetedStatDescText.enabled = true;
            selecetedStatDescText.text = selectedEnhanceData.Desc;
        }
    }

    private void Awake()
    {
        enhanceTable = DataTableManager.Get<EnhanceTable>(DataTableIds.Enhance);
        for (int i = 0; i < (int)PlayerStat.Count; i++)
        {
            var panel = Instantiate(enhanceStatPanelPrefab);
            panel.transform.SetParent(content.transform, false);
            panel.Data = enhanceTable.Get((PlayerStat)i);

            var button = panel.GetComponent<Button>();
            button.onClick.AddListener( () => {
                SelectedEnhanceData = panel.Data;
            });

            enhanceStats.Add((PlayerStat)i, panel);
        }
    }

    private void Start()
    {
        scrollRect.verticalNormalizedPosition = 1f;

        var buttonImage = exitButton.GetComponent<Image>();
        if (buttonImage != null)
            buttonImage.alphaHitTestMinimumThreshold = 0.1f;

        exitButton.onClick.AddListener(() => {
            uiManager.Status = UiStatus.StageSelect;
        });

        selectedStatIcon.enabled = false;
        selecetedStatNameText.enabled = false;
        selecetedStatLevelText.enabled = false;
        selecetedStatDescText.enabled = false;

        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        goldText.text = string.Format(goldTextFormat, Utils.NumberToString(Variables.SaveData.Gold));
    }

    public void EnhanceStat()
    {
        if (SelectedEnhanceData == null)
            return;
        
        int currentLevel = Variables.SaveData.EnhanceStatData[(PlayerStat)SelectedEnhanceData.Stat];
        float requiredGold = SelectedEnhanceData.RequiredGold + SelectedEnhanceData.RequiredGoldIncrease * currentLevel;

        Logger.Log($"현재 레벨: {currentLevel}");
        Logger.Log($"필요 금액: {Utils.NumberToString(requiredGold)}");

        if (Variables.SaveData.Gold < requiredGold)
        {
            Logger.Log("돈 없음");
            return;
        }

        if (currentLevel >= SelectedEnhanceData.MaxLevel)
        {
            Logger.Log("최대 레벨");
            return;
        }

        Variables.SaveData.Gold -= requiredGold;
        currentLevel = ++Variables.SaveData.EnhanceStatData[(PlayerStat)SelectedEnhanceData.Stat];
        Logger.Log($"강화: {currentLevel}");
        Logger.Log($"남은 금액: {Utils.NumberToString(Variables.SaveData.Gold)}");
        UpdateGoldText();

        foreach (var enhanceStat in enhanceStats)
        {
            if (enhanceStat.Value != null)
                enhanceStat.Value.Data = enhanceStat.Value.Data;
        }

        SaveLoadSystem.Save(Variables.SaveData);
    }
}
