using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Enemy Wave", menuName = "Scriptable Objects/New Enemy Wave", order = 3)]
public class EnemyWave_SO : ScriptableObject
{
    public float waveDuration;
    public List<GameObject> enemiesList = new();
    public List<int> totalEnemies = new();
    public List<SpawnMethod> spawnMethod = new();
    public List<float> spawnDelay = new();

    public enum SpawnMethod
    {
        OnLine,
        Corners
    }
}
