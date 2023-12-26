using System;

public enum SummonType
{
    Weapon,
    Armor
}

[Serializable]
public class Summon
{
    private SummonType summonType;
    private int level;
    private int maxExp;
    private int curExp;
}
