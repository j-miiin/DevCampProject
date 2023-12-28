public class SummonDataHandler : DataHandler
{
    private Summon[] summonList;

    // 소환 정보 리스트 저장
    public void SaveSummonList()
    {
        ES3.Save<Summon[]>("summonList", summonList);
    }

    // 소환 정보 리스트 로드
    public Summon[] LoadSummonList()
    {
        if (ES3.KeyExists("summonList"))
        {
            summonList = ES3.Load<Summon[]>("summonList");
        }
        else
        {
            CreateSummonList();
        }
        return summonList;
    }

    public void CreateSummonList()
    {
        int typeCnt = System.Enum.GetValues(typeof(SummonType)).Length;
        summonList = new Summon[typeCnt];
        for (int i = 0; i < typeCnt; i++)
        {
            summonList[i] = new Summon(1, 200);
        }
        SaveSummonList();
    }
}
