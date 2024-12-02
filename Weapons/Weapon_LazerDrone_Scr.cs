using System.Drawing;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon_LazerDrone_Scr : MonoBehaviour
{
    [SerializeField] private bool isRightDrone;
    private float distanceToMaintain = 1.5f;
    private float modelRotationSpeed = 25f;
    private Transform modelTransform;
    private float lastEnemySearchTime = 0f; private float searchDelay = 2f;
    private Transform targetTransform;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float lazerLifetime = 4f, shotDelay = 2f; private float nextShotToggle = 0f, shotStartTime; private bool isShooting = false;
    [SerializeField] private float trackingSpeed = 100f;
    [SerializeField] private AnimationCurve lazerLengthCurve, lazerScaleCurve;
    private CapsuleCollider capsuleCollider;
    private float size;

    //TODO: deal damage
    //TODO: separate left and right targets
    //TODO: make Weapon_LazerDrones_Scr

    private void Awake()
    {
        modelTransform = transform.GetChild(0);
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        size = GetComponentInChildren<VisualEffect>().GetFloat("Size");
        lazerLifetime = GetComponentInChildren<VisualEffect>().GetFloat("Lifetime");
        capsuleCollider.gameObject.SetActive(false);
    }

    void Update()
    {
        DroneBehaviour();
    }

    private void DroneBehaviour()
    {
        MaintainDistance();
        RotateModel();
        if (lastEnemySearchTime < Time.time)
        {
            SearchEnemy();
            lastEnemySearchTime += searchDelay;
        }
        TargetTracking();
        ShootLazer();
    }
    private void MaintainDistance()
    {
        Vector3 newPosition = Player_Stats_Scr.instance.transform.position;
        newPosition += new Vector3(distanceToMaintain * (isRightDrone ? 1 : -1), 0, 0);
        transform.position = newPosition;
    }
    private void RotateModel()
    {
        modelTransform.Rotate(Vector3.forward, modelRotationSpeed * (isRightDrone ? -1 : 1) * Time.deltaTime, Space.Self);
    }
    private void TargetTracking()
    {
        Vector3 targetPosition;
        if (targetTransform == null)
            targetPosition = new Vector3(0, 0, 5f);
        else
            targetPosition = targetTransform.position - transform.position;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetPosition, Vector3.up), trackingSpeed * Time.deltaTime);
    }
    private void ShootLazer()
    {
        if (isShooting)
            UpdateCollider();

        if (nextShotToggle >= Time.time)
            return;

        if (isShooting)
        {
            nextShotToggle += shotDelay;
            isShooting = false;
            capsuleCollider.gameObject.SetActive(isShooting);
            modelRotationSpeed = 25f;
        }
        else
        {
            shotStartTime = nextShotToggle;
            nextShotToggle += lazerLifetime;
            isShooting = true;
            capsuleCollider.gameObject.SetActive(isShooting);
            modelRotationSpeed = 75f;
        }
    }
    private void UpdateCollider()
    {
        float t = (Time.time - shotStartTime) / lazerLifetime;
        capsuleCollider.radius = size * lazerScaleCurve.Evaluate(t);
        capsuleCollider.height = size * 17 * lazerLengthCurve.Evaluate(t);
        capsuleCollider.center = new Vector3(0, 0, capsuleCollider.height / 2);
    }
    private void SearchEnemy()
    {
        Collider[] firstCollider = new Collider[1];
        float curRadius = 0;
        while (targetTransform == null && curRadius <= 40)
        {
            curRadius += 5;
            if (Physics.OverlapSphereNonAlloc(transform.position, curRadius, firstCollider, targetLayer, QueryTriggerInteraction.Collide) == 0)
                continue;
            targetTransform = firstCollider[0].transform;
        }
    }
}