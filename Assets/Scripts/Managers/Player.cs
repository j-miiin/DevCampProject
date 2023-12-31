using Keiwando.BigInteger;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Action<Equipment> OnEquip;
    public static Action<EquipmentType> OnUnEquip;

    public static Player instance;

    [SerializeField]
    PlayerStatus status;

    [SerializeField][Header("경험치 증가 계수")]
    private int expNum;
    [SerializeField][Header("현재 경험치")]
    private int curExp;
    [SerializeField][Header("현재 최대 경험치")]
    private int maxExp;
    [SerializeField][Header("현재 레벨")]
    private int level;

    [SerializeField][Header("총 공격력")]
    private BigInteger currentAttack;
    [SerializeField][Header("총 체력")]
    private BigInteger currentHealth;
    [SerializeField][Header("총 방어력")]
    private BigInteger currentDefense;
    [SerializeField][Header("총 크리티컬 확률")]
    private BigInteger currentCritChance;
    [SerializeField][Header("총 크리티컬 데미지")]
    private BigInteger currentCritDamage;
    [SerializeField][Header("총 공격 속도")]
    private BigInteger currentAtkSpeed;

    [SerializeField]
    WeaponInfo equiped_Weapon = null;
    [SerializeField]
    ArmorInfo equiped_Armor = null;

    private PlayerDataHandler playerDataHandler;
    private EquipmentDataHandler equipmentDataHandler;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerDataHandler = DataManager.instance.GetDataHandler<PlayerDataHandler>();
        equipmentDataHandler = DataManager.instance.GetDataHandler<EquipmentDataHandler>();
        SetupEventListeners();
        SetPlayerLevelData();
    }

    // 이벤트 설정하는 메서드
    void SetupEventListeners()
    {
        StatusUpgradeManager.OnAttackUpgrade += status.IncreaseBaseStat;
        StatusUpgradeManager.OnHealthUpgrade += status.IncreaseBaseStat;
        StatusUpgradeManager.OnDefenseUpgrade += status.IncreaseBaseStat;
        StatusUpgradeManager.OnCritChanceUpgrade += status.IncreaseBaseStat;
        StatusUpgradeManager.OnCritDamageUpgrade += status.IncreaseBaseStat;
        StatusUpgradeManager.OnAtkSpeedUpgrade += status.IncreaseBaseStat;

        OnEquip += Equip;
        OnUnEquip += UnEquip;
    }

    private void SetPlayerLevelData()
    {
        PlayerLevelData data = playerDataHandler.LoadLevelStatus();
        level = data.level;
        curExp = data.curExp;
        maxExp = data.maxExp;
    }

    public void GetExp()
    {
        curExp += expNum;
        if (curExp >= maxExp) LevelUp();

        playerDataHandler.SaveLevelStatus(new PlayerLevelData(level, curExp, maxExp));
    }

    public void LevelUp()
    {
        level++;
        maxExp += maxExp / 5;
        curExp = 0;

        status.IncreaseBaseStat(StatusType.ATK, 2);
        status.IncreaseBaseStat(StatusType.HP, 50);
        status.IncreaseBaseStat(StatusType.DEF, 2);

        playerDataHandler.SaveLevelStatus(new PlayerLevelData(level, curExp, maxExp));

        Debug.Log($"레벨 업! 현재 레벨 : {level}\n현재 최대 경험치 : {maxExp}");
        Debug.Log($"현재 공격력 : {currentAttack}\n현재 체력 : {currentHealth}\n현재 방어력 : {currentDefense}");
    }

    public void UpdateBaseStat(StatusType statusType, int addValue)
    {
        status.IncreaseBaseStat(statusType, addValue);
    }

    // 현재 능력치를 불러오는 메서드
    public BigInteger GetCurrentStatus(StatusType statusType)
    {
        switch (statusType)
        {
            case StatusType.ATK:
                return currentAttack;
            case StatusType.HP:
                return currentHealth;
            case StatusType.DEF:
                return currentDefense;
            case StatusType.CRIT_CH:
                return currentCritChance;
            case StatusType.CRIT_DMG:
                return currentCritDamage;
            case StatusType.ATK_SPD:
                return currentAtkSpeed;
        }
        return null;
    }

    // 현재 능력치를 업데이트 하는 메서드
    public void SetCurrentStatus(StatusType statusType, BigInteger statusValue)
    {
        switch (statusType)
        {
            case StatusType.ATK:
                Debug.Log("공격력 강화 " + statusValue );
                currentAttack = statusValue;
                break;
            case StatusType.HP:
                Debug.Log("체력 강화 " + statusValue);
                currentHealth = statusValue;
                break;
            case StatusType.DEF:
                currentDefense = statusValue;
                break;
            case StatusType.CRIT_CH:
                currentCritChance = statusValue;
                break;
            case StatusType.CRIT_DMG:
                currentCritDamage = statusValue;
                break;
            case StatusType.ATK_SPD:
                currentAtkSpeed = statusValue;
                break;
        }
    }

    // 장비 장착하는 메서드 
    public void Equip(Equipment equipment)
    {
        //equipment.OnEquipped = true;
        switch(equipment.type)
        {
            case EquipmentType.Weapon:

                UnEquip(equipment.type);
                
                equiped_Weapon = equipment.GetComponent<WeaponInfo>();

                equiped_Weapon.OnEquipped = true;

                status.IncreaseBaseStatByPercent(StatusType.ATK, equiped_Weapon.equippedEffect);

                EquipmentUI.UpdateEquipmentUI?.Invoke(equiped_Weapon.OnEquipped);
                //equiped_Weapon.SaveEquipment();
                equipmentDataHandler.SaveEquipment(equiped_Weapon);
                Debug.Log("무기 장비 장착" + equiped_Weapon.name);
                break;
            case EquipmentType.Armor:

                UnEquip(equipment.type);

                equiped_Armor = equipment.GetComponent<ArmorInfo>();

                equiped_Armor.OnEquipped = true;

                status.IncreaseBaseStatByPercent(StatusType.HP, equiped_Armor.equippedEffect);

                EquipmentUI.UpdateEquipmentUI?.Invoke(equiped_Armor.OnEquipped);
                //equiped_Armor.SaveEquipment();
                equipmentDataHandler.SaveEquipment(equiped_Armor);
                Debug.Log("방어구 장비 장착" + equiped_Armor.name);
                break;
        }
    }

    // 장비 장착 해제하는 메서드 
    public void UnEquip(EquipmentType equipmentType)
    {
        // 퍼센트 차감 로직 구현 필요.
        switch (equipmentType)
        {
            case EquipmentType.Weapon:
                if (equiped_Weapon == null) return;
                equiped_Weapon.OnEquipped = false;
                EquipmentUI.UpdateEquipmentUI?.Invoke(equiped_Weapon.OnEquipped);
                status.DecreaseBaseStatByPercent(StatusType.ATK, equiped_Weapon.equippedEffect);
                //equiped_Weapon.SaveEquipment();
                equipmentDataHandler.SaveEquipment(equiped_Weapon);
                Debug.Log("무기 장비 장착 해제" + equiped_Weapon.name);
                equiped_Weapon = null;
                break;
            case EquipmentType.Armor:
                if (equiped_Armor == null) return;
                equiped_Armor.OnEquipped = false;
                EquipmentUI.UpdateEquipmentUI?.Invoke(equiped_Armor.OnEquipped);
                status.DecreaseBaseStatByPercent(StatusType.HP, equiped_Armor.equippedEffect);
                //equiped_Armor.SaveEquipment();
                equipmentDataHandler.SaveEquipment(equiped_Armor);
                Debug.Log("방어구 장비 장착 해제" + equiped_Armor.name);
                equiped_Armor = null;
                break;
        }
    }

    public WeaponInfo GetCurrentEquipedWeapon()
    {
        return equiped_Weapon;
    }

    public ArmorInfo GetCurrentEquipedArmor()
    {
        return equiped_Armor;
    }
}
