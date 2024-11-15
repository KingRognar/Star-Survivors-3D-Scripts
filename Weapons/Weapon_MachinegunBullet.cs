using UnityEngine;

public class Weapon_MachinegunBullet : Weapon_BaseProjectile_Scr
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        other.GetComponent<IDamageable>().TakeDamage(Player_Stats_Scr.Machinegun.damage, transform.position);

        //other.GetComponent<Enemy_Scr>().TakeDamage(Player_Stats_Scr.Machinegun.damage, transform.position);
        //Sound_FXManager_Scr.instance.PlayRandomFXClip(hitAudioClips, transform, 1); //TODO: Add FXManager
        Destroy(gameObject);
    }
}
