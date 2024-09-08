using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReInvokeData : MonoBehaviour
{
    [Header("Panel")]
    public GameObject mainMenuPanel;
    public GameObject stageSelectionPanel;
    public Transform cardParent;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("FromGameplay", 0) == 1)
        {
            PlayerPrefs.SetInt("FromGameplay", 0);
            MainMenuManager.Instance.CheckingData();
            GoToStageSelection();
        }   
    }
    public void GoToStageSelection()
    {
        mainMenuPanel.SetActive(false);
        stageSelectionPanel.SetActive(true);
    }
}
