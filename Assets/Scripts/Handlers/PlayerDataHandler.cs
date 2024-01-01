public struct PlayerLevelData
{
    public int level;
    public int curExp;
    public int maxExp;

    public PlayerLevelData(int level, int curExp, int maxExp)
    {
        this.level = level;
        this.curExp = curExp;
        this.maxExp = maxExp;
    }
}

public class PlayerDataHandler : DataHandler
{
    // 플레이어 레벨 및 경험치 저장
    public void SaveLevelStatus(PlayerLevelData data)
    {
        ES3.Save<int>("level", data.level);
        ES3.Save<int>("curExp", data.curExp);
        ES3.Save<int>("maxExp", data.maxExp);
    }

    // 플레이어 레벨 및 경험치 정보 로드
    public PlayerLevelData LoadLevelStatus()
    {
        PlayerLevelData data = new PlayerLevelData();
        if (ES3.KeyExists("level")) data.level = ES3.Load<int>("level");
        else data.level = 1;

        if (ES3.KeyExists("curExp")) data.curExp = ES3.Load<int>("curExp");
        else data.curExp = 0;

        if (ES3.KeyExists("maxExp")) data.maxExp = ES3.Load<int>("maxExp");
        else data.maxExp = 30;

        return data;
    }
}
