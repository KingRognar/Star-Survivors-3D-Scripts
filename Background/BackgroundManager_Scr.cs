using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager_Scr : MonoBehaviour
{
    [SerializeField] private List<GameObject> backgroundElementPrefabs = new();
    [SerializeField] private float timeBetweenSpawns = 1f;
    private float lastSpawnTime = -1f;

    public static float backgroundSpeed = 45f;

    void Update()
    {
        RunParallax();
    }

    private void RunParallax()
    {
        if (Time.timeScale != 1)
            return;
        if (lastSpawnTime > Time.time)
            return;


        int rnd = Random.Range(0, backgroundElementPrefabs.Count);
        Instantiate(backgroundElementPrefabs[rnd], new Vector3(0, -100, 80), Quaternion.identity); //TODO: определять z динамически
        lastSpawnTime = Time.time + timeBetweenSpawns;
    }
}
