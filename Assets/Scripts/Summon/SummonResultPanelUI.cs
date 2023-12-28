using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonResultPanelUI : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform container;

    public void SetResultList(List<Equipment> equipmentList)
    {
        container.anchoredPosition = new Vector3(container.anchoredPosition.x, 0, 0);

        int slotCnt = container.childCount;
        int dataCnt = equipmentList.Count;

        if (slotCnt < dataCnt)
        {
            for (int i = 0; i < (dataCnt - slotCnt); i++)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/SummonResultSlot"));
                obj.transform.SetParent(container);
                obj.transform.localScale = Vector3.one;
            }
        }

        for (int i = 0; i < dataCnt; i++)
        {
            GameObject obj = container.GetChild(i).gameObject;
            obj.SetActive(true);
            obj.GetComponent<SummonResultSlotUI>().SetSummonResultSlotUI(equipmentList[i]);
        }

        for (int i = dataCnt; i < slotCnt; i++)
        {
            GameObject obj = container.GetChild(i).gameObject;
            obj.SetActive(false);
        }
    }
}
