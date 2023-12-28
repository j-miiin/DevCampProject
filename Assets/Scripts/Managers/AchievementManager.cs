using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class AchievementManager : SerializedMonoBehaviour
{
    public static AchievementManager instance;

    public event Action<AchievementType> OnTryAchievement;

    [SerializeField] private Dictionary<string, Achievement> achievementDic;
    [SerializeField] private Dictionary<AchievementType, List<Achievement>> achievementWithTypeDic;

    private AchievementDataHandler dataHandler;

    private void Awake()
    {
        instance = this;
    }

    public void InitAchievementManager()
    {
        dataHandler = DataManager.instance.GetDataHandler<AchievementDataHandler>();
        achievementDic = dataHandler.LoadAchievementDictionary();
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

    // 업적 업데이트 (업적 ID, 업데이트 수치)
    public void UpdateAchievement(string id, int achievementValue)
    {
        if (!achievementDic.ContainsKey(id)) return;

        achievementDic[id].UpdateAchievementValue(achievementValue);
    }

    // 업적 업데이트 (업적 타입, 업데이트 수치)
    public void UpdateAchievement(AchievementType achievementType, int achievementValue)
    {
        if (achievementWithTypeDic == null) CreateAchievementWithTypeDictionary();

        if (!achievementWithTypeDic.ContainsKey(achievementType)) return;

        foreach (Achievement achievement in achievementWithTypeDic[achievementType])
        {
            achievement.UpdateAchievementValue(achievementValue);
        }
    }

    // 업적 달성
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

        SaveAchievementData();
    }

    public void TryAchieve(AchievementType type)
    {
        OnTryAchievement?.Invoke(type);
    }

    // 타입 별 업적 Dictionary 생성
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

    public void SaveAchievementData()
    {
        dataHandler.SaveAchievementDictionary();
    }
}
