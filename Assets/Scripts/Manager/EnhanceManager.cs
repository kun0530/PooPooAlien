using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceManager : MonoBehaviour
{
    private EnhanceTable enhanceTable;

    public TextMeshProUGUI goldText;
    private string goldTextFormat = "{0}";

    public ScrollRect scrollRect;
    public GameObject content;
    public EnhanceStatPanel enhanceStatPanel;

    private void Awake()
    {
        enhanceTable = DataTableManager.Get<EnhanceTable>(DataTableIds.Enhance);
        for (int i = 0; i < (int)PlayerStat.Count; i++)
        {
            var panel = Instantiate(enhanceStatPanel);
            panel.transform.SetParent(content.transform, false);
            panel.Data = enhanceTable.Get((PlayerStat)i);
        }
    }

    private void Start()
    {
        scrollRect.verticalNormalizedPosition = 1f;

        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        goldText.text = string.Format(goldTextFormat, Utils.NumberToString(Variables.SaveData.Gold));
    }
}
