using UnityEngine;

public class Weapon_LazerDrone_Scr : MonoBehaviour
{
    [SerializeField] private bool isRightDrone;
    private float distanceToMaintain = 1.5f;
    private float modelRotationSpeed = 25f;
    private Transform modelTransform;
    private float lastEnemySearchTime = 0f; private float searchDelay = 2f;
    private Transform targetTransform;
    [SerializeField] private LayerMask targetLayer;
    private float trackingSpeed = 200f;

    //TODO: deal damage
    //TODO: hook up vfx to attacks
    //TODO: dmg colliders
    //TODO: separate left and right targets
    //TODO: make Weapon_LazerDrones_Scr

    private void Awake()
    {
        modelTransform = transform.GetChild(0);
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
        EnemyTracking();
    }
    private void MaintainDistance()
    {
        Vector3 newPosition = Player_Stats_Scr.instance.transform.position;
        newPosition += new Vector3(distanceToMaintain * (isRightDrone ? 1 : -1), 0, 0);
        transform.position = newPosition;
    }
    private void RotateModel() //TODO: speedup when shooting
    {
        modelTransform.Rotate(Vector3.forward, modelRotationSpeed * (isRightDrone ? -1 : 1) * Time.deltaTime, Space.Self);
    }
    private void EnemyTracking()
    {
        //TODO
        Vector3 targetPosition;
        if (targetTransform == null)
            targetPosition = transform.position + new Vector3(0, 0, 5f);
        else
            targetPosition = targetTransform.position - transform.position;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetPosition, Vector3.up), trackingSpeed * Time.deltaTime);
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
