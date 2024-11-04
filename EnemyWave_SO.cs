using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Enemy Wave", menuName = "Scriptable Objects/New Enemy Wave", order = 3)]
public class EnemyWave_SO : ScriptableObject
{
    public float waveDuration;
    public List<EnemyInWave> enemiesInWave = new();

    public enum SpawnMethod
    {
        OnLine,
        Corners
    }
    [Serializable] public struct EnemyInWave
    {
        public GameObject enemyPrefab;
        public int enemiesCount;
        public SpawnMethod spawnMethod;
        public float spawnDelay;

        public EnemyInWave(GameObject _enemyPrefab, int _enemiesCount, SpawnMethod _spawnMethod, float _spawnDelay)
        {
            enemyPrefab = _enemyPrefab;
            enemiesCount = _enemiesCount;
            spawnMethod = _spawnMethod;
            spawnDelay = _spawnDelay;
        }
    }
}
