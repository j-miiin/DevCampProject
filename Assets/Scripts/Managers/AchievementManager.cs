using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AchievementManager : SerializedMonoBehaviour
{
    public static AchievementManager instance;

    [SerializeField] private Dictionary<AchievementType, List<Achievement>> achievementDic;

    private void Awake()
    {
        instance = this;
    }

    public void InitAchievementManager()
    {
        if (!LoadAchievementList()) CreateAchievementList();
    }

    public void UpdateAchievement(AchievementType achievementType, int achievementValue)
    {
        if (!achievementDic.ContainsKey(achievementType)) return;

        foreach (Achievement achievement in achievementDic[achievementType])
        {
            achievement.UpdateAchievementValue(achievementValue);
            if (achievement.isCompleted)
            {
                // UI 업데이트
            }
        }
    }

    // 업적 정보 리스트 생성
    public void CreateAchievementList()
    {
        AchievementDataSO[] dataSOList = Resources.LoadAll<AchievementDataSO>("ScriptableObjects/Achievements");
        for (int i = 0; i < dataSOList.Length; i++)
        {
            AchievementType key = dataSOList[i].AchievementType;
            if (!achievementDic.ContainsKey(key)) achievementDic.Add(key, new List<Achievement>());
            achievementDic[key].Add(new Achievement(dataSOList[i]));
        }
    }

    // 업적 정보 리스트 저장
    public void SaveAchievementList()
    {
        ES3.Save<Dictionary<AchievementType, List<Achievement>>>("achievementDic", achievementDic);

    }

    // 업적 정보 리스트 로드
    public bool LoadAchievementList()
    {
        if (ES3.KeyExists("achievementDic"))
            achievementDic = ES3.Load<Dictionary<AchievementType, List<Achievement>>>("achievementDic");
        else return false;

        return true;
    }

}
