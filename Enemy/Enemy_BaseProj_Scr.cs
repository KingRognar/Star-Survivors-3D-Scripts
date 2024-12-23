using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BaseProj_Scr : MonoBehaviour
{
    [SerializeField] protected float projMovementSpeed;
    public int projDamage;

    //TODO: ����������� � OnBecameInvisible - �� �����-�� ������� ����������� ������ ��� 50 

    protected virtual void Update()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        transform.position += -transform.forward * projMovementSpeed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
