using UnityEngine;

public class Weapon_SawBlade_Scr : Weapon_BaseProjectile_Scr
{
    private Transform sawModelTransform;
    private int collisionsLeft;

    //TODO: Придумать как нормально отскакивать от границ экрана

    private void Start()
    {
        collisionsLeft = Player_Stats_Scr.Grinder.collisionsNum;
        sawModelTransform = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Border"))
            return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy_Scr>().TakeDamage(Player_Stats_Scr.Grinder.damage, transform.position);
            bounceRandom(other.transform.position);
        }
        if (other.gameObject.CompareTag("Border"))
        {
            bounceRandom(transform.position + other.transform.position.normalized);
        }

        //Sound_FXManager_Scr.instance.PlayRandomFXClip(hitAudioClips, transform, 1); //TODO: Add FXManager
    }

    protected override void ProjectileMovement()
    {
        sawModelTransform.Rotate(transform.up, -720 * Time.deltaTime, Space.Self);
        //transform.Rotate(transform.up, 180 * Time.deltaTime, Space.Self); //TODO: сделать из этого апгрейд
        base.ProjectileMovement();
    }
    private void bounceRandom(Vector3 otherObjPosition)
    {
        collisionsLeft--;
        if (collisionsLeft < 0)
            Destroy(gameObject);

        Vector3 generalDirection = transform.position - otherObjPosition;
        generalDirection = Quaternion.AngleAxis(Random.Range(-10, 10), Vector3.up) * generalDirection;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, generalDirection);
    }
}
