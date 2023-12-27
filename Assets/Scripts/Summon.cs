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
    public int level;
    public int maxExp;
    public int curExp;

    public Summon(int level, int maxExp)
    {
        this.level = level;
        this.maxExp = maxExp;
        this.curExp = 0;
    }

    public void GetExp(int exp)
    {
        curExp += exp;
        if (curExp >= maxExp) LevelUp();
    }

    public void LevelUp()
    {
        level++;
        curExp -= maxExp;
        maxExp += 100;
    }
}
