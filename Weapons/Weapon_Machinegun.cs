using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Machinegun : MonoBehaviour
{
    public GameObject bulletPrefab;
    private float bulletSpreadCurAngle = 0f;
    //private bool spreadDirectionIsRight = true;

    private float lastBulletSpawnTime = -1f;


    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && (lastBulletSpawnTime + Player_Stats_Scr.Machinegun.spawnDelay < Time.time))
        {
            InstantiateNewBullet();
            lastBulletSpawnTime = Time.time;
        }
    }

    void InstantiateNewBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.transform.RotateAround(newBullet.transform.position, Vector3.forward, Random.Range(-Player_Stats_Scr.Machinegun.spreadAngle, Player_Stats_Scr.Machinegun.spreadAngle));
        newBullet.transform.localScale = Vector3.one * Player_Stats_Scr.Machinegun.projectileScale;
        newBullet.transform.RotateAround(newBullet.transform.position, Vector3.forward, bulletSpreadCurAngle);
    }
}
