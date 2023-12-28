using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "SO/AchievementData", order = 0)]
[Serializable]
public class AchievementDataSO : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private AchievementType achievementType;
    [SerializeField] private int requiredAchievementValue;
    [SerializeField] private RewardType rewardType;
    [SerializeField] private int rewardValue;

    public string ID => id;
    public AchievementType AchievementType => achievementType;
    public int RequiredAchievementValue => requiredAchievementValue;
    public RewardType RewardType => rewardType;
    public int RewardValue => rewardValue;
}
