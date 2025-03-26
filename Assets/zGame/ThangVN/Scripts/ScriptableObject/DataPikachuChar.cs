using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataChar
{
    public int id;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "DataPikachuChar", menuName = "ScriptableObjects/DataPikachuChar")]
public class DataPikachuChar : ScriptableObject
{
    public List<DataChar> data;
}
