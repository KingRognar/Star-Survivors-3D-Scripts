using System.Collections;
using UnityEngine;

public class Weapon_MachinegunBullet : Weapon_BaseProjectile_Scr
{
    private float lifetime;

    private void Start()
    {
        lifetime = Time.time + 4f;
    }
    protected override void ProjectileMovement()
    {
        base.ProjectileMovement();
        if (lifetime < Time.time)
            DestroyOnLifetime();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                other.GetComponent<IDamageable>().TakeDamage(Player_Stats_Scr.Machinegun.damage, transform.position);

                //other.GetComponent<Enemy_Scr>().TakeDamage(Player_Stats_Scr.Machinegun.damage, transform.position);
                //Sound_FXManager_Scr.instance.PlayRandomFXClip(hitAudioClips, transform, 1); //TODO: Add FXManager
                if (Player_Stats_Scr.Machinegun.spawnDelayMod > 0.6f)
                    Player_Stats_Scr.Machinegun.spawnDelayMod -= 0.1f;

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

    private void DestroyOnLifetime()
    {
        Player_Stats_Scr.Machinegun.spawnDelayMod = 1f;
        Destroy(gameObject);
    }
}
