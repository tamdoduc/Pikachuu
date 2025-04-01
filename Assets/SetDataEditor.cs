using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(DataGame), true)]
public class SetDataEditor :Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DataGame savePositonGame16 = (DataGame)target;
        if (GUILayout.Button("Update Data"))
        {
            savePositonGame16.UpdateJson();
        }
        if (GUILayout.Button("Save Data"))
        {
            savePositonGame16.SaveToJson();
        }
        if (GUILayout.Button("Load Data"))
        {
            savePositonGame16.LoadFromJson();
        }
    }
}
#endif
