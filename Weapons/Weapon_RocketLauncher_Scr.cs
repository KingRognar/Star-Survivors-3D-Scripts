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

    void UpgradeProjectileSpeed()
    {
        Player_Stats_Scr.RocketLauncher.projectileSpeed += 5f;
    }
    void UpgradeProjectileDamage()
    {
        Player_Stats_Scr.RocketLauncher.damage += 1;
    }
    void UpgradeprojectileCount()
    {
        Player_Stats_Scr.RocketLauncher.projectileCount += 1;
    }
}
