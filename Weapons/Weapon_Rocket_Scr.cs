using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class Weapon_Rocket_Scr : MonoBehaviour
{
    private Spline spline;
    private SplineContainer container;
    [SerializeField] private float movementTime = 2f;
    private float movementSpeed = 15f;
    private float distanceCovered = 0;
    //private float startTime;
    private Vector3 tangentVector;
    private Transform targetTransform;
    [SerializeField] private LayerMask targetLayer;

    //TODO: поменять tangent на более правильный

    private void Start()
    {
        SeekTarget();
        CreateMovementSpline();

        //startTime = Time.time;
    }
    private void Update()
    {
        RocketMovement();
    }

    private void RocketMovement()
    {
        if (spline == null)
        {
            Debug.Log("У ракеты отсутствует spline");
            return;
        }

        /*if ((Time.time - startTime) > movementTime)
            Destroy(gameObject);
        //transform.position = spline.EvaluatePosition((Time.time - startTime) / movementTime);*/

        distanceCovered += movementSpeed * Time.deltaTime / spline.GetLength();
        if (distanceCovered >= 1)
        {
            Destroy(gameObject);
        }
        transform.position = spline.EvaluatePosition(distanceCovered);
        transform.LookAt(spline.EvaluatePosition(distanceCovered + 0.01f));

        UpdateMovementSpline();
    }
    private void CreateMovementSpline()
    {
        if (targetTransform == null)
        {
            Debug.Log("Ракета не нашла цель");
            return;
        }

        container = GetComponent<SplineContainer>();

        if (Random.Range(0f, 1f) >= 0.5f)
            tangentVector = Vector3.Cross((targetTransform.position - transform.position).normalized, Vector3.up);
        else
            tangentVector = Vector3.Cross((targetTransform.position - transform.position).normalized, Vector3.down);

        spline = new()
        {
            new BezierKnot(transform.position, Vector3.zero, tangentVector * 3),
            new BezierKnot(targetTransform.position, tangentVector, Vector3.zero)
        };

        container.AddSpline(spline);
    }
    private void UpdateMovementSpline()
    {
        if (targetTransform == null)
        {
            OutOfTargetDestroy();
            return;
        }
        spline.SetKnot(1, new(targetTransform.position, tangentVector, Vector3.zero));
    }
    private void SeekTarget()
    {
        Collider[] firstCollider = new Collider[1];
        float curRadius = 0;
        while (targetTransform == null || curRadius >= 30)
        {
            curRadius += 5;
            if (Physics.OverlapSphereNonAlloc(transform.position, curRadius, firstCollider, targetLayer, QueryTriggerInteraction.Collide) == 0)
                continue;
            targetTransform = firstCollider[0].transform;
        }

    }
    private void OutOfTargetDestroy()
    {
        Destroy(gameObject);
    }
}
