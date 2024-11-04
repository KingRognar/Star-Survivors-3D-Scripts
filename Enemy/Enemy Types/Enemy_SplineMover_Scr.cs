using UnityEngine;
using UnityEngine.Splines;

public class Enemy_SplineMover_Scr : Enemy_Scr
{
    Spline spline;

    private float distanceCovered = 0f;

    protected override void EnemyMovement()
    {
        distanceCovered += movementSpeed * Time.deltaTime / spline.GetLength();
        transform.position = spline.EvaluatePosition(distanceCovered);
        transform.LookAt(spline.EvaluatePosition(distanceCovered + 0.01f));

        if (distanceCovered >= 1)
            Disappear();
    }

    protected override void Initialize()
    {
        base.Initialize();
        spline = GetComponent<SplineContainer>().Spline;
    }
}
