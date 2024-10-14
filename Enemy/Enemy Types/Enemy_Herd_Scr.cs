using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Herd_Scr : Enemy_Scr
{
    protected override void EnemyMovement()
    {
        //base.EnemyMovement();
    }
    protected override void Pushback(int damage)
    {
        //base.Pushback(damage);
    }
    protected override void Die()
    {
        base.Die();
        transform.parent.parent.GetComponent<Enemy_Herder_Scr>().herdTransforms.Remove(transform);
    }
}
