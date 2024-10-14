using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon_MachinegunBullet : MonoBehaviour
{
    public float bulletSpeed = 2f;

    [SerializeField] private AudioClip[] hitAudioClips;

    void Update()
    {
        transform.parent.position += transform.parent.forward * Time.deltaTime * bulletSpeed;
        //transform.position += transform.forward * Time.deltaTime * bulletSpeed;
    }

    private void OnBecameInvisible()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        other.GetComponent<Enemy_Scr>().TakeDamage(Player_Stats_Scr.Machinegun.damage, transform.position);

        //Sound_FXManager_Scr.instance.PlayRandomFXClip(hitAudioClips, transform, 1); //TODO: Add FXManager
        Destroy(gameObject);
    }
}
