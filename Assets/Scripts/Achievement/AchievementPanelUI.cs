using System.Collections.Generic;
using UnityEngine;

public class AchievementPanelUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;

    private void OnEnable()
    {
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
            AchievementSlotUI slot = obj.GetComponent<AchievementSlotUI>();
            slot.SetSlotUI(achievementList[i]);
            slot.OnComplete += SetAchievementList;
        }

        for (int i = dataCnt; i < slotCnt; i++)
        {
            GameObject obj = container.GetChild(i).gameObject;
            obj.SetActive(false);
        }
    }
}
