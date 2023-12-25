using UnityEngine;

public class LongClickEnhancer : LongClickAction
{
    [SerializeField] private EquipmentUI equipmentUI;

    protected override void UpgradeAction()
    {
        equipmentUI.OnClickEnhance();
    }
}
