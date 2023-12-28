using System.Collections.Generic;
using UnityEngine;

public class AchievementDataHandler : DataHandler
{
    private Dictionary<string, Achievement> achievementDic;

    // 업적 정보 Dictionary 저장
    public void SaveAchievementDictionary()
    {
        ES3.Save<Dictionary<string, Achievement>>("achievementDic", achievementDic);
    }

    // 업적 정보 Dictionary 로드
    public Dictionary<string, Achievement> LoadAchievementDictionary()
    {
        if (ES3.KeyExists("achievementDic"))
        {
            achievementDic = ES3.Load<Dictionary<string, Achievement>>("achievementDic");
            LoadAchievementDataDictionary();
        }
        else
        {
            CreateAchievementDictionary();
        }
        return achievementDic;
    }

    // 업적 정보 Dictionary 생성
    public void CreateAchievementDictionary()
    {
        achievementDic = new Dictionary<string, Achievement>();
        AchievementDataSO[] dataSOList = Resources.LoadAll<AchievementDataSO>("ScriptableObjects/Achievements");
        for (int i = 0; i < dataSOList.Length; i++)
        {
#if UNITY_EDITOR
            if (achievementDic.ContainsKey(dataSOList[i].ID))
                Debug.LogError("Achievement already exist");
#endif
            achievementDic.Add(dataSOList[i].ID, new Achievement(dataSOList[i]));
        }
        LoadAchievementDataDictionary();
        SaveAchievementDictionary();
    }

    // 업적 데이터 SO 로드 및 Dictionary 생성
    public void LoadAchievementDataDictionary()
    {
        AchievementDataSO[] dataSOList = Resources.LoadAll<AchievementDataSO>("ScriptableObjects/Achievements");
        for (int i = 0; i < dataSOList.Length; i++)
        {
            string key = dataSOList[i].ID;
            if (achievementDic.ContainsKey(key))
                achievementDic[key].achievementDataSO = dataSOList[i];
        }
    }
}
