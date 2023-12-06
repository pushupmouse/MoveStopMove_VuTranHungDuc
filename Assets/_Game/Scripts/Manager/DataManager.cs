using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : Singleton<DataManager>
{
    public void SaveData<T>(T data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json", json);
    }

    public T LoadData<T>() 
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
        return JsonUtility.FromJson<T>(json);
    }

    public void DeleteData<T>()
    {
        File.Delete(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
    }

    public bool HasData<T>()
    {
        return File.Exists(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
    }

}
