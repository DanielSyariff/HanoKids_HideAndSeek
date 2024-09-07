using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private float timeLimit = 0;                       
    [SerializeField] private int maxHiddenObjectToFound = 6;            
    [SerializeField] private ObjectHolder objectHolderPrefab;           

    [HideInInspector] public GameStatus gameStatus = GameStatus.NEXT;   
    private List<HiddenObjectData> activeHiddenObjectList;              
    private float currentTime;                                          
    private int totalHiddenObjectsFound = 0;                            
    private TimeSpan time;                                              
    private RaycastHit2D hit;
    private Vector3 pos;                                               

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
    }

    void Start()
    {
        activeHiddenObjectList = new List<HiddenObjectData>();          
        AssignHiddenObjects();
    }

    void AssignHiddenObjects()  
    {
        ObjectHolder objectHolder = Instantiate(objectHolderPrefab, Vector3.zero, Quaternion.identity);
        totalHiddenObjectsFound = 0;                                        
        activeHiddenObjectList.Clear();                                     
        gameStatus = GameStatus.PLAYING;                                    
        GameplayUIManager.instance.TimerText.text = "" + timeLimit;                 
        currentTime = timeLimit;                                            

        for (int i = 0; i < objectHolder.HiddenObjectList.Count; i++)       
        {
            objectHolder.HiddenObjectList[i].hiddenObj.GetComponent<Collider2D>().enabled = false;
        }

        int k = 0; //int to keep count
        while (k < maxHiddenObjectToFound) 
        {
            int randomNo = UnityEngine.Random.Range(0, objectHolder.HiddenObjectList.Count);

            if (!objectHolder.HiddenObjectList[randomNo].makeHidden)
            {

                objectHolder.HiddenObjectList[randomNo].hiddenObj.name = "" + k;    

                objectHolder.HiddenObjectList[randomNo].makeHidden = true;          
                                                                                    
                objectHolder.HiddenObjectList[randomNo].hiddenObj.GetComponent<Collider2D>().enabled = true;
                activeHiddenObjectList.Add(objectHolder.HiddenObjectList[randomNo]);
                k++;                                                                
            }
        }

        GameplayUIManager.instance.PopulateHiddenObjectIcons(activeHiddenObjectList);   
        gameStatus = GameStatus.PLAYING;                                        
    }

    private void Update()
    {
        if (gameStatus == GameStatus.PLAYING)                               
        {
            if (Input.GetMouseButtonDown(0))                                
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  
                hit = Physics2D.Raycast(pos, Vector2.zero);                 
                if (hit && hit.collider != null)                            
                {
                    hit.collider.gameObject.SetActive(false);               
                    
                    GameplayUIManager.instance.CheckSelectedHiddenObject(hit.collider.gameObject.name); 

                    for (int i = 0; i < activeHiddenObjectList.Count; i++)
                    {
                        if (activeHiddenObjectList[i].hiddenObj.name == hit.collider.gameObject.name)
                        {
                            activeHiddenObjectList.RemoveAt(i);
                            break;
                        }
                    }

                    totalHiddenObjectsFound++;                              

                    if (totalHiddenObjectsFound >= maxHiddenObjectToFound)
                    {
                        Debug.Log("You won the game");                      
                        GameplayUIManager.instance.GameCompleteObj.SetActive(true); 
                        gameStatus = GameStatus.NEXT;                       
                    }
                }
            }

            currentTime -= Time.deltaTime;  

            time = TimeSpan.FromSeconds(currentTime);                      
            GameplayUIManager.instance.TimerText.text = time.ToString("mm':'ss");   
            if (currentTime <= 0)                                           
            {
                Debug.Log("Time Up");                                      
                GameplayUIManager.instance.GameCompleteObj.SetActive(true);         
                gameStatus = GameStatus.NEXT;                              
            }
        }
    }

    public IEnumerator HintObject() 
    {
        int randomValue = UnityEngine.Random.Range(0, activeHiddenObjectList.Count);
        Vector3 originalScale = activeHiddenObjectList[randomValue].hiddenObj.transform.localScale;
        activeHiddenObjectList[randomValue].hiddenObj.transform.localScale = originalScale * 1.25f;
        yield return new WaitForSeconds(0.25f);
        activeHiddenObjectList[randomValue].hiddenObj.transform.localScale = originalScale;
    }
}

[System.Serializable]
public class HiddenObjectData
{
    public string name;
    public GameObject hiddenObj;
    public bool makeHidden = false;
}

public enum GameStatus
{
    PLAYING,
    NEXT
}