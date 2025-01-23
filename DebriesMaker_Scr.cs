using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DebriesMaker_Scr : MonoBehaviour
{
    public static DebriesMaker_Scr instance;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject hitPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ExplodeOnPos(Vector3 explosionPos)
    {
        Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
    }
    public async Task BigObjectExplosions(Collider collider,Transform trans, float radius, int explMin, int explMax)
    {
        int explCount = Random.Range(explMin, explMax);

        for (int i = 0; i < explCount; i++)
        {
            Vector3 explosionPos = Random.onUnitSphere * radius;
            explosionPos = collider.ClosestPoint(explosionPos + trans.position);
            ExplodeOnPos(explosionPos);

            int delay = (int)(Random.Range(0, 0.1f) * 1000);
            await Task.Delay(delay);
        }
    }
    public void HitFromPosAndDir(Vector3 pos, Vector3 direction)
    {
        Instantiate(hitPrefab, pos, Quaternion.FromToRotation(Vector3.up, direction));
    }



}
