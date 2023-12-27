using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonSlotUI : MonoBehaviour
{
    [SerializeField] private SummonType type;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Button summon50Button;
    [SerializeField] private Button summon100Button;

    private void Start()
    {
        summon50Button.onClick.AddListener(() => SummonManager.instance.SummonEquipmentWithType(type, SummonCountType.Fifty));
        summon100Button.onClick.AddListener(() => SummonManager.instance.SummonEquipmentWithType(type, SummonCountType.Hundred));
        SummonManager.instance.OnSummon += UpdateSummonUIWithTypeCheck;
    }

    private void OnEnable()
    {
        UpdateSummonUI();
    }

    public void UpdateSummonUIWithTypeCheck(SummonType summonType)
    {
        if (summonType == type) UpdateSummonUI();
    }

    public void UpdateSummonUI()
    {
        Summon summon = SummonManager.instance.GetSummonInfo(type);
        levelText.text = $"{summon.level}";
        expText.text = $"{summon.curExp}/{summon.maxExp}";
        expSlider.maxValue = summon.maxExp;
        expSlider.value = summon.curExp;
    }
}
