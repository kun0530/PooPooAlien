using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceStatPanel : MonoBehaviour
{
    private EnhanceManager enhanceManager;

    public Image statIcon;

    private string statNameFormat = "{0}\n<size=12>{1} / {2}</size>";
    public TextMeshProUGUI statName;

    public TextMeshProUGUI enhanceEnableText;

    private EnhanceData data;
    public EnhanceData Data{
        get { return data; }
        set {
            if (value == null)
                return;

            data = value;

            statIcon.sprite = data.GetIcon();

            int currentLevel = Variables.SaveData.EnhanceStatData[(PlayerStat)data.Stat];
            statName.text = string.Format(statNameFormat, data.GetName(), currentLevel, data.MaxLevel);

            if (currentLevel >= data.MaxLevel)
            {
                Variables.SaveData.EnhanceStatData[(PlayerStat)data.Stat] = data.MaxLevel;
                enhanceEnableText.enabled = false;
                return;
            }

            float requiredGold = data.RequiredGold + data.RequiredGoldIncrease * currentLevel;
            if (Variables.SaveData.Gold >= requiredGold)
            {
                enhanceEnableText.enabled = true;
            }
            else
            {
                enhanceEnableText.enabled = false;
            }
        }
    }

    private void Awake()
    {
        enhanceManager = GameObject.FindWithTag("EnhanceManager").GetComponent<EnhanceManager>();
    }

    private void Start()
    {
        var stringTable = DataTableManager.GetStringTable();
        enhanceEnableText.text = stringTable.Get(StringTableIds.Enhance_Possible_Text);
    }
}