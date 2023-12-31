using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class EquipmentUI : MonoBehaviour
{
    public static event Action<Equipment> OnClickSelectEquipment;
    public static Action<bool> UpdateEquipmentUI;

    public static EquipmentUI instance;

    [SerializeField] TabViewController tabViewController;

    [SerializeField] Equipment[] selectableEquipments;
    [SerializeField] Equipment selectEquipment;
    [SerializeField] TMP_Text selectEquipmentName;
    [SerializeField] TMP_Text selectEquipment_equippedEffect;
    [SerializeField] TMP_Text selectEquipment_ownedEffect;
    [SerializeField] TMP_Text selectEquipment_enhancementLevel;

    [SerializeField] Button equipBtn;
    [SerializeField] Button unEquipBtn;
    [SerializeField] Button autoEquipBtn;
    [SerializeField] Button enhancePnaelBtn;
    [SerializeField] Button compositeBtn;
    [SerializeField] Button allCompositeBtn;

    [Header("강화 패널")]
    [SerializeField] Equipment[] selectableEnhanceEquipments;
    [SerializeField] Equipment enhanceEquipment; // 강화 무기
    [SerializeField] Button enhanceBtn; // 강화 버튼
    [SerializeField] TMP_Text enhanceLevelText; // 강화 레벨 / 장비 강화 (0/0)
    [SerializeField] TMP_Text EquippedPreview; // 장착 효과 미리보기 / 장착 효과 0 → 0
    [SerializeField] TMP_Text OwnedPreview;// 보유 효과 미리보기 / 보유 효과 0 → 0
    [SerializeField] TMP_Text EnhanceCurrencyText; // 현재 재화
    [SerializeField] TMP_Text RequiredCurrencyText; // 필요 재화

    private EquipmentType curTabType;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //SetupEventListeners();
        InitializeButtonListeners();
    }

    // 이벤트 설정하는 메서드
    private void OnEnable()
    {
        OnClickSelectEquipment += SelectEquipment;
        UpdateEquipmentUI += SetOnEquippedBtnUI;
        tabViewController.OnTabChanged += CallOnTabChanged;
    }

    private void OnDisable()
    {
        OnClickSelectEquipment -= SelectEquipment;
        UpdateEquipmentUI -= SetOnEquippedBtnUI;
        tabViewController.OnTabChanged -= CallOnTabChanged;
    }

    // 버튼 클릭 리스너 설정하는 메서드 
    void InitializeButtonListeners()
    {
        equipBtn.onClick.AddListener(OnClickEquip);
        unEquipBtn.onClick.AddListener(OnClickUnEquip);
        autoEquipBtn.onClick.AddListener(OnClickAutoEquip);
        enhancePnaelBtn.onClick.AddListener(OnClickEnhancePanel);
        enhanceBtn.onClick.AddListener(OnClickEnhance);
        compositeBtn.onClick.AddListener(OnClickComposite);
        allCompositeBtn.onClick.AddListener(OnClickAllComposite);
    }

    // 장비 선택 이벤트 트리거 하는 메서드 
    public static void TriggerSelectEquipment(Equipment equipment)
    {
        OnClickSelectEquipment?.Invoke(equipment);
    }

    // 장비 클릭 했을 때 불리는 메서드
    public void SelectEquipment(Equipment equipment)
    {
        switch (equipment.type)
        {
            case EquipmentType.Weapon:
                selectableEquipments[(int)EquipmentType.Weapon].gameObject.SetActive(true);
                selectableEquipments[(int)EquipmentType.Armor].gameObject.SetActive(false);
                selectEquipment = selectableEquipments[(int)EquipmentType.Weapon];
                selectEquipment.GetComponent<WeaponInfo>().SetWeaponInfo(equipment.GetComponent<WeaponInfo>());
                UpdateSelectedEquipmentUI(selectEquipment);
                break;
            case EquipmentType.Armor:
                selectableEquipments[(int)EquipmentType.Weapon].gameObject.SetActive(false);
                selectableEquipments[(int)EquipmentType.Armor].gameObject.SetActive(true);
                selectEquipment = selectableEquipments[(int)EquipmentType.Armor];
                selectEquipment.GetComponent<ArmorInfo>().SetArmorInfo(equipment.GetComponent<ArmorInfo>());
                UpdateSelectedEquipmentUI(selectEquipment);
                break;
        }

        compositeBtn.interactable = (equipment.quantity >= 4);
    }
    
    private void UpdateSelectedEquipmentUI(Equipment equipment)
    {
        equipment.SetQuantityUI();

        switch (selectEquipment.type)
        {
            case EquipmentType.Weapon:
                selectEquipment.GetComponent<WeaponInfo>().SetUI();
                break;
            case EquipmentType.Armor:
                selectEquipment.GetComponent<ArmorInfo>().SetUI();
                break;
        }

        SetOnEquippedBtnUI(selectEquipment.OnEquipped);
        SetselectEquipmentTextUI(equipment);
    }

    // 선택 장비 데이터 UI로 보여주는 메서드
    void SetselectEquipmentTextUI(Equipment equipment)
    {
        selectEquipmentName.text = equipment.name;
        selectEquipment_equippedEffect.text = $"{BigInteger.ChangeMoney(equipment.equippedEffect.ToString())}%";
        selectEquipment_ownedEffect.text = $"{equipment.ownedEffect}%";
    }

    // 장착 버튼 활성화 / 비활성화 메서드
    void SetOnEquippedBtnUI(bool Onequipped)
    {
        if (Onequipped)
        {
            equipBtn.gameObject.SetActive(false);
            unEquipBtn.gameObject.SetActive(true);
        }
        else
        {
            equipBtn.gameObject.SetActive(true);
            unEquipBtn.gameObject.SetActive(false);
        }
    }

    // 강화 판넬 버튼 눌렸을 때 불리는 메서드
    public void OnClickEnhancePanel()
    {
        switch (selectEquipment.type)
        {
            case EquipmentType.Weapon:
                selectableEnhanceEquipments[(int)EquipmentType.Weapon].gameObject.SetActive(true);
                selectableEnhanceEquipments[(int)EquipmentType.Armor].gameObject.SetActive(false);
                enhanceEquipment = selectableEnhanceEquipments[(int)EquipmentType.Weapon];
                Equipment enhanceEquipmentTemp = EquipmentManager.GetEquipment(selectEquipment.name);

                Debug.Log($"현재 강화 무기 : {enhanceEquipmentTemp.name}");

                enhanceLevelText.text = $"장비 강화 ({enhanceEquipmentTemp.enhancementLevel} / {enhanceEquipmentTemp.enhancementMaxLevel}</color>)"; //장비 강화(0 / 0)
                EquippedPreview.text = $"장착 효과 {enhanceEquipmentTemp.equippedEffect} → <color=green>{enhanceEquipmentTemp.equippedEffect + enhanceEquipmentTemp.basicEquippedEffect}</color>"; // 장착 효과 0 → 0
                OwnedPreview.text = $"보유 효과 {enhanceEquipmentTemp.ownedEffect} → <color=green>{enhanceEquipmentTemp.ownedEffect + enhanceEquipmentTemp.basicOwnedEffect}</color>";

                EnhanceCurrencyText.text = CurrencyManager.instance.GetCurrencyAmount("EnhanceStone");

                Debug.Log("필요 강화석 : " + enhanceEquipmentTemp.GetEnhanceStone());
                RequiredCurrencyText.text = enhanceEquipmentTemp.GetEnhanceStone().ToString();

                enhanceEquipment.GetComponent<WeaponInfo>().SetWeaponInfo(enhanceEquipmentTemp.GetComponent<WeaponInfo>());

                enhanceEquipment.SetUI();
                break;
            case EquipmentType.Armor:
                selectableEnhanceEquipments[(int)EquipmentType.Weapon].gameObject.SetActive(false);
                selectableEnhanceEquipments[(int)EquipmentType.Armor].gameObject.SetActive(true);
                enhanceEquipment = selectableEnhanceEquipments[(int)EquipmentType.Armor];
                Equipment tmpEquipment = EquipmentManager.GetEquipment(selectEquipment.name);

                Debug.Log($"현재 강화 방어구 : {tmpEquipment.name}");

                enhanceLevelText.text = $"장비 강화 ({tmpEquipment.enhancementLevel} / {tmpEquipment.enhancementMaxLevel}</color>)"; //장비 강화(0 / 0)
                EquippedPreview.text = $"장착 효과 {tmpEquipment.equippedEffect} → <color=green>{tmpEquipment.equippedEffect + tmpEquipment.basicEquippedEffect}</color>"; // 장착 효과 0 → 0
                OwnedPreview.text = $"보유 효과 {tmpEquipment.ownedEffect} → <color=green>{tmpEquipment.ownedEffect + tmpEquipment.basicOwnedEffect}</color>";

                EnhanceCurrencyText.text = CurrencyManager.instance.GetCurrencyAmount("EnhanceStone");

                Debug.Log("필요 강화석 : " + tmpEquipment.GetEnhanceStone());
                RequiredCurrencyText.text = tmpEquipment.GetEnhanceStone().ToString();

                enhanceEquipment.GetComponent<ArmorInfo>().SetArmorInfo(tmpEquipment.GetComponent<ArmorInfo>());

                enhanceEquipment.SetUI();
                break;
        }
    }

    // 합성 버튼 눌렸을 때 불리는 메서드
    public void OnClickComposite()
    {
        EquipmentManager.instance.Composite(selectEquipment);
        selectEquipment.SetQuantityUI();
        UpdateSelectEquipmentData();

        CheckAutoEquipActive();
        CheckAllComposite();
    }

    // 전체 합성
    public void OnClickAllComposite()
    {
        EquipmentManager.instance.AllComposite(curTabType);

        TriggerSelectEquipment(EquipmentManager.GetEquipment(selectEquipment.name));
        CheckAutoEquipActive();
        CheckAllComposite();
    }

    // 강화 버튼 눌렸을 때 불리는 메서드
    public void OnClickEnhance()
    {
        if (selectEquipment.enhancementLevel >= selectEquipment.enhancementMaxLevel) return;
        if (selectEquipment.GetEnhanceStone() > new BigInteger(CurrencyManager.instance.GetCurrencyAmount("EnhanceStone"))) return;
        CurrencyManager.instance.SubtractCurrency("EnhanceStone",selectEquipment.GetEnhanceStone());
        selectEquipment.Enhance();
        SetselectEquipmentTextUI(selectEquipment);

        if (selectEquipment.OnEquipped) OnClickEquip();

        UpdateSelectEquipmentData();

        OnClickEnhancePanel();
        CheckAutoEquipActive();
    }

    // 장착 버튼 눌렸을 때 불리는 메서드
    public void OnClickEquip()
    {
        Debug.Log("장착 됨 ");
        Player.OnEquip?.Invoke(EquipmentManager.GetEquipment(selectEquipment.name));
        CheckAutoEquipActive();
    }

    // 장착 해제 버튼 눌렀을 때 불리는 메서드
    public void OnClickUnEquip()
    {
        Player.OnUnEquip?.Invoke(selectEquipment.type);
    }

    // 장비 자동 장착
    public void OnClickAutoEquip()
    {
        Equipment recommendedEquipment = null;
        switch (selectEquipment.type)
        {
            case EquipmentType.Weapon:
                recommendedEquipment = EquipmentManager.instance.GetRecommendedWeapon();
                Player.OnEquip?.Invoke(recommendedEquipment);
                break;
            case EquipmentType.Armor:
                recommendedEquipment = EquipmentManager.instance.GetRecommendedArmor();
                Player.OnEquip?.Invoke(recommendedEquipment);
                break;
        }
        if (recommendedEquipment != null) TriggerSelectEquipment(recommendedEquipment);
        autoEquipBtn.interactable = false;
    }

    // 선택한 장비 데이터 업데이트 (저장한다고 생각하면 편함)
    public void UpdateSelectEquipmentData()
    {
        EquipmentManager.instance.SetEquipment(selectEquipment.name, selectEquipment);
    }

    public void CheckAutoEquipActive()
    {
        switch (curTabType)
        {
            case EquipmentType.Weapon:
                autoEquipBtn.interactable
                    = Player.instance.GetCurrentEquipedWeapon() != EquipmentManager.instance.GetRecommendedWeapon();
                break;
            case EquipmentType.Armor:
                autoEquipBtn.interactable
                    = Player.instance.GetCurrentEquipedArmor() != EquipmentManager.instance.GetRecommendedArmor();
                break;
        }
    }

    public void CheckAllComposite()
    {
        allCompositeBtn.interactable = EquipmentManager.instance.IsAllCompositable(curTabType);
    }

    public void CallOnTabChanged(EquipmentType tabType)
    {
        curTabType = tabType;
        TriggerSelectEquipment(EquipmentManager.instance.GetEquipment(curTabType, 0));
        CheckAutoEquipActive();
        CheckAllComposite();
    }
}
