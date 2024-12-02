using UnityEngine;

public class Weapon_SawBlade_Scr : Weapon_BaseProjectile_Scr
{
    private Transform sawModelTransform;
    private int collisionsLeft;

    //TODO: Придумать как нормально отскакивать от границ экрана
    //TODO: Придумать как нормально отскакивать...

    private void Start()
    {
        collisionsLeft = Player_Stats_Scr.Grinder.collisionsNum;
        sawModelTransform = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                other.GetComponent<IDamageable>().TakeDamage(Player_Stats_Scr.Grinder.damage, transform.position);
                BounceRandom(other.transform.position);
                return;
            case "Obstacle":
            case "Border":
                Vector3 collisionPoint = other.ClosestPoint(transform.position);
                DebriesMaker_Scr.instance.HitFromPosAndDir(collisionPoint, transform.position - collisionPoint);
                BounceRandom(other.transform.position);
                return;
            default:
                return;
        }

        //Sound_FXManager_Scr.instance.PlayRandomFXClip(hitAudioClips, transform, 1); //TODO: Add FXManager
    }

    protected override void ProjectileMovement()
    {
        sawModelTransform.Rotate(transform.up, -720 * Time.deltaTime, Space.Self);
        //transform.Rotate(transform.up, 180 * Time.deltaTime, Space.Self); //TODO: сделать из этого апгрейд
        base.ProjectileMovement();
    }
    private void BounceRandom(Vector3 otherObjPosition) 
    {
        collisionsLeft--;
        if (collisionsLeft < 0)
            Destroy(gameObject);

        otherObjPosition.y = 0;
        Vector3 generalDirection = transform.position - otherObjPosition;
        generalDirection = Quaternion.AngleAxis(Random.Range(-10, 10), Vector3.up) * generalDirection;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, generalDirection);
    }
}
