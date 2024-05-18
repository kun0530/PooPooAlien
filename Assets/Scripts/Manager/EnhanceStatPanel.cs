using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceStatPanel : MonoBehaviour
{
    private EnhanceManager enhanceManager;

    public TextMeshProUGUI statName;
    public Button enhanceButton;

    public EnhanceData data;
    public EnhanceData Data{
        get { return data; }
        set {
            data = value;
            statName.text = data.Name;
            enhanceButton.onClick.AddListener(() => {
                int currentLevel = Variables.SaveData.EnhanceStatData[(PlayerStat)data.Stat];
                float requiredGold = data.RequiredGold + data.RequiredGoldIncrease * currentLevel;

                Logger.Log($"현재 레벨: {currentLevel}");
                Logger.Log($"필요 금액: {Utils.NumberToString(requiredGold)}");

                if (Variables.SaveData.Gold < requiredGold)
                {
                    Logger.Log("돈 없음");
                    return;
                }

                if (currentLevel >= data.MaxLevel)
                {
                    Logger.Log("최대 레벨");
                    return;
                }

                Variables.SaveData.Gold -= requiredGold;
                currentLevel = ++Variables.SaveData.EnhanceStatData[(PlayerStat)data.Stat];
                Logger.Log($"강화: {currentLevel}");
                Logger.Log($"남은 금액: {Utils.NumberToString(Variables.SaveData.Gold)}");
                enhanceManager.UpdateGoldText();
            });
        }
    }

    private void Awake()
    {
        enhanceManager = GameObject.FindWithTag("EnhanceManager").GetComponent<EnhanceManager>();
    }
}