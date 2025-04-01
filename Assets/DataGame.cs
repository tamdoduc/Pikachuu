using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class data
{
    public int level;
    public int row;
    public int col;
    public float time;
}

public class DataGamePikachu
{
    public List<data> dataPikachu = new List<data>();
}

public class DataGame : MonoBehaviour
{
    public List<data> Datas = new List<data>();
    public List<data> DataUpdated = new List<data>();
    public List<data> DataLoaded = new List<data>();
    public static DataGame instance;
    public DataGamePikachu dataPikachu;

    private void Awake()
    {
        instance = this;
    }

    public void SaveToJson()
    {
        DataGamePikachu data = new DataGamePikachu();
        data.dataPikachu = DataUpdated;
        string json = JsonUtility.ToJson(data);
        //  File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        File.WriteAllText("Assets/Resources/Data/DataGame.json", json);
    }

    public void UpdateJson()
    {
        for (int i = 0; i < Datas.Count; i++)
        {
            DataUpdated.Add(Datas[i]);
        }
    }

    public DataGamePikachu LoadFromJson()
    {
        DataGamePikachu result = new DataGamePikachu();
        string data = "";
#if UNITY_EDITOR
        data = File.ReadAllText("Assets/Resources/Data/DataGame.json");
#else
            data = Resources.Load<TextAsset>("Data/DataGame").text;

#endif
        result = JsonUtility.FromJson<DataGamePikachu>(data);
        dataPikachu = result;
        DataLoaded = dataPikachu.dataPikachu;
        return result;
    }
}