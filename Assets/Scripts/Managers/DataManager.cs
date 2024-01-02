using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private readonly Dictionary<string, DataHandler> dataHandlerDic = new Dictionary<string, DataHandler>();

    private void Awake()
    {
        instance = this;
    }

    public T GetDataHandler<T>() where T : DataHandler, new()
    {
        string key = typeof(T).Name;
        if (!dataHandlerDic.ContainsKey(key))
        {
            dataHandlerDic.Add(key, new T());
        }

        return dataHandlerDic[key] as T;
    }
}
