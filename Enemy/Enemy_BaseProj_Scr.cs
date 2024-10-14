using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BaseProj_Scr : MonoBehaviour
{
    [SerializeField] protected float projMovementSpeed;
    public int projDamage;

    protected virtual void Update()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        transform.position += transform.up * projMovementSpeed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
