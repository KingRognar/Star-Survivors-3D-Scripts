using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class Weapon_Rocket_Scr : Weapon_BaseProjectile_Scr
{
    private Spline spline;
    private float distanceCovered = 0;
    private Vector3 tangentVector;
    private Transform targetTransform;
    [SerializeField] private LayerMask targetLayer;

    //TODO: добавить VFX для взрыва
    //TODO: добавить VFX для огня\дыма из сопла

    private void Start()
    {
        SeekTarget();
        CreateMovementSpline();
    }

    protected override void ProjectileMovement()
    {
        if (spline == null) //TODO: решить что делать с ракетой когда нет сплайна с целью
        {
            Debug.Log("У ракеты отсутствует spline");
            return;
        }

        distanceCovered += projectileSpeed * Time.deltaTime / spline.GetLength();
        if (distanceCovered >= 1)
        {
            Explode();
        }
        transform.position = spline.EvaluatePosition(distanceCovered);
        transform.LookAt(spline.EvaluatePosition(distanceCovered + 0.01f));

        UpdateMovementSpline();
    }
    private void CreateMovementSpline()
    {
        Vector3 targetPosition;

        if (targetTransform == null)
        {
            targetPosition = Camera.main.GetRandomPointFromScreen(100, 100);
        }
        else { 
            targetPosition = targetTransform.position;
        }

        if (Random.Range(0f, 1f) >= 0.5f)
            tangentVector = Vector3.Cross((targetPosition - transform.position).normalized, Vector3.up);
        else
            tangentVector = Vector3.Cross((targetPosition - transform.position).normalized, Vector3.down);

        spline = new()
        {
            new BezierKnot(transform.position, Vector3.zero, tangentVector * 3),
            new BezierKnot(targetPosition, tangentVector, Vector3.zero)
        };
    }
    private void UpdateMovementSpline()
    {
        if (targetTransform == null)
        {
            //OutOfTargetDestroy();
            return;
        }
        spline.SetKnot(1, new(targetTransform.position, tangentVector, Vector3.zero));
    }
    private void SeekTarget()
    {
        Collider[] firstCollider = new Collider[1];
        float curRadius = 0;
        while (targetTransform == null && curRadius <= 30)
        {
            curRadius += 5;
            if (Physics.OverlapSphereNonAlloc(transform.position, curRadius, firstCollider, targetLayer, QueryTriggerInteraction.Collide) == 0)
                continue;
            targetTransform = firstCollider[0].transform;
        }
    }
    private void Explode()
    {
        if (targetTransform != null)
            targetTransform.GetComponent<Enemy_Scr>().TakeDamage(Player_Stats_Scr.Machinegun.damage, transform.position);

        GetComponentInChildren<RocketSmoke_Scr>().rocketIsDestroyed = true;
        transform.DetachChildren();
        //Sound_FXManager_Scr.instance.PlayRandomFXClip(hitAudioClips, transform, 1); //TODO: Add FXManager
        Destroy(gameObject);
    }
    private void OutOfTargetDestroy()
    {
        Destroy(gameObject);
    }
}
