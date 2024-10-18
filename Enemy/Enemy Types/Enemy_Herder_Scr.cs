using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Herder_Scr : Enemy_Scr
{
    [HideInInspector] public List<Transform> herdTransforms = new List<Transform>();
    private List<Vector3> herdLocalPositions = new List<Vector3>();
    private List<Vector3> herdPrevPositions = new List<Vector3>();
    private Transform rotorTransform;
    private bool inDefenceMode = false;
    [SerializeField] private float timeToMove = 3f; private float timeToStop; private float startTime;
    [SerializeField] private float timeToChangeMode = 10f; private float nextModeTime;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float attackDelay = 0.45f; private float nextAttackTime = 0;
    private Vector3 positionToMove, oldPosition;


    private void Start()
    {
        AddCountToDirector(EnemyId);
        GetChildrenTransforms();
        ArrangeHerd();
        GetNewMovePosition();
    }

    protected override void EnemyMovement()
    {
        rotorTransform.Rotate(transform.up, 180 * Time.deltaTime, Space.Self);
        
        if (nextModeTime < Time.time)
            ArrangeHerd();

        if (Time.time < timeToStop)
            MoveHerdToNewPositions();

        if ((inDefenceMode && Time.time > nextModeTime - 1f) || (!inDefenceMode))
            HerdAttackGlow();

        if (inDefenceMode)
            MoveToNewPosition();
        else
            EnemyAttack();
        //base.EnemyMovement();
    }
    protected override void EnemyAttack()
    {
        if (nextAttackTime >= Time.time)
            return;

        foreach (Transform herd in herdTransforms)
        {
            if (herd == null || bulletPrefab == null)
                continue;
            Transform newBullet = Instantiate(bulletPrefab, herd.position, herd.rotation).transform;
            newBullet.GetComponent<Renderer>().material.SetFloat("_GlowIntensity", 1f);
        }

        nextAttackTime = Time.time + attackDelay;
    }

    private void MoveToNewPosition()
    {
        transform.position = Vector3.Lerp(oldPosition, positionToMove, (Time.time - startTime) / timeToChangeMode);
    }
    private void GetNewMovePosition()
    {
        positionToMove = Camera.main.GetRandomPointFromScreen(50, 50);
        oldPosition = transform.position;
    }
    #region Herd Behaviour
    private void MoveHerdToNewPositions()
    {
        for (int i = 0; i < herdTransforms.Count; i++)
        {
            if (herdTransforms[i] == null)
                continue;
            herdTransforms[i].localPosition = Vector3.Lerp(herdPrevPositions[i], herdLocalPositions[i], (Time.time - startTime) / timeToMove);
            if (Time.time < (startTime + 3f))
            {
                if (inDefenceMode)
                    herdTransforms[i].rotation = Quaternion.Lerp(herdTransforms[i].rotation,
                        Quaternion.FromToRotation(Vector3.forward, herdTransforms[i].position - transform.position), (Time.time - startTime) / 3f);
                else
                    herdTransforms[i].rotation = Quaternion.Lerp(herdTransforms[i].rotation,
                        Quaternion.FromToRotation(Vector3.forward, transform.position - herdTransforms[i].position), (Time.time - startTime) / 3f);
            }
            else
            {
                if (inDefenceMode)
                    herdTransforms[i].rotation = Quaternion.FromToRotation(Vector3.forward, herdTransforms[i].position - transform.position);
                else
                    herdTransforms[i].rotation = Quaternion.FromToRotation(Vector3.forward, transform.position - herdTransforms[i].position);
            }
        }
    }
    private void ArrangeHerd()
    {
        herdLocalPositions = new List<Vector3>();
        herdPrevPositions = new List<Vector3>();
        for (int i = 0; i < herdTransforms.Count; i++)
        {
            herdPrevPositions.Add(herdTransforms[i].localPosition);
        }

        if (inDefenceMode)
        {
            inDefenceMode = false;
            nextAttackTime = Time.time + attackDelay;
            ArrangeHerdInDiamond();
        }
        else
        {
            GetNewMovePosition();
            inDefenceMode = true;
            ArrangeHerdInCircle();
        }

        startTime = Time.time;
        timeToStop = Time.time + timeToMove;
        nextModeTime = Time.time + timeToChangeMode;
    }
    private void ArrangeHerdInCircle()
    {
        for (int i = 0; i < herdTransforms.Count; i++)
        {
            herdLocalPositions.Add(Quaternion.AngleAxis(360 / herdTransforms.Count * i, -rotorTransform.up) * rotorTransform.forward);
        }
    }
    private void ArrangeHerdInDiamond()
    {
        switch (herdTransforms.Count)
        {
            case 6:
                    herdLocalPositions.AddRange(new List<Vector3>()
                    { new Vector3(0, 0, 1), new Vector3(-1, 0, 1), new Vector3(-1, 0, 0),
                    new Vector3(0, 0, -1), new Vector3(1, 0, -1), new Vector3(1, 0, 0) });
                break;
            case 5:
                herdLocalPositions.AddRange(new List<Vector3>()
                    { new Vector3(0, 0, 1), new Vector3(-1, 0, 1), new Vector3(-1, 0, 0),
                    new Vector3(0, 0, -1), new Vector3(1, 0, 0) });
                break;
            case 4:
                herdLocalPositions.AddRange(new List<Vector3>()
                    { new Vector3(0, 0, 1), new Vector3(-1, 0, 0),
                    new Vector3(0, 0, -1), new Vector3(1, 0, 0) });
                break;
            case 3:
                herdLocalPositions.AddRange(new List<Vector3>()
                    { new Vector3(0, 0, 1), new Vector3(-1, 0, 1), new Vector3(-1, 0, 0)});
                break;
            case 2:
                herdLocalPositions.AddRange(new List<Vector3>()
                    { new Vector3(0, 0, 1), new Vector3(0, 0, -1)});
                break;
            case 1:
                herdLocalPositions.Add(new Vector3(0, 0, 1));
                break;
        }
    }
    private void HerdAttackGlow()
    {
        foreach (Transform herd in herdTransforms)
        {
            herd.GetComponent<Renderer>().materials[0].SetFloat("_GlowIntensity", 1f);
        }
    }
    private void GetChildrenTransforms()
    {
        rotorTransform = transform.GetChild(0);
        for (int i = 0; i < rotorTransform.childCount; i++)
        {
            herdTransforms.Add(rotorTransform.GetChild(i));
        }
    }
    #endregion
}
