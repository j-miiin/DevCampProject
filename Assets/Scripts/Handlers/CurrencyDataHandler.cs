using System.Collections.Generic;

public class CurrencyDataHandler : DataHandler
{
    private List<Currency> currencies;

    // 모든 통화를 로컬에 저장시키는 메서드
    public void SaveCurrencies()
    {
        ES3.Save<List<Currency>>("currencies", currencies);
    }

    public void SaveCurrencies(List<Currency> currencyList)
    {
        ES3.Save<List<Currency>>("currencies", currencyList);
    }

    // 로컬에 저장되어있는 모든 통화를 불러오는 메서드
    public List<Currency> LoadCurrencies()
    {
        if (ES3.KeyExists("currencies"))
        {
            currencies = ES3.Load<List<Currency>>("currencies");
            SaveCurrencies();
            return currencies;
        }
        else return null;
    }
}
