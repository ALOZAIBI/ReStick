using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(Ability),true)]
[CanEditMultipleObjects]
public class CustomAbilityInspector : Editor
{
    public override void OnInspectorGUI() {
        Ability targetScript = (Ability)target;
        DrawDefaultInspector();
        targetScript.numberOfValues = EditorGUILayout.IntField("Number of Values", targetScript.numberOfValues);
        targetScript.OnValidate();
        //Creates horizontal Display for value names 
        EditorGUIUtility.labelWidth = 20;
        EditorGUILayout.LabelField("Value Names", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.valueNames.Count; i++) {
            targetScript.valueNames[i] = EditorGUILayout.TextField("", targetScript.valueNames[i]);
        }
        EditorGUILayout.EndHorizontal();

        //Creates horizontal Display for Baseamt
        EditorGUILayout.LabelField("Base Amount", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.baseAmt.Count; i++) {
            targetScript.baseAmt[i] = EditorGUILayout.FloatField("", targetScript.baseAmt[i]);
        }
        EditorGUILayout.EndHorizontal();

        //Creates horizontal Display for PDRatio
        EditorGUILayout.LabelField("Power Ratio", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.PDRatio.Count; i++) {
            targetScript.PDRatio[i] = EditorGUILayout.FloatField("", targetScript.PDRatio[i]);
        }
        EditorGUILayout.EndHorizontal();

        //Creates horizontal Display for MDRatio
        EditorGUILayout.LabelField("Magic Ratio", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.MDRatio.Count; i++) {
            targetScript.MDRatio[i] = EditorGUILayout.FloatField("", targetScript.MDRatio[i]);
        }
        EditorGUILayout.EndHorizontal();

        //Creates horizontal Display for INFRatio
        EditorGUILayout.LabelField("Influence Ratio", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.INFRatio.Count; i++) {
            targetScript.INFRatio[i] = EditorGUILayout.FloatField("", targetScript.INFRatio[i]);
        }
        EditorGUILayout.EndHorizontal();

        //Creates horizontal Display for HPMaxRatio
        EditorGUILayout.LabelField("Max HP Ratio", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.HPMaxRatio.Count; i++) {
            targetScript.HPMaxRatio[i] = EditorGUILayout.FloatField("", targetScript.HPMaxRatio[i]);
        }
        EditorGUILayout.EndHorizontal();

        //Creates horizontal Display for HPRatio
        EditorGUILayout.LabelField("HP Ratio", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.HPRatio.Count; i++) {
            targetScript.HPRatio[i] = EditorGUILayout.FloatField("", targetScript.HPRatio[i]);
        }
        EditorGUILayout.EndHorizontal();

        //Creates horizontal Display for LVLRatio
        EditorGUILayout.LabelField("Level Ratio", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.LVLRatio.Count; i++) {
            targetScript.LVLRatio[i] = EditorGUILayout.FloatField("", targetScript.LVLRatio[i]);
        }
        EditorGUILayout.EndHorizontal();

        //Creates horizontal Display for MSRatio
        EditorGUILayout.LabelField("Speed Ratio", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.MSRatio.Count; i++) {
            targetScript.MSRatio[i] = EditorGUILayout.FloatField("", targetScript.MSRatio[i]);
        }
        EditorGUILayout.EndHorizontal();


        //Creates horizontal Display for ASRatio
        EditorGUILayout.LabelField("Attack Speed Ratio", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.ASRatio.Count; i++) {
            targetScript.ASRatio[i] = EditorGUILayout.FloatField("", targetScript.ASRatio[i]);
        }
        EditorGUILayout.EndHorizontal();


        //Creates horizontal Display for ValueAmt
        EditorGUILayout.LabelField("Value Amount", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < targetScript.valueAmt.Count; i++) {
            targetScript.valueAmt[i] = EditorGUILayout.FloatField("", targetScript.valueAmt[i]);
        }
        EditorGUILayout.EndHorizontal();

        //To save the changes to prefab. Without this the changes reset when loading the project
        if (GUI.changed) { EditorUtility.SetDirty(targetScript); }
    }
}
#endif