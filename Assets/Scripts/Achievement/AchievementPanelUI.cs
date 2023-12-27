using System.Collections.Generic;
using UnityEngine;

public class AchievementPanelUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;

    public void SetAchievementList(List<Achievement> achievementList)
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/AchievementSlot"));
            obj.transform.SetParent(container);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<AchievementSlotUI>().SetSlotUI(achievementList[i]);
        }
    }
}
