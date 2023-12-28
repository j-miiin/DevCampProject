using System;
using System.Collections.Generic;
using UnityEngine;

public enum SummonCountType
{
    Fifty = 50,
    Hundred = 100
}

public class SummonManager : MonoBehaviour
{
    public static SummonManager instance;

    public event Action<SummonType> OnSummon;

    [SerializeField] private Summon[] summonList;
    [SerializeField] private SummonLevelProbability[] summonLevelProbList;
    [SerializeField] private SummonResultPanelUI summonResultPanelUI;

    private SummonDataHandler dataHandler;

    private void Awake()
    {
        instance = this;
    }

    public void InitSummonManager()
    {
        dataHandler = DataManager.instance.GetDataHandler<SummonDataHandler>();
        summonList = dataHandler.LoadSummonList();
    }

    public Summon GetSummonInfo(SummonType type)
    {
        return summonList[(int)type];
    }

    public void SummonEquipmentWithType(SummonType summonType, SummonCountType countType)
    {
        switch (summonType)
        {
            case SummonType.Weapon:
                SummonWeapon(countType);
                break;
            case SummonType.Armor:
                SummonArmor(countType);
                break;
        }
    }

    public void SummonWeapon(SummonCountType countType)
    {
        SummonLevelProbability probSO = summonLevelProbList[(int)SummonType.Weapon];
        int levelIdx = summonList[(int)SummonType.Weapon].level - 1;
        SummonProbability prob = probSO.SummonLevelProbList[levelIdx];

        List<Equipment> resultWeaponList = EquipmentManager.instance.AddRandomSummonWeapon(countType, prob);
        summonResultPanelUI.SetResultList(resultWeaponList);
        summonResultPanelUI.gameObject.SetActive(true);

        summonList[(int)SummonType.Weapon].GetExp((int)countType);
        dataHandler.SaveSummonList();
        CurrencyManager.instance.SubtractCurrency("Dia", (int)countType * 5);
        OnSummon?.Invoke(SummonType.Weapon);

        AchievementManager.instance.UpdateAchievement(AchievementType.SummonEquipment, (int)countType);
    }

    public void SummonArmor(SummonCountType countType)
    {
        SummonLevelProbability probSO = summonLevelProbList[(int)SummonType.Armor];
        int levelIdx = summonList[(int)SummonType.Armor].level - 1;
        SummonProbability prob = probSO.SummonLevelProbList[levelIdx];

        List<Equipment> resultArmorList = EquipmentManager.instance.AddRandomSummonArmor(countType, prob);
        summonResultPanelUI.SetResultList(resultArmorList);
        summonResultPanelUI.gameObject.SetActive(true);

        summonList[(int)SummonType.Armor].GetExp((int)countType);
        dataHandler.SaveSummonList();
        CurrencyManager.instance.SubtractCurrency("Dia", (int)countType * 5);
        OnSummon?.Invoke(SummonType.Armor);

        AchievementManager.instance.UpdateAchievement(AchievementType.SummonEquipment, (int)countType);
    }
}
