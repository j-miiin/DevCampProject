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

    [SerializeField] private Summon[] summonList;
    [SerializeField] private SummonLevelProbability[] summonLevelProbList;

    private void Awake()
    {
        instance = this;
    }

    public void InitSummonManager()
    {
        if (!LoadSummonList()) CreateSummonList();
    }

    public void SummonWeapon(SummonCountType count)
    {
        SummonLevelProbability probSO = summonLevelProbList[(int)SummonType.Weapon];
        int levelIdx = summonList[(int)SummonType.Weapon].level - 1;
        SummonProbability prob = probSO.SummonLevelProbList[levelIdx];

        List<WeaponInfo> summonWeaponList = new List<WeaponInfo>();
        for (int i = 0; i < (int)count; i++)
        {
            int rndNum = Random.Range(0, 1000);
            if (rndNum < prob.commonProb)
                summonWeaponList.Add(EquipmentManager.instance.GetRandomWeaponWithRarity(Rarity.Common));
            else if (rndNum < prob.uncommonProb)
                summonWeaponList.Add(EquipmentManager.instance.GetRandomWeaponWithRarity(Rarity.Uncommon));
            else if (rndNum < prob.rareProb)
                summonWeaponList.Add(EquipmentManager.instance.GetRandomWeaponWithRarity(Rarity.Rare));
            else if (rndNum < prob.epicProb)
                summonWeaponList.Add(EquipmentManager.instance.GetRandomWeaponWithRarity(Rarity.Epic));
            else if (rndNum < prob.ancientProb)
                summonWeaponList.Add(EquipmentManager.instance.GetRandomWeaponWithRarity(Rarity.Ancient));
            else if (rndNum < prob.legendaryProb)
                summonWeaponList.Add(EquipmentManager.instance.GetRandomWeaponWithRarity(Rarity.Legendary));
            else
                summonWeaponList.Add(EquipmentManager.instance.GetRandomWeaponWithRarity(Rarity.Mythology));
        }

        EquipmentManager.instance.AddWeapons(summonWeaponList);

        summonList[(int)SummonType.Weapon].GetExp((int)count);
        SaveSummonList();
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
