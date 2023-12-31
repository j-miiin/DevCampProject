using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Start()
    {

        CurrencyManager.instance.InitCurrencyManager();
        EquipmentManager.instance.InitEquipmentManager();
        StatusUpgradeManager.instance.InitStatusUpgradeManager();
        SummonManager.instance.InitSummonManager();
        AchievementManager.instance.InitAchievementManager();

        ES3.Save<bool>("Init_Game", true);
    }

    private void OnApplicationQuit()
    {
        AchievementManager.instance.SaveAchievementData();
    }
}
