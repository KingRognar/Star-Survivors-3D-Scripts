using System.Collections;
using UnityEngine;

public class Weapon_MachinegunBullet : Weapon_BaseProjectile_Scr
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                other.GetComponent<IDamageable>().TakeDamage(Player_Stats_Scr.Machinegun.damage, transform.position);

                //other.GetComponent<Enemy_Scr>().TakeDamage(Player_Stats_Scr.Machinegun.damage, transform.position);
                //Sound_FXManager_Scr.instance.PlayRandomFXClip(hitAudioClips, transform, 1); //TODO: Add FXManager
                Destroy(gameObject);
                return;
            case "Obstacle":
                Vector3 collisionPoint = other.ClosestPoint(transform.position);
                DebriesMaker_Scr.instance.HitFromPosAndDir(collisionPoint, transform.position - collisionPoint);
                Destroy(gameObject);
                return;
            default:
                return;
        }
    }
}
