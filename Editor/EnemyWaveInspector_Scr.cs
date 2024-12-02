using EnemyWave;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

[CustomEditor(typeof(EnemyWave_SO))]
public class EnemyWaveInspector_Scr : Editor
{
    //int count;
    private GUIStyle labelStyle;

    public override void OnInspectorGUI()
    {
        Display();
    }

    private void Display()
    {
        serializedObject.Update();

        labelStyle = new(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        //---// Длительность волны
        SerializedProperty waveDuration = serializedObject.FindProperty("waveDuration");
        EditorGUILayout.PropertyField(waveDuration, new GUIContent("Wave Duration (sec)"));
        EditorGUILayout.Space(20f);

        //---// Враги в волне
        SerializedProperty enemiesInWave = serializedObject.FindProperty("enemiesInWave");

        DisplayListHeader(enemiesInWave, "Enemies in wave");

        #region Enemy List Elemnets
        EditorGUI.indentLevel++;
        if (enemiesInWave.isExpanded)
        {
            GUILayout.Space(10f);
            DisplayListLabels(new() { "Enemy Prefab", "Count", "Spawn Method", "Delay" }, new() { 100, 80, 105, 80 });

            for (int i = 0; i < enemiesInWave.arraySize; i++)
                DisplayEnemyInWave(enemiesInWave.GetArrayElementAtIndex(i));
        }
        EditorGUI.indentLevel--;
        #endregion

        //---// Сквады в волне
        SerializedProperty squadsInWave = serializedObject.FindProperty("enemySquads");

        DisplayListHeader(squadsInWave, "Squads in wave");

        #region Squad List Elements
        EditorGUI.indentLevel++;
        if (squadsInWave.isExpanded)
        {
            GUILayout.Space(10f);
            DisplayListLabels(new() { "Enemy Squad", "Start time" }, new() { 200, 200 });

            for (int i = 0; i < squadsInWave.arraySize; i++)
                DisplaySquadInWave(squadsInWave.GetArrayElementAtIndex(i));
        }
        EditorGUI.indentLevel--;
        #endregion

        //---// WEE в волне
        SerializedProperty wee = serializedObject.FindProperty("waveEndEvent");

        if (wee.objectReferenceValue != null)
        {

            switch (wee.objectReferenceValue.GetType().ToString())
            {
                case "MoveBackground_WEE_SO":
                    SerializedObject serObj = new(wee.objectReferenceValue);
                    EditorGUILayout.PropertyField(wee);
                    EditorGUILayout.PropertyField(serObj.FindProperty("newHeight"));
                    serObj.ApplyModifiedProperties();
                    break;
                default:
                    EditorGUILayout.PropertyField(wee);
                    break;
            }
        }
        else
        {
            EditorGUILayout.PropertyField(wee);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ListExpand(SerializedProperty listProperty)
    {
        listProperty.arraySize++;
    }
    private void ListRemoveLastElement(SerializedProperty listProperty)
    {
        if (listProperty.arraySize > 0)
            listProperty.arraySize--;
    }
    private void DisplayEnemyInWave(SerializedProperty enemyProperty)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(enemyProperty.FindPropertyRelative("enemyPrefab"), GUIContent.none);
        EditorGUILayout.PropertyField(enemyProperty.FindPropertyRelative("enemiesCount"), GUIContent.none);
        EditorGUILayout.PropertyField(enemyProperty.FindPropertyRelative("spawnMethod"), GUIContent.none);
        EditorGUILayout.PropertyField(enemyProperty.FindPropertyRelative("spawnDelay"), GUIContent.none);
        GUILayout.EndHorizontal();
    }
    private void DisplaySquadInWave(SerializedProperty squadProperty)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(squadProperty.FindPropertyRelative("enemySquad"), GUIContent.none);
        EditorGUILayout.PropertyField(squadProperty.FindPropertyRelative("startTime"), GUIContent.none);
        GUILayout.EndHorizontal();
    }
    private void DisplayListHeader(SerializedProperty listProperty, string labelText)
    {
        GUILayout.BeginHorizontal();
        listProperty.isExpanded = EditorGUILayout.Foldout(listProperty.isExpanded, labelText, true);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", GUILayout.MinWidth(80f)))
        {
            ListExpand(listProperty);
        }
        EditorGUILayout.LabelField(listProperty.arraySize.ToString(), labelStyle, GUILayout.MaxWidth(30f));
        if (GUILayout.Button("-", GUILayout.MinWidth(80f)))
        {
            ListRemoveLastElement(listProperty);
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayListLabels(List<String> labelsText, List<float> labelsWidth)
    {
        if (labelsText.Count != labelsWidth.Count)
        {
            Debug.Log("Не удалось отобразить надписи для списка, длина labelsText не совпадает с labelsWidth");
            return;
        }

        GUILayout.BeginHorizontal();
        for (int i = 0; i < labelsText.Count; i++)
        {
            if (i != 0)
                GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(labelsText[i], labelStyle, GUILayout.Width(labelsWidth[i]));
        }
        GUILayout.EndHorizontal();
    }

/*    private void OldDisplay()
    {
        serializedObject.Update();

        CheckElements();

        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        SerializedProperty waveDuration = serializedObject.FindProperty("waveDuration");
        EditorGUILayout.PropertyField(waveDuration, new GUIContent("Wave Duration (in seconds)"));
        EditorGUILayout.Space(20f);

        SerializedProperty enemy = serializedObject.FindProperty("enemiesList");
        SerializedProperty enemyNum = serializedObject.FindProperty("totalEnemies");
        SerializedProperty enemySpawn = serializedObject.FindProperty("spawnMethod");
        SerializedProperty spawnDelay = serializedObject.FindProperty("spawnDelay");
        count = enemy.arraySize;

        //---// Кнопки для расширения\сокращения списка
        EditorGUILayout.LabelField("Enemy Types and their total number", style);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.MinWidth(80f)))
        {
            AddElements();
        }
        //GUILayout.FlexibleSpace();

        EditorGUILayout.LabelField(count.ToString(), style, GUILayout.ExpandWidth(true), GUILayout.MinWidth(30f));
        //GUILayout.FlexibleSpace();
        if (GUILayout.Button("-", GUILayout.MinWidth(80f)))
        {
            RemoveElements();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10f);

        //---// Список Врагов и их количество
        for (int i = 0; i < count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(enemy.GetArrayElementAtIndex(i), GUIContent.none);
            EditorGUILayout.PropertyField(enemyNum.GetArrayElementAtIndex(i), GUIContent.none);
            EditorGUILayout.PropertyField(enemySpawn.GetArrayElementAtIndex(i), GUIContent.none);
            EditorGUILayout.PropertyField(spawnDelay.GetArrayElementAtIndex(i), GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }

        //EditorGUILayout.PropertyField(enemy);
        //EditorGUILayout.PropertyField(enemyNum);

        serializedObject.ApplyModifiedProperties();
    }
    void AddElements()
    {
        CheckElements();
        SerializedProperty enemy = serializedObject.FindProperty("enemiesList");
        SerializedProperty enemyNum = serializedObject.FindProperty("totalEnemies");
        SerializedProperty enemySpawn = serializedObject.FindProperty("spawnMethod");
        SerializedProperty spawnDelay = serializedObject.FindProperty("spawnDelay");

        enemy.arraySize++;
        enemyNum.arraySize++;
        enemySpawn.arraySize++;
        spawnDelay.arraySize++;

        count = enemy.arraySize;
    }
    void RemoveElements()
    {
        CheckElements();
        SerializedProperty enemy = serializedObject.FindProperty("enemiesList");
        SerializedProperty enemyNum = serializedObject.FindProperty("totalEnemies");
        SerializedProperty enemySpawn = serializedObject.FindProperty("spawnMethod");
        SerializedProperty spawnDelay = serializedObject.FindProperty("spawnDelay");

        if (enemy.arraySize > 0)
            enemy.arraySize--;
        if (enemyNum.arraySize > 0)
            enemyNum.arraySize--;
        if (enemySpawn.arraySize > 0)
            enemySpawn.arraySize--;
        if (spawnDelay.arraySize > 0)
            spawnDelay.arraySize--;

        count = enemy.arraySize;
    }
    void CheckElements()
    {
        SerializedProperty enemy = serializedObject.FindProperty("enemiesList");
        SerializedProperty enemyNum = serializedObject.FindProperty("totalEnemies");
        SerializedProperty enemySpawn = serializedObject.FindProperty("spawnMethod");
        SerializedProperty spawnDelay = serializedObject.FindProperty("spawnDelay");

        while (enemy.arraySize != enemyNum.arraySize)
        {
            if (enemyNum.arraySize > enemy.arraySize)
                enemyNum.arraySize--;
            else
                enemyNum.arraySize++;
        }
        while (enemy.arraySize != enemySpawn.arraySize)
        {
            if (enemySpawn.arraySize > enemy.arraySize)
                enemySpawn.arraySize--;
            else
                enemySpawn.arraySize++;
        }
        while (enemy.arraySize != spawnDelay.arraySize)
        {
            if (spawnDelay.arraySize > enemy.arraySize)
                spawnDelay.arraySize--;
            else
                spawnDelay.arraySize++;
        }
    }*/
}
