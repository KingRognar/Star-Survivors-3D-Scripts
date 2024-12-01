using System.Collections.Generic;
using UnityEngine;
using EnemySquad;

[CreateAssetMenu(fileName = "New Enemy Squad", menuName = "Scriptable Objects/New Enemy Squad", order = 4)]
public class EnemySquad_SO : ScriptableObject
{
    public SquadSpawnMethod squadSpawnMethod;
    public float spawnDelay;
    public List<GameObject> squadEnemies = new ();
    public List<Vector3> spawnPositions;
}

namespace EnemySquad
{
    public enum SquadSpawnMethod
    {
        fromOnePointWithDelay,
        fromMultiplePointsAtOnce,
        inWedgeFormation
    }
}
