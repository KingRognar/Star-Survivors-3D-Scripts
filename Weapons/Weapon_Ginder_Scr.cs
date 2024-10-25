using UnityEngine;

public class Weapon_Ginder_Scr : Weapon_Scr
{
    public GameObject sawPrefab;
    //private float bulletSpreadCurAngle = 0f;
    //private bool spreadDirectionIsRight = true;

    private float nextBulletSpawnTime = -1f;


    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && (nextBulletSpawnTime < Time.time))
        {
            InstantiateNewBullet();
            nextBulletSpawnTime = Time.time + Player_Stats_Scr.Grinder.spawnDelay;
        }
    }

    void InstantiateNewBullet()
    {
        for (int i = 0; i < Player_Stats_Scr.Grinder.projectileCount; i++)
        {
            float xShift = -(Player_Stats_Scr.Grinder.projectileCount - 1) * 0.2f + i * 0.4f; // TODO: придумать другой спрэд
            GameObject newBullet = Instantiate(sawPrefab, transform.position + new Vector3(xShift, 0, 0), Quaternion.identity);
            newBullet.transform.localScale = Vector3.one * Player_Stats_Scr.Grinder.projectileScale;
        }
    }

    public void UpgradeDamage(float damage)
    {
        Player_Stats_Scr.Grinder.damage += (int)damage;
        Player_Stats_Scr.Grinder.projectileScale += 0.1f;
    }
    public void UpgadeAttackSpeed(float delay)
    {
        Player_Stats_Scr.Grinder.spawnDelay -= delay;
    }
    public void UpgradeProjCount(float count)
    {
        Player_Stats_Scr.Grinder.projectileCount += (int)count;
    }
    public void UpgradeCollisionsNum(float count)
    {
        Player_Stats_Scr.Grinder.collisionsNum += (int)count;
    }

}
