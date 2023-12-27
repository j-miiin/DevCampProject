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

    private void Awake()
    {
        instance = this;
    }

    public void InitSummonManager()
    {
        if (!LoadSummonList()) CreateSummonList();
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
        SaveSummonList();
        CurrencyManager.instance.SubtractCurrency("Dia", (int)countType * 5);
        OnSummon?.Invoke(SummonType.Weapon);
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
        SaveSummonList();
        CurrencyManager.instance.SubtractCurrency("Dia", (int)countType * 5);
        OnSummon?.Invoke(SummonType.Armor);
    }

    public void CreateSummonList()
    {
        int typeCnt = System.Enum.GetValues(typeof(SummonType)).Length;
        summonList = new Summon[typeCnt];
        for (int i = 0; i < typeCnt; i++)
        {
            summonList[i] = new Summon(1, 200);
        }
        SaveSummonList();
    }

    // 소환 정보 리스트 저장
    public void SaveSummonList()
    {
        ES3.Save<Summon[]>("summonList", summonList);
    }

    // 소환 정보 리스트 로드
    public bool LoadSummonList()
    {
        if (ES3.KeyExists("summonList")) summonList = ES3.Load<Summon[]>("summonList");
        else return false;

        return true;
    }
}
