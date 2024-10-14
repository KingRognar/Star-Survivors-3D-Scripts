using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HitEffect_Scr : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private Color particleColor;
    [SerializeField] private int minParticlesCount = 2; [SerializeField] private int maxParticlesCount = 4;
    [SerializeField] private float scatterAngleRange = 20f;


    public void SpawnParticles(Vector3 projectilePosition)
    {
        Vector3 collisionPoint = GetComponent<Collider>().ClosestPoint(projectilePosition);
        //Vector3 collisionPoint = GetComponent<Collider2D>().ClosestPoint(projectilePosition);

        int particlesCount = Random.Range(minParticlesCount, maxParticlesCount);
        for (int i = 0; i < particlesCount; i++)
        {
            Transform particle = Instantiate(particlePrefab, collisionPoint, Quaternion.identity).transform;
            float colorVariability = Random.Range(-0.05f, 0.05f);
            particle.GetComponent<Renderer>().material.color = particleColor + new Color(colorVariability, colorVariability, colorVariability);
            particle.RotateAround(particle.position, particle.up, Random.Range(-180, 180));
            Vector3 directionToMove = projectilePosition - collisionPoint;
            directionToMove.Normalize();
            directionToMove = Quaternion.AngleAxis(Random.Range(-scatterAngleRange,scatterAngleRange),transform.up) * directionToMove;
            particle.GetComponent<Enemy_HitParticles_Scr>().directionToMove = directionToMove;
        }
    }
}
