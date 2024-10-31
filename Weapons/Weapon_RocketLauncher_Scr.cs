using UnityEngine;

public class Weapon_RocketLauncher_Scr : Weapon_Scr
{
    public GameObject rocketPrefab;
    private float nextRocketSpawnTime = -1f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && (nextRocketSpawnTime < Time.time))
        {
            InstantiateNewBullet();
            nextRocketSpawnTime = Time.time + Player_Stats_Scr.RocketLauncher.spawnDelay;
        }
    }

    void InstantiateNewBullet()
    {
        for (int i = 0; i < Player_Stats_Scr.RocketLauncher.projectileCount; i++)
        {
            GameObject newRocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            //newBullet.transform.localScale = Vector3.one * Player_Stats_Scr.Machinegun.projectileScale;
        }
    }
}
