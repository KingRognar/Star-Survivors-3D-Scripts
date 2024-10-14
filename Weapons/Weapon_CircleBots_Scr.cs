using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_CircleBots_Scr : Weapon_Scr
{
    private float lastBulletSpawnTime = 0f;
    private int nextBotToShoot = 0;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject circleBotPrefab;
    private List<Transform> botsTransforms = new List<Transform>();
    private int curBotsCount;

    protected override void Start()
    {
        base.Start();

        curBotsCount = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            botsTransforms.Add(transform.GetChild(i));
        }
        UpdateBotsRadialPositions();
    }
    private void Update()
    {
        transform.Rotate(transform.forward, -Player_Stats_Scr.CircleBots.rotationSpeed * Time.deltaTime, Space.Self);

        if (lastBulletSpawnTime + Player_Stats_Scr.CircleBots.bulletSpawnDelay / curBotsCount < Time.time)
        {
            SpawnBullet();
            lastBulletSpawnTime = Time.time;
        }
    }

    private void SpawnBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, botsTransforms[nextBotToShoot].position, Quaternion.identity); // TODO: подправить чтоб стрелял откуда надо
        nextBotToShoot++;
        if (nextBotToShoot >= botsTransforms.Count)
            nextBotToShoot = 0;
    }
    private void UpdateBotsRadialPositions()
    {
        for (int i = 0; i < curBotsCount; i++)
        {
            botsTransforms[i].localPosition = Quaternion.AngleAxis(360 / curBotsCount * i, Vector3.forward) * Vector3.right * Player_Stats_Scr.CircleBots.circlingDistance;
            botsTransforms[i].rotation = Quaternion.identity;
        }
    }


    #region Upgrade Methods
    public void AddBots(float numOfBotsToAdd)
    {
        for (int i = 0; i < numOfBotsToAdd; i++)
        {
            botsTransforms.Add(Instantiate(circleBotPrefab, transform).transform);
            curBotsCount++;
        }
        UpdateBotsRadialPositions();
    }
    public void IncreaseCirclingRadius(float radiusIncrease)
    {
        Player_Stats_Scr.CircleBots.circlingDistance += radiusIncrease;
        UpdateBotsRadialPositions();
    }
    public void IncreaseRotationSpeed(float speedIncrease)
    {
        Player_Stats_Scr.CircleBots.rotationSpeed += speedIncrease;
    }
    public void DecreaseShotDelay(float delayDecrease)
    {
        Player_Stats_Scr.CircleBots.bulletSpawnDelay -= delayDecrease;
    }
    #endregion
}
