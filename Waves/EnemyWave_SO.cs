using System;
using System.Collections.Generic;
using UnityEngine;
using EnemyWave;

[CreateAssetMenu(fileName = "New Enemy Wave", menuName = "Scriptable Objects/New Enemy Wave", order = 3)]
public class EnemyWave_SO : ScriptableObject
{
    public float waveDuration;
    public List<EnemyInWave> enemiesInWave = new();
    public List<SquadInWave> enemySquads = new();
    public WaveEndEvent_SO waveEndEvent;
}

namespace EnemyWave
{
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
    [Serializable] public struct SquadInWave
    {
        public EnemySquad_SO enemySquad;
        public float startTime;

        public SquadInWave(EnemySquad_SO _enemySquad, float _startTime)
        {
            enemySquad = _enemySquad;
            startTime = _startTime;
        }
    }
    public enum SpawnMethod
    {
        OnLine,
        Corners
    }
}

