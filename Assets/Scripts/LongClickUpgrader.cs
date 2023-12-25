using UnityEngine;

public class LongClickUpgrader : LongClickAction
{
    [SerializeField] private StatusType btnType;

    protected override void UpgradeAction()
    {
        StatusUpgradeManager.instance.UpgradeStat(btnType);
    }
}
