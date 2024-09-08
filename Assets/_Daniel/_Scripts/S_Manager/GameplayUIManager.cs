using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Collections;

public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager instance;

    [SerializeField] private GameObject hiddenObjectIconHolder;     
    [SerializeField] private GameObject hiddenObjectIconPrefab;     
    [SerializeField] private GameObject gameCompleteObj;            
    [SerializeField] private TextMeshProUGUI timerText;                        
    [SerializeField] private TextMeshProUGUI coin;
    [SerializeField] private GameObject[] star;
    public AudioClip starSound;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BackToMainMenu();
        }
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

    public void GameComplete(int score)
    {
        GameCompleteObj.SetActive(true);
        StartCoroutine(AnimateStar(score));

        MainMenuManager.Instance.UpdateSelected(score > 4 ? 3 : score);
        
    }
    IEnumerator AnimateStar(int score)
    {
        if (score > 4)
        {
            for (int i = 0; i < star.Length; i++)
            {
                star[i].transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
                AudioManager.instance.PlayOneShot(starSound);
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if (score == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                star[i].transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
                AudioManager.instance.PlayOneShot(starSound);
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if (score == 1)
        {
            for (int i = 0; i < 1; i++)
            {
                star[i].transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
                AudioManager.instance.PlayOneShot(starSound);
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            yield return 0;
        }
    }

    public void UpdateCoin()
    {
        coin.text = "Coins : " + CoinManager.Instance.GetCoin();
    }

    public void NextButton()                                                   
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);       
    }

    public void HintButton()
    {
        StartCoroutine(LevelManager.instance.HintObject());
    }
    public void BackToMainMenu()
    {
        PlayerPrefs.SetInt("FromGameplay", 1);
        SceneManager.LoadScene("MainMenu");
    }
}
