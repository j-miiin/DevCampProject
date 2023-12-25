using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class LongClickAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isClicked;
    private bool isStartUpgrade;
    private float clickTime;
    private Coroutine curCoroutine;

    private void Update()
    {
        if (isClicked)
        {
            clickTime += Time.deltaTime;
            if (clickTime > 2f && !isStartUpgrade)
            {
                curCoroutine = StartCoroutine(COUpgradeAction());
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
        StopCoroutine(curCoroutine);
        curCoroutine = null;
    }

    private IEnumerator COUpgradeAction()
    {
        WaitForSeconds interval = new WaitForSeconds(0.3f);
        while (true)
        {
            UpgradeAction();
            yield return interval;
        }
    }

    protected abstract void UpgradeAction();
}
