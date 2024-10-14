using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ArcMoving_Scr : Enemy_Scr
{
    [SerializeField] private AnimationCurve movementCurve;
    private float movementTime = 5f; private float elapsedTime = 0;
    private float t = 0;
    private Vector3 initialPosition;
    public bool moveToRight = true;
    private int screenWidth, screenHeight;
    private float visibleAreaHeight, visibleAreaWidth;

    private void Start()
    {
        screenWidth = Camera.main.pixelWidth - 100;
        screenHeight = Camera.main.pixelHeight - 100;

        visibleAreaWidth = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenWidth, 0)).x - Camera.main.ScreenToWorldPoint(new Vector3(100, screenHeight, 0)).x;
        visibleAreaHeight = Camera.main.ScreenToWorldPoint(new Vector3(100, screenHeight, 0)).y - Camera.main.ScreenToWorldPoint(new Vector3(100, 0, 0)).y;

        initialPosition = transform.position;
    }

    protected override void EnemyMovement()
    {
        if (elapsedTime < movementTime)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / movementTime;
            if (moveToRight)
                transform.position = initialPosition + new Vector3(visibleAreaWidth * t, movementCurve.Evaluate(t) * 2 * visibleAreaHeight, 0);
            else
                transform.position = initialPosition + new Vector3(-visibleAreaWidth * t, movementCurve.Evaluate(t) * 2 * visibleAreaHeight, 0);

        }
        else
        {
            Disappear();
        }
    }
}
