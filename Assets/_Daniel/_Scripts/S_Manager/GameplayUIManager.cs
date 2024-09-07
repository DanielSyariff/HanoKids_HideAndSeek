using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager instance;

    [SerializeField] private GameObject hiddenObjectIconHolder;     
    [SerializeField] private GameObject hiddenObjectIconPrefab;     
    [SerializeField] private GameObject gameCompleteObj;            
    [SerializeField] private TextMeshProUGUI timerText;                        

    private List<GameObject> hiddenObjectIconList;                  

    public GameObject GameCompleteObj { get => gameCompleteObj; }   
    public TextMeshProUGUI TimerText { get => timerText; }                     

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        hiddenObjectIconList = new List<GameObject>();              
    }

    /// <param name="hiddenObjectData">Data of active hidden objects</param>
    public void PopulateHiddenObjectIcons(List<HiddenObjectData> hiddenObjectData)
    {
        hiddenObjectIconList.Clear();                               
        for (int i = 0; i < hiddenObjectData.Count; i++)           
        {
            GameObject icon = Instantiate(hiddenObjectIconPrefab, hiddenObjectIconHolder.transform);
            icon.name = hiddenObjectData[i].hiddenObj.name;         
            Image childImg = icon.transform.GetChild(0).GetComponent<Image>();  
            TextMeshProUGUI childText = icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>(); 

            childImg.sprite = hiddenObjectData[i].hiddenObj.GetComponent<SpriteRenderer>().sprite; 
            childText.text = hiddenObjectData[i].name;                          
            hiddenObjectIconList.Add(icon);                                    
        }
    }

    /// <param name="index">Name of hidden object</param>
    public void CheckSelectedHiddenObject(string index)
    {
        for (int i = 0; i < hiddenObjectIconList.Count; i++)                    
        {
            if (index == hiddenObjectIconList[i].name)                          
            {
                hiddenObjectIconList[i].SetActive(false);                       
                break;                                                          
            }
        }
    }

    public void NextButton()                                                   
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);       
    }

    public void HintButton()
    {
        StartCoroutine(LevelManager.instance.HintObject());
    }
}
