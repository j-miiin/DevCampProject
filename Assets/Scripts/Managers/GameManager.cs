using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Start()
    {

        CurrencyManager.instance.InitCurrencyManager();
        EquipmentManager.instance.InitEquipmentManager();
        StatusUpgradeManager.instance.InitStatusUpgradeManager();
        SummonManager.instance.InitSummonManager();

        ES3.Save<bool>("Init_Game", true);
    }

}
