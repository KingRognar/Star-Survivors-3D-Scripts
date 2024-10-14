using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Enemy Wave", menuName = "Scriptable Objects/New Enemy Wave", order = 3)]
public class EnemyWave_SO : ScriptableObject
{
    public float waveDuration;
    public List<GameObject> enemiesList = new List<GameObject>();
    public List<int> totalEnemies = new List<int>();
    public List<SpawnMethod> spawnMethod = new List<SpawnMethod>();
    public List<float> spawnDelay = new List<float>();

    public enum SpawnMethod
    {
        OnLine,
        Corners
    }
}
