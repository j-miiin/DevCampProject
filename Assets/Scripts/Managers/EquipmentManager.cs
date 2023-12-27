using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    [SerializeField] List<WeaponInfo> weapons = new List<WeaponInfo>();
    [SerializeField] List<ArmorInfo> armors = new List<ArmorInfo>();

    [SerializeField]
    private static Dictionary<string, Equipment> allEquipment = new Dictionary<string, Equipment>();

    Rarity[] rarities = { Rarity.Common, Rarity.Uncommon, Rarity.Rare, Rarity.Epic, Rarity.Ancient, Rarity.Legendary, Rarity.Mythology };

    [SerializeField] Color[] colors;

    int maxLevel = 4;

    private WeaponInfo recommendedWeapon;
    private ArmorInfo recommendedArmor;

    private void Awake()
    {
        instance = this;
    }

    // 장비 매니저 초기화 메서드
    public void InitEquipmentManager()
    {
        SetAllWeapons();
    }

    // 장비들 업데이트 하는 메서드
    void SetAllWeapons()
    {
        if (ES3.KeyExists("Init_Game"))
        {
            LoadAllWeapon();
        }
        else
        {
            CreateAllWeapon();
        }
    }

    // 로컬에 저장되어 있는 장비 데이터들 불러오는 메서드
    public void LoadAllWeapon()
    {
        int weaponCount = 0;
        int armorCount = 0;
        int rarityIntValue = 0;

        foreach (Rarity rarity in rarities)
        {
            rarityIntValue = Convert.ToInt32(rarity);
            for (int level = 1; level <= maxLevel; level++)
            {
                string name = $"{rarity}_Weapon_{level}";
                WeaponInfo weapon = weapons[weaponCount];

                weapon.LoadEquipment(name);

                weapon.GetComponent<Button>().onClick.AddListener(() => EquipmentUI.TriggerSelectEquipment(weapon));

                AddEquipment(name, weapon);

                if (weapon.OnEquipped) Player.OnEquip(weapon);

                weaponCount++;

                // 임시
                weapon.myColor = colors[rarityIntValue];
                weapon.SetUI();

                // Armor
                string armorName = $"{rarity}_Armor_{level}";
                ArmorInfo armor = armors[armorCount];

                armor.LoadEquipment(armorName);

                armor.GetComponent<Button>().onClick.AddListener(() => EquipmentUI.TriggerSelectEquipment(armor));

                AddEquipment(armorName, armor);

                if (armor.OnEquipped) Player.OnEquip(armor);

                armorCount++;

                armor.myColor = colors[rarityIntValue];
                armor.SetUI();
            }
        }
    }

    // 장비 데이터를 만드는 메서드
    void CreateAllWeapon()
    {
        int weaponCount = 0;
        int armorCount = 0;
        int rarityIntValue = 0;

        foreach (Rarity rarity in rarities)
        {
            if (rarity == Rarity.None) continue;
            rarityIntValue = Convert.ToInt32(rarity);
            for (int level = 1; level <= maxLevel; level++)
            {
                WeaponInfo weapon = weapons[weaponCount];

                string name = $"{rarity}_Weapon_{level}";// Weapon Lv

                int equippedEffect = level * ((int)Mathf.Pow(10, rarityIntValue + 1));
                int ownedEffect = (int)(equippedEffect * 0.5f);
                string equippedEffectText = $"{equippedEffect}%";
                string ownedEffectText = $"{ownedEffect}%"; 

                weapon.SetWeaponInfo(name, 1, level, false, EquipmentType.Weapon, rarity,
                                 1, equippedEffect, ownedEffect, colors[rarityIntValue]);

                weapon.GetComponent<Button>().onClick.AddListener(() => EquipmentUI.TriggerSelectEquipment(weapon));

                AddEquipment(name, weapon);

                weapon.SaveEquipment(name);

                weaponCount++;

                // Armor
                ArmorInfo armor = armors[armorCount];
                string armorName = $"{rarity}_Armor_{level}";// Weapon Lv

                int armorEquippedEffect = level * ((int)Mathf.Pow(10, rarityIntValue + 1));
                int armorOwnedEffect = (int)(armorEquippedEffect * 0.4f);
                string armorEquippedEffectText = $"{armorEquippedEffect}%";
                string armorOwnedEffectText = $"{armorOwnedEffect}%";

                armor.SetArmorInfo(armorName, 1, level, false, EquipmentType.Armor, rarity,
                                 1, armorEquippedEffect, armorOwnedEffect, colors[rarityIntValue]);

                armor.GetComponent<Button>().onClick.AddListener(() => EquipmentUI.TriggerSelectEquipment(armor));

                AddEquipment(armorName, armor);

                armor.SaveEquipment(armorName);

                armorCount++;
            }
        }
    }

    // 매개변수로 받은 장비 합성하는 메서드
    public int Composite(Equipment equipment)
    {
        if (equipment.quantity < 4) return -1;
        if (equipment.type == EquipmentType.Weapon
            && equipment.name == weapons[weapons.Count - 1].name) return -1;
        if (equipment.type == EquipmentType.Armor
            && equipment.name == armors[armors.Count - 1].name) return -1;

        int compositeCount = equipment.quantity / 4;

        equipment.quantity -= (compositeCount * 4);
        equipment.SetQuantityUI();
        equipment.SaveEquipment(equipment.name);

        Equipment nextEquipment = GetNextEquipment(equipment.name, equipment.type);
        nextEquipment.quantity += compositeCount;
        nextEquipment.SetQuantityUI();
        nextEquipment.SaveEquipment(nextEquipment.name);

        return compositeCount;
    }

    public void AllComposite(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                for (int i = 0; i < weapons.Count; i++) Composite(weapons[i]);
                break;
            case EquipmentType.Armor:
                for (int i = 0; i < armors.Count; i++) Composite(armors[i]);
                break;
        }
    }

    public bool IsAllCompositable(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                for (int i = 0; i < weapons.Count; i++)
                {
                    if (weapons[i].quantity >= 4 && i != weapons.Count - 1) return true;
                }
                return false;
            case EquipmentType.Armor:
                for (int i = 0; i < armors.Count; i++)
                {
                    if (armors[i].quantity >= 4 && i != armors.Count - 1) return true;
                }
                return false;
        }
        return false;
    }

    // AllEquipment에 Equipment 더하는 메서드
    public static void AddEquipment(string equipmentName, Equipment equipment)
    {
        if (!allEquipment.ContainsKey(equipmentName))
        {
            allEquipment.Add(equipmentName, equipment);
        }
        else
        {
            Debug.LogWarning($"Equipment already exists in the dictionary: {equipmentName}");
        }
    }

    // AllEquipment에서 매개변수로 받은 string을 key로 사용해 Equipment 찾는 매서드
    public static Equipment GetEquipment(string equipmentName)
    {
        if (allEquipment.TryGetValue(equipmentName, out Equipment equipment))
        {
            return equipment;
        }
        else
        {
            Debug.LogError($"Equipment not found: {equipmentName}");
            return null;
        }
    }

    public Equipment GetEquipment(EquipmentType type, int idx)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                if (idx >= weapons.Count) return null;
                return weapons[idx];
            case EquipmentType.Armor:
                if (idx >= armors.Count) return null;
                return armors[idx];
        }
        return null;
    }

    // AllEquipment에서 매개변수로 받은 key을 사용하는 Equipment 업데이트 하는 메서드
    public static void SetEquipment(string equipmentName, Equipment equipment)
    {
        Equipment targetEquipment = allEquipment[equipmentName];
        Debug.Log("이름 : "+ allEquipment[equipmentName].gameObject.name);
        targetEquipment.equippedEffect = equipment.equippedEffect;
        targetEquipment.ownedEffect = equipment.ownedEffect;
        targetEquipment.quantity = equipment.quantity;
        targetEquipment.OnEquipped = equipment.OnEquipped;
        targetEquipment.enhancementLevel = equipment.enhancementLevel;  

        targetEquipment.SetQuantityUI();

        targetEquipment.SaveEquipment(targetEquipment.name);
    }

    // 매개변수로 받은 key값을 사용하는 장비의 다음레벨 장비를 불러오는 메서드
    public Equipment GetNextEquipment(string currentKey, EquipmentType type = EquipmentType.Weapon)
    {
        int currentRarityIndex = -1;
        int currentLevel = -1;
        int maxLevel = 4; // 최대 레벨 설정

        // 현재 키에서 희귀도와 레벨 분리
        foreach (var rarity in rarities)
        {
            if (currentKey.StartsWith(rarity.ToString()))
            {
                currentRarityIndex = Array.IndexOf(rarities, rarity);
                if (type == EquipmentType.Weapon) int.TryParse(currentKey.Replace(rarity + "_Weapon_", ""), out currentLevel);
                else if (type == EquipmentType.Armor) int.TryParse(currentKey.Replace(rarity + "_Armor_", ""), out currentLevel);
                break;
            }
        }

        if (currentRarityIndex != -1 && currentLevel != -1)
        {
            if (currentLevel < maxLevel)
            {
                // 같은 희귀도 내에서 다음 레벨 찾기
                string nextKey =
                    (type == EquipmentType.Weapon)
                    ? rarities[currentRarityIndex] + "_Weapon_" + (currentLevel + 1)
                    : rarities[currentRarityIndex] + "_Armor_" + (currentLevel + 1);
                return allEquipment.TryGetValue(nextKey, out Equipment nextEquipment) ? nextEquipment : null;
            }
            else if (currentRarityIndex < rarities.Length - 1)
            {
                // 희귀도를 증가시키고 첫 번째 레벨의 장비 찾기
                string nextKey =
                    (type == EquipmentType.Weapon)
                    ? rarities[currentRarityIndex + 1] + "_Weapon_1"
                    : rarities[currentRarityIndex + 1] + "_Armor_1";
                return allEquipment.TryGetValue(nextKey, out Equipment nextEquipment) ? nextEquipment : null;
            }
        }

        // 다음 장비를 찾을 수 없는 경우
        return null;
    }

    // 매개변수로 받은 key값을 사용하는 장비의 이전레벨 장비를 불러오는 메서드
    public Equipment GetPreviousEquipment(string currentKey)
    {
        int currentRarityIndex = -1;
        int currentLevel = -1;

        // 현재 키에서 희귀도와 레벨 분리
        foreach (var rarity in rarities)
        {
            if (currentKey.StartsWith(rarity.ToString()))
            {
                currentRarityIndex = Array.IndexOf(rarities, rarity);
                int.TryParse(currentKey.Replace(rarity + "_", ""), out currentLevel);
                break;
            }
        }

        if (currentRarityIndex != -1 && currentLevel != -1)
        {
            if (currentLevel > 1)
            {
                // 같은 희귀도 내에서 이전 레벨 찾기
                string previousKey = rarities[currentRarityIndex] + "_" + (currentLevel - 1);
                return allEquipment.TryGetValue(previousKey, out Equipment prevEquipment) ? prevEquipment : null;
            }
            else if (currentRarityIndex > 0)
            {
                // 희귀도를 낮추고 최대 레벨의 장비 찾기
                string previousKey = rarities[currentRarityIndex - 1] + "_4";
                return allEquipment.TryGetValue(previousKey, out Equipment prevEquipment) ? prevEquipment : null;
            }
        }

        // 이전 장비를 찾을 수 없는 경우
        return null;
    }

    public WeaponInfo GetRecommendedWeapon()
    {
        if (recommendedWeapon == null) recommendedWeapon = weapons[0];
        for (int i = 0; i < weapons.Count; i++)
        {
            if (recommendedWeapon.equippedEffect < weapons[i].equippedEffect)
                recommendedWeapon = weapons[i];
        }
        return recommendedWeapon;
    }

    public ArmorInfo GetRecommendedArmor()
    {
        if (recommendedArmor == null) recommendedArmor = armors[0];
        for (int i = 0; i < armors.Count; i++)
        {
            if (recommendedArmor.equippedEffect < armors[i].equippedEffect)
                recommendedArmor = armors[i];
        }
        return recommendedArmor;
    }

    public WeaponInfo GetRandomWeaponWithRarity(Rarity rarity)
    {
        List<WeaponInfo> weaponList = new List<WeaponInfo>(10);
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].rarity == rarity) weaponList.Add(weapons[i]);
        }
        int rndNum = Random.Range(0, weaponList.Count);
        return weaponList[rndNum];
    }

    public ArmorInfo GetRandomArmorWithRarity(Rarity rarity)
    {
        List<ArmorInfo> armorList = new List<ArmorInfo>(10);
        for (int i = 0; i < armors.Count; i++)
        {
            if (armors[i].rarity == rarity) armorList.Add(armors[i]);
        }
        int rndNum = Random.Range(0, armorList.Count);
        return armorList[rndNum];
    }

    public void AddRandomSummonWeapon(SummonCountType count, SummonProbability prob)
    {
        List<WeaponInfo> commonWeapons = new List<WeaponInfo>();
        for (int i = 0; i < weapons.Count; i++)
            if (weapons[i].rarity == Rarity.Common) commonWeapons.Add(weapons[i]);

        List<WeaponInfo> commonWeapons = new List<WeaponInfo>();
        for (int i = 0; i < weapons.Count; i++)
            if (weapons[i].rarity == Rarity.Common) commonWeapons.Add(weapons[i]);

        List<WeaponInfo> commonWeapons = new List<WeaponInfo>();
        for (int i = 0; i < weapons.Count; i++)
            if (weapons[i].rarity == Rarity.Common) commonWeapons.Add(weapons[i]);

        List<WeaponInfo> commonWeapons = new List<WeaponInfo>();
        for (int i = 0; i < weapons.Count; i++)
            if (weapons[i].rarity == Rarity.Common) commonWeapons.Add(weapons[i]);

        List<WeaponInfo> commonWeapons = new List<WeaponInfo>();
        for (int i = 0; i < weapons.Count; i++)
            if (weapons[i].rarity == Rarity.Common) commonWeapons.Add(weapons[i]);


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

    }

    public void TestGetRandomEquipment()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            int rndNum = Random.Range(0, 4);
            weapons[i].quantity += rndNum;
            weapons[i].SetQuantityUI();
            weapons[i].SaveEquipment(weapons[i].name);
        }

        for (int i = 0; i < armors.Count; i++)
        {
            int rndNum = Random.Range(0, 4);
            armors[i].quantity += rndNum;
            armors[i].SetQuantityUI();
            armors[i].SaveEquipment(armors[i].name);
        }
    }
}
