using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class StageData
{
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
    public List<LevelData> levelData;
    public List<StageData> stageData;

    private string saveFilePath;

    void Start()
    {
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
    }

    void InitializeStageData()
    {
        stageData = new List<StageData>();

        for (int i = 0; i < levelData.Count; i++)
        {
            StageData data = new StageData
            {
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
    #endregion
}
