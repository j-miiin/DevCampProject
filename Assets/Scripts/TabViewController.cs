using UnityEngine;
using UnityEngine.UI;

public class TabViewController : MonoBehaviour
{
    [SerializeField] private Toggle weaponTabToggle;
    [SerializeField] private Toggle armorTabToggle;
    [SerializeField] private GameObject weaponTabView;
    [SerializeField] private GameObject armorTabView;

    private void Start()
    {
        weaponTabToggle.onValueChanged.AddListener((bool isOn) => { weaponTabView.SetActive(isOn); });
        armorTabToggle.onValueChanged.AddListener((bool isOn) => { armorTabView.SetActive(isOn); });
    }
}
