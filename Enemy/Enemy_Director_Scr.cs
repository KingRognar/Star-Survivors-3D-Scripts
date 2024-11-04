using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy_Director_Scr : MonoBehaviour
{
    public static Enemy_Director_Scr instance;

    [SerializeField] private List<EnemyWave_SO> wavesSOs = new();
    static public Dictionary<int,int> enemyCountByID = new();
    //[SerializeField] private List<int> spawnedEnemiesByType = new List<int>();
    private int currentWave = 0;
    private float nextWaveIn = 0;

    private float upperPoint = 0; private float leftmostPoint = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        upperPoint = Camera.main.GetUpperRightCorner().y;
        leftmostPoint = Camera.main.GetUpperRightCorner().x;

        nextWaveIn = 0;

        _ = SpawnEnemiesOnTime();
    }

    private async Task SpawnEnemiesOnTime() //TODO: придумать как не крутить по всему списку каждый кадр
    {
        while (true)
        {
            if (destroyCancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (Time.timeScale != 0)
            {
                if (Time.time > nextWaveIn)
                {
                    Debug.Log("Started wave " + currentWave);

                    if (currentWave >= wavesSOs.Count)
                    {
                        Debug.Log("Waves are ended");
                        return;
                    }

                    for (int i = 0; i < wavesSOs[currentWave].enemiesInWave.Count; i++)
                    {
                        _ = SpawnEnemy(
                            wavesSOs[currentWave].enemiesInWave[i].enemyPrefab,
                            wavesSOs[currentWave].enemiesInWave[i].enemiesCount, 
                            wavesSOs[currentWave].enemiesInWave[i].spawnMethod, 
                            wavesSOs[currentWave].enemiesInWave[i].spawnDelay,
                            wavesSOs[currentWave].waveDuration);
                    }

                    nextWaveIn += wavesSOs[currentWave].waveDuration;
                    currentWave++;
                }
            }
            await Task.Yield();
        }
    }

    private async Task SpawnEnemy(GameObject enemyPrefab, int totalEnemies, EnemyWave_SO.SpawnMethod spawnMethod, float spawnDelay, float waveDuration)
    {
        int enemyID = enemyPrefab.GetComponent<Enemy_Scr>().EnemyId;
        float endTime = Time.time + waveDuration;
        float nextSpawnTime = Time.time;
        while (Time.time < endTime)
        {
            if (destroyCancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (Time.timeScale != 0)
            {
                if (Time.time >= nextSpawnTime)
                {
                    if (enemyCountByID.ContainsKey(enemyID))
                    {
                        if (enemyCountByID[enemyID] < totalEnemies)
                        {
                            SpawnEnemyByMethod(enemyPrefab, spawnMethod);
                            nextSpawnTime = Time.time + spawnDelay;
                        }
                    }
                    else
                    {
                        SpawnEnemyByMethod(enemyPrefab, spawnMethod);
                        nextSpawnTime = Time.time + spawnDelay;
                    }
                }
            }
            await Task.Yield();
        }
    }
    #region ----Spawn Methods
    /// <summary>
    /// Метод позволяющий выбрать способ спавна врага
    /// </summary>
    /// <param name="enemyPrefab">Префаб врага</param>
    /// <param name="spawnMethod">Enum способа спавна</param>
    private void SpawnEnemyByMethod(GameObject enemyPrefab, EnemyWave_SO.SpawnMethod spawnMethod)
    {
        switch (spawnMethod)
        {
            case EnemyWave_SO.SpawnMethod.OnLine:
                {
                    SpawnRandomOnLane(enemyPrefab);
                    break;
                }
            case EnemyWave_SO.SpawnMethod.Corners:
                {
                    SpawnOnUpperCorner(enemyPrefab, true);
                    break;
                }
        }
    }
    /// <summary>
    /// Метод для рандомного спавна врага на горизонтальной линии, находящейся за видимой областью
    /// </summary>
    /// <param name="enemyPrefab">Префаб врага</param>
    /// <returns>Объект заспавненного врага</returns>
    private GameObject SpawnRandomOnLane(GameObject enemyPrefab)
    {
        //return Instantiate(enemyPrefab, new Vector3(Random.Range(leftmostPoint, rightmostPoint), upperPoint, 0), Quaternion.identity);
        return Instantiate(enemyPrefab, Camera.main.GetRandomPointOnHorizontalLine(50, Camera.main.pixelHeight), Quaternion.identity);
    }
    /// <summary>
    /// Метод для спавна врага в левом или правом верхнем углу
    /// </summary>
    /// <param name="enemyPrefab">Префаб врага</param>
    /// <param name="spawnFromRightCorner">Спавнить врага в правом углу?</param>
    /// <returns>Объект заспавненного врага</returns>
    private GameObject SpawnOnUpperCorner(GameObject enemyPrefab, bool spawnFromRightCorner)
    {
        if (spawnFromRightCorner)
            return Instantiate(enemyPrefab, new Vector3(-leftmostPoint, upperPoint, 0), Quaternion.identity);
        else
            return Instantiate(enemyPrefab, new Vector3(leftmostPoint, upperPoint, 0), Quaternion.identity);
    }
    #endregion


    /*    //// OLD VERSION
        public static Enemy_Director_Scr instance;


        [SerializeField] private GameObject basicEnemyPrefab;
        [SerializeField] private GameObject arcEnemyPrefab;
        [SerializeField] private GameObject railgunEnemyPrefab;
        public static List<Transform> railgunEnemiesList = new List<Transform>();

        [SerializeField] private float enemySpawnDelay = 1f;
        //private float lastTimeEnemySpawned = -1f;

        [SerializeField] private float waveTime = 120f;
        [SerializeField] private int waveNum = 0;
        private int numberOfArcsInSquad = 4;
        private bool spawnArcsFromLeft = true;

        private int screenWidth, screenHeight;
        private float leftmostPoint, rightmostPoint, upperPoint;


        //TODO: сделать SO уровня, который будет контролировать спавн врагов


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            screenHeight = Camera.main.pixelHeight + 100;
            screenWidth = Camera.main.pixelWidth - 100;

            float cameraZPos = Camera.main.transform.position.z;
            leftmostPoint = Camera.main.ScreenToWorldPoint(new Vector3(100, screenHeight, -cameraZPos)).x;
            rightmostPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenWidth, -cameraZPos)).x;
            upperPoint = Camera.main.ScreenToWorldPoint(new Vector3(100, screenHeight, -cameraZPos)).y;

            _ = SpawnEnemiesOnTime();
        }


        private async Task SpawnEnemiesOnTime ()
        {
            float timeRateIncrDelay = 10f;
            float lastTimeRateIncr = timeRateIncrDelay;

            while (true)
            {
                if (destroyCancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (Time.timeScale != 0)
                {
                    if (lastTimeRateIncr <= Time.time)
                    {
                        enemySpawnDelay *= 0.99f;
                        lastTimeRateIncr = Time.time + timeRateIncrDelay;
                    }

                    waveNum = (int)(Time.time / waveTime);
                    if (waveNum % 2 == 0)
                    {
                        if (Random.Range(0,100) < 40)
                        {
                            railgunEnemiesList.Add(SpawnRandomOnLane(railgunEnemyPrefab).transform);
                        }

                        SpawnBasicEnemy();
                        await Task.Delay((int)(1000 * enemySpawnDelay));
                    }
                    else
                    {
                        await SpawnArcSquad();
                        await Task.Delay((int)(2000 * enemySpawnDelay));
                    }

                } else
                {
                    await Task.Yield();
                }

            }


        }



        private void SpawnBasicEnemy()
        {
            SpawnRandomOnLane(basicEnemyPrefab);
        }
        private async Task SpawnArcSquad()
        {
            for (int i = 0; i < numberOfArcsInSquad; i++)
            {
                if (destroyCancellationToken.IsCancellationRequested)
                {
                    return;
                }

                Enemy_ArcMoving_Scr arcEnemy = SpawnOnUpperCorner(arcEnemyPrefab, spawnArcsFromLeft).GetComponent<Enemy_ArcMoving_Scr>();
                arcEnemy.moveToRight = !spawnArcsFromLeft;

                await Task.Delay(400);
            }
            spawnArcsFromLeft = !spawnArcsFromLeft;
        }

        private GameObject SpawnRandomOnLane(GameObject enemyPrefab)
        {
            //return Instantiate(enemyPrefab, new Vector3(Random.Range(leftmostPoint, rightmostPoint), upperPoint, 0), Quaternion.identity);
            return Instantiate(enemyPrefab, Camera.main.GetRandomPointOnHorizontalLine(50, Camera.main.pixelWidth + 100), Quaternion.identity);
        }
        private GameObject SpawnOnUpperCorner(GameObject enemyPrefab, bool spawnFromRightCorner)
        {
            if (spawnFromRightCorner)
                return Instantiate(enemyPrefab, new Vector3(-leftmostPoint, upperPoint, 0), Quaternion.identity);
            else
                return Instantiate(enemyPrefab, new Vector3(leftmostPoint, upperPoint, 0), Quaternion.identity);
        }*/

}
