using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon_LazerDrone_Lazer_Scr : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    private float shotStartTime, lazerLifetime, size;
    [SerializeField] private AnimationCurve lazerScaleCurve, lazerLengthCurve;
    private float nextTickTime = 0f;
    private HashSet<Collider> collidersInteractedWithThisTick = new HashSet<Collider>();

    void Start()
    {
       capsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void OnEnable()
    {
        size = GetComponentInChildren<VisualEffect>().GetFloat("Size");
        lazerLifetime = GetComponentInChildren<VisualEffect>().GetFloat("Lifetime");
        shotStartTime = Time.time;
        nextTickTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCollider();
        CountTicks();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Obstacle")
            Debug.DrawLine(other.ClosestPointOnBounds(transform.position), other.ClosestPointOnBounds(transform.position) + Vector3.up * 5, Color.red, 1f);
    }
    private void OnTriggerStay(Collider other)
    {
        if (collidersInteractedWithThisTick.Contains(other)) return;

        switch (other.gameObject.tag)
        {
            case "Enemy":
                other.GetComponent<IDamageable>().TakeDamage(Player_Stats_Scr.LazerDrones.damage, transform.position);
                collidersInteractedWithThisTick.Add(other);
                return;
            case "Obstacle":
                Vector3 collisionPoint = other.ClosestPoint(transform.position);
                DebriesMaker_Scr.instance.HitFromPosAndDir(collisionPoint, transform.position - collisionPoint);
                collidersInteractedWithThisTick.Add(other);
                return;
            default:
                return;
        }
    }

    private void CountTicks()
    {
        if (nextTickTime <= Time.time)
        {
            nextTickTime += Player_Stats_Scr.LazerDrones.timeBetweenTicks;
            collidersInteractedWithThisTick.Clear();
        }
    }
    private void UpdateCollider()
    {
        float t = (Time.time - shotStartTime) / lazerLifetime;
        capsuleCollider.radius = size * lazerScaleCurve.Evaluate(t);
        capsuleCollider.height = size * 17 * lazerLengthCurve.Evaluate(t);
        capsuleCollider.center = new Vector3(0, 0, capsuleCollider.height / 2);
    }
}
