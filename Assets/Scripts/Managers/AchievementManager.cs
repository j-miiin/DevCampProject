using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class AchievementManager : SerializedMonoBehaviour
{
    public static AchievementManager instance;

    [SerializeField] private Dictionary<string, Achievement> achievementDic;
    [SerializeField] private Dictionary<AchievementType, List<Achievement>> achievementWithTypeDic;

    private void Awake()
    {
        instance = this;
    }

    public void InitAchievementManager()
    {
        if (ES3.KeyExists("achievementDic"))
        {
            LoadAchievementDictionary();
        }
        else
        {
            CreateAchievementDictionary();
        }
    }

    public List<Achievement> GetAchievementList()
    {
        List<Achievement> achievementList = achievementDic.Values.ToList();
        achievementList.Sort((a, b) =>
        {
            if (a.isCompleted && !b.isCompleted) return 1;
            else if (!a.isCompleted && b.isCompleted) return -1;
            else
            {
                if (((float)a.curAchievementValue / a.achievementDataSO.RequiredAchievementValue)
                >= ((float)b.curAchievementValue / b.achievementDataSO.RequiredAchievementValue)) return -1;
                else return 1;
            }
        });
        return achievementList;
    }

    public Achievement GetAchievement(string id)
    {
        if (!achievementDic.ContainsKey(id)) return null;

        return achievementDic[id];
    }

    public void UpdateAchievement(string id, int achievementValue)
    {
        if (!achievementDic.ContainsKey(id)) return;

        achievementDic[id].UpdateAchievementValue(achievementValue);
    }

    public void UpdateAchievement(AchievementType achievementType, int achievementValue)
    {
        if (achievementWithTypeDic == null) CreateAchievementWithTypeDictionary();

        if (!achievementWithTypeDic.ContainsKey(achievementType)) return;

        foreach (Achievement achievement in achievementWithTypeDic[achievementType])
        {
            achievement.UpdateAchievementValue(achievementValue);
        }
    }

    public void CompleteAchievement(string id)
    {
        if (!achievementDic.ContainsKey(id)) return;

        achievementDic[id].CompleteAchievement();

        // 보상 제공 로직
        AchievementDataSO dataSO = achievementDic[id].achievementDataSO;
        switch (dataSO.RewardType)
        {
            case RewardType.Dia:
                CurrencyManager.instance.AddCurrency("Dia", dataSO.RewardValue);
                break;
            case RewardType.ATK_Stat:
                Debug.Log($"보상 전 공격력 : {Player.instance.GetCurrentStatus(StatusType.ATK)}");
                Player.instance.UpdateBaseStat(StatusType.ATK, dataSO.RewardValue);
                Debug.Log($"보상 후 공격력 : {Player.instance.GetCurrentStatus(StatusType.ATK)}");
                break;
        }

        SaveAchievementDictionary();
    }

    // 업적 정보 리스트 생성
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

    public void CreateAchievementWithTypeDictionary()
    {
        achievementWithTypeDic = new Dictionary<AchievementType, List<Achievement>>();
        foreach (Achievement achievement in achievementDic.Values)
        {
            AchievementType key = achievement.achievementDataSO.AchievementType;
            if (!achievementWithTypeDic.ContainsKey(key)) achievementWithTypeDic.Add(key, new List<Achievement>());
            achievementWithTypeDic[key].Add(achievement);
        }
    }

    // 업적 정보 리스트 저장
    public void SaveAchievementDictionary()
    {
        ES3.Save<Dictionary<string, Achievement>>("achievementDic", achievementDic);

    }

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

    // 업적 정보 리스트 로드
    public bool LoadAchievementDictionary()
    {
        if (ES3.KeyExists("achievementDic"))
            achievementDic = ES3.Load<Dictionary<string, Achievement>>("achievementDic");
        else return false;
        LoadAchievementDataDictionary();
        return true;
    }
}
