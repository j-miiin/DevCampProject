using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanelUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private ScrollRect scrollRect;

    private void OnEnable()
    {
        AchievementManager.instance.SaveAchievementDictionary();
        SetAchievementList();
    }

    public void SetAchievementList()
    {
        List<Achievement> achievementList = AchievementManager.instance.GetAchievementList();

        int slotCnt = container.childCount;
        int dataCnt = achievementList.Count;

        if (slotCnt < dataCnt)
        {
            for (int i = 0; i < (dataCnt - slotCnt); i++)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/AchievementSlot"));
                obj.transform.SetParent(container);
                obj.transform.localScale = Vector3.one;
            }
        }

        for (int i = 0; i < dataCnt; i++)
        {
            GameObject obj = container.GetChild(i).gameObject;
            obj.SetActive(true);
            AchievementSlotUI slot = obj.GetComponent<AchievementSlotUI>();
            slot.SetSlotUI(achievementList[i]);
            slot.OnComplete += SetAchievementList;
            slot.SetScrollRect(scrollRect);
        }

        for (int i = dataCnt; i < slotCnt; i++)
        {
            GameObject obj = container.GetChild(i).gameObject;
            obj.SetActive(false);
        }
    }
}
