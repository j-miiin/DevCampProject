using Keiwando.BigInteger;
using UnityEngine;

public class EquipmentDataHandler : DataHandler
{
    // 장비 정보 저장
    public void SaveEquipment(Equipment equipment)
    {
        string name = equipment.name;

        Debug.Log("��� ���� ���� " + name);

        ES3.Save<string>("name_" + name, name);
        ES3.Save<int>("quantity_" + name, equipment.quantity);
        ES3.Save<int>("level_" + name, equipment.level);
        ES3.Save<bool>("onEquipped_" + name, equipment.OnEquipped);
        ES3.Save<EquipmentType>("type_" + name, equipment.type);
        ES3.Save<Rarity>("rarity_" + name, equipment.rarity);
        ES3.Save<int>("enhancementLevel_" + name, equipment.enhancementLevel);
        ES3.Save<int>("basicEquippedEffect_" + name, equipment.basicEquippedEffect);
        ES3.Save<int>("basicOwnedEffect_" + name, equipment.basicOwnedEffect);

        ES3.Save<string>("equippedEffect_" + name, equipment.equippedEffect.ToString());
        ES3.Save<string>("ownedEffect_" + name, equipment.ownedEffect.ToString());
    }

    public void SaveEquipment(Equipment equipment, string equipmentID)
    {
        Debug.Log("��� ���� ���� " + equipmentID);

        ES3.Save<string>("name_" + equipmentID, equipment.name);
        ES3.Save<int>("quantity_" + equipmentID, equipment.quantity);
        ES3.Save<int>("level_" + equipmentID, equipment.level);
        ES3.Save<bool>("onEquipped_" + equipmentID, equipment.OnEquipped);
        ES3.Save<EquipmentType>("type_" + equipmentID, equipment.type);
        ES3.Save<Rarity>("rarity_" + equipmentID, equipment.rarity);
        ES3.Save<int>("enhancementLevel_" + equipmentID, equipment.enhancementLevel);
        ES3.Save<int>("basicEquippedEffect_" + equipmentID, equipment.basicEquippedEffect);
        ES3.Save<int>("basicOwnedEffect_" + equipmentID, equipment.basicOwnedEffect);

        ES3.Save<string>("equippedEffect_" + equipmentID, equipment.equippedEffect.ToString());
        ES3.Save<string>("ownedEffect_" + equipmentID, equipment.ownedEffect.ToString());
    }

    public void SaveEquipmentAttribute(Equipment equipment, EquipmentAttribute attr, string equipmentID)
    {
        switch (attr)
        {
            case EquipmentAttribute.Name:
                ES3.Save<string>("name_" + equipmentID, equipment.name);
                break;
            case EquipmentAttribute.Quantity:
                ES3.Save<int>("quantity_" + equipmentID, equipment.quantity);
                break;
            case EquipmentAttribute.Level:
                ES3.Save<int>("level_" + equipmentID, equipment.level);
                break;
            case EquipmentAttribute.OnEquipped:
                ES3.Save<bool>("onEquipped_" + equipmentID, equipment.OnEquipped);
                break;
            case EquipmentAttribute.Type:
                ES3.Save<EquipmentType>("type_" + equipmentID, equipment.type);
                break;
            case EquipmentAttribute.Rarity:
                ES3.Save<Rarity>("rarity_" + equipmentID, equipment.rarity);
                break;
            case EquipmentAttribute.EnhancementLevel:
                ES3.Save<int>("enhancementLevel_" + equipmentID, equipment.enhancementLevel);
                break;
            case EquipmentAttribute.BasicEquippedEffect:
                ES3.Save<int>("basicEquippedEffect_" + equipmentID, equipment.basicEquippedEffect);
                break;
            case EquipmentAttribute.BasicOwnedEffect:
                ES3.Save<int>("basicOwnedEffect_" + equipmentID, equipment.basicOwnedEffect);
                break;
            case EquipmentAttribute.EquippedEffect:
                ES3.Save<string>("equippedEffect_" + equipmentID, equipment.equippedEffect.ToString());
                break;
            case EquipmentAttribute.OwnedEffect:
                ES3.Save<string>("ownedEffect_" + equipmentID, equipment.ownedEffect.ToString());
                break;
        }
    }

    // 장비 정보 로드
    public void LoadEquipment(Equipment equipment)
    {
        string name = equipment.name;

        if (!ES3.KeyExists("name_" + name)) return;

        Debug.Log("��� ���� �ҷ����� " + name);

        equipment.name = ES3.Load<string>("name_" + name);
        equipment.quantity = ES3.Load<int>("quantity_" + name);
        equipment.level = ES3.Load<int>("level_" + name);
        equipment.OnEquipped = ES3.Load<bool>("onEquipped_" + name);
        equipment.type = ES3.Load<EquipmentType>("type_" + name);
        equipment.rarity = ES3.Load<Rarity>("rarity_" + name);
        equipment.enhancementLevel = ES3.Load<int>("enhancementLevel_" + name);
        equipment.basicEquippedEffect = ES3.Load<int>("basicEquippedEffect_" + name);
        equipment.basicOwnedEffect = ES3.Load<int>("basicOwnedEffect_" + name);

        equipment.equippedEffect = new BigInteger(ES3.Load<string>("equippedEffect_" + name));
        equipment.ownedEffect = new BigInteger(ES3.Load<string>("ownedEffect_" + name));
    }

    public void LoadEquipment(Equipment equipment, string equipmentID)
    {
        if (!ES3.KeyExists("name_" + equipmentID)) return;

        Debug.Log("��� ���� �ҷ����� " + equipmentID);

        equipment.name = ES3.Load<string>("name_" + equipmentID);
        equipment.quantity = ES3.Load<int>("quantity_" + equipmentID);
        equipment.level = ES3.Load<int>("level_" + equipmentID);
        equipment.OnEquipped = ES3.Load<bool>("onEquipped_" + equipmentID);
        equipment.type = ES3.Load<EquipmentType>("type_" + equipmentID);
        equipment.rarity = ES3.Load<Rarity>("rarity_" + equipmentID);
        equipment.enhancementLevel = ES3.Load<int>("enhancementLevel_" + equipmentID);
        equipment.basicEquippedEffect = ES3.Load<int>("basicEquippedEffect_" + equipmentID);
        equipment.basicOwnedEffect = ES3.Load<int>("basicOwnedEffect_" + equipmentID);

        equipment.equippedEffect = new BigInteger(ES3.Load<string>("equippedEffect_" + equipmentID));
        equipment.ownedEffect = new BigInteger(ES3.Load<string>("ownedEffect_" + equipmentID));
    }
}
