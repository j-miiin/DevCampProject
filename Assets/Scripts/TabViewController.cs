using System;
using UnityEngine;
using UnityEngine.UI;

public class TabViewController : MonoBehaviour
{
    public event Action<EquipmentType> OnTabChanged;

    [SerializeField] private Toggle weaponTabToggle;
    [SerializeField] private Toggle armorTabToggle;
    [SerializeField] private GameObject weaponTabView;
    [SerializeField] private GameObject armorTabView;

    private EquipmentType tabType = EquipmentType.Weapon;

    private void Start()
    {
        weaponTabToggle.onValueChanged.AddListener((bool isOn) => {
                weaponTabView.SetActive(isOn);
                tabType = (isOn) ? EquipmentType.Weapon : EquipmentType.Armor;
                OnTabChanged?.Invoke(tabType);
            });
        armorTabToggle.onValueChanged.AddListener((bool isOn) => {
            armorTabView.SetActive(isOn);
            tabType = (isOn) ? EquipmentType.Armor : EquipmentType.Weapon;
            OnTabChanged?.Invoke(tabType);
        });
    }

    private void OnEnable()
    {
        OnTabChanged?.Invoke(tabType);
    }
}
