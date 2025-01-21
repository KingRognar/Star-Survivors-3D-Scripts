using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Machinegun : Weapon_Scr
{
    public GameObject bulletPrefab;
    //private float bulletSpreadCurAngle = 0f;
    //private bool spreadDirectionIsRight = true;

    private float nextBulletSpawnTime = -1f;


    void Update() //TODO: выделить ускорение при попадании в отдельное улучшение
    {
        if (Input.GetKey(KeyCode.Mouse0) && (nextBulletSpawnTime < Time.time))
        {
            InstantiateNewBullet();
            nextBulletSpawnTime = Time.time + Player_Stats_Scr.Machinegun.spawnDelay*Player_Stats_Scr.Machinegun.spawnDelayMod;
            //Debug.Log(Player_Stats_Scr.Machinegun.spawnDelay * Player_Stats_Scr.Machinegun.spawnDelayMod);
        }
    }

    void InstantiateNewBullet()
    {
        float spread = Random.Range(-Player_Stats_Scr.Machinegun.spreadAngle, Player_Stats_Scr.Machinegun.spreadAngle);
        for (int i = 0; i < Player_Stats_Scr.Machinegun.projectileCount; i++)
        {
            float xShift = -(Player_Stats_Scr.Machinegun.projectileCount - 1) * 0.2f + i * 0.4f;
            GameObject newBullet = Instantiate(bulletPrefab, transform.position + new Vector3(xShift, 0, 0), Quaternion.identity);
            newBullet.transform.RotateAround(newBullet.transform.position, Vector3.up, spread);
            newBullet.transform.localScale = Vector3.one * Player_Stats_Scr.Machinegun.projectileScale;
            //newBullet.transform.RotateAround(newBullet.transform.position, Vector3.forward, bulletSpreadCurAngle);
        }
    }

    public void UpgradeDamage(float damage)
    {
        Player_Stats_Scr.Machinegun.damage += (int)damage;
        Player_Stats_Scr.Machinegun.projectileScale += 0.1f;
    }
    public void UpgadeAttackSpeed(float delay)
    {
        Player_Stats_Scr.Machinegun.spawnDelay -= delay;
    }
    public void UpgradeProjCount(float count)
    {
        Player_Stats_Scr.Machinegun.projectileCount += (int)count;
    }

}
