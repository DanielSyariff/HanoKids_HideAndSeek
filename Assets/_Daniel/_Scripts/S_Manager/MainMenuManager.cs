using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StageData
{
    public int index;
    public int starCompletion;
    public int stageStatus;
}
[System.Serializable]
public class StageDataList
{
    public List<StageData> stages;
}

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }
    public List<LevelData> levelData;
    public List<StageData> stageData;

    public StageData selectedStage;

    public StageCard prefabCard;

    private string saveFilePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        PlayerPrefs.SetInt("FromGameplay", 0);
        CheckingData();
    }

    #region Save and Load JSON
    public void CheckingData()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "stageData.json");

        if (!File.Exists(saveFilePath))
        {
            InitializeStageData();
            SaveStageData();
        }
        else
        {
            LoadStageData();
            SyncStageDataWithLevelData();
            SaveStageData();
        }

        InstantiateStageData();
    }

    void InitializeStageData()
    {
        stageData = new List<StageData>();

        for (int i = 0; i < levelData.Count; i++)
        {
            StageData data = new StageData
            {
                index = i,
                starCompletion = 0,
                stageStatus = i == 0 ? 1 : 0
            };

            stageData.Add(data);
        }
    }

    void SyncStageDataWithLevelData()
    {
        int levelCount = levelData.Count;
        int stageCount = stageData.Count;

        if (stageCount < levelCount)
        {
            // Add missing StageData entries
            for (int i = stageCount; i < levelCount; i++)
            {
                StageData data = new StageData
                {
                    index = i,
                    starCompletion = 0,
                    stageStatus = i == 0 ? 1 : 0
                };
                stageData.Add(data);
            }
        }
        else if (stageCount > levelCount)
        {
            stageData.RemoveRange(levelCount, stageCount - levelCount);
        }
        for (int i = 0; i < stageData.Count; i++)
        {
            if (stageData[i].stageStatus != 1)
            {
                stageData[i].stageStatus = i == 0 ? 1 : 0;
            }

            stageData[i].index = i;
        }
    }

    public void SaveStageData()
    {
        string json = JsonUtility.ToJson(new StageDataList { stages = stageData }, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadStageData()
    {
        string json = File.ReadAllText(saveFilePath);
        StageDataList dataList = JsonUtility.FromJson<StageDataList>(json);
        stageData = dataList.stages;
    }

    public void InstantiateStageData()
    {
        ReInvokeData re = GameObject.Find("Main Camera").GetComponent<ReInvokeData>();

        for (int i = 0; i < stageData.Count; i++)
        {
            GameObject tmpLevel = Instantiate(prefabCard.gameObject, re.cardParent);
            StageCard stageCard = tmpLevel.GetComponent<StageCard>();
            stageCard.SetStageData(stageData[i], i + 1);
        }
    }
    #endregion
    #region Play Games
    public void StartGame(StageData selected)
    {
        selectedStage = selected;
        SceneManager.LoadScene("Gameplay");
    }
    public void UpdateSelected(int star)
    {
        if (selectedStage.starCompletion < star)
        {
            selectedStage.starCompletion = star;
        }

        if (HasNextIndex(selectedStage.index))
        {
            stageData[selectedStage.index + 1].stageStatus = 1;
        }

        SaveStageData();
    }

    bool HasNextIndex(int index)
    {
        return index + 1 < stageData.Count;
    }
    #endregion
}
