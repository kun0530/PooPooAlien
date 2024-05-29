using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHelp : MonoBehaviour
{
    public List<Image> helpImages;
    public Button exitButton;
    private int currentPage;
    private int CurrentPage
    {
        get { return currentPage; }
        set
        {
            currentPage = value;
            GoToPage(currentPage);
        }
    }

    private void OnEnable()
    {
        var lang = Variables.SaveData.CurrentLang;
        var filePaths = ResourcesPath.HelpUiResouces[(int)lang];
        string resourcesPathFormat = "HelpUIResource/{0}";
        for (int i = 0; i < helpImages.Count; i++)
        {
            helpImages[i].sprite = Resources.Load<Sprite>(string.Format(resourcesPathFormat, filePaths[i]));
        }

        CurrentPage = 0;
    }

    private void Start()
    {
        var buttonImage = exitButton.GetComponent<Image>();
        if (buttonImage != null)
            buttonImage.alphaHitTestMinimumThreshold = 0.1f;
    }

    private void Update()
    {
        if (TouchManager.Instance.Swipe == Directions.Left)
        {
            CurrentPage++;
        }
        else if (TouchManager.Instance.Swipe == Directions.Right)
        {
            CurrentPage--;
        }
    }

    private void GoToPage(int num)
    {
        if (helpImages == null || helpImages.Count == 0)
            return;

        num = Mathf.Clamp(num, 0, helpImages.Count - 1);
        for (int i = 0; i < helpImages.Count; i++)
        {
            if (i == num)
            {
                helpImages[i].gameObject.SetActive(true);
            }
            else
            {
                helpImages[i].gameObject.SetActive(false);
            }
        }

        exitButton.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(helpImages[num].rectTransform.rect.xMax, helpImages[num].rectTransform.rect.yMax);
        var imageSize = Utils.CalculateImageSize(helpImages[num]);
        var buttonImageSize = Utils.CalculateImageSize(exitButton.GetComponent<Image>());
        exitButton.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((imageSize.width - buttonImageSize.width) / 2f, (imageSize.height - buttonImageSize.height) / 2f);
    }
}
