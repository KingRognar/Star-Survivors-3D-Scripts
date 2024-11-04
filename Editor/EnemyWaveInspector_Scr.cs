using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        EditorGUILayout.PropertyField(waveDuration, new GUIContent("Wave Duration (in seconds)"));
        EditorGUILayout.Space(20f);

        SerializedProperty enemiesInWave = serializedObject.FindProperty("enemiesInWave");

        enemiesInWave.isExpanded = EditorGUILayout.Foldout(enemiesInWave.isExpanded, "Enemies in wave", true);
        EditorGUI.indentLevel++;
        if (enemiesInWave.isExpanded)
        {
            for (int i = 0; i < enemiesInWave.arraySize; i++)
                EditorGUILayout.PropertyField(enemiesInWave.GetArrayElementAtIndex(i));
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
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
