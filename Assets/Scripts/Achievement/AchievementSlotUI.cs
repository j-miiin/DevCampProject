using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSlotUI : MonoBehaviour
{
    private readonly string ACHIEVEMENT_ENHANCE_EQUIPMENT = "장비 강화";
    private readonly string ACHIEVEMENT_SUMMON_EQUIPMENT = "장비 소환";
    private readonly string ACHIEVEMENT_UPGRADE_STAT = "스탯 업그레이드";

    private readonly string REWARD_DIA = "다이아";
    private readonly string REWARD_ATK_STAT = "공격력";

    private readonly string GET_BUTTON_TEXT = "획득";
    private readonly string COMPLETE_BUTTON_TEXT = "완료";

    public event Action OnComplete;

    [SerializeField] private TMP_Text achievementText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text achievementValueText;
    [SerializeField] private Slider achievementSlider;
    [SerializeField] private Button getRewardButton;
    private TMP_Text getRewardButtonText;

    private string id;

    private void Awake()
    {
        getRewardButtonText = getRewardButton.GetComponentInChildren<TMP_Text>();
        getRewardButton.onClick.AddListener(UpdateAchievementComplete);
    }

    public void SetSlotUI(Achievement achievement)
    {
        AchievementDataSO dataSO = achievement.achievementDataSO;
        id = dataSO.ID;
        switch (dataSO.AchievementType)
        {
            case AchievementType.EnhanceEquipment:
                achievementText.text = ACHIEVEMENT_ENHANCE_EQUIPMENT;
                break;
            case AchievementType.SummonEquipment:
                achievementText.text = ACHIEVEMENT_SUMMON_EQUIPMENT;
                break;
            case AchievementType.UpgradeStat:
                achievementText.text = ACHIEVEMENT_UPGRADE_STAT;
                break;
        }

        if (achievement.isCompleted)
        {
            rewardText.gameObject.SetActive(false);
            achievementValueText.gameObject.SetActive(false);
            achievementSlider.gameObject.SetActive(false);
            getRewardButtonText.text = COMPLETE_BUTTON_TEXT;
            getRewardButton.interactable = false;
        } else
        {
            rewardText.gameObject.SetActive(true);
            achievementValueText.gameObject.SetActive(true);
            achievementSlider.gameObject.SetActive(true);
            switch (dataSO.RewardType)
            {
                case RewardType.Dia:
                    rewardText.text = $"{REWARD_DIA} {dataSO.RewardValue}개";
                    break;
                case RewardType.ATK_Stat:
                    rewardText.text = $"{REWARD_ATK_STAT} {dataSO.RewardValue} 증가";
                    break;
            }
            achievementValueText.text = $"{achievement.curAchievementValue}/{dataSO.RequiredAchievementValue}";
            achievementSlider.maxValue = dataSO.RequiredAchievementValue;
            achievementSlider.value = achievement.curAchievementValue;
            getRewardButtonText.text = GET_BUTTON_TEXT;
            getRewardButton.interactable = (achievement.curAchievementValue >= dataSO.RequiredAchievementValue);
        }
    }

    public void UpdateAchievementComplete()
    {
        AchievementManager.instance.CompleteAchievement(id);
        OnComplete?.Invoke();
    }
}
