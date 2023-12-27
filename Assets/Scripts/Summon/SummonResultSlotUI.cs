using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonResultSlotUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private TMP_Text levelText;

    public void SetSummonResultSlotUI(Equipment equipment)
    {
        background.color =
            (equipment.type == EquipmentType.Weapon)
            ? (equipment as WeaponInfo).myColor
            : (equipment as ArmorInfo).myColor;
        rarityText.text = equipment.rarity.ToString();
        levelText.text = equipment.level.ToString();
    }
}
