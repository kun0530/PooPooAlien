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
    public EnhanceStatPanel enhanceStatPanel;

    private void Awake()
    {
        enhanceTable = DataTableManager.Get<EnhanceTable>(DataTableIds.Enhance);
        for (int i = 0; i < (int)PlayerStat.Count; i++)
        {
            var panel = Instantiate(enhanceStatPanel);
            panel.transform.SetParent(content.transform, false);
            panel.Data = enhanceTable.Get((PlayerStat)i);

            var button = panel.GetComponent<Button>();
            // button.onClick.AddListener( () => {
            //     // 강화 정보 info 패널에 데이터 띄우고,
            //     // selectedStat에 data 저장하고
            // });
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

        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        goldText.text = string.Format(goldTextFormat, Utils.NumberToString(Variables.SaveData.Gold));
    }
}
