using UnityEngine;

[CreateAssetMenu(fileName = "SummonLevelProbability", menuName = "SO/SummonLevelProbability", order = 0)]
public class SummonLevelProbability : ScriptableObject
{
    [SerializeField] private SummonProbability[] summonLevelProbList;

    public SummonProbability[] SummonLevelProbList => summonLevelProbList;
}
