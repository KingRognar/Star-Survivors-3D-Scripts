using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackgroundManager_Scr : MonoBehaviour
{
    public static BackgroundManager_Scr instance;

    [SerializeField] private List<GameObject> backgroundElementPrefabs = new();
    private static List<GameObject> spawnedElements = new();
    [SerializeField] private float timeBetweenSpawns = 1f;
    private float lastSpawnTime = -1f;
    private Vector3 oldSpawnPosition, newSpawnPosition;
    public Vector3 spawnPosition = new Vector3(0, -100, 80);
    public float newSpawnHeight;
    private float t = 1;

    public float backgroundSpeed = 45f;

    //TODO: set spawn delay relative to background speed

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Update()
    {
        RunParallax();
        if (t != 1) //TODO: integrate it in to Enemy Director and\or Bosses themselvs
            MoveToNewHeight();
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetNewHeight(newSpawnHeight);
        }
    }

    private void RunParallax()
    {
        if (Time.timeScale != 1)
            return;
        if (lastSpawnTime > Time.time)
            return;


        int rnd = Random.Range(0, backgroundElementPrefabs.Count);
        Instantiate(backgroundElementPrefabs[rnd], spawnPosition, Quaternion.identity); //TODO: определять z динамически
        lastSpawnTime = Time.time + (timeBetweenSpawns * 45 / backgroundSpeed);
    }
    public void SetNewHeight(float newHeight)
    {
        t = 0;
        oldSpawnPosition = spawnPosition;
        newSpawnPosition = spawnPosition;
        newSpawnPosition.y = newHeight;
        DOTween.To(() => t, x => t = x, 1f, 6f);
    }
    private void MoveToNewHeight()
    {
        //t = Mathf.MoveTowards(t, 1, Time.deltaTime);
        spawnPosition = Vector3.Lerp(oldSpawnPosition, newSpawnPosition, t);
        foreach (GameObject bgElement in spawnedElements)
            bgElement.transform.position = new(bgElement.transform.position.x, spawnPosition.y, bgElement.transform.position.z);
    }

    public static void AddToSpawnedElements(GameObject gameObject)
    {
        spawnedElements.Add(gameObject);
    }
    public static void RemoveSpawnedElements(GameObject gameObject)
    {
        spawnedElements.Remove(gameObject);
    }
}
