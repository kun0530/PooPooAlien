using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHelp : MonoBehaviour
{
    public List<GameObject> helpImages;
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
                helpImages[i].SetActive(true);
            }
            else
            {
                helpImages[i].SetActive(false);
            }
        }
    }
}
