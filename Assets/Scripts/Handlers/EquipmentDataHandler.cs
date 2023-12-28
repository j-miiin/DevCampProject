using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDataHandler : DataHandler
{
    private Rarity[] rarities = { Rarity.Common, Rarity.Uncommon, Rarity.Rare, Rarity.Epic, Rarity.Ancient, Rarity.Legendary, Rarity.Mythology };

    private Dictionary<string, Equipment> allEquipment = new Dictionary<string, Equipment>();

    private Color[] colors;

    private int maxLevel = 4;

    public void SetColorArray(Color[] colorArray)
    {
        colors = colorArray;
    }

    public Rarity[] LoadRarityDatas()
    {
        return rarities;
    }

    // 로컬에 저장되어 있는 장비 데이터들 불러오는 메서드
    public void LoadAllEquipment(List<WeaponInfo> weapons, List<ArmorInfo> armors)
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
    public void CreateAllEquipment(List<WeaponInfo> weapons, List<ArmorInfo> armors)
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

    public Dictionary<string, Equipment> LoadAllEquipmentDic()
    {
        return allEquipment;
    }

    // AllEquipment에 Equipment 더하는 메서드
    public void AddEquipment(string equipmentName, Equipment equipment)
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
}
