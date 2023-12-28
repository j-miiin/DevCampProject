using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenuCtrl : SerializedMonoBehaviour
{
    [Header("버튼과 패널")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] panels;

    // 업적 관련 알림 마크
    [SerializeField] private Dictionary<AchievementType, Button> buttonRelatedAchievementDic;
    [SerializeField] private Dictionary<AchievementType, GameObject> achievementAlarmMarkDic;

    private void Start()
    {
        // 각 버튼에 이벤트 리스너 할당
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // 현재 인덱스 캡처
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }

        InitAchievemenetAlarmMark();
        AchievementManager.instance.OnTryAchievement += UpdateAchievementAlarmMark;
    }

    // 버튼 클릭 시 호출되는 메서드
    private void OnButtonClicked(int index)
    {
        // 모든 패널을 순회하면서 상태 설정
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
    }

    private void InitAchievemenetAlarmMark()
    {
        foreach (KeyValuePair<AchievementType, Button> pair in buttonRelatedAchievementDic)
        {
            pair.Value.onClick.AddListener(() => achievementAlarmMarkDic[pair.Key].SetActive(false));
        }
    }

    private void UpdateAchievementAlarmMark(AchievementType type)
    {
        if (!achievementAlarmMarkDic.ContainsKey(type)) return;
        achievementAlarmMarkDic[type].SetActive(true);
    }
}
