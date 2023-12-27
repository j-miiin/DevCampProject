using System;
using UnityEngine;

public enum AchievementType
{
    UpgradeStat,
    SummonEquipment,
    EnhanceEquipment,
}

public enum RewardType
{
    Gold,
    Dia,
    ATK_Stat,
}

public class Achievement : MonoBehaviour
{
    public AchievementDataSO achievementDataSO;
    public int curAchievementValue;
    public bool isCompleted;

    public Achievement(AchievementDataSO dataSO)
    {
        achievementDataSO = dataSO;
        curAchievementValue = 0;
        isCompleted = false;
    }

    public void UpdateAchievementValue(int value)
    {
        curAchievementValue += value;
        if (curAchievementValue >= achievementDataSO.RequiredAchievementValue)
            isCompleted = true;
    }
}
