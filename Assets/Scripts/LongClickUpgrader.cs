using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongClickUpgrader : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public StatusType btnType;
    private bool isClicked;
    private bool isStartUpgrade;
    private float clickTime;

    private void Update()
    {
        if (isClicked)
        {
            clickTime += Time.deltaTime;
            if (clickTime > 2f && !isStartUpgrade)
            {
                StartCoroutine(COUpgradeStat());
                isStartUpgrade = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        isStartUpgrade = false;
        clickTime = 0;
        StopAllCoroutines();
    }

    private IEnumerator COUpgradeStat()
    {
        WaitForSeconds interval = new WaitForSeconds(0.3f);
        while (true)
        {
            StatusUpgradeManager.instance.UpgradeStat(btnType);
            yield return interval;
        }
    }

}
