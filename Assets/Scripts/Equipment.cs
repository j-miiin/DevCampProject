using Keiwando.BigInteger;
using UnityEngine;

// 장비 타입
public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory
    // 기타 장비 타입...
}

// 희귀도 
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Ancient,
    Legendary,
    Mythology,
    None
    // 기타 희귀도...
}

public enum EquipmentAttribute
{
    Name,
    Quantity,
    Level,
    OnEquipped,
    Type,
    Rarity,
    EnhancementLevel,
    BasicEquippedEffect,
    BasicOwnedEffect,
    EquippedEffect,
    OwnedEffect,
}

public class Equipment : MonoBehaviour
{
    public string name;          // 장비의 이름
    public int quantity;         // 장비의 개수
    public int level;
    public bool OnEquipped;
    public EquipmentType type;   // 장비의 타입 (예: 무기, 방어구 등)
    public Rarity rarity;        // 장비의 희귀도
    public int enhancementLevel; // 강화 상태 (예: 0, 1, 2, ...)
    public int basicEquippedEffect;
    public BigInteger equippedEffect;  // 장착효과
    public int basicOwnedEffect;
    public BigInteger ownedEffect;     // 보유효과
   [HideInInspector] public int enhancementMaxLevel = 100;

    public Equipment(string name, int quantity, int level, bool OnEquipped, EquipmentType type, Rarity rarity,
                 int enhancementLevel, int basicEquippedEffect, int basicOwnedEffect)
    {
        this.name = name;
        this.quantity = quantity;
        this.level = level;
        this.OnEquipped = OnEquipped;
        this.type = type;
        this.rarity = rarity;
        this.enhancementLevel = enhancementLevel;
        this.basicEquippedEffect = basicEquippedEffect;
        this.basicOwnedEffect = basicOwnedEffect;

        equippedEffect = this.basicEquippedEffect;
        ownedEffect = this.basicOwnedEffect;
    }

    // 강화 메서드
    public virtual void Enhance()
    {
        // 강화 로직...
        equippedEffect += basicEquippedEffect;
        ownedEffect += basicOwnedEffect;

        enhancementLevel++;
        // 강화효과 업데이트...
        AchievementManager.instance.UpdateAchievement(AchievementType.EnhanceEquipment, 1);
    }

    // 강화할 때 필요한 강화석 return 시키는 메서드
    public BigInteger GetEnhanceStone()
    {
        Debug.Log($"{ownedEffect}  {basicOwnedEffect}");
        var requipredEnhanceStone = equippedEffect - basicOwnedEffect;

        return requipredEnhanceStone;
    }

    // 개수 체크하는 메서드
    public bool CheckQuantity()
    {
        if (quantity >= 4)
        {
            return true;
        }

        SetQuantityUI();
        return false;
    }

    // WeaponInfo 확인.
    public virtual void SetQuantityUI(){}
    public virtual void SetUI(){}
}
