using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageCard : MonoBehaviour
{
    public StageData stageData;
    public TextMeshProUGUI titleText;
    public Button interactButton;
    public GameObject star;
    public GameObject[] starComplete;

    public void SetStageData(StageData data, int level)
    {
        titleText.text = "Level : " + level.ToString();
        stageData = data;
        GetStatus();
        GetStar();
    }

    public void GetStatus()
    {
        if (stageData.stageStatus != 0)
        {
            interactButton.interactable = true;
            star.SetActive(true);
        }
        else
        {
            interactButton.interactable = false;
            star.SetActive(false);
        }
    }
    private void GetStar()
    {
        for (int i = 0; i < stageData.starCompletion; i++)
        {
            starComplete[i].SetActive(true);
        }
    }
    public void GoToGameplay()
    {
        MainMenuManager.Instance.StartGame(stageData);
    }
}
